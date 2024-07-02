using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SpellAudioEvent spellAudioEvent;
    [SerializeField] private EnemyAudioEvent enemyAudioEvent;
    [SerializeField] private PlayerAudioEvent playerAudioEvent;
    [SerializeField] private MenuAudioEvent menuAudioEvent;
    [SerializeField] private CollectableAudioEvent collectableAudioEvent;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup masterG;
    [SerializeField] private AudioMixerGroup BGMusicG;
    [SerializeField] private AudioMixerGroup SFXG;
    [SerializeField] private AudioMixerGroup ShopkeeperG;
    [SerializeField] private AudioMixerGroup spellCastG;
    [SerializeField] private AudioMixerGroup collectG;

    private Dictionary<SpellBook, AudioSource> spellAudioSources = new Dictionary<SpellBook, AudioSource>();
    private List<AudioSource> unusedSources = new List<AudioSource>();
    private List<AudioSource> allSources = new List<AudioSource>();

    private void OnEnable()
    {
        spellAudioEvent.Cast.AddListener(PlaySpellCastAudio);
        spellAudioEvent.Looping.AddListener(PlaySpellLooping);
        menuAudioEvent.PlayBGMusic.AddListener(PlayMenuMusic);
        menuAudioEvent.ButtonClick.AddListener(PlayButtonClick);
        menuAudioEvent.ShopKeeper.AddListener(PlayShopKeeper);
        menuAudioEvent.StopAllAudio.AddListener(StopAllAudio);
        collectableAudioEvent.ItemCollected.AddListener(PlayItemCollected);
        enemyAudioEvent.OnWitchScream.AddListener(PlayWitchScream);

    }

    private void OnDisable()
    {
        spellAudioEvent.Cast.RemoveListener(PlaySpellCastAudio);
        spellAudioEvent.Looping.RemoveListener(PlaySpellLooping);
        menuAudioEvent.PlayBGMusic.RemoveListener(PlayMenuMusic);
        menuAudioEvent.ButtonClick.RemoveListener(PlayButtonClick);
        menuAudioEvent.ShopKeeper.RemoveListener(PlayShopKeeper);
        menuAudioEvent.StopAllAudio.RemoveListener(StopAllAudio);
        collectableAudioEvent.ItemCollected.RemoveListener(PlayItemCollected);
        enemyAudioEvent.OnWitchScream.RemoveListener(PlayWitchScream);

    }

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            unusedSources.Add(source);
            allSources.Add(source);
        }
    }

    private void PlayItemCollected(AudioClip clip)
    {
        AudioSource speaker = FindUnusedAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        if (clip == null)
        {
            Debug.Log("Item audio clip is missing.");
        }
        else
        {
            speaker.outputAudioMixerGroup = collectG;
            speaker.clip = clip;
            speaker.PlayOneShot(clip);
            speaker.clip = null;
        }
    }

    private void PlayMenuMusic(AudioClip clip)
    {
        AudioSource speaker = FindUnusedAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        if (clip == null)
        {
            Debug.Log("Menu audio clip is missing.");
        }
        else
        {
            speaker.outputAudioMixerGroup = BGMusicG;
            speaker.clip = clip;
            speaker.loop = true;
            speaker.Play();
        }
    }

    private void PlayButtonClick(AudioClip clip)
    {
        AudioSource speaker = FindUnusedAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        speaker.outputAudioMixerGroup = SFXG;
        speaker.clip = clip;
        speaker.PlayOneShot(clip);
        speaker.clip = null;
    }
    private void PlayShopKeeper(AudioClip clip)
    {
        AudioSource speaker = FindUnusedAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        speaker.outputAudioMixerGroup = ShopkeeperG;
        speaker.clip = clip;
        speaker.PlayOneShot(clip);
        speaker.clip = null;
    }

    private void PlayWitchScream(BossEnemy witch)
    {
        AudioSource speaker = FindUnusedAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        speaker.outputAudioMixerGroup = SFXG;
        speaker.clip = witch.screamClip;
        speaker.PlayOneShot(witch.screamClip);
        speaker.clip = null;
    }


    private void PlaySpellCastAudio(SpellBook spell)
    {
        AudioSource speaker = GetAudioSourceForSpell(spell);
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }

        // Assign clip to speaker before playing
        if (spell.castClip != null)
        {
            speaker.clip = spell.castClip;
        }
        else
        {
            Debug.LogWarning("Spell audio clip is missing.");
            return;
        }
        speaker.PlayOneShot(speaker.clip);
        speaker.clip = null;
    }

    private void PlaySpellLooping(SpellBook spell)
    {
        AudioSource speaker = GetAudioSourceForSpell(spell);
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }
        speaker.clip = spell.castClip;
        speaker.loop = true;
        speaker.Play();

        // Stop the looping audio after the spell's duration
        StartCoroutine(StopLoopingAudioAfterDuration(spell));
    }

    private IEnumerator StopLoopingAudioAfterDuration(SpellBook spell)
    {
        float duration = spell.ReturnDuration();
        float timer = 0;
       while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        StopLoopingAudio(spell);
    }

    private void StopLoopingAudio(SpellBook spell)
    {
        AudioSource speaker = GetAudioSourceForSpell(spell);
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }
        speaker.outputAudioMixerGroup = masterG;
        speaker.loop = false;
        speaker.Stop();
        speaker.clip = null;
    }


    private AudioSource GetAudioSourceForSpell(SpellBook spell)
    {
        // Check if we already have an AudioSource for this spell
        if (spellAudioSources.ContainsKey(spell))
        {
            return spellAudioSources[spell];
        }

        // If not, try to find an unused AudioSource
        AudioSource unusedSource = FindUnusedAudioSource();
        unusedSource.outputAudioMixerGroup = spellCastG;

        // Add the spell and its AudioSource to the dictionary
        spellAudioSources[spell] = unusedSource;
        return unusedSource;
    }

    private AudioSource FindUnusedAudioSource()
    {
        foreach (AudioSource source in unusedSources)
        {
            if (source.clip == null)
            {
                source.outputAudioMixerGroup = SFXG;
                return source;
            }
        }
        return null;
    }

    public void StopAllAudio()
    {
        foreach (AudioSource source in allSources)
        {
            source.Stop();
            source.outputAudioMixerGroup = null;
            source.clip = null;
        }
    }

    public void StopOnlySounds()
    {
        foreach (AudioSource source in allSources)
        {
            if (source.outputAudioMixerGroup == BGMusicG)
            {
              continue; //skip the looped sounds
            }
            source.Stop();
            source.clip = null;
        }
    }

}
