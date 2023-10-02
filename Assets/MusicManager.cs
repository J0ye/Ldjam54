using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip track1;
    public AudioClip track2;
    public float fadeDuration = 2f;

    protected AudioSource audioSource;
    protected bool isChanging = false;
    protected bool isPlaying1 = true;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator SwitchTrack()
    {
        if(!isChanging)
        {
            float halfD = fadeDuration / 2;
            isChanging = true;
            if (isPlaying1)
            {
                audioSource.DOFade(0f, halfD);
                yield return new WaitForSeconds(halfD);
                audioSource.clip = track2;
                audioSource.DOFade(100f, halfD);
                isPlaying1 = false;
                yield return new WaitForSeconds(halfD);
            }
            else
            {
                audioSource.DOFade(0f, halfD);
                yield return new WaitForSeconds(halfD);
                audioSource.clip = track1;
                audioSource.DOFade(100f, halfD);
                isPlaying1 = true;
                yield return new WaitForSeconds(halfD);
            }
            isChanging = false;
        }
    }
}
