using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;

public class VoiceChatUI : MonoBehaviour
{
    [SerializeField] RectTransform playerUIHolder;
    [SerializeField] VoiceChatPlayerUI playerUIPrefab;

    private Dictionary<string, VoiceChatPlayerUI> currentPlayerUIs = new();


    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        VoiceChat.Instance.onChannelJoined += OnChannelJoined;
        VoiceChat.Instance.onChannelLeft += OnChannelLeft;
        VoiceChat.Instance.onParticipantJoinedChannel += OnParticipantJoined;
        VoiceChat.Instance.onParticipantLeftChannel += OnParticipantLeft;
    }

    private void OnChannelJoined(string channelName)
    {
        ClearExistingUI();
        CreateNewUI();
    }

    private void OnChannelLeft(string channelName)
    {
        ClearExistingUI();
    }

    private void OnParticipantJoined(VivoxParticipant participant)
    {
        AddToUI(participant);
    }

    private void OnParticipantLeft(VivoxParticipant participant)
    {
        RemoveFromUI(participant);
    }

#region UI
    private void ClearExistingUI()
    {
        foreach(var playerUI in currentPlayerUIs)
            Destroy(playerUI.Value.gameObject);

        currentPlayerUIs.Clear();
    }

    private void CreateNewUI()
    {
        foreach(var participant in VoiceChat.Instance.CurrentChannelParticipants)
        {
            AddToUI(participant.Value);
        }
    }

    private void AddToUI(VivoxParticipant participant)
    {
        VoiceChatPlayerUI playerUI = Instantiate(playerUIPrefab, playerUIHolder);
        playerUI.Setup(participant.DisplayName, VoiceChat.Instance.ToggleLocalPlayerMute);

        currentPlayerUIs.Add(participant.DisplayName, playerUI);
    }

    private void RemoveFromUI(VivoxParticipant participant)
    {
        if(currentPlayerUIs.TryGetValue(participant.DisplayName, out VoiceChatPlayerUI playerUI))
        {
            Destroy(playerUI.gameObject);
            currentPlayerUIs.Remove(participant.DisplayName);
        }
    }

#endregion
}
