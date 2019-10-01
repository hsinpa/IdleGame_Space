using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager
{

    private List<BaseModel> _models;

    public ModelManager() {
        RegisterModels();
    }

    void RegisterModels() {
        _models = new List<BaseModel>();
        _models.Add(new CharacterModel());
    }

}
