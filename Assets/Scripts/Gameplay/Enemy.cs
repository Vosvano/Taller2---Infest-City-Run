using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // velocidad demovimiento hacia el jugador
    public float moveDirectionX = -1f; // -1 = corre hacia -X
    public float laneOffset = 2f; // distancia entre carriles en Z
    [Range(0, 2)]
    public int laneIndex = 1;
    public float obstacleCheckBuffer = 0.05f;

    private float targetLaneZ;
    private Collider enemyCollider;
    private Rigidbody enemyRigidbody;
    private bool isBlocked;

    void Start()
    {
        enemyCollider = GetComponent<Collider>();
        enemyRigidbody = GetComponent<Rigidbody>();

        if (enemyRigidbody != null)
        {
            enemyRigidbody.useGravity = false;
            enemyRigidbody.isKinematic = true;
            enemyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        FaceOppositeToPlayer();

        targetLaneZ = SnapToLane(transform.position.z);
        laneIndex = LaneIndexFromZ(targetLaneZ);

        Vector3 position = transform.position;
        position.z = targetLaneZ;
        transform.position = position;
    }

    void Update()
    {
        if (isBlocked) return;

        float moveDistance = speed * Time.deltaTime;
        if (WillHitObstacle(moveDistance))
        {
            isBlocked = true;
            return;
        }

        // Corre hacia -X sin abandonar el carril
        Vector3 position = transform.position;
        position.x += moveDirectionX * moveDistance;
        position.z = targetLaneZ;
        transform.position = position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsProjectile(other))
        {
            // Primero reproducimos el sonido del golpe
            AudioManager.Instance.PlayEnemyHit();

            // Luego destruimos los objetos físicos
            Destroy(GetProjectileRoot(other));
            Destroy(gameObject);
            return;
        }
        else if (IsBlockingObstacle(other))
        {
            isBlocked = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsProjectile(collision.collider))
        {
            // Primero reproducimos el sonido del golpe
            AudioManager.Instance.PlayEnemyHit();

            // Luego destruimos los objetos físicos
            Destroy(GetProjectileRoot(collision.collider));
            Destroy(gameObject);
            return;
        }

        if (IsBlockingObstacle(collision.collider))
        {
            isBlocked = true;
        }
    }

    private bool WillHitObstacle(float moveDistance)
    {
        if (enemyCollider == null) return false;

        Bounds bounds = enemyCollider.bounds;
        Vector3 origin = bounds.center;
        Vector3 direction = moveDirectionX < 0f ? Vector3.left : Vector3.right;
        float rayDistance = moveDistance + bounds.extents.x + obstacleCheckBuffer;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            return IsBlockingObstacle(hit.collider);
        }

        return false;
    }

    private bool IsBlockingObstacle(Collider other)
    {
        if (other == null) return false;

        return other.CompareTag("Obstacle") || other.GetComponent<Obstacle>() != null;
    }

    private bool IsProjectile(Collider other)
    {
        if (other == null) return false;

        if (other.CompareTag("Projectile"))
        {
            return true;
        }

        Transform root = other.transform.root;
        return root != null && root.CompareTag("Projectile");
    }

    private GameObject GetProjectileRoot(Collider other)
    {
        if (other == null) return null;

        Transform root = other.transform.root;
        if (root != null && root.CompareTag("Projectile"))
        {
            return root.gameObject;
        }

        return other.gameObject;
    }
    // Hace que el enemigo mire en la dirección opuesta a la del jugador
    private void FaceOppositeToPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;

        Vector3 direction = -playerObject.transform.forward;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }
    // Ajusta la posición Z para que el zombie quede exactamente en un carril
    private float SnapToLane(float worldZ)
    {
        return Mathf.Round(worldZ / laneOffset) * laneOffset;
    }

    private int LaneIndexFromZ(float worldZ)
    {
        return Mathf.Clamp(Mathf.RoundToInt((worldZ / laneOffset) + 1f), 0, 2);
    }
}
