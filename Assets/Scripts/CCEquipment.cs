using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CCEquipment : Equipment
{
    public GameObject soundPrefab;
    public float moveAnimationDuration = 0.3f;

    protected Vector3 startpos = new Vector3();

    private void Start()
    {
        startpos = transform.localPosition;
    }

    protected override IEnumerator Attack(GameObject target)
    {
        StartCoroutine(base.Attack(target));
        Enemy e = target.GetComponent<Enemy>();
        onCooldown = true;
        transform.DOMove(target.transform.position, moveAnimationDuration);
        yield return new WaitForSeconds(moveAnimationDuration);
        transform.DOLocalMove(startpos, moveAnimationDuration);
        if (e != null)
        {
            DealDamage(e);
            Instantiate(soundPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(attackCooldown);
        }
        onCooldown = false;
    }

    protected override List<GameObject> GetEnemiesInRange()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(Player.instance.transform.position, range, Vector3.zero);
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
        if(Player.instance != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Player.instance.transform.position, range);
        }
    }
}

