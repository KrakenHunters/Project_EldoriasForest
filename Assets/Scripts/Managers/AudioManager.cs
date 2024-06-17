using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

   [SerializeField] private SpellAudioEvent spellAudioEvent;
   [SerializeField] private EnemyAudioEvent enemyAudioEvent;
   [SerializeField] private PlayerAudioEvent playerAudioEvent;
   [SerializeField] private MenuAudioEvent menuAudioEvent;
   [SerializeField] private CollectableAudioEvent collectableAudioEvent;

    private List<AudioSource> sourcesInUse = new List<AudioSource>();
    private List<AudioSource> sources = new List<AudioSource>();
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
            sources.Add(source);
        }
    }

    private void Update()
    {
        foreach (AudioSource source in sourcesInUse)
        {
            if (!source.isPlaying)
            {
                source.clip = null;

            }
        }
    }


    private void PlaySpellCastAudio(SpellBook spell)
    {
        AudioSource speaker = GetAudioSource();
        if (speaker == null)
        {
            Debug.Log("No Audio Source Available");
            return;
        }
        speaker.PlayOneShot(speaker.clip);
    }

    public AudioSource GetAudioSource()
    {
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying && source.clip != null)
            {
                return source;
            }
        }
        //Make New Source
        return null;
    }

}
