using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager
{
    private InGameSpriteManager spriteManager;
    private List<BaseModel> _models;

    public T GetModel<T>() where T : BaseModel {

        return (T)_models.Find(delegate (BaseModel it) { return (it is T); });

        foreach (BaseModel m in _models) {
            Debug.Log(_models.GetType().ToString());
            if (_models is T) {
                return (T)m;
            }
        }
        return null;
    }

    public ModelManager(InGameSpriteManager spriteManager) {
        RegisterModels();
    }

    void RegisterModels() {
        _models = new List<BaseModel>();
        _models.Add(new CharacterModel(spriteManager));
    }

}
