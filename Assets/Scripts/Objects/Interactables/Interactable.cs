using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class Interactable : BaseObject
{
    [HideInInspector]
    public bool canInteract;

    [SerializeField]
    private TMPro.TextMeshProUGUI _holdToInteract;

    private ParticleSystem particles;

    private Transform Player;

    public float waitTime;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform;

        canInteract = true;
        particles = GetComponentInChildren<ParticleSystem>();

    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, Player.position) < 10f && canInteract && !particles.isPlaying)
        {
            particles.Play();
        }
        else if((Vector3.Distance(this.transform.position, Player.position) > 10f || !canInteract) && particles.isPlaying)
        {
            particles.Stop();
        }
    }

    public virtual void Interact()
    {
        Time.timeScale = 0.0f;
    }

    public void ActivateInteractable()
    {
        _holdToInteract.enabled = true;
    }

    public void DeactivateInteractable()
    {
        _holdToInteract.enabled = false;

    }


}
