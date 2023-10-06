using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RightHand))]
public class RightHandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        //RightHand rightHand = (RightHand)target;

        //if (rightHand.GetComponentInParent<MeshRenderer>()?.gameObject.GetComponentInParent<Character>().GetComponentInParent<Player>().currentWeapon != null && !rightHand.holdingWeapon) //if weapon gets dropped into hand from unity inspector, call to funct
        //{
        //    //EditorGUILayout.Space(); // Add some spacing
        //    rightHand.HoldWeapon(rightHand.GetComponentInParent<MeshRenderer>()?.gameObject.GetComponentInParent<Character>().GetComponentInParent<Player>().currentWeapon);
        //}
        //else Debug.Log("Weapon not found in hand.");

        EditorGUILayout.Space(); // Add some spacing

        //if (GUILayout.Button("Drop Weapon"))
        //{
        //    rightHand.DropWeapon();
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
