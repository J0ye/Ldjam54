using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    public GameObject pointStarPrefab;
    public GameObject waterSplashPrefab;
    public float range = 10f;
    public float attackCoolDown = 1f;

    public UnityEvent OnDeath = new UnityEvent();


    private bool attackOnCoolDown = false;

    private void Awake()
    {
        GameObject temp = Instantiate(waterSplashPrefab, transform.position, waterSplashPrefab.transform.rotation);
        Destroy(temp, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.INSTANCE.paused)
        {
            if (InRange().Count <= 0)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Attack();
            }
        }
    }

    protected List<GameObject> InRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);
        List<GameObject> ret = new List<GameObject>();
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))
            {
                ret.Add(hits[i].collider.gameObject);
            }
        }
        return ret;
    }

    protected void Attack()
    {
        if (!attackOnCoolDown)
        {
            if (InRange().Count > 0)
            {
                InRange()[0].GetComponent<Player>().DealDamage();
                attackOnCoolDown = true;
                Invoke(nameof(ResetCooldown), attackCoolDown);
            }
        }
    }

    protected override IEnumerator Die()
    {
        OnDeath.Invoke();
        Instantiate(pointStarPrefab, transform.position, pointStarPrefab.transform.rotation);
        return base.Die();
    }

    protected void MoveTowardsPlayer()
    {
        Vector3 dir = Player.instance.gameObject.transform.position - transform.position;
        dir = dir.normalized;

        transform.position += dir * speed * Time.deltaTime;
    }

    private void ResetCooldown()
    {
        attackOnCoolDown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
