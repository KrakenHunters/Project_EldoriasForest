using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/ Audio Event/ Collectable Audio Event")]
public class CollectableAudioEvent : SOEvent
{
   public UnityEvent<AudioClip> ItemCollected;

}
