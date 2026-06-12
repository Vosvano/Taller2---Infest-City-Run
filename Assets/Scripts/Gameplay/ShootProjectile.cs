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

                    AudioManager.Instance.PlayShoot();



                    Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

                    if (projectileRigidbody != null)

                    {

                        projectileRigidbody.useGravity = false;

                        projectileRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                        projectileRigidbody.AddForce(offPoint.forward * speed, ForceMode.Impulse);

                    }

                    if (newProjectile.GetComponent<ProjectileCollision>() == null)

                    {

                        newProjectile.AddComponent<ProjectileCollision>();

                    }



                    shotRateTimer = Time.time + shotRate;



                    Destroy(newProjectile, lifeTime);

                }

            }

           

        {

    }

    }



    void OnTriggerEnter(Collider other)

    {

        if (IsDestroyableTarget(other))

        {

            Destroy(GetTargetRoot(other));

            Destroy(gameObject);

        }

    }



    void OnCollisionEnter(Collision collision)

    {

        if (IsDestroyableTarget(collision.collider))

        {

            Destroy(GetTargetRoot(collision.collider));

            Destroy(gameObject);

        }

    }



    private bool IsDestroyableTarget(Collider other)

    {

        if (other == null) return false;



        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))

        {

            return true;

        }



        if (other.GetComponentInParent<Enemy>() != null || other.GetComponentInParent<Obstacle>() != null)

        {

            return true;

        }



        if (other.CompareTag("Vehicle") || other.GetComponentInParent<Vehicle>() != null)

        {

            return false;

        }



        return false;

    }

    private GameObject GetTargetRoot(Collider other)

    {

        if (other == null) return null;



        if (other.GetComponentInParent<Enemy>() != null)

        {

            return other.GetComponentInParent<Enemy>().gameObject;

        }



        if (other.GetComponentInParent<Obstacle>() != null)

        {

            return other.GetComponentInParent<Obstacle>().gameObject;

        }



        if (other.GetComponentInParent<Vehicle>() != null)

        {

            return other.GetComponentInParent<Vehicle>().gameObject;

        }



        return other.gameObject;

    }

}