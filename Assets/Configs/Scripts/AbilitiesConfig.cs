using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Abilities", menuName = "TestTask/Abilities")]
public class AbilitiesConfig : ScriptableObject
{
    [SerializeField] private List<Ability> abilities;

    public IEnumerable<Ability> GetAbilities(AbilitySize abilitySize)
    {
        if (abilities == null || abilities.Count == 0) return new List<Ability>();

        var count = abilitySize switch
        {
            AbilitySize.NONE => 0,
            AbilitySize.MIN => Random.Range(0, 3),
            AbilitySize.MAX => abilities.Count,
            AbilitySize.RANDOM => Random.Range(0, abilities.Count + 1),
            _ => 0
        };
        
        count = Math.Min(count, abilities.Count);

        var selectedAbilities = new List<Ability>();

        for (var i = 0; i < count; i++)
        {
            var indexRandom = Random.Range(0, abilities.Count);
            selectedAbilities.Add(abilities[indexRandom]);
        }

        return selectedAbilities;
    }
}