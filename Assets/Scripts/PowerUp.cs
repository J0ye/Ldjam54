using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {RangedWeapon, CCWeapon, RangeUpgrade, Health, IslandTileUpgrade }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public GameObject rangePrefab;
    public GameObject ccPrefab;
    public GameObject heartParticlePrefab;
    public GameObject crossParticlePrefab;
    public GameObject sandParticlePrefab;
    public bool doRandomizeType = true;
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
    }

    protected void RandomizeType()
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        int rand = 0;
        if(Player.instance.slots.Count <= 0)
        {
            // cannot have new weapons
            rand = UnityEngine.Random.Range(2, values.Length);
        }
        else
        {
            // can have new weapons
            rand = UnityEngine.Random.Range(0, values.Length);
        }
        type = (PowerUpType)values.GetValue(rand);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            UsePowerUp(type);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Use tzher power up
    /// </summary>
    /// <param name="powerUp"></param>
    void UsePowerUp(PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case PowerUpType.RangedWeapon:
                Player.instance.SetNewWeapon(rangePrefab);
                break;

            case PowerUpType.CCWeapon:
                Player.instance.SetNewWeapon(ccPrefab);
                break;

            case PowerUpType.RangeUpgrade:
                GameObject temp = Instantiate(crossParticlePrefab, transform.position, crossParticlePrefab.transform.rotation);
                Destroy(temp, 5f);
                Player.instance.range += 0.3f;
                break;

            case PowerUpType.Health:
                Player.instance.health++;
                GameObject temp2 = Instantiate(heartParticlePrefab, transform.position, heartParticlePrefab.transform.rotation);
                Destroy(temp2, 5f);
                break;

            case PowerUpType.IslandTileUpgrade:
                Player.instance.maxTileCount++;
                GameObject temp3 = Instantiate(heartParticlePrefab, transform.position, heartParticlePrefab.transform.rotation);
                Destroy(temp3, 5f);
                break;

            default:
                Debug.Log("Unknown power bestowed, where it leads, who knows!");
                break;
        }
    }
}
