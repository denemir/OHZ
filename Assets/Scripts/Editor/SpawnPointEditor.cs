using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnPoints))]
public class SpawnPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnPoints spawnPoint = (SpawnPoints)target;

        EditorGUILayout.Space(); // Add some spacing

        if (GUILayout.Button("Spawn"))
        {
            spawnPoint.Spawn();
        }
    }
}
