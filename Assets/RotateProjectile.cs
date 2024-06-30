using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateProjectile : MonoBehaviour
{
    // Velocidade de rotação em graus por segundo.
    public float rotationSpeed = 100f;

    void Update()
    {
        // Calcula a rotação em torno do eixo Y.
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplica a rotação ao objeto.
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
