using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlaceholderFromPlayerPrefs : MonoBehaviour
{
    public TMP_Text textfield;

    // Start is called before the first frame update
    void Start()
    {
        textfield.text = PlayerPrefs.GetString("Name", "Enter Name");
    }
}
