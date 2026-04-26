using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private PartyConfig _partyConfig;
    [SerializeField] private AbilitiesConfig _abilitiesConfig;
    [SerializeField] private ModificationsConfig _modificationsConfig;
    [SerializeField] private WindowCharactersView _windowCharacters;

    private List<Character> _characters;

    private void Start()
    {
        _characters = new List<Character>();

        foreach (var config in _partyConfig.Characters)
        {
            _characters.Add(new Character(config, _abilitiesConfig, _modificationsConfig));
        }

        var windowVm = new WindowCharactersViewModel(_characters);
        var windowView = Instantiate(_windowCharacters);
        windowView.Init(windowVm);
    }
}