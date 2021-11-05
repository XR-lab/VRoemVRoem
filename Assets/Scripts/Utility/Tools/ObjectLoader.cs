using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace XRLab.VRoem.Utility
{
    public enum PolyType
    {
        Auto,
        Obstakel,
        Gebouw

    };


    public class ObjectLoader : EditorWindow
    {
        public List<string> possibleObjects = new List<string> { "Appartement", "Politie auto", "speler auto", "tiny shop", "modern shop", "weg", "vuilnisbak",
        "pion", "spijkermat", "wolkenkrabber","lantaarnpaal", "reclamebord_1", "reclamebord_2", "reclamebord_3", "verkeersbord_1", "verkeersbord_2", "verkeersbord_3",
        "verkeersbord_4", "verkeersbord_5", "verkeersbord_6", "verkeersbord_7", "verkeersbord_8"};

        private string[] objectTypes = new string[] { "Cars", "Obstacles", "Environment" };
        public int objectDropDownIndex = 0;
        public int objectTypeDropDownIndex = 0;

        private bool enablePrefabLoadButton;
        public Object source;

        private ChunkCreater chunkCreaterInstance;
        public Dictionary<int, bool> objectExistences = new Dictionary<int, bool>();

        public static ObjectLoader instance = null;

        [Header("Polycount variables")]
        private bool selectionChanged = true;
        private int totalMeshes = 0;
        private int totalVertices = 0;
        private int totalTris = 0;
        private Dictionary<int, int> topList = new Dictionary<int, int>();
        private IOrderedEnumerable<KeyValuePair<int, int>> sortedTopList;
        private MeshFilter[] meshes;

        private const float triangulationNumber = 3f;
        private float verts = 0;
        private int maxCarsPolys = 1500;
        private int maxObstaclesPolys = 300;
        private int maxEnvironmentPolys = 2000;


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);

            chunkCreaterInstance = FindObjectOfType<ChunkCreater>();

            //add objects to dictionary according to list
            for (int i = 0; i < possibleObjects.Count; i++)
            {
                objectExistences.Add(i, false);
            }

        }

        [MenuItem("Tools/Object Loader")]
        private static void Init()
        {
            EditorWindow window = GetWindow(typeof(ObjectLoader));
            window.Show();
            window.maxSize = new Vector2(480f, 180f);
            window.minSize = window.maxSize;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Object Validation");
            source = EditorGUILayout.ObjectField(source, typeof(GameObject), true);
            objectTypeDropDownIndex = EditorGUILayout.Popup(objectTypeDropDownIndex, objectTypes);
            if (GUILayout.Button("Check for polycount"))
            {
                CheckForObjectPolyCount();
            }
            GUILayout.TextField(verts.ToString());
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(enablePrefabLoadButton == false);
            
            EditorGUILayout.LabelField("Please select in de dropdown menu below, which object you want to add");
            objectDropDownIndex = EditorGUILayout.Popup(objectDropDownIndex, possibleObjects.ToArray());
            if (GUILayout.Button("Add prefab"))
            {

                CheckForObjectExistence();
            }

            EditorGUI.EndDisabledGroup();


        }


        void CheckForObjectExistence()
        {
            bool value;
            bool existenceValue = objectExistences.TryGetValue(objectDropDownIndex, out value);

            //if already in existence replace model
            if (existenceValue)
            {
                chunkCreaterInstance.modelsToLoad.RemoveAt(objectDropDownIndex);
                chunkCreaterInstance.modelsToLoad.Insert(objectDropDownIndex, (GameObject)source);

            }
            //if not in existence add to list
            else
            {
                chunkCreaterInstance.modelsToLoad.Insert(objectDropDownIndex, (GameObject)source);
                objectExistences[objectDropDownIndex] = true;
            }





        }

        void CheckForObjectPolyCount()
        {
            var selection = (GameObject)source;
            
            if (selection != null)
            {
                //EditorGUILayout.LabelField("No object found, please drag an object to the field");
                if (selectionChanged)
                {
                    selectionChanged = false;
                    topList.Clear();

                    totalMeshes = 0;
                    totalVertices = 0;
                    totalTris = 0;

                    meshes = selection.GetComponentsInChildren<MeshFilter>();
                    for (int i = 0, length = meshes.Length; i < length; i++) 
                    {
                        verts = meshes[i].sharedMesh.vertexCount;
                        totalVertices += (int)verts;
                        totalTris += meshes[i].sharedMesh.triangles.Length / 3;
                        totalMeshes++;
                        topList.Add(i, (int)verts);

                    }

                     verts = (verts / triangulationNumber);

                    sortedTopList = topList.OrderByDescending(x => x.Value);
                }
            }

            switch (objectTypeDropDownIndex)
            {
                case 0:
                    //Cars
                    if(verts <= maxCarsPolys)
                    {
                        enablePrefabLoadButton = true;
                    }
                    break;

                case 1:
                    //Obstacles
                    if (verts <= maxObstaclesPolys)
                    {
                        enablePrefabLoadButton = true;
                    }
                    break;

                case 2:
                    //Environment
                    if (verts <= maxEnvironmentPolys)
                    {
                        enablePrefabLoadButton = true;
                    }
                    break;
            }
            
         
        }


    }
}
