using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI Pointer Event")]
public class UIPointerEvent : SOEvent
{
    public UnityEvent<Transform> SendTargetPos;
    public UnityEvent EndTargetTracking;
}
