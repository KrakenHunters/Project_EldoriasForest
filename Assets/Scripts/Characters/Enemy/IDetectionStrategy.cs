using UnityEngine;
using Utilities;

public interface IDetectionStrategy
{
    bool Execute(Transform player, Transform detector, CountdownTimer timer);
    bool IsPlayerInLineOfSight(Transform player, Transform detector);
}
