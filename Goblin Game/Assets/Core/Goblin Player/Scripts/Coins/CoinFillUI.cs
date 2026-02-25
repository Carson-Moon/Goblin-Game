using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinFillUI : MonoBehaviour
{
    private int _maxCoins;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image fillDisplay;
    [SerializeField] TextMeshProUGUI textDisplay;


    public void Initialize(int maxCoins)
    {


        _maxCoins = maxCoins;

        fillDisplay.fillAmount = 0;
        canvasGroup.TurnOn();
    }

    public void UpdateUI(int coins)
    {
        SetFill(coins);
        SetText(coins);
    }

    public void SetFill(int coins)
    {
        float fillPercentage = Mathf.Clamp((float)coins / _maxCoins, 0, 1);

        fillDisplay.fillAmount = fillPercentage;
    }

    public void SetText(int coins)
    {
        textDisplay.text = coins.ToString();
    }
}
