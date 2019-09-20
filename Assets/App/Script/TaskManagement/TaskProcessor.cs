using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskProcessor : MonoBehaviour
{
    [SerializeField]
    private Button ProceedButton;

    private Text ProceedButtonText;

    private float finishedTime;

    private bool isProceed = false;

    public System.Action OnTaskDone;

    public void Update ()
    {
        if (!isProceed) {
            return;
        }

        int roundTime = Mathf.CeilToInt(finishedTime - Time.time);

        if (roundTime < 0) {
            isProceed = false;
            TaskFinish();
            return;
        }

        ProceedButtonText.text = "Skip ("+roundTime+"s)";
    }

    /// <summary>
    /// Update the wait time information, this time will be use when user click Proceed Button
    /// </summary>
    /// <param name="taskParseResult"></param>
    public void UpdateProcessData(TaskCalculationHelper.ParseResult taskParseResult) {
        string timekey = "time";

        if (taskParseResult.costDict.TryGetValue(timekey, out int timeValue)) {
            finishedTime = Time.time + timeValue;
        }
    }

    public void Proceed() {

        if (Time.time > finishedTime)
            return;

        ProceedButtonText = ProceedButton.GetComponentInChildren<Text>();
        isProceed = true;
    }

    private void TaskFinish() {
        ProceedButtonText.text = "Proceed";

        if (OnTaskDone != null)
            OnTaskDone();
    }

}
