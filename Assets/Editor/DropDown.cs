using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PickUps))]
public class DropDown : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        PickUps script = (PickUps)target;

        GUIContent arraylist = new GUIContent("upgrades");
        script.ListIndex = EditorGUILayout.Popup(arraylist, script.ListIndex, script.upgrades);
    }
}

   

