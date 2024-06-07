using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game Event")]
public class EmptyGameEvent : GameEvent<Empty> { }


public readonly struct Empty { }