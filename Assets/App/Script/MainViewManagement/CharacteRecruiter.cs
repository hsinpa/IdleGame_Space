using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;
using Utility;
using System.Linq;

namespace Game.Character {
    public class CharacteRecruiter
    {
        private CharacterSCAssets _characterSCAsset;

        public CharacteRecruiter(CharacterSCAssets characterSCAsset) {
            _characterSCAsset = characterSCAsset;
        }

        public CharacterStats Generate() {
            return GetRandomCharacter();
        }

        /// <summary>
        /// Create character by total luck
        /// </summary>
        /// <returns></returns>
        private CharacterStats GetRandomCharacter()
        {
            if (_characterSCAsset != null && _characterSCAsset.famaily_name_list.Count > 0 
                && _characterSCAsset.first_name_list.Count > 0 && _characterSCAsset.sprites.Length > 0)
            {
                UDataStruct rFamilyData = _characterSCAsset.famaily_name_list[Random.Range(0, _characterSCAsset.famaily_name_list.Count)];
                UDataStruct rFirstNameData = _characterSCAsset.first_name_list[Random.Range(0, _characterSCAsset.first_name_list.Count)];

                CharacterStats characterStats = new CharacterStats();
                characterStats._id = System.Guid.NewGuid().ToString();

                characterStats.first_name = rFirstNameData.variable_1;
                characterStats.family_name = rFamilyData.variable_1;
                characterStats.gender = rFirstNameData.variable_2;

                characterStats.icon_name = "character_" + characterStats.gender;

                characterStats.positiveCharStat = GetCharacteristicsStat(true);
                characterStats.negativeCharStat = GetCharacteristicsStat(false);

                return characterStats;
            }

            return default(CharacterStats);
        }

        private CharacteristicsStats GetCharacteristicsStat(bool isPositive) {
            string filterTag = (isPositive) ? "Positive" : "Negative";
            List<UDataStruct> filterData = _characterSCAsset.characteristics_list.FindAll(x => x.variable_2 == filterTag);

            CharacteristicsStats characteristicsStat = new CharacteristicsStats();

            if (filterData.Count > 0) {
                UDataStruct cData = filterData[Random.Range(0, filterData.Count)];

                characteristicsStat._id = cData._id;
                characteristicsStat.name = cData.variable_1;
                characteristicsStat.tag = cData.variable_2;
                characteristicsStat.description = cData.variable_3;
                characteristicsStat.effect = cData.variable_4;
            }

            return characteristicsStat;
        }

    }
}