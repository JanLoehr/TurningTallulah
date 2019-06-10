using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameStates CurrentState;

    [Header("Object Refs")]
    public GameObject Tallulah;

    public LevelManager LevelManager;

    [Header("State Object Refs")]
    public List<GameObject> LaunchMenuItems;

    public List<GameObject> LevelSelectionItems;

    public List<GameObject> PauseMenuItems;

    public List<GameObject> GameItems;

    public List<GameObject> FinishItems;

    private Vector3 _tallulahStartPos;
    private Quaternion _tallulahStartRotation;

    // Start is called before the first frame update
    void Start()
    {
        _tallulahStartPos = Tallulah.transform.position;
        _tallulahStartRotation = Tallulah.transform.rotation;
        
        SwitchGameState("MainMenu");
    }

    public void SwitchGameState(string state)
    {
        if (Enum.TryParse<GameStates>(state, out GameStates s) && CurrentState != s)
        {
            LaunchMenuItems.ForEach(i => i.SetActive(false));
            PauseMenuItems.ForEach(i => i.SetActive(false));
            FinishItems.ForEach(i => i.SetActive(false));
            GameItems.ForEach(i => i.SetActive(false));
            LevelSelectionItems.ForEach(i => i.SetActive(false));

            switch (s)
            {
                case GameStates.MainMenu:

                    LaunchMenuItems.ForEach(i => i.SetActive(true));

                    Tallulah.GetComponent<TallulhaControl>().enabled = false;

                    Tallulah.transform.position = _tallulahStartPos;
                    Tallulah.transform.rotation = _tallulahStartRotation;
                    Tallulah.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Tallulah.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    Tallulah.GetComponent<TallulhaControl>().VolumeDown();

                    break;

                case GameStates.LevelSelection:

                    LevelSelectionItems.ForEach(i => i.SetActive(true));

                    Tallulah.GetComponent<TallulhaControl>().enabled = false;

                    break;

                case GameStates.Playing:

                    GameItems.ForEach(i => i.SetActive(true));

                    Tallulah.GetComponent<TallulhaControl>().SetPlaying();

                    break;

                case GameStates.Pause:

                    PauseMenuItems.ForEach(i => i.SetActive(true));

                    Tallulah.GetComponent<TallulhaControl>().enabled = false;

                    Tallulah.GetComponent<TallulhaControl>().VolumeDown();

                    break;

                case GameStates.Finish:

                    FinishItems.ForEach(i => i.SetActive(true));

                    FinishItems.ForEach(i => i.GetComponent<FinishScreenController>()?.ShowStats((int)((float)100 / LevelManager.LevelCropsCount * LevelManager.CropsReached), LevelManager.StarsCollected));

                    LevelManager.LevelFinished();
                    break;
            }

            CurrentState = s;
        }
    }
}

public enum GameStates
{
    None,
    MainMenu,
    LevelSelection,
    Playing,
    Pause,
    Finish
}
