using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    //public unitAttackController attackController;
    public int unitDamage;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the player
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            var unit = collision.gameObject.GetComponent<UnitSinglePlayer>();
            if (unit != null)
            {
                unit.TakeDamage(unitDamage);
            }
            
            DestroyProjectile();
        }

        // Check if the projectile hits the ground
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
