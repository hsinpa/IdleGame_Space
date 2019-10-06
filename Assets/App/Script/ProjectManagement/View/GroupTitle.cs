using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupTitle : MonoBehaviour
{
    [SerializeField]
    Text title;

    public string groupTitle {
        get {
            return title.text;
        }
        set {
            title.text = value;
        }
    }

}
