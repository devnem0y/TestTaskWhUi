using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifications", menuName = "TestTask/Modifications")]
public class ModificationsConfig : ScriptableObject
{
    [SerializeField] private List<Modification> _modifications;

    public IEnumerable<Modification> GetModifications()
    {
        if (_modifications == null || _modifications.Count == 0)
            return new List<Modification>();

        var count = Random.Range(0, _modifications.Count + 1);
        var selectedModifications = new List<Modification>();
        var usedIndices = new HashSet<int>();

        for (var i = 0; i < count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, _modifications.Count);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);
            selectedModifications.Add(_modifications[randomIndex]);
        }

        return selectedModifications.OrderBy(m => m.Type).ToList();;
    }
}