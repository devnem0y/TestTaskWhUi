using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Abilities", menuName = "TestTask/Abilities")]
public class AbilitiesConfig : ScriptableObject
{
    [SerializeField] private List<Ability> abilities;

    public IEnumerable<Ability> GetAbilities(AbilitySize abilitySize)
    {
        if (abilities == null || abilities.Count == 0)
            return new List<Ability>();

        var count = abilitySize switch
        {
            AbilitySize.NONE => 0,
            AbilitySize.MIN => Random.Range(1, 3),
            AbilitySize.MAX => abilities.Count,
            AbilitySize.RANDOM => Random.Range(1, abilities.Count + 1),
            _ => 0
        };

        count = Math.Min(count, abilities.Count);
        
        var shuffledAbilities = new List<Ability>(abilities);
        ShuffleList(shuffledAbilities);
        
        return shuffledAbilities.Take(count);
    }
    
    private static void ShuffleList<T>(IList<T> list)
    {
        var n = list.Count;
        
        while (n > 1)
        {
            n--;
            var k = Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}