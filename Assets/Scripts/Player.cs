using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player instance;

    public List<Transform> slots = new List<Transform>();
    public GameObject islandPartPrefab;
    public Collider2D gameBounds;
    public int score = 0;
    public float speed = 0.1f;
    public float range = 0f;
    public float directionIncrease = 1f;
    public int maxTileCount = 4;
    public int health = 4;
    [Header("Animation Settings")]
    public Vector3 shakeStrength = Vector3.one;
    public float damageAnimationDuration = 1f;
    public TextMeshProUGUI sandCounterText;
    public TextMeshProUGUI scoreText;

    private SpriteRenderer sr;
    private Vector3 moveVector = new Vector3();
    private Vector3 inputVectorRaw = new Vector3();
    private Vector3 inputVectorIncreased = new Vector3();
    private Vector3 positionCheckOffset = new Vector3();
    private bool safeFrame = false;
    private int currentTileCount = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerPrefs.SetInt("Score", score);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.INSTANCE.paused)
        {
            positionCheckOffset = new Vector3(transform.position.x, sr.bounds.min.y, 0);
            SetInput();
        }

        UpdateSandCounter();
    }

    public void DealDamage()
    {
        if(!safeFrame)
        {
            health--;
            StartCoroutine(DamageAnimation());
            if (health <= 0)
            {
                GameManager.INSTANCE.paused = true;
                GameManager.INSTANCE.LoadScore(2f);
                Destroy(gameObject);
                //Game Over
            }
        }
    }

    public void IncreaseScore(int val)
    {
        score += val;
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("Score", score);
    }

    public void SetNewWeapon(GameObject newEquip)
    {
        if(slots.Count > 0)
        {
            Vector3 spot = slots[0].position;
            slots.RemoveAt(0);

            GameObject newWeapon = Instantiate(newEquip, spot, Quaternion.identity);
            newWeapon.transform.parent = transform;
            newWeapon.GetComponent<Equipment>().range += range;
        }
    }

    public void UpgradeRange(float val)
    {
        range += val;
        Equipment e;

        foreach(Transform child in transform)
        {
            if(child.gameObject.TryGetComponent<Equipment>(out e))
            {
                e.range += val;
            }
        }
    }

    private void FixedUpdate()
    {
        moveVector = inputVectorRaw * speed * Time.deltaTime;
        if (inputVectorRaw != Vector3.zero 
            && !GameManager.INSTANCE.paused
            && gameBounds.OverlapPoint(positionCheckOffset + moveVector))
        {
            if (CheckForLand(positionCheckOffset + moveVector))
            {
                transform.position += moveVector;
            }
            else if (currentTileCount < maxTileCount)
            {
                SpawnIslandPart();
            }
            else if(CheckForLand(positionCheckOffset + new Vector3(moveVector.x, 0, 0)))
            {
                transform.position += new Vector3(moveVector.x, 0, 0);
            }
            else if (CheckForLand(positionCheckOffset + new Vector3(0, moveVector.y, 0)))
            {
                transform.position += new Vector3(0, moveVector.y, 0);
            }
        }

        inputVectorRaw = new Vector3();
    }

    private void SetInput()
    {
        float vRaw = Input.GetAxisRaw("Vertical");
        float hRaw = Input.GetAxisRaw("Horizontal");
        inputVectorRaw = new Vector3(hRaw, vRaw, 0);
        inputVectorIncreased = new Vector3(inputVectorRaw.x * directionIncrease, 
                                            inputVectorRaw.y * directionIncrease, 0);

        if(hRaw < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void SpawnIslandPart()
    {
        IslandPart currenIslandPart = GetClosestIslandPart();
        Vector3 freeSpace = currenIslandPart.GetClosestFreeSpaceTo(transform.position + inputVectorIncreased);
        Vector3 inputVectorOnlyX = new Vector3(inputVectorIncreased.x, 0, 0); 
        Vector3 inputVectorOnlyY = new Vector3(0, inputVectorIncreased.y, 0);
        Vector3 freeSpaceXAlgined = currenIslandPart.GetClosestFreeSpaceTo(transform.position + inputVectorOnlyX);
        Vector3 freeSpaceYAlgined = currenIslandPart.GetClosestFreeSpaceTo(transform.position + inputVectorOnlyY);

        if (!IslandPart.GetIsOccupied(freeSpace))
        {
            CreateIslandPart(freeSpace);
        }
        else if(!IslandPart.GetIsOccupied(freeSpaceXAlgined))
        {
            CreateIslandPart(freeSpaceXAlgined);
        }
        else if(!IslandPart.GetIsOccupied(freeSpaceYAlgined))
        {
            CreateIslandPart(freeSpaceYAlgined);
        }
        else
        {
            Debug.LogWarning("No space to spawn new island");
        }
        //GetClosestIslandPart().GetClosestFreeSpaceTo(transform.position);
    }

    private void CreateIslandPart(Vector3 space)
    {
        GameObject newPart = Instantiate(islandPartPrefab);
        newPart.transform.position = space;
        currentTileCount++;
    }

    private bool CheckForLand(Vector3 position)
    {
        bool ret = false;
        foreach(IslandPart ip in IslandPart.island)
        {
            if(ip.GetIsOnLand(position))
            {
                ret = true;
            }
        }
        return ret;
    }

    private IslandPart GetClosestIslandPart()
    {
        IslandPart ret = IslandPart.island[0];
        foreach (IslandPart ip in IslandPart.island)
        {
            if (ret.GetDistanceFromCenterTo(positionCheckOffset) > ip.GetDistanceFromCenterTo(positionCheckOffset))
            {
                ret = ip;
            }
        }

        return ret;
    }

    private IEnumerator DamageAnimation()
    {
        safeFrame = true;
        sr.DOColor(Color.red, damageAnimationDuration / 2);
        transform.DOShakeScale(damageAnimationDuration / 2, shakeStrength);
        yield return new WaitForSeconds(damageAnimationDuration / 2);
        sr.DOColor(Color.white, damageAnimationDuration / 2);
        yield return new WaitForSeconds(damageAnimationDuration / 2);
        safeFrame = false;
    }

    private void UpdateSandCounter()
    {
        int val = maxTileCount - currentTileCount;

        sandCounterText.text = val.ToString();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position 
            + new Vector3(inputVectorRaw.x * directionIncrease, inputVectorRaw.y * directionIncrease, 0), 0.1f);
        Gizmos.DrawLine(positionCheckOffset, positionCheckOffset + new Vector3(inputVectorRaw.x*5, 0, 0));
        Gizmos.DrawLine(positionCheckOffset, positionCheckOffset + new Vector3(0, inputVectorRaw.y*5, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(positionCheckOffset + moveVector, 0.15f);
    }
}
