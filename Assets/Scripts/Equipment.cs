using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Equipment : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damage = 1;
    public float range = 10f;
    public float attackCooldown = 1f;

    [Header("Settings")]
    public Vector3 punchVector = Vector3.one;
    public float punchDuration = 0.1f;
    public float rotationOffset = 0f;

    protected SpriteRenderer sr;
    protected Tween punchTween;
    protected Vector3 startSize;
    protected bool onCooldown = false;
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startSize = transform.localScale;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GetEnemiesInRange().Count > 0 && !GameManager.INSTANCE.paused)
        {
            GameObject target = GetClosestEnemyInRange();
            if (!onCooldown) StartCoroutine(Attack(target));
            FaceTarget(target);
        }
    }

    protected virtual IEnumerator Attack(GameObject target)
    {
        if (punchTween != null) punchTween.Complete();
        transform.localScale = startSize;
        punchTween = transform.DOPunchScale(punchVector, punchDuration, 10, 0);
        yield return 0;
    }

    protected virtual void DealDamage(Enemy target)
    {
        for (int i = 0; i < damage; i++)
        {
            target.DealDamage();
        }
    }

    protected virtual void FaceTarget(GameObject target)
    {
        // Calculate the direction from the object to the target
        Vector2 directionToTarget = target.transform.position - transform.position;

        // Calculate the angle between the object and the target
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        // Add the offset rotation
        angle += rotationOffset;

        // Set the object's rotation to face the target
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public GameObject GetClosestEnemyInRange()
    {
        List<GameObject> enemies = GetEnemiesInRange();
        if (enemies.Count <= 0)
        {
            return null;
        }

        GameObject closest = enemies[0];
        foreach (GameObject e in GetEnemiesInRange())
        {
            if (Vector3.Distance(closest.transform.position, transform.position)
                > Vector3.Distance(e.transform.position, transform.position))
            {
                closest = e;
            }
        }
        return closest;
    }

    protected virtual List<GameObject> GetEnemiesInRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
        List<GameObject> enemies = new List<GameObject>();

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(hits[i].collider.gameObject);
            }
        }

        return enemies;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
