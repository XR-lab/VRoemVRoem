using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace XRLab.VRoem.Utility
{
    public class LampEmissionPrefabricator : EditorWindow
    {
        //private List<Material> ligthEmmisions = new List<Material>();

        [SerializeField] private List<Material> lightEmissions = new List<Material>();
        [SerializeField] int selected;
        [SerializeField] private string[] colors = new string[] { "Orange", "Magenta", "Purple", "Light Blue", "Lighter Blue", "Dark Blue", "Darker Blue", "Aqua", "Light Aqua", "Yellow", "Pink", "Dark Pink" };


        private GameObject lamp;


        [MenuItem("Window/Lamp Emission Prefabricator")]
        private static void StartPrefabricating()
        {
            LampEmissionPrefabricator window = (LampEmissionPrefabricator)EditorWindow.GetWindow(typeof(LampEmissionPrefabricator));
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            selected = EditorGUILayout.Popup("Color", selected, colors);
            if(GUILayout.Button("Start Prefabricating"))
            {
                FuseMaterialAndObject(lamp);
            }
            GUILayout.Space(20);
            if(GUILayout.Button("Close")) this.Close();
            
        }


        //This function will add the material on to the object, so that there will be no hard tweeking
        void FuseMaterialAndObject(GameObject lamp) 
        {
            GameObject fusedPrefab = new GameObject();
            Material selectedMaterial = lightEmissions[selected];


            //generates the path where it will get saved, and the nam it will get
            string localPath = "Assets/" + fusedPrefab.name + colors[selected] + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            //make prefab and put it in folder
            PrefabUtility.SaveAsPrefabAssetAndConnect(fusedPrefab, localPath, InteractionMode.UserAction);
        }

    }
}
