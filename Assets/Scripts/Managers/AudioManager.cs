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
    }

    private void OnDisable()
    {
        spellAudioEvent.Cast.RemoveListener(PlaySpellCastAudio);
    }

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            unusedSources.Add(source);
        }
    }

    private void Update()
    {
        // Update logic as needed
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
            if (!source.isPlaying && source.clip == null)
            {
                return source;
            }
        }
        return null;
    }

}
