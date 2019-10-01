﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;
using UnityEngine.UI;

public class CharacterModal : Modal
{

    public enum PageType {
        Inspection,
        Recruitment
    }

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text salaryText;

    [SerializeField]
    Text negativeFeatureText;

    [SerializeField]
    Text positiveFeatureText;

    [SerializeField]
    Button actionButton;

    [SerializeField]
    Image avatarIcon;

    private CharacterStats _characterStats;

    public void SetUp(CharacterStats characterStats, PageType pageType, System.Action<CharacterStats> ActionButton) {
        _characterStats = characterStats;

        nameText.text = _characterStats.full_name;
        salaryText.text = "$2000";

        negativeFeatureText.text = _characterStats.negativeCharStat.name;
        positiveFeatureText.text = _characterStats.positiveCharStat.name;

        avatarIcon.sprite = _characterStats.icon;

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() => { ActionButton(_characterStats); });

        var buttonText = actionButton.GetComponentInChildren<Text>();
        buttonText.text = (pageType == PageType.Inspection) ? "Dismiss" : "Wanted";
    }

    private void FindUnsignGameObject() {
        if (nameText == null)
            nameText = transform.Find("Info Panel/name").GetComponent<Text>();

        if (salaryText == null)
            salaryText = transform.Find("Info Panel/salary").GetComponent<Text>();

        if (negativeFeatureText == null)
            negativeFeatureText = transform.Find("Info Panel/negative_feature").GetComponent<Text>();

        if (positiveFeatureText == null)
            positiveFeatureText = transform.Find("Info Panel/positive_feature").GetComponent<Text>();

        if (avatarIcon == null)
            avatarIcon = transform.Find("Info Panel/icon").GetComponent<Image>();

        if (actionButton == null)
            actionButton = transform.Find("Info Panel/action_button").GetComponent<Button>();

    }

    private void OnValidate()
    {
        FindUnsignGameObject();
    }

}
