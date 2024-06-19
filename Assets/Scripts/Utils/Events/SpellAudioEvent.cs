using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/ Audio Event/ Spell Audio Event")]
public class SpellAudioEvent : SOEvent
{
  public UnityEvent<SpellBook> Cast;
  public UnityEvent<SpellBook> Hit;
  public UnityEvent<SpellBook> End;
  public UnityEvent<SpellBook> Travel;
}
