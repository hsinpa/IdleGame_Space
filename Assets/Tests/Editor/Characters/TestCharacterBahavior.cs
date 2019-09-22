using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using Game.Character;
using UnityEditor;
using IG.Database;

namespace Test.Character {
    public class TestCharacterBahavior
    {
        CharacteRecruiter _characterRecruiter;
        CharacterSCAssets _characterAssets;

        [SetUp]
        public void Init()
        {
            string ASSET_FOLDER = "Assets/Database";
            _characterAssets = (CharacterSCAssets)AssetDatabase.LoadAssetAtPath(ASSET_FOLDER + "/[Character]Generator.asset", typeof(CharacterSCAssets));

            _characterRecruiter = new CharacteRecruiter(_characterAssets);
        }

        [Test]
        public void TestCharacterGeneration() {

            CharacterStats characterStats = _characterRecruiter.Generate();

            Debug.Log(characterStats.family_name);
            Debug.Log(characterStats.first_name);
            Debug.Log(characterStats.gender);

            bool validStats = !string.IsNullOrEmpty(characterStats.family_name);

            if (validStats)
                validStats = !string.IsNullOrEmpty(characterStats.first_name);

            if (validStats)
                validStats = !string.IsNullOrEmpty(characterStats.gender);

            Assert.True(validStats);
        }
    }
}
