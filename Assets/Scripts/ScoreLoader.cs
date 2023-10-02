using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreLoader : MonoBehaviour
{
    public TextMeshProUGUI scoretext;

    private void Awake()
    {
        scoretext.text = PlayerPrefs.GetInt("Score").ToString();
    }
}
