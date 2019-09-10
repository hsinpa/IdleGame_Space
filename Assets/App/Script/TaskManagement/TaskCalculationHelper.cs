using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCalculationHelper
{


    public string PredictTaskOutput(List<TaskDataSlot> taskSlots) {
        string outString = "Effect Output\n";

        int taskLength = taskSlots.Count;
        Dictionary<string, int> recordEffect = new Dictionary<string, int>();
        Dictionary<string, int> costEffect = new Dictionary<string, int>();

        for (int i = 0; i < taskLength; i++) {
            //Effect
            recordEffect = CombineDictionary(recordEffect, ParseRawString(taskSlots[i].effect));

            //Cost
            costEffect = CombineDictionary(costEffect, ParseRawString(taskSlots[i].cost));
        }

        string effectString = GetDictPureString(recordEffect);
        string costString = GetDictPureString(costEffect);

        outString += effectString + "\n\n";
        outString += costString;

        return outString;
    }

    private string GetDictPureString(Dictionary<string, int> costDict) {        
        string output = "";
        foreach (KeyValuePair<string, int> keyPair in costDict)
        {
            output += keyPair.Key + " + " + keyPair.Value + "\n";
        }

        return output;
    }

    private Dictionary<string, int> CombineDictionary(Dictionary<string, int> dict1, Dictionary<string, int> dict2) {
        foreach (KeyValuePair<string, int> keyPair in dict2) {
            if (dict1.TryGetValue(keyPair.Key, out int numValue))
            {
                dict1[keyPair.Key] += keyPair.Value;
            }
            else {
                dict1.Add(keyPair.Key, keyPair.Value);
            }
        }
        return dict1;
    }

    private Dictionary<string, int> ParseRawString(string raw_string) {
        Dictionary<string, int> resultDict = new Dictionary<string, int>();
        string[] pairs = raw_string.Split(new string[] { "&" }, System.StringSplitOptions.None);
        int pairLength = pairs.Length;

        for (int i = 0; i < pairLength; i++)
        {
            string[] effectPair = pairs[i].Split(new string[] { ":" }, System.StringSplitOptions.None);
            if (effectPair.Length > 2 && !resultDict.ContainsKey(effectPair[0]) && int.TryParse(effectPair[1], out int parseNum)) {
                resultDict.Add(effectPair[0], parseNum);
            }
        }

        return resultDict;
     }
}
