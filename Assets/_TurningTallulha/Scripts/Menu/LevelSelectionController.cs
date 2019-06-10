using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController : MonoBehaviour
{
    public LevelManager LevelManager;

    public GameObject LevelItem;

    public Transform ItemsGrid;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemsGrid.childCount; i++)
        {
            Destroy(ItemsGrid.GetChild(i).gameObject);
        }

        bool enabled = true;
        for (int i = 0; i < LevelManager.Levels.Count; i++)
        {
            LevelSelectionItem item = Instantiate(LevelItem, ItemsGrid).GetComponent<LevelSelectionItem>();
            TurningTallulah.Database.LevelData data = LevelManager.GetLevelData(i);

            item.ButtonClicked.AddListener(LevelItemClicked);
            item.Init(LevelManager.Levels[i].name, data, enabled);

            enabled = data != null;
        }
    }

    private void OnEnable()
    {
        bool enabled = true;
        for (int i = 0; i < LevelManager.Levels.Count; i++)
        {
            LevelSelectionItem item = ItemsGrid.GetChild(i).GetComponent<LevelSelectionItem>();
            TurningTallulah.Database.LevelData data = LevelManager.GetLevelData(i);

            item.Init(LevelManager.Levels[i].name, data, enabled);

            enabled = data != null;
        }
    }

    private void LevelItemClicked(string levelName)
    {
        LevelManager.LoadLevel(levelName);
    }
}
