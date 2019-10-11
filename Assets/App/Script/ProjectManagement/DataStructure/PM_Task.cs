using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    [System.Serializable]
    public struct PM_Task
    {
        public string _id;
        public int duration;
        public int start_time;
        public int personnels;
        public string name;
        public string[] dependency;

    }
}