using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;
    [SerializeField] private VisualEffect vfxGraph;
    [SerializeField] float dissolveRate = 0.0125f;
    [SerializeField] float refreshRate = 0.025f;

    private Material skinnedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
            skinnedMaterial = skinnedMesh.material;
    }

    public void OnDead()
    {
        StartCoroutine(DissolveCharacter());

    }

    IEnumerator DissolveCharacter()
    {
        if (vfxGraph != null)
        {
            Debug.Log("Play VFX");
            vfxGraph.SetFloat("Duration", (1 / dissolveRate) * refreshRate);
            vfxGraph.Play();
        }

        float counter = 0;
        while (skinnedMaterial.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            skinnedMaterial.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
