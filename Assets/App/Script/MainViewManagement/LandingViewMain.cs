﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Game.Character;
using IG.Database;

public class LandingViewMain : ScrollableElement
{
    [SerializeField]
    private CharacterSCAssets characterAssets;

    [SerializeField]
    private Image characterIcon;

    [SerializeField]
    private Text characterText;

    private CharacteRecruiter characterRecruiter;

    void Start()
    {
        characterRecruiter = new CharacteRecruiter(characterAssets);

        characterText.text = "";

    }

    public void Recruit() {
        var character = characterRecruiter.Generate();

        if (characterText != null)
            characterText.text = character.first_name + " " + character.family_name;


    }

}