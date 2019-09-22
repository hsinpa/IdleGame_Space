using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IG.Database { 
    [System.Serializable]
    public struct CharacterStats
    {
        public string _id;

        public string first_name;
        public string family_name;

        public string gender;
        public string sprite_name;
    }
}