using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PM.Model;
using System.Linq;

namespace PM.View {
    public class GanttChartView : MonoBehaviour
    {

        [Header("View Object")]
        [SerializeField]
        private Transform GanttLeftPanel;

        [SerializeField]
        private Transform GanttRightPanel;

        [SerializeField]
        private Transform TimelinePanel;

        [Header("Prefab Reference")]

        [SerializeField]
        private GameObject GroupContentPrefab;

        [SerializeField]
        private GameObject GroupTitlePrefab;

        [SerializeField]
        private GameObject GroupTaskPrefab;

        [SerializeField]
        private GameObject TimeObjectPrefab;

        private List<GroupTask> groupTasks;
        private PM_Task first_pm_task;
        private PM_Task last_pm_task;

        private PMUtility pm_utility;

        // Start is called before the first frame update
        public void SetUp(PMUtility pm_utility)
        {
            this.pm_utility = pm_utility;
            PM_Picture picture = GetPictureFromJSON();

            GenerateGanttChart(picture);
        }

        #region View Generation
        private void GenerateGanttChart(PM_Picture picture) {
            groupTasks = new List<GroupTask>();

            UtilityMethod.ClearChildObject(GanttLeftPanel);
            UtilityMethod.ClearChildObject(GanttRightPanel);
            UtilityMethod.ClearChildObject(TimelinePanel);

            foreach (PM_Group group in picture.groups) {
                GroupTitle title = GenerateGroupTitle(group);

                GroupContent content = GenerateGroupContent(group);

                int contentHeight = 0;
                int taskLength = group.tasks.Length;
                for (int i = 0; i  < taskLength; i++)
                {
                    PM_Task task = group.tasks[i];
                    GroupTask groupTask = GenerateGroupTask(content, task);

                    Debug.Log("Task time " + task.start_time +" , duration " + task.duration);

                    Vector2 anchoredPos = groupTask.rectTran.anchoredPosition;
                    float baseAnchorX = task.duration / 2f;

                    groupTask.rectTran.anchoredPosition = new Vector2(pm_utility.GetTimeToWorldValue(task.start_time) + baseAnchorX, anchoredPos.y - contentHeight);
                    SetObjectSize(groupTask, new Vector2(pm_utility.GetTimeToWorldValue(task.duration), groupTask.rectTran.sizeDelta.y));
                    groupTasks.Add(groupTask);

                    //Update PMTask
                    if (first_pm_task.start_time >= task.start_time)
                        first_pm_task = task;

                    if (last_pm_task.start_time + last_pm_task.duration <= task.start_time + task.duration)
                        last_pm_task = task;

                    contentHeight += (int)groupTask.rectTran.sizeDelta.y;
                }

                SetObjectSize(content, new Vector2(content.rectTran.sizeDelta.x, contentHeight));
                SetObjectSize(title, new Vector2(title.rectTran.sizeDelta.x, contentHeight));
            }

            OranizeTimeLine();
        }

        private void OranizeTimeLine() {
            int startTime = first_pm_task.start_time;
            int endTime = last_pm_task.start_time + last_pm_task.duration;

            for (int i = startTime; i < endTime; i++) {
                TimeObject timeObject = UtilityMethod.CreateObjectToParent(TimelinePanel, TimeObjectPrefab).GetComponent<TimeObject>();
                Debug.Log("Time Position " + pm_utility.GetTimeToWorldValue(i) +", Time value " + i);

                Vector2 anchoredPos = timeObject.rectTran.anchoredPosition;
                Vector2 size = timeObject.rectTran.sizeDelta;

                timeObject.rectTran.anchoredPosition = new Vector2(pm_utility.GetTimeToWorldValue(i), size.y * 0.5f);

                if (i == startTime)
                    timeObject.timeText.text = "";
                else
                    timeObject.timeText.text = i + "";

            }
        }

        private GroupTitle GenerateGroupTitle(PM_Group group)
        {
            GroupTitle gameObject = UtilityMethod.CreateObjectToParent(GanttLeftPanel, GroupTitlePrefab).GetComponent<GroupTitle>();

            gameObject.groupTitle = group.name;

            return gameObject;
        }

        private GroupContent GenerateGroupContent(PM_Group group) {
            GroupContent contentObject = UtilityMethod.CreateObjectToParent(GanttRightPanel, GroupContentPrefab).GetComponent<GroupContent>();


            return contentObject;
        }

        private GroupTask GenerateGroupTask(GroupContent groupContent, PM_Task task)
        {
            GroupTask taskObject = UtilityMethod.CreateObjectToParent(groupContent.transform, GroupTaskPrefab).GetComponent<GroupTask>();


            return taskObject;
        }

        private void SetObjectSize(BaseGroup group, Vector2 size) {
            group.rectTran.sizeDelta = size;
        }
        #endregion
        PM_Picture GetPictureFromJSON()
        {
            try
            {
                string jsonPath = Application.streamingAssetsPath + "/ExternalDatabase/JSON/SampleProject.json";
                string text = System.IO.File.ReadAllText(@jsonPath);

                return JsonUtility.FromJson<PM_Picture>(text);
            }
            catch {
                Debug.LogError("GetPictureFromJSON Parsed Error");
            }

            return default(PM_Picture);
        }
    }
}