using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DoubleFloatEvent")]
public class DoubleFloatEvent : SOEvent 
{
    public UnityEvent<float, float> OnValueChanged;
    public UnityEvent<float, float> OnPlayerGotHit;
    public UnityEvent<float, float> OnPlayerHeal;

    public UnityEvent<float, float> OnUltimateCast;
    public UnityEvent OnCancelInteract;

}
