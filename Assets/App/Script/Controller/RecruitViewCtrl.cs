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

    void Start()
    {
        recruiter = new CharacteRecruiter(characterAssets);
        UpdateCharacterCard();
    }

    public void UpdateCharacterCard()
    {
        if (recruitView != null)
        {
            recruitView.GenerateCharacterCard(GetTestCharacterStats(6), CharacterCardClickEvent);
        }
    }

    private CharacterStats[] GetTestCharacterStats(int generate_num)
    {

        CharacterStats[] charArray = new CharacterStats[generate_num];

        for (int i = 0; i < generate_num; i++) {
            charArray[i] = recruiter.Generate();
        }

        return charArray;
    }

    private void CharacterCardClickEvent(CharacterStats characterStats)
    {
        Debug.Log("Character Click Event : " + characterStats.full_name);
        CharacterModal modal = MainApp.Instance.modalView.GetModal<CharacterModal>();
        modal.SetUp(characterStats);

        MainApp.Instance.modalView.Open(modal);
    }
}
