using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IG.Database
{
    [CreateAssetMenu(fileName = "[Task]Holder", menuName = "Task/Holder", order = 1)]
    public class TaskHolder : ScriptableObject
    {
        public List<TaskStats> stpObjectHolder = new List<TaskStats>();

        public T FindObject<T>(string id) where T : TaskStats
        {
            return stpObjectHolder.Find(x => x.id == id) as T;
        }

        public List<T> FindObjectByType<T>() where T : TaskStats
        {
            List<T> findObjects = new List<T>();
            for (int i = 0; i < stpObjectHolder.Count; i++)
            {
                if (stpObjectHolder[i].GetType() == typeof(T))
                    findObjects.Add((T)stpObjectHolder[i]);
            }
            return findObjects;
        }
    }
}