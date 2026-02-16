using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VoiceChatPlayerUI : MonoBehaviour
{
    private string displayName;

    [SerializeField] TextMeshProUGUI nameDisplay;

    [Header("Mute")]
    [SerializeField] private bool muted = false;
    [SerializeField] Image muteImage;
    [SerializeField] Sprite unmutedSprite;
    [SerializeField] Sprite mutedSprite;
    private Action<string, bool, Action<bool>> toggleMuteAction;

    [Header("Volume")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] int minVolume;
    [SerializeField] int maxVolume;

    private Action<string, float, Action<float>> adjustVolumeAction;


    public void Setup(string _displayName, Action<string, bool, Action<bool>> _toggleMuteAction, Action<string, float, Action<float>> _adjustVolumeAction)
    {
        displayName = _displayName;
        toggleMuteAction += _toggleMuteAction;
        adjustVolumeAction = _adjustVolumeAction;

        nameDisplay.text = displayName;

        // Initialize starting volume at 0.5.
        volumeSlider.value = 0.5f;
    }

#region Mute
    public void ToggleMute()
    {
        toggleMuteAction?.Invoke(displayName, !muted, OnToggleSuccess);
    }

    private void OnToggleSuccess(bool _muted)
    {
        muted = _muted;
        muteImage.sprite = muted ? mutedSprite : unmutedSprite;
    }

#endregion

#region Volume Adjust
    public void AdjustVolume()
    {
        adjustVolumeAction?.Invoke(displayName, volumeSlider.value, OnVolumeAdjustSuccess);
    }

    private void OnVolumeAdjustSuccess(float newVolume)
    {
        
    }

#endregion

    void OnDestroy()
    {
        toggleMuteAction = null;
    }
}
