using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void SetPlayerName(string name)
    {
        PlayerPrefs.SetString("Name", name);
    }
}
