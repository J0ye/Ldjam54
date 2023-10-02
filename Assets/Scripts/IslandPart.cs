using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IslandPart : MonoBehaviour
{
    public static List<IslandPart> island = new List<IslandPart>();

    public static bool GetIsOccupied(Vector3 target)
    {
        foreach(IslandPart sp in island)
        {
            if(sp.transform.position == target)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject soundPrefab;
    public ParticleSystem bubble;
    public ParticleSystem dirt;
    public Color animationColor = Color.black;
    public Color fadeColor = Color.black;
    public Vector2 existenceDuration = new Vector2(0, 2);
    [Range(0f, 2f)]
    public float colorAnimationDuration = 1f;
    public float fadeAnimationDuration = 1f;
    public bool showContact = false;
    public bool showSpaces = false;

    private Collider2D col;
    private SpriteRenderer sr;
    private Tween colorTween;
    private bool contactToPlayer = false;

    // Start is called before the first frame update
    void Awake()
    {
        island.Add(this);
        if(!TryGetComponent<Collider2D>(out col))
        {
            Debug.Log("Warning: No Collider2D on " + gameObject.name);
            Destroy(this);
        }

        sr = GetComponent<SpriteRenderer>();
        float rand = UnityEngine.Random.Range(existenceDuration.x, existenceDuration.y);
        StartCoroutine(WashAway(rand));
        dirt.Play();
    }

    public IEnumerator WashAway(float timeUntilWash)
    {
        sr.DOColor(fadeColor, timeUntilWash);
        yield return new WaitForSeconds(timeUntilWash);
        print("States for wash away: " + contactToPlayer + " and " + GameManager.INSTANCE.paused);
        yield return new WaitWhile(() => contactToPlayer || GameManager.INSTANCE.paused);
        if(island.Contains(this))
        {
            bubble.Play();
            island.Remove(this);
            Player.instance.maxTileCount++;
        }
        transform.DOScale(Vector3.zero, fadeAnimationDuration);
        Instantiate(soundPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(fadeAnimationDuration);
        Destroy(gameObject);
    }

    public bool GetIsOnLand(Vector3 position)
    {
        return col.OverlapPoint(new Vector2(position.x, position.y));
    }

    public float GetDistanceFromCenterTo(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    public Vector3 GetClosestFreeSpaceTo(Vector3 target)
    {
        List<Vector3> spaces = GetSpaces();
        if(spaces.Count > 0)
        {
            Vector3 ret = spaces[0];
            foreach (Vector3 pos in GetSpaces())
            {
                if (Vector3.Distance(ret, target) > Vector3.Distance(pos, target))
                {
                    ret = pos;
                }
            }
            return ret;
        }
        Debug.LogWarning("No spaces in list");
        return Vector3.zero;
    }

    public bool GetConnectedToPlayer()
    {
        return contactToPlayer;
    }

    private List<Vector3> GetSpaces()
    {
        List<Vector3> spaces = new List<Vector3>();
        if(col == null)
        {
            if (!TryGetComponent<Collider2D>(out col))
            {
                Debug.Log("Warning: No Collider2D on " + gameObject.name);
                Destroy(this);
            }

        }
        int size = (int)col.bounds.size.x;

        for (int i = -size; i <= size; i += size)
        {
            for (int j = -size; j <= size; j += size)
            {
                if (/*(i == 0 || j == 0) &&*/ /*(i != j)*/
                    i != 0 || j != 0)
                {
                    Vector3 newSpace = new Vector3(i, j, 0) + transform.position;
                    spaces.Add(newSpace);
                }
            }
        }

        return spaces;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(showContact) AnimateColor(animationColor);
            contactToPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (showContact) AnimateColor(Color.white);
            contactToPlayer = false;
        }
    }

    private void AnimateColor(Color target)
    {
        if (colorTween != null)
        {
            colorTween.Complete();
        }
        colorTween = sr.DOColor(target, colorAnimationDuration);
    }

    private void OnDrawGizmos()
    {
        if(showSpaces)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 s in GetSpaces())
            {
                Gizmos.DrawSphere(s, 0.05f);
            }
        }
    }
}
