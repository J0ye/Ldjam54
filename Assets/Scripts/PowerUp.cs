using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PowerUpType {RangedWeapon, CCWeapon, RangeUpgrade, Health, IslandTileUpgrade, Points }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject rangePrefab;
    public GameObject ccPrefab;
    public GameObject heartParticlePrefab;
    public GameObject crossParticlePrefab;
    public GameObject sandParticlePrefab;
    public float flightDuration = 1f;
    public float pickUpRange = 1f;
    public bool doRandomizeType = true;
    [Header("Animation")]
    public Gradient colorDanceGrad = new Gradient();
    public bool doColorAnim = false;
    public float duration = 1f;

    protected SpriteRenderer sr;
    protected Sequence colorDance;

    // Start is called before the first frame update
    void Awake()
    {
        if(doRandomizeType)
        {
            RandomizeType();

            if (type == PowerUpType.CCWeapon || type == PowerUpType.RangedWeapon)
            {
                RandomizeType();
            }
        }

        sr = GetComponent<SpriteRenderer>();

        if (doColorAnim) colorDance = sr.DOGradientColor(colorDanceGrad, duration);
    }

    private void Update()
    {
        if(!colorDance.IsActive() && doColorAnim)
        {
            colorDance = sr.DOGradientColor(colorDanceGrad, duration);
        }
    }

    public void RandomizeWeaponType()
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        int rand = 0;
        if (Player.instance.slots.Count <= 0)
        {
            // cannot have new weapons
            rand = UnityEngine.Random.Range(2, values.Length);
        }
        else
        {
            // can have new weapons
            rand = UnityEngine.Random.Range(0, 2);
        }
        type = (PowerUpType)values.GetValue(rand);
    }

    public void RandomizeType()
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        int rand = 0;
        if(Player.instance.slots.Count <= 0)
        {
            // cannot have new weapons
            rand = UnityEngine.Random.Range(2, values.Length-1);
        }
        else
        {
            // can have new weapons
            rand = UnityEngine.Random.Range(0, values.Length-1);
        }
        type = (PowerUpType)values.GetValue(rand);
    }

    private IEnumerator FlyTo(Vector3 target)
    {
        transform.DOMove(target, flightDuration);
        yield return new WaitForSeconds(flightDuration);
        UsePowerUp(type);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            StartCoroutine(FlyTo(collision.gameObject.transform.position));
            GetComponent<Collider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Use tzher power up
    /// </summary>
    /// <param name="powerUp"></param>
    void UsePowerUp(PowerUpType powerUp)
    {
        Player.instance.IncreaseScore(5);
        switch (powerUp)
        {
            case PowerUpType.RangedWeapon:
                Player.instance.SetNewWeapon(rangePrefab);
                LogCreator.instance.AddLog("New Ranged Weapon");
                break;

            case PowerUpType.CCWeapon:
                Player.instance.SetNewWeapon(ccPrefab);
                LogCreator.instance.AddLog("New CC Weapon");
                break;

            case PowerUpType.RangeUpgrade:
                GameObject temp = Instantiate(crossParticlePrefab, transform.position, crossParticlePrefab.transform.rotation);
                Destroy(temp, 5f);
                Player.instance.range += 0.3f;
                LogCreator.instance.AddLog("Range Upgrade");
                break;

            case PowerUpType.Health:
                Player.instance.health++;
                GameObject temp2 = Instantiate(heartParticlePrefab, transform.position, heartParticlePrefab.transform.rotation);
                Destroy(temp2, 5f);
                LogCreator.instance.AddLog("Heald and Health Upgrade");
                break;

            case PowerUpType.IslandTileUpgrade:
                Player.instance.maxTileCount++;
                GameObject temp3 = Instantiate(sandParticlePrefab, transform.position, sandParticlePrefab.transform.rotation);
                Destroy(temp3, 5f);
                LogCreator.instance.AddLog("More Sand");
                break;

            case PowerUpType.Points:
                Player.instance.IncreaseScore(15);
                LogCreator.instance.AddLog("Points");
                break;

            default:
                Debug.Log("Unknown power bestowed, where it leads, who knows!");
                break;
        }
    }
}
