using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSelectionController))]
public class LevelSelectionControllerEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Build Menu"))
        {
            BuildMenu();
        }
    }

    private void BuildMenu()
    {
        LevelSelectionController controller = target as LevelSelectionController;

        for (int i = controller.ItemsGrid.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(controller.ItemsGrid.GetChild(i).gameObject);
        }

        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/_TurningTallulha/Levels");

        for (int i = 0; i < fileEntries.Length; i++)
        {
            if (fileEntries[i].EndsWith("meta"))
            {
                continue;
            }

            LevelSelectionItem item = (PrefabUtility.InstantiatePrefab(controller.LevelItem) as GameObject).GetComponent<LevelSelectionItem>();
            item.transform.SetParent(controller.ItemsGrid, false);

            item.Init(Path.GetFileName(fileEntries[i].Split('.')[0]), null, i < 4);
        }
    }
}
