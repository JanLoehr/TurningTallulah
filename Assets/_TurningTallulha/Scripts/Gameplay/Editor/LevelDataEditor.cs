using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        LevelData data = target as LevelData;

        if (GUILayout.Button("Refresh"))
        {
            data.CropLines = data.transform.GetComponentsInChildren<CropLineBehaviour>().Length;
        }
    }
}
