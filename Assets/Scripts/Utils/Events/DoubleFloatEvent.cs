using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DoubleFloatEvent")]
public class DoubleFloatEvent : SOEvent 
{
    public UnityEvent<float, float> OnValueChanged;
    public UnityEvent OnCancelInteract;

}
