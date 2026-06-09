using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform offPoint;
    public float speed = 20f;
    public float shotRate = 0.5f;
    private float shotRateTimer = 0f;
    public float lifeTime = 3f;

    void Start()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("No se ha asignado el prefab de la bala.");
        }
        if (offPoint == null)
        {
            Debug.LogError("No se ha asignado el punto de disparo.");
        }

    }

    void Update()
    {
        if (projectilePrefab != null && offPoint != null)

            if(Input.GetKeyDown(KeyCode.F))
            {
                if (Time.time > shotRateTimer)
                {
                    GameObject newProjectile;
                    newProjectile = Instantiate(projectilePrefab,offPoint.position, offPoint.rotation);

                    newProjectile.GetComponent<Rigidbody>().AddForce(offPoint.forward * speed, ForceMode.Impulse);

                    shotRateTimer = Time.time + shotRate;

                    Destroy(newProjectile, lifeTime);
                }
            }
            
        {
    }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
