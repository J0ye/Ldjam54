using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource secondSource;
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
            isChanging = true;
            if (isPlaying1)
            {
                audioSource.DOFade(0f, fadeDuration);
                secondSource.Play();
                secondSource.DOFade(100f, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
                isPlaying1 = false;
            }
            else
            {
                secondSource.DOFade(0f, fadeDuration);
                audioSource.Play();
                audioSource.DOFade(100f, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
                isPlaying1 = true;
            }
            isChanging = false;
        }
    }
}
