using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    
    public int coins = 0;
    private bool isDead = false;
    public event Action OnPlayerDeath;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        HandleCollisionObject(hit.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollisionObject(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        HandleCollisionObject(other.gameObject);
    }

    private void HandleCollisionObject(GameObject obj)
    {
        if (isDead) return;

        if (obj == null) return;

        if (IsDeathCollision(obj))
        {
            Debug.Log("Player collided with: " + obj.name);
            isDead = true;
            GameManager.Instance.StopScoreIncrease();
            OnPlayerDeath?.Invoke();
            
        }
    }

    private bool IsDeathCollision(GameObject obj)
    {
        if (obj.CompareTag("Enemy") || obj.CompareTag("Obstacle"))
        {
            return true;
        }

        Transform root = obj.transform.root;
        if (root != null && (root.CompareTag("Enemy") || root.CompareTag("Obstacle")))
        {
            return true;
        }

        if (obj.GetComponentInParent<Enemy>() != null || obj.GetComponentInParent<Obstacle>() != null)
        {
            return true;
        }

        return false;
    }

}
