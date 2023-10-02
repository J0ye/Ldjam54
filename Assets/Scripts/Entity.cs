using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    public GameObject hitSoundPrefab;
    public int health = 2;
    public float speed = 50f;

    [Header("Animation Settings")]
    public int damageAnimationDuration;
    public float shakeStrength;

    protected SpriteRenderer sr;

    private void Awake()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void DealDamage()
    {
        health--;
        StartCoroutine(DamageAnimation());
        if (health <= 0)
        {
            // Death
            Instantiate(hitSoundPrefab, transform.position, Quaternion.identity);
            StartCoroutine(Die());
        }
    }

    protected virtual IEnumerator Die()
    {
        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }
    private IEnumerator DamageAnimation()
    {
        sr.DOColor(Color.red, damageAnimationDuration / 2);
        transform.DOShakeScale(damageAnimationDuration / 2, shakeStrength);
        yield return new WaitForSeconds(damageAnimationDuration / 2);
        sr.DOColor(Color.white, damageAnimationDuration / 2);
        yield return new WaitForSeconds(damageAnimationDuration / 2);
    }
}
