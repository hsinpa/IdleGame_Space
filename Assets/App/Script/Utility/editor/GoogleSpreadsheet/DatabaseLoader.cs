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

    const string DATABASE_FOLDER = "Assets/Database";

    /// <summary>
    /// Main app instance.
    /// </summary>
	static void UnityDownloadGoogleSheet(Dictionary<string, string> url_clone)
    {
        string path = "Assets/Database/CSV";

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

            File.WriteAllText(path + "/" + firstItem.Key + ".csv", s);
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
        TaskHolder statsHolder = (TaskHolder)AssetDatabase.LoadAssetAtPath(DATABASE_FOLDER + "/[Task]Holder.asset", typeof(TaskHolder));

        if (statsHolder != null)
        {
            FileUtil.DeleteFileOrDirectory(DATABASE_FOLDER + "/Asset");
            AssetDatabase.CreateFolder(DATABASE_FOLDER, "Asset");

            statsHolder.stpObjectHolder.Clear();
            CreateTaskStats(statsHolder);
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
        TextAsset csvText = (TextAsset)AssetDatabase.LoadAssetAtPath(DATABASE_FOLDER + "/CSV/database - task.csv", typeof(TextAsset));
        CSVFile csvFile = new CSVFile(csvText.text);

        AssetDatabase.CreateFolder(DATABASE_FOLDER + "/Asset", "Task");

        int csvCount = csvFile.length;
        for (int i = 0; i < csvCount; i++)
        {
            string id = csvFile.Get<string>(i, "ID");

            if (string.IsNullOrEmpty(id))
                continue;

            TaskStats c_prefab = ScriptableObjectUtility.CreateAsset<TaskStats>(DATABASE_FOLDER + "/Asset/Task/", "[TaskStats] " + id);
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
            { "database - task", Regex.Replace( url, ":id", "0")}
        });
    }

}