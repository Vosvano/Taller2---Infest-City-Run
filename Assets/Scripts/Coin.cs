using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    // Vertical offset applied at spawn (meters above platform base)
    public float verticalOffset = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        // Accept Player on collider or on parent hierarchy
        Player player = other.GetComponent<Player>() ?? other.GetComponentInParent<Player>();
        if (player != null)
        {
            player.AddCoin(value);
            Destroy(gameObject);
            return;
        }

        // also accept if root has Player tag
        if (other.transform.root != null && other.transform.root.CompareTag("Player"))
        {
            Player p = other.transform.root.GetComponent<Player>();
            if (p != null)
            {
                p.AddCoin(value);
                Destroy(gameObject);
            }
        }
    }
}
