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
    private InGameSpriteManager _spriteManager;
    private List<CharacterStats> _chararcterList = new List<CharacterStats>();
    public List<CharacterStats> characterList
    {
        get {
            return _chararcterList;
        }
    }

    public System.Action<CharacterStats> OnCharacterDimiss;
    public System.Action<CharacterStats> OnCharacterHire;

    public CharacterModel(InGameSpriteManager spriteManager) {
        _spriteManager = spriteManager;
        UpdateFromDatabase();
    }

    public void Hire(CharacterStats character) {
        _chararcterList.Add(character);

        if (OnCharacterHire != null)
            OnCharacterHire(character);

        SaveToDatabase();
    }

    public void FireEmployee(CharacterStats character)
    {
        _chararcterList.Remove(character);

        if (OnCharacterDimiss != null)
            OnCharacterDimiss(character);

        SaveToDatabase();
    }

    public void UpdateFromDatabase() {
        _chararcterList.Clear();

        string rawDataString = UtilityMethod.PrefGet(ParameterFlag.SaveSlotKey.Character, "");

        if (!string.IsNullOrEmpty(rawDataString)) {
            _chararcterList = JsonHelper.FromJson<CharacterStats>(rawDataString).ToList();
        }
    }

    public void SaveToDatabase() {
        string rawData = JsonHelper.ToJson<CharacterStats>(_chararcterList.ToArray());
        UtilityMethod.PrefSave(ParameterFlag.SaveSlotKey.Character, rawData);
    }

}