using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

        // Bala pre fabricada

    public float velocityProj = 20f;
    
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
