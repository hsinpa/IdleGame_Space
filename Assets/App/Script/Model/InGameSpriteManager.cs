using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class InGameSpriteManager : MonoBehaviour
{
    [SerializeField]
    private SpriteData[] sprite_list;

    public Sprite FindSprite(string p_name, string p_tag) {
        if (sprite_list == null)
            return null;

        foreach (SpriteData tagList in sprite_list) {
            if (tagList.tag == p_tag)
                return UtilityMethod.LoadSpriteFromMulti(tagList.sprite_list, p_name);
        }

        return null;
    }

    [System.Serializable]
    public struct SpriteData {
        public string tag;
        public Sprite[] sprite_list;
    }

}
