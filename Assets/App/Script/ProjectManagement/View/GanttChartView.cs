using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace PM {
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

        // Start is called before the first frame update
        void Start()
        {
            PM_Picture picture = GetPictureFromJSON();

            GenerateGanttChart(picture);
        }

        #region View Generation
        private void GenerateGanttChart(PM_Picture picture) {

            foreach (PM_Group group in picture.groups) {
                GroupTitle title = GenerateGroupTitle(group);

                int taskLength = group.tasks.Length;
                for (int i = 0; i  > taskLength; i++)
                {
                    PM_Task task = group.tasks[i];
                    GroupContent content = GenerateGroupContent(task, title);
                }
            }
        }

        private GroupContent GenerateGroupContent(PM_Task tasks, GroupTitle groupTitle) {
            UtilityMethod.ClearChildObject(GanttRightPanel);

            GroupContent contentObject = UtilityMethod.CreateObjectToParent(GanttLeftPanel, GroupTitlePrefab).GetComponent<GroupContent>();


            return contentObject;
        }

        private GroupTitle GenerateGroupTitle(PM_Group group)
        {
            UtilityMethod.ClearChildObject(GanttLeftPanel);
            GroupTitle gameObject = UtilityMethod.CreateObjectToParent(GanttLeftPanel, GroupTitlePrefab).GetComponent<GroupTitle>();

            gameObject.groupTitle = group.name;

            return gameObject;
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