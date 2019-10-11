using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PM.Model;

namespace PM.View {
    public class GanttChartView : MonoBehaviour
    {
        [SerializeField]
        private Transform GanttLeftPanel;

        [SerializeField]
        private Transform GanttRightPanel;

        [SerializeField]
        private GameObject GroupContentPrefab;

        [SerializeField]
        private GameObject GroupTitlePrefab;

        [SerializeField]
        private GameObject GroupTaskPrefab;

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

            UtilityMethod.ClearChildObject(GanttLeftPanel);
            UtilityMethod.ClearChildObject(GanttRightPanel);

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
                    contentHeight += (int)groupTask.rectTran.sizeDelta.y;
                }

                SetObjectSize(content, new Vector2(content.rectTran.sizeDelta.x, contentHeight));
                SetObjectSize(title, new Vector2(title.rectTran.sizeDelta.x, contentHeight));
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