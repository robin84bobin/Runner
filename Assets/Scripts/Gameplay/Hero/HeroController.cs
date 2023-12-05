using UnityEngine;

internal class HeroController : MonoBehaviour
{
    private HeroModel _model;

    public void Setup(HeroModel model)
    {
        _model = model;
    }
}