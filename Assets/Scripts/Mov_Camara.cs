using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Camara : MonoBehaviour
{

     // Velocidad de movimiento
    public float velocidad = 3f;
    // CharacterController para mover al jugador
    private CharacterController characterController;
    // Gravedad para la caída del jugador
    void Start()
    {
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
    }
}
