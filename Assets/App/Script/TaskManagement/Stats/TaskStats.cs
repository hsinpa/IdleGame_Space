using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IG.Database
{
    public class TaskStats : ScriptableObject
    {
        public string id;
        public string tag;
        public string label;
        public string desc;
        public string cost;
        public string effect;
    }
}