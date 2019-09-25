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
        /// 1 => Family name
        /// </summary>
        public List<UDataStruct> famaily_name_list = new List<UDataStruct>();

        /// <summary>
        /// 1 => Name, 2 => Gender
        /// </summary>
        public List<UDataStruct> first_name_list = new List<UDataStruct>();

        /// <summary>
        /// 1 => Name, 2 => Tag, 3 => Description, 4 => Effect
        /// </summary>
        public List<UDataStruct> characteristics_list = new List<UDataStruct>();

        public Sprite[] sprites = new Sprite[0];
    }
}