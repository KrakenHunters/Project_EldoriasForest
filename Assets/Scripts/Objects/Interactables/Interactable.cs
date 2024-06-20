using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : BaseObject
{
    [HideInInspector]
    public bool canInteract;

    [SerializeField]
    private TMPro.TextMeshProUGUI _holdToInteract;

    private ParticleSystem particles;

    public float waitTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        canInteract = true;
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public virtual void Interact()
    {
        Time.timeScale = 0.0f;
    }

    public void ActivateInteractable()
    {
        _holdToInteract.enabled = true;
        particles.Play();
    }

    public void DeactivateInteractable()
    {
        _holdToInteract.enabled = false;
        particles.Stop();

    }


}
