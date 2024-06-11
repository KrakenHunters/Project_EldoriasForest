using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Village : Interactable
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Shop");

    }
}
