using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerInteractionInfo", menuName = "SO/Player/PlayerInteractionInfo", order = 0)]
public class PlayerInteractionInformationSO : ScriptableObject
{
    public event Action<Vector3> playerLocationChange;
    public UnityEvent<Vector3> playerLocationChangedEvent;
    public event Action playerTookDamage;
    public UnityEvent playerTookDamageEvent;

    public void PlayerLocationChanged(Vector3 newLocation)
    {
        playerLocationChange?.Invoke(newLocation);
        playerLocationChangedEvent?.Invoke(newLocation);
    }
    public void PlayerTookDamage()
    {
        playerTookDamageEvent?.Invoke();
        playerTookDamage?.Invoke();
    }

}
