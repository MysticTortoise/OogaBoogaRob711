


using System;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static int levelsBeaten;
    
    public static void SaveGame()
    {
        PlayerPrefs.SetInt("levels", levelsBeaten);
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        levelsBeaten = PlayerPrefs.GetInt("levels");
    }

    private void Start()
    {
        LoadGame();
    }
}
