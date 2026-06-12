using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Fog : MonoBehaviour
{
    public float velocidad = 5f;
    // Rigidboy para colisiones
    public Rigidbody rb;
    // Fuerza de salto

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {

        // Movimiento hacia adelante
        transform.Translate(Vector3.down * velocidad * Time.deltaTime);
    }
}
