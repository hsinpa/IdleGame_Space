using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PM.View
{
    public class GroupTitle : BaseGroup
    {
        [SerializeField]
        Text title;

        public string groupTitle
        {
            get
            {
                return title.text;
            }
            set
            {
                title.text = value;
            }
        }

    }
}