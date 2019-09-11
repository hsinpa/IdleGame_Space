﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;
using UnityEngine.UI;

public class TaskDataSlot : MonoBehaviour
{
    public string _id;

    //Effect after the task is done
    public string effect;

    //Resource need to execute this task
    public string cost;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;



    private TaskStats _taskStat;
    public TaskStats taskStat {
        get { return _taskStat; }
    }

    public void SetUp(TaskStats taskStat)
    {
        _taskStat = taskStat;
    }



}
