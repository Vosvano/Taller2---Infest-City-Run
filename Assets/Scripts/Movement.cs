using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

     // Velocidad de movimiento
    public float velocidad = 5f;
    // Rigidboy para colisiones
    public Rigidbody rb;
    // Fuerza de salto
    public float jumpForce = 10f;
    // Variable para verificar si el jugador está en el suelo
    public bool isGrounded;
    // CharacterController para mover al jugador
    private CharacterController characterController;
    // Gravedad para la caída del jugador
    public float gravity = -9.81f;
    void Start()
    {
        isGrounded = true;
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {

        characterController.Move(transform.forward * velocidad * Time.deltaTime);

        float hor = Input.GetAxis("Horizontal");

        if (hor < 0)
        {
            characterController.Move(transform.right * hor * 8 * Time.deltaTime);
        }
        else if (hor > 0)
        {
            characterController.Move(transform.right * hor * 8 * Time.deltaTime);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        }

    }
}
