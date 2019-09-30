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

    [SerializeField]
    private CharacterSCAssets characterAssets;

    private CharacterModel characterModel;

    private CharacteRecruiter recruiter;

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
        recruiter = new CharacteRecruiter(characterAssets);
        characterModel = new CharacterModel();
        UpdateCharacterCard();
    }

    public void UpdateCharacterCard() {
        if (characterView != null) {
            characterView.GenerateCharacterCard(GetTestCharacterStats(), CharacterCardClickEvent);
        }
    }

    private CharacterStats[] GetTestCharacterStats() {
        return new CharacterStats[] { recruiter.Generate() };
    }

    private void CharacterCardClickEvent(CharacterStats characterStats) {
        Debug.Log("Character Click Event : " + characterStats.full_name);
        CharacterModal modal = MainApp.Instance.modalView.GetModal<CharacterModal>();
        modal.SetUp(characterStats);

        MainApp.Instance.modalView.Open(modal);
    }

}
