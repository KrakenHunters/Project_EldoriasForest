using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateProjectile : MonoBehaviour
{
    // Velocidade de rota��o em graus por segundo.
    public float rotationSpeed = 100f;

    void Update()
    {
        // Calcula a rota��o em torno do eixo Y.
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplica a rota��o ao objeto.
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
