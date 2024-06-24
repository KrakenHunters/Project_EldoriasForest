using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SpellAudioEvent spellAudioEvent;
    [SerializeField] private EnemyAudioEvent enemyAudioEvent;
    [SerializeField] private PlayerAudioEvent playerAudioEvent;
    [SerializeField] private MenuAudioEvent menuAudioEvent;
    [SerializeField] private CollectableAudioEvent collectableAudioEvent;

    private Dictionary<SpellBook, AudioSource> spellAudioSources = new Dictionary<SpellBook, AudioSource>();
    private List<AudioSource> unusedSources = new List<AudioSource>();

    private void OnEnable()
    {
        spellAudioEvent.Cast.AddListener(PlaySpellCastAudio);
        spellAudioEvent.Looping.AddListener(PlaySpellLooping);
        menuAudioEvent.PlayBGMusic.AddListener(PlayMenuMusic);
        menuAudioEvent.ButtonClick.AddListener(PlayButtonClick);
        collectableAudioEvent.ItemCollected.AddListener(PlayItemCollected);
    }

    private void OnDisable()
    {
        spellAudioEvent.Cast.RemoveListener(PlaySpellCastAudio);
        spellAudioEvent.Looping.RemoveListener(PlaySpellLooping);
        menuAudioEvent.PlayBGMusic.RemoveListener(PlayMenuMusic);
        menuAudioEvent.ButtonClick.RemoveListener(PlayButtonClick);
        collectableAudioEvent.ItemCollected.RemoveListener(PlayItemCollected);
    }

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            unusedSources.Add(source);
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

        speaker.clip = clip;
        speaker.PlayOneShot(clip);
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
    }

    private void PlaySpellLooping(SpellBook spell)
    {
        AudioSource speaker = GetAudioSourceForSpell(spell);
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }
         Debug.Log("Playing Looping Audio");
        speaker.clip = spell.castClip;
        speaker.loop = true;
        speaker.Play();

        // Stop the looping audio after the spell's duration
        StartCoroutine(StopLoopingAudioAfterDuration(spell, spell.ReturnDuration()));
    }

    private IEnumerator StopLoopingAudioAfterDuration(SpellBook spell, float duration)
    {
        yield return new WaitForSeconds(duration);

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

        // If no unused source is available, create a new one
        if (unusedSource == null)
        {

        }

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
                return source;
            }
        }
        return null;
    }

}
