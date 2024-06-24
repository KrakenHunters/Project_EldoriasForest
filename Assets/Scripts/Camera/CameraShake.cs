using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shakeTimer;
    [SerializeField]
    private DoubleFloatEvent gameEvent;

    private void OnEnable()
    {
        gameEvent.OnPlayerGotHit.AddListener(ShakeCamera);
        gameEvent.OnUltimateCast.AddListener(ShakeCamera);

    }

    private void OnDisable()
    {
        gameEvent.OnPlayerGotHit.RemoveListener(ShakeCamera);
        gameEvent.OnUltimateCast.RemoveListener(ShakeCamera);
    }


    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer < 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
