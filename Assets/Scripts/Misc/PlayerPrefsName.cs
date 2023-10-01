using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsName : MonoBehaviour
{
    // Set the player name in PlayerPrefs
    public void SetPlayerName(string newName)
    {
        // Store the new name in PlayerPrefs using the key "Name"
        PlayerPrefs.SetString("Name", newName);
    }

    // Get the player name from PlayerPrefs
    public static string GetPlayerName()
    {
        if (RandomValues.INSTANCE() != null)
        {
            // If RandomValues.INSTANCE() is not null, return the player name stored in PlayerPrefs,
            // or a random string from RandomValues.INSTANCE() if the player name is not set
            return PlayerPrefs.GetString("Name", RandomValues.INSTANCE().GetRandomString());
        }

        // If RandomValues.INSTANCE() is null, return the player name stored in PlayerPrefs,
        // or the default value "No Name" if the player name is not set
        return PlayerPrefs.GetString("Name", "No Name");
    }
}
