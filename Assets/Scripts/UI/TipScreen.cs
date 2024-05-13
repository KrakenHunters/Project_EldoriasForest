using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class TipScreen : MonoBehaviour
{

    [SerializeField]
    GameObject tip;


    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        if (tip != null)
            tip.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tip != null)
                tip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tip != null)
                tip.SetActive(false);

            Destroy(this.gameObject);
        }
    }
           
   
}
