using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
    public class GameEventContainor : ScriptableObject
{
    private PlayerSpellCastManager playerCast;

    public void RegisterListener(PlayerSpellCastManager playerLister)
    {
        playerLister = playerCast;
    }
    public void UnregisterListener(PlayerSpellCastManager playerLister)
    {
     playerCast = null;
    }

    public void Raise()
    {
    
    }
}
