using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
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
        Collider other = collision.collider;
        if (IsDestroyableTarget(other))
        {
            Destroy(GetTargetRoot(other));
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

        return false;
    }

    private GameObject GetTargetRoot(Collider other)
    {
        if (other == null) return null;

        var enemyComp = other.GetComponentInParent<Enemy>();
        if (enemyComp != null) return enemyComp.gameObject;

        var obsComp = other.GetComponentInParent<Obstacle>();
        if (obsComp != null) return obsComp.gameObject;

        return other.gameObject;
    }
}
