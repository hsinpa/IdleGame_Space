using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;
using Utility;
using System.Threading.Tasks;
using SimpleJSON;
using System.Linq;

public class CharacterModel : BaseModel
{

    List<CharacterStats> characterStats = new List<CharacterStats>();

    public CharacterModel() {
        UpdateFromDatabase();
    }

    public async void UpdateFromDatabase() {
        characterStats.Clear();

        string rawDataString = UtilityMethod.PrefGet(ParameterFlag.SaveSlotKey.Character, "");

        if (!string.IsNullOrEmpty(rawDataString)) {
            JSONNode jsonObject = await UtilityMethod.GetJSONObject(rawDataString);
            characterStats = JsonHelper.FromJson<CharacterStats>(rawDataString).ToList();
        }
    }

    public void SaveToDatabase() {
        string rawData = JsonHelper.ToJson<CharacterStats>(characterStats.ToArray());
        Debug.Log(rawData);
    }
}