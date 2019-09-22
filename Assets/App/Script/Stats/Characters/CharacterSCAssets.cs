using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace IG.Database
{
    [CreateAssetMenu(fileName = "[Character]Generator", menuName = "Character/Create Generator", order = 1)]
    public class CharacterSCAssets : ScriptableObject
    {
        /// <summary>
        /// 1 => ID, 2 => Family name
        /// </summary>
        public List<UDataStruct> famaily_name_list = new List<UDataStruct>();

        /// <summary>
        /// 1 => ID, 2 => Name, 3 => Gender
        /// </summary>
        public List<UDataStruct> first_name_list = new List<UDataStruct>();

        public Sprite[] sprites = new Sprite[0];

        /// <summary>
        /// Create character by total luck
        /// </summary>
        /// <returns></returns>
        public CharacterStats GenerateCharacter()
        {
            if (famaily_name_list.Count > 0 && first_name_list.Count > 0) {

                UDataStruct rFamilyData = famaily_name_list[Random.Range(0, famaily_name_list.Count)];
                UDataStruct rFirstNameData = first_name_list[Random.Range(0, first_name_list.Count)];

                CharacterStats characterStats = new CharacterStats();
                characterStats.first_name = rFirstNameData.variable_1;
                characterStats.family_name = rFamilyData.variable_1;
                characterStats.gender = rFirstNameData.variable_2;

                return characterStats;
            }

            return default(CharacterStats);
        }
    }
}