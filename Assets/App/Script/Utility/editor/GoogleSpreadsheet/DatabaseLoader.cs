using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Linq;
using Utility;
using System.Text.RegularExpressions;
using IG.Database;

/// <summary>
/// Organize gameobjects in the scene.
/// </summary>
public class DatabaseLoader : Object
{

    const string CSV_FOLDER = "Assets/StreamingAssets/ExternalDatabase/CSV";
    const string ASSETS_FOLDER = "Assets/Database";

    /// <summary>
    /// Main app instance.
    /// </summary>
	static void UnityDownloadGoogleSheet(Dictionary<string, string> url_clone)
    {

        if (url_clone.Count > 0)
        {
            KeyValuePair<string, string> firstItem = url_clone.First();

            WebRequest myRequest = WebRequest.Create(firstItem.Value);

            //store the response in myResponse 
            WebResponse myResponse = myRequest.GetResponse();

            //register I/O stream associated with myResponse
            Stream myStream = myResponse.GetResponseStream();

            //create StreamReader that reads characters one at a time
            StreamReader myReader = new StreamReader(myStream);

            string s = myReader.ReadToEnd();
            myReader.Close();//Close the reader and underlying stream

            File.WriteAllText(CSV_FOLDER + "/" + firstItem.Key + ".csv", s);
            url_clone.Remove(firstItem.Key);
            UnityDownloadGoogleSheet(url_clone);
            Debug.Log(firstItem.Key);

        }
        else
        {
            Debug.Log("Done");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Assets/App/Database/UpdateStatsAsset", false, 1)]
    static private void UpdateStatsAsset() {
        TaskHolder statsHolder = (TaskHolder)AssetDatabase.LoadAssetAtPath(ASSETS_FOLDER + "/[Task]Holder.asset", typeof(TaskHolder));

        if (statsHolder != null)
        {
            FileUtil.DeleteFileOrDirectory(ASSETS_FOLDER+ "/Asset");
            AssetDatabase.CreateFolder(ASSETS_FOLDER, "Asset");

            statsHolder.stpObjectHolder.Clear();
            CreateTaskStats(statsHolder);

            CreateCharacterStats();
        }
        else
        {
            Debug.LogError("[Stats]Holder.asset has not been created yet!");
        }

        EditorUtility.SetDirty(statsHolder);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static private void CreateTaskStats(TaskHolder statsHolder)
    {
        //TextAsset csvText = (TextAsset)AssetDatabase.LoadAssetAtPath(CSV_FOLDER + "/database - task.csv", typeof(TextAsset));
        string csvText = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/ExternalDatabase/CSV/database - task.csv");

        CSVFile csvFile = new CSVFile(csvText);

        AssetDatabase.CreateFolder(ASSETS_FOLDER + "/Asset", "Task");

        int csvCount = csvFile.length;
        for (int i = 0; i < csvCount; i++)
        {
            string id = csvFile.Get<string>(i, "ID");

            if (string.IsNullOrEmpty(id))
                continue;

            TaskStats c_prefab = ScriptableObjectUtility.CreateAsset<TaskStats>(ASSETS_FOLDER + "/Asset/Task/", "[TaskStats] " + id);
            EditorUtility.SetDirty(c_prefab);

            c_prefab.id = id;
            c_prefab.tag = csvFile.Get<string>(i, "Tag");
            c_prefab.label = csvFile.Get<string>(i, "Name");

            c_prefab.cost = csvFile.Get<string>(i, "Cost");
            c_prefab.effect = csvFile.Get<string>(i, "Effect");
            c_prefab.desc = csvFile.Get<string>(i, "Description");

            statsHolder.stpObjectHolder.Add(c_prefab);
        }
    }

    static private void CreateCharacterStats()
    {
        CharacterSCAssets characterAssets = (CharacterSCAssets)AssetDatabase.LoadAssetAtPath(ASSETS_FOLDER + "/[Character]Generator.asset", typeof(CharacterSCAssets));
        string firstNameText = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/ExternalDatabase/CSV/database - character first name.csv");
        string familyNameText = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/ExternalDatabase/CSV/database - character family name.csv");

        CSVFile firstNameCSV = new CSVFile(firstNameText);
        CSVFile familyNameCSV = new CSVFile(familyNameText);

        characterAssets.famaily_name_list.Clear();
        characterAssets.first_name_list.Clear();

        int csvCount = firstNameCSV.length;
        for (int i = 0; i < csvCount; i++)
        {
            UDataStruct uDataStruct = new UDataStruct();
            uDataStruct._id  = firstNameCSV.Get<string>(i, "ID");
            uDataStruct.variable_1 = firstNameCSV.Get<string>(i, "Name");
            uDataStruct.variable_2 = firstNameCSV.Get<string>(i, "Gender");

            characterAssets.first_name_list.Add(uDataStruct);
        }

        csvCount = familyNameCSV.length;
        for (int i = 0; i < csvCount; i++)
        {
            UDataStruct uDataStruct = new UDataStruct();
            uDataStruct._id = familyNameCSV.Get<string>(i, "ID");
            uDataStruct.variable_1 = familyNameCSV.Get<string>(i, "Name");

            characterAssets.famaily_name_list.Add(uDataStruct);
        }

        EditorUtility.SetDirty(characterAssets);
    }



    [MenuItem("Assets/App/Database/Reset", false, 0)]
    static public void Reset()
    {
        PlayerPrefs.DeleteAll();
        Caching.ClearCache();
    }

    [MenuItem("Assets/App/Database/DownloadGoogleSheet", false, 0)]
    static public void OnDatabaseDownload()
    {
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQrwT4H0ipTfj_PcMxq4hEWkt7rz6QGjWO5-nJ8iZv74kZStPgnUyV6lTfGLMW1NNRXRGmHAjpuxb0W/pub?gid=:id&single=true&output=csv";
        UnityDownloadGoogleSheet(new Dictionary<string, string> {
            { "database - task", Regex.Replace( url, ":id", "0")},
            { "database - character first name", Regex.Replace( url, ":id", "1242016170")},
            { "database - character family name", Regex.Replace( url, ":id", "742275494")}
        });
    }

}