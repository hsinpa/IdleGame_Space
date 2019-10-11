using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM.View
{
    public class BaseGroup : MonoBehaviour
    {
        [SerializeField]
        RectTransform _rectTran;

        public RectTransform rectTran
        {
            get
            {
                return _rectTran;
            }
        }

    }
}