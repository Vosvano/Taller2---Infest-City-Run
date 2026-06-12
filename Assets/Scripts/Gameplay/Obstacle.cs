using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
    }
}
