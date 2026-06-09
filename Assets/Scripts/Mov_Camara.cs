using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Camara : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 5f, -10f);
    // velocidad de la camara
    public float smoothSpeed = 5f;

    [Tooltip("Si está activado, la cámara solo seguirá la X del jugador (mantiene Y/Z fijos)")]
    public bool followXOnly = true;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Mov_Camara: no se ha asignado target (Player). Asigna el Transform del jugador en el inspector.");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition;
        if (followXOnly)
        {
            // Seguir únicamente la X del jugador; mantener Y y Z fijos.
            desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
        }
        else
        {
            desiredPosition = target.position + offset;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        if (!followXOnly) transform.LookAt(target);
    }
}
