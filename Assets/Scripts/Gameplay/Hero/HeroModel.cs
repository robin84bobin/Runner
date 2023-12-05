using UnityEngine;

public class HeroModel
{
    public string HeroPrefabName { get; }

    public HeroModel(string heroPrefabName)
    {
        HeroPrefabName = heroPrefabName;
    }

    public void ProcessInputMove(Vector2 inputMoveDirection)
    {
        throw new System.NotImplementedException();
    }
}