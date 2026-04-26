using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _lblName;
    [SerializeField] private Image _imgPortrait; 
    [SerializeField] private TMP_Text _lblHp;
    [SerializeField] private TMP_Text _lblArmor;

    public void Init(CharacterViewModel characterVm)
    {
        _lblName.text = characterVm.Name;
        _imgPortrait.sprite = characterVm.Portrait;
        _lblHp.text = $"{characterVm.HP}/{characterVm.MaxHP}";
        _lblArmor.text = $"{characterVm.Armor}/{characterVm.MaxArmor}";
    }
}