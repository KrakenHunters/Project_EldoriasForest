using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/DoubleFloatEvent")]
public  class DoubleFloatEvent : ScriptableObject
{
    public UnityAction<float,float> floatEvent;


    public void Raise(float value1,float value2)
    {
        floatEvent.Invoke(value1,value2);
    }


}


