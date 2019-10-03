using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using IG.Database;
using Game.Character;

public class CharacterViewCtrl : Observer
{

    [SerializeField]
    CharacterView characterView;

    //Models
    private CharacterModel characterModel;

    public override void OnNotify(string p_event, params object[] p_objects)
    {
        base.OnNotify(p_event, p_objects);

        switch (p_event) {
            case EventFlag.Game.SetUp:
                {
                    SetUp();
                }
            break;

            case EventFlag.Scrollview.OnScrollView: {
                

            }
            break;
        }
    }

    public void SetUp() {
        Debug.Log("Setup");
        characterView.SetUp(MainApp.Instance.spriteManager);

        characterModel = MainApp.Instance.models.GetModel<CharacterModel>();
        characterModel.OnCharacterHire += OnHireEvent;

        UpdateCharacterCard(characterModel.characterList);
    }

    public void UpdateCharacterCard(List<CharacterStats> characterList) {
        if (characterView != null && characterModel != null) {
            characterView.RenewAllCVCard(characterList, CharacterCardClickEvent);
        }
    }

    private void CharacterCardClickEvent(CharacterStats characterStats) {
        CharacterModal modal = MainApp.Instance.modalView.GetModal<CharacterModal>();
        modal.SetUp(MainApp.Instance.spriteManager, characterStats, CharacterModal.PageType.Inspection, DismissButtonEvent);

        MainApp.Instance.modalView.Open(modal);
    }

    private void DismissButtonEvent(CharacterStats characterStats) {
        characterModel.FireEmployee(characterStats);
        characterView.UpdateCharacterCard(characterStats, true);
    }

    private void OnHireEvent(CharacterStats characterStats)
    {

        characterView.UpdateCharacterCard(characterStats, false);
    }

    private void OnDestroy()
    {
        characterModel.OnCharacterHire -= OnHireEvent;
    }
}
