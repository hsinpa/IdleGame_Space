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

    [SerializeField]
    private RectTransform emptyText;

    private List<CVCard> _cacheCharacterList = new List<CVCard>();

    private System.Action<CharacterStats> CharacterClickEvent;
    private InGameSpriteManager spriteManager;

    public void SetUp(InGameSpriteManager spriteManager)
    {
        this.spriteManager = spriteManager;
    }

    public void UpdateCharacterCard(CharacterStats characterStats)
    {
        CVCard cacheCard = _cacheCharacterList.Find(x => x._id == characterStats._id);

        if (cacheCard == null)
        {
            Debug.LogError("CVCard Object not found");
            return;
        }

        cacheCard.hireIcon.enabled = true;
        cacheCard.button.interactable = false;

        UpdateBodyState();
    }

    public void RenewAllCVCard(List<CharacterStats> characterStats, System.Action<CharacterStats> CharacterClickEvent)
    {
        this.CharacterClickEvent = CharacterClickEvent;

        UtilityMethod.ClearChildObject(characterBody);
        _cacheCharacterList.Clear();

        foreach (CharacterStats stat in characterStats)
        {
            CVCard cvCard = UpdateCardInfo(stat, null);
            _cacheCharacterList.Add(cvCard);
        }

        UpdateBodyState();
    }

    private void UpdateBodyState()
    {
        bool hasContent = _cacheCharacterList.Count > 0;

        DisplayObject(characterBody.gameObject, hasContent);
        //DisplayObject(emptyText.gameObject, !hasContent);
    }

    private CVCard UpdateCardInfo(CharacterStats stat, CVCard cvCard)
    {
        if (cvCard == null)
        {
            var CVCardObj = UtilityMethod.CreateObjectToParent(characterBody, cvcardPrefab);
            cvCard = CVCardObj.GetComponent<CVCard>();
        }

        cvCard._id = stat._id;
        cvCard.icon.sprite = this.spriteManager.FindSprite(stat.icon_name, ParameterFlag.SpriteTag.Character);
        cvCard.titleText.text = stat.full_name;

        cvCard.button.onClick.RemoveAllListeners();
        cvCard.button.onClick.AddListener(delegate
        {
            if (this.CharacterClickEvent != null)
                this.CharacterClickEvent(stat);
        });

        return cvCard;
    }

    private void DisplayObject(GameObject p_object, bool p_display)
    {
        p_object.SetActive(p_display);
    }

}
