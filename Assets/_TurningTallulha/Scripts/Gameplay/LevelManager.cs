using System;
using System.Collections;
using System.Collections.Generic;
using TurningTallulah.Database;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Current Level")]
    public int CurrentLevelIndex = 0;
    public GameObject CurrentLevelObject;

    [Header("Level Info")]
    public int LevelCropsCount;
    public int CropsReached;
    public int StarsCollected;

    [Header("Object Refs")]
    public GameStateManager GameSateManager;
    public CameraIntroController CamIntroController;

    public Slider ProgressSlider;

    public Transform Tallulah;

    [Header("Data")]
    public List<GameObject> Levels;

    private DbService _dbService;

    // Start is called before the first frame update
    void Start()
    {
        _dbService = GetComponent<DbService>();
    }

    public TurningTallulah.Database.LevelData GetLevelData(int id)
    {
        TurningTallulah.Database.LevelData data = _dbService?.GetLevelData(id);

        return data;
    }

    public void LoadContinueLevel()
    {
        CurrentLevelIndex = _dbService.GetHighestLevelIndex() + 1;

        if (CurrentLevelIndex > Levels.Count - 1)
        {
            CurrentLevelIndex = Levels.Count - 1;
        }

        ReloadLevel();
    }

    public void ReloadLevel()
    {
        LoadLevel(CurrentLevelIndex);
    }

    public void LoadNextLevel()
    {
        LoadLevel(CurrentLevelIndex + 1);
    }

    public void LoadLevel(string levelName)
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            Debug.Log(Levels[i].name);
            if (Levels[i].name == levelName)
            {
                LoadLevel(i);

                break;
            }
        }
    }

    public void LoadLevel(int index)
    {
        GameSateManager.SwitchGameState("Playing");

        CamIntroController.StartIntroAnimation();

        DestroyImmediate(CurrentLevelObject);

        CurrentLevelIndex = index;
        CurrentLevelObject = Instantiate(Levels[CurrentLevelIndex]);

        LevelCropsCount = CurrentLevelObject.GetComponent<LevelData>().CropLines;

        Tallulah.GetComponent<TallulhaControl>().Reset(true);

        ProgressSlider.value = 0;
        CropsReached = 0;
        StarsCollected = 0;
    }

    public void CropReached()
    {
        CropsReached++;

        ProgressSlider.value = (float)CropsReached / (float)LevelCropsCount;
    }

    public void StarCollected()
    {
        StarsCollected++;
    }

    public void LevelFinished()
    {
        LevelData data = CurrentLevelObject.GetComponent<LevelData>();

        _dbService.AddLevelData(new TurningTallulah.Database.LevelData()
        {
            Id = CurrentLevelIndex,
            Score = (int)((float)100 / LevelCropsCount * CropsReached),
            Stars = StarsCollected,
            Cluster = data.Cluster
        });
    }
}
