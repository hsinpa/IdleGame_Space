using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using IG.Database;
using Utility;

public class RecruitView : ScrollableElement
{
    [SerializeField]
    private GameObject cvcardPrefab;

    [SerializeField]
    private RectTransform characterBody;

    public void GenerateCharacterCard(CharacterStats[] characterStats, System.Action<CharacterStats> CharacterClickEvent)
    {
        UtilityMethod.ClearChildObject(characterBody);

        bool hasContent = characterStats.Length > 0;

        DisplayObject(characterBody.gameObject, hasContent);

        if (hasContent)
        {
            foreach (CharacterStats stat in characterStats)
            {
                var CVCardObj = UtilityMethod.CreateObjectToParent(characterBody, cvcardPrefab);
                CVCard cvCard = CVCardObj.GetComponent<CVCard>();

                cvCard.icon.sprite = stat.icon;
                cvCard.titleText.text = stat.full_name;

                cvCard.button.onClick.RemoveAllListeners();
                cvCard.button.onClick.AddListener(delegate
                {
                    if (CharacterClickEvent != null)
                        CharacterClickEvent(stat);
                });
            }
        }
    }

    private void DisplayObject(GameObject p_object, bool p_display)
    {
        p_object.SetActive(p_display);
    }
}
