using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _armorText;

    private CharacterViewModel _currentCharacterVm;
    private readonly CompositeDisposable _disposables = new ();

    public void UpdateView(CharacterViewModel characterVm)
    {
        _nameText.text = characterVm.Name;
        _portraitImage.sprite = characterVm.Portrait;
        _hpText.text = $"{characterVm.HP}/{characterVm.MaxHP}";
        _armorText.text = $"{characterVm.Armor}/{characterVm.MaxArmor}";
    }

    private void OnDestroy()
    {
        _disposables?.Dispose();
    }
}