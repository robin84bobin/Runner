using Model;
using TMPro;
using UnityEngine;
using Zenject;

public class AbilitiesUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private AbilityService _abilityService;

    [Inject]
    public void Construct(AbilityService abilityService)
    {
        _abilityService = abilityService;
        _abilityService.OnAbilitiesUpdate += OnAbilityUpdate;
    }

    private void OnAbilityUpdate()
    {
        string abilityText  = string.Empty;
        foreach (var ability in _abilityService.Abilities)
        {
            abilityText += $"{ability.Data.title}: \n " +
                           $"{string.Format(ability.Data.description, (int)ability.TotalSeconds)} \n";
        }

        _text.text = abilityText;
    }

}
