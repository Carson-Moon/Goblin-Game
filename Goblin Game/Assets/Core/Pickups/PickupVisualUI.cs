using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupVisualUI : MonoBehaviour
{
    [SerializeField] CanvasGroup visualGroup;
    [SerializeField] Image mainImage;
    [SerializeField] TextMeshProUGUI coinText;

    private Pickup _currentPickup;

    // I hate this but whatever.
    [SerializeField] Sprite jarSprite;


    void Awake()
    {
        visualGroup.TurnOff();
        coinText.text = string.Empty;
    }

    public void OnPickup(Pickup newPickup)
    {
        _currentPickup = newPickup;

        SetMainPickupSprite(newPickup.ID);

        if(_currentPickup.HoldsCoins)
        {
            coinText.text = _currentPickup.PickupCoins.Coins.ToString();
            _currentPickup.PickupCoins.OnCoinValueChange += OnCoinValueChange;
        }
        else
        {
            coinText.text = string.Empty;
        }

        visualGroup.TurnOn();
    }

    public void OffPickup()
    {
        if(_currentPickup.HoldsCoins)
        {
            _currentPickup.PickupCoins.OnCoinValueChange -= OnCoinValueChange;
            coinText.text = string.Empty;
        }

        _currentPickup = null;

        visualGroup.TurnOff();
    }

    private void SetMainPickupSprite(PickupID id)
    {
        switch(id)
        {
            case PickupID.None:
                mainImage.sprite = null;
                break;

            case PickupID.Jar:
                mainImage.sprite = jarSprite;
                break;
        }
    }

    private void OnCoinValueChange(int newValue)
    {
        coinText.text = newValue.ToString();
    }
}
