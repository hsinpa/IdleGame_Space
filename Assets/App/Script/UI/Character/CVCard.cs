using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character {

    public class CVCard : MonoBehaviour
    {

        [SerializeField]
        private Image _icon;

        public Image icon {
            get { return _icon; }
        }

        [SerializeField]
        private Text _titleText;

        public Text titleText
        {
            get { return _titleText; }
        }

        private Button _button;
        public Button button
        {
            get {
                if (_button == null) {
                    _button = GetComponent<Button>();
                }

                return _button;
            }
        }

    }

}
