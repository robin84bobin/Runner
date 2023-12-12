using Model;
using TMPro;
using UnityEngine;
using Zenject;

public class AbilitiesUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private AbilitiesModel _abilitiesModel;

    [Inject]
    public void Construct(AbilitiesModel abilitiesModel)
    {
        _abilitiesModel = abilitiesModel;
        _abilitiesModel.OnAbilitiesUpdate += OnAbilitiesUpdate;
    }

    private void OnAbilitiesUpdate()
    {
        string abilityText  = string.Empty;
        foreach (var ability in _abilitiesModel.Abilities)
        {
            abilityText += $"{ability.Data.title}: \n " +
                           $"{string.Format(ability.Data.description, (int)ability.TotalSeconds)} \n";
        }

        _text.text = abilityText;
    }

}
