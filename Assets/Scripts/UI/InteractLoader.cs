using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractLoader : MonoBehaviour
{
    [SerializeField]
    private DoubleFloatEvent OnLoadingUI;
    [SerializeField]
    private Image image;
    private void OnEnable()
    {
        OnLoadingUI.OnValueChanged.AddListener(SetLoader);
        OnLoadingUI.OnCancelInteract.AddListener(DisableLoader);

    }

    private void OnDisable()
    {
        OnLoadingUI.OnValueChanged.RemoveListener(SetLoader);
        OnLoadingUI.OnCancelInteract.RemoveListener(DisableLoader);

    }

    private void SetLoader(float timer, float waitTime)
    {
        image.gameObject.SetActive(true);
        Debug.Log("Set Laoder");
        image.fillAmount = timer / waitTime;

        if (timer >= waitTime)
        {
            DisableLoader();
        }
    }

    private void DisableLoader()
    {
        image.gameObject.SetActive(false);
    }
}
