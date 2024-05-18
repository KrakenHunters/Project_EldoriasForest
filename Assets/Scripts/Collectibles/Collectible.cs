using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            ItemCollected(other.GetComponent<PlayerController>());
            //Play Collected SFX and VFX
            Invoke("DestroyGameObj", 0.5f); //Destroy game object after VFX and SFX are done
        }
    }

    protected virtual void ItemCollected(PlayerController player)
    {

    }

    private void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}
