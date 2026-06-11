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
    public float jumpForce = 6f;
    // Variable para verificar si el jugador está en el suelo
    public bool isGrounded;
    // CharacterController para mover al jugador
    private CharacterController characterController;
    // Velocidad vertical usada para saltos y gravedad
    private float verticalVelocity = 0f;
    // Gravedad para la caída del jugador
    public float gravity = -9.81f;
    // Carriles
    public float laneOffset = 2f; // distancia entre carriles en Z
    [Range(0,2)]
    public int laneIndex = 1; 
    public float laneChangeSpeed = 6f; // unidades por segundo para cambiar de carril
    void Start()
    {
        isGrounded = true;
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Movement: CharacterController no encontrado en el GameObject.");
        }
    }

    
    void Update()
    {

        // Avance constante del mundo (el jugador corre hacia +X)
        // Control de carriles: A/Left = izquierda, D/Right = derecha
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            laneIndex = Mathf.Min(2, laneIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            laneIndex = Mathf.Max(0, laneIndex - 1);
        }

        // velocidad vertical y salto
        if (characterController == null)
        {
            // no podemos procesar salto sin CharacterController
            return;
        }

        if (characterController.isGrounded)
        {
            isGrounded = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Movement: Space pressed - attempting jump");
                verticalVelocity = jumpForce; // velocidad incial de salto
                Debug.Log("Movement: verticalVelocity set to " + verticalVelocity);
            }
            else if (verticalVelocity < 0f)
            {
                verticalVelocity = -1f; // fuerza hacia abajo para mantener al jugador en el piso
            }
        }
        else
        {
            isGrounded = false;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = Vector3.right * velocidad; // avance constante

        // calcular objetivo Z según carril
        float targetZ = (laneIndex - 1) * laneOffset;
        // mover suavemente la posición Z hacia targetZ
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, laneChangeSpeed * Time.deltaTime);
        float deltaZ = newZ - transform.position.z;
        float lateralSpeed = 0f;
        if (Mathf.Abs(deltaZ) > 0f)
        {
            lateralSpeed = deltaZ / Time.deltaTime;
        }
        move += Vector3.forward * lateralSpeed;
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);

    }
}
