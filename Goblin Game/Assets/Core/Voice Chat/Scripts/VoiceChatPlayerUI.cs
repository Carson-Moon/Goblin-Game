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


    public void Setup(string _displayName, Action<string, bool, Action<bool>> _toggleMuteAction)
    {
        displayName = _displayName;
        toggleMuteAction += _toggleMuteAction;

        nameDisplay.text = displayName;
    }

#region Mute
    public void ToggleMute()
    {
        Debug.Log("Toggle mute.");

        toggleMuteAction?.Invoke(displayName, !muted, OnToggleSuccess);
    }

    private void OnToggleSuccess(bool _muted)
    {
        muted = _muted;
        muteImage.sprite = muted ? mutedSprite : unmutedSprite;
    }

#endregion

    void OnDestroy()
    {
        toggleMuteAction = null;
    }
}
