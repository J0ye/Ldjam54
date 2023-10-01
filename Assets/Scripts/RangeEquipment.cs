using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEquipment : Equipment
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 50f;

    protected override IEnumerator Attack(GameObject target)
    {
        onCooldown = true;
        StartCoroutine(base.Attack(target));
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Destroy(bullet, range);
        Vector3 dir = target.transform.position - bullet.transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * projectileSpeed);
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }

    protected override void FaceTarget(GameObject target)
    {
        base.FaceTarget(target);
        Vector3 eulers = transform.rotation.eulerAngles;
        if(eulers.z >= 91 && eulers.z <= 270)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }
}
