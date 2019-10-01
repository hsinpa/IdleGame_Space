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

        public string full_name {
            get { return first_name + " " + family_name; }
        }

        public string gender;

        private Sprite _icon;
        public Sprite icon {
            get {
                return _icon;
            }
            set {
                icon_name = value.name;
                _icon = value;
            }
        }

        public string icon_name;

        public CharacteristicsStats positiveCharStat;
        public CharacteristicsStats negativeCharStat;
    }
}