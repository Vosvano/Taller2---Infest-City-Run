using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // movement speed towards the player (along -X)

    void Start()
    {
        // ensure we have a collider set as trigger or not depending on design
    }

    void Update()
    {
        // Move towards negative X in world space (towards the player who runs in +X)
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
