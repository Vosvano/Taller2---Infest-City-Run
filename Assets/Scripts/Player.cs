using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileOffset = 1f;

    private bool isDead = false;

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

        if (obj.CompareTag("Enemy") || obj.CompareTag("Obstacle"))
        {
            Debug.Log("Player collided with: " + obj.name);
            isDead = true;
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
