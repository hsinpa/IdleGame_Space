using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM {
    [CreateAssetMenu(fileName = "[PM]ProjectAsset", menuName = "PM/Create ProjectAsset", order = 1)]
    public class SA_Project : ScriptableObject
    {
        public PM_Picture pm_picture;
    }
}