using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Party", menuName = "TestTask/Party")]
public class PartyConfig : ScriptableObject
{
    [SerializeField] private List<CharacterConfig> _characters;
    public List<CharacterConfig> Characters => _characters;
}