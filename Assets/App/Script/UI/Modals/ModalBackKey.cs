using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModalBackKey : MonoBehaviour
{

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        AssignBackEvent();
    }

    void AssignBackEvent() {
        if (button != null) {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                MainApp.Instance.modalView.Close();
            });
        }
    }
}
