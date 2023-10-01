using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Equipment
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy target;
        if(collision.gameObject.TryGetComponent<Enemy>(out target))
        {
            DealDamage(target);
            Destroy(gameObject);
        }
    }
}
