using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    public UnityAction NextLevel;

    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _amountCoins;
    [SerializeField] private Button _nextBtn;


    private void OnEnable()
    {
        _nextBtn.onClick.AddListener(NextLevel);
        _nextBtn.onClick.AddListener(OnChangeVisible);
    }

    private void OnDisable()
    {
        _nextBtn.onClick.RemoveListener(NextLevel);
        _nextBtn.onClick.RemoveListener(OnChangeVisible);
    }

    public void ShowCanvas(LevelData data)
    {
        if(_levelText != null)
        _levelText.text = data.NameLevel;
        _amountCoins.text = data.AmountReward.ToString();

        gameObject.SetActive(true);
    }

    private void OnChangeVisible()
    {
        gameObject.SetActive(false);
    }

}