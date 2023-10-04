using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEquipment : Equipment
{
    public GameObject projectilePrefab;
    public GameObject soundPrefab;
    [Tooltip("X for min x spread. Y for max x spread. Z for min y spread. W for max Y spread.")]
    public Vector4 spread = Vector4.one;
    public float projectileSpeed = 50f;

    protected override IEnumerator Attack(GameObject target)
    {
        onCooldown = true;
        StartCoroutine(base.Attack(target));
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Instantiate(soundPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, range);
        Vector3 dir = (target.transform.position + GetRandom2DSpread()) - bullet.transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(dir * projectileSpeed);
        bullet.GetComponent<Equipment>().damage = damage;
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

    protected Vector3 GetRandom2DSpread()
    {
        float randX = UnityEngine.Random.Range(spread.x, spread.y);
        float randY = UnityEngine.Random.Range(spread.z, spread.w);

        return new Vector3(randX, randY, 0f);
    }
}
