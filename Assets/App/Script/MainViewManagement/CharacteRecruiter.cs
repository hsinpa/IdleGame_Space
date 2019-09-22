using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;

namespace Game.Character {
    public class CharacteRecruiter
    {
        private CharacterSCAssets _characterSCAsset;

        public CharacteRecruiter(CharacterSCAssets characterSCAsset) {
            _characterSCAsset = characterSCAsset;
        }

        public CharacterStats Generate() {
            return _characterSCAsset.GenerateCharacter();
        }

        
    }
}