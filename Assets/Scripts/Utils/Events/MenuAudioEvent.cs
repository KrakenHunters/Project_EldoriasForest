using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/ Audio Event/ Menu Audio Event")]
public class MenuAudioEvent : SOEvent
{
    public UnityEvent<AudioClip> PlayBGMusic;
    public UnityEvent<AudioClip> ButtonClick;
    public UnityEvent<AudioClip> ShopKeeper;
    public UnityEvent StopAllAudio;
}
