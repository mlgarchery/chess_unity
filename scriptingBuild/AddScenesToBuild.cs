// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// public class Add_Scenes : EditorWindow
// {
//     [MenuItem ("MyTools/AddScenesToBuild")]
 
 
 
//     static void Init ()
//     {
//         List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
//         List<string> SceneList =  new List<string> ();
//         string MainFolder   = "Assets/";
       
 
//         DirectoryInfo d = new DirectoryInfo(@MainFolder);
//         FileInfo[] Files = d.GetFiles("*.unity"); //Getting unity files
       
//         foreach(FileInfo file in Files )
//         {
//             Debug.Log ("file name:" + file.Name);
//             SceneList.Add(file.Name);
//         }
       
       
     
     
//         int i = 0;
       
     
//         for (i = 0; i < SceneList.Count; i ++)
//         {
//             string scenePath = MainFolder + "/" + SceneList[i];
//             Debug.Log ("i = " + i);
//             Debug.Log("scene path:" + scenePath);
//             editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
           
         
//         }
     
//         EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
//     }
 
// }