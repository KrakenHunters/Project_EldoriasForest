using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/ Audio Event/ Enemy Audio Event")]
public class EnemyAudioEvent : SOEvent
{
    public UnityEvent<BossEnemy> OnWitchScream;


}
