using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using Dan.Models;
using System;

public class ScoreLoader : MonoBehaviour
{
    public TextMeshProUGUI scoretext;
    [SerializeField] private TextMeshProUGUI[] _entryFields;
    public string scoreKey;

    protected LeaderboardReference leaderboardReference;

    private void Start()
    {
        try
        {
            LeaderboardCreator.GetLeaderboard(scoreKey, OnLeaderboardLoaded);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error with handeling online leaderboard: " + e);
        }
        scoretext.text = PlayerPrefs.GetInt("Score").ToString();
    }

    private void OnLeaderboardLoaded(Entry[] entries)
    {
        foreach (var entryField in _entryFields)
        {
            entryField.text = "";
        }

        for (int i = 0; i < entries.Length; i++)
        {
            _entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score}";
        }
    }
}
