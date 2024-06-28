using UnityEngine;
using UnityEngine.Playables;

public class CinematicController : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }
    public void StartCinematic()
    {
        playableDirector.Play();
    }

}