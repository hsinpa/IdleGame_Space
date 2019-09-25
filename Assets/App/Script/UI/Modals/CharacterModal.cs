using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IG.Database;
using UnityEngine.UI;

public class CharacterModal : Modal
{

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

    public void SetUp(CharacterStats characterStats) {
        _characterStats = characterStats;

        nameText.text = _characterStats.full_name;
        salaryText.text = _characterStats.full_name;
        negativeFeatureText.text = _characterStats.negativeCharStat.name;
        positiveFeatureText.text = _characterStats.positiveCharStat.name;

        avatarIcon.sprite = _characterStats.icon;
    }

    private void FindUnsignGameObject() {
        if (nameText == null)
            nameText = transform.Find("Info_Panel/name").GetComponent<Text>();

        if (salaryText == null)
            salaryText = transform.Find("Info_Panel/salary").GetComponent<Text>();

        if (negativeFeatureText == null)
            negativeFeatureText = transform.Find("Info_Panel/negative_feature").GetComponent<Text>();

        if (positiveFeatureText == null)
            positiveFeatureText = transform.Find("Info_Panel/positive_feature").GetComponent<Text>();

        if (avatarIcon == null)
            avatarIcon = transform.Find("Info_Panel/icon").GetComponent<Image>();
    }

    private void OnValidate()
    {
        FindUnsignGameObject();
    }

}
