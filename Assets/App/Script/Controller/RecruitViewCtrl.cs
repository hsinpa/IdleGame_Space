using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using IG.Database;
using Game.Character;

public class RecruitViewCtrl : MonoBehaviour
{
    [SerializeField]
    RecruitView recruitView;

    [SerializeField]
    private CharacterSCAssets characterAssets;

    private CharacteRecruiter recruiter;

    //Models
    private CharacterModel characterModel;

    void Start()
    {
        recruitView.SetUp(MainApp.Instance.spriteManager);
        recruiter = new CharacteRecruiter(characterAssets);
        characterModel = MainApp.Instance.models.GetModel<CharacterModel>();
        characterModel.OnCharacterHire += OnCharacterHire;
        UpdateCharacterCard();
    }

    public void UpdateCharacterCard()
    {
        if (recruitView != null)
        {
            recruitView.RenewAllCVCard(GetTestCharacterStats(6), CharacterCardClickEvent);
        }
    }

    private List<CharacterStats> GetTestCharacterStats(int generate_num)
    {

        List<CharacterStats> charArray = new List<CharacterStats>(generate_num);

        for (int i = 0; i < generate_num; i++) {
            charArray.Add(recruiter.Generate());
        }

        return charArray;
    }

    private void CharacterCardClickEvent(CharacterStats characterStats)
    {
        Debug.Log("Character Click Event : " + characterStats.full_name);
        CharacterModal modal = MainApp.Instance.modalView.GetModal<CharacterModal>();
        modal.SetUp(MainApp.Instance.spriteManager, characterStats, CharacterModal.PageType.Recruitment, RecruitEvent);

        MainApp.Instance.modalView.Open(modal);
    }

    private void RecruitEvent(CharacterStats characterStats) {
        Debug.Log(characterModel == null);
        characterModel.Hire(characterStats);
    }

    private void OnCharacterHire(CharacterStats characterStats) {
        recruitView.UpdateCharacterCard(characterStats);
    }

    private void OnDestroy()
    {
        
    }

}
