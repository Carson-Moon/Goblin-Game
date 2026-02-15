using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.Audio;

public class VoiceChat : MonoBehaviour
{
    public static VoiceChat Instance {get; private set;}

    // This string should be replaced with a unique ID tied to the player object for proximity chat.
    private Dictionary<string, VivoxParticipant> currentChannelParticipants = new();
    public Dictionary<string, VivoxParticipant> CurrentChannelParticipants => currentChannelParticipants;

    public event Action<string> onChannelJoined;
    public event Action<string> onChannelLeft;
    public event Action<VivoxParticipant> onParticipantJoinedChannel;
    public event Action<VivoxParticipant> onParticipantLeftChannel;

    [Header("Audio Tap Settings")]
    [SerializeField] AudioMixerGroup audioMixer;
    [SerializeField] float spatialBlend;
    [SerializeField] AudioRolloffMode rolloffMode;
    [SerializeField] float rolloffMinDistance;
    [SerializeField] float rolloffMaxDistance;


    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start() 
    {
        BindRelayActions(true);
    }

    private void BindVivoxActions(bool bind)
    {
        if(bind)
        {
            VivoxService.Instance.ChannelJoined += OnChannelJoined;
            VivoxService.Instance.ChannelLeft += OnChannelLeft;
            VivoxService.Instance.ParticipantAddedToChannel += OnParticipantAddedToChannel;
            VivoxService.Instance.ParticipantRemovedFromChannel += OnParticipantRemovedFromChannel;
        }
        else
        {
            VivoxService.Instance.ChannelJoined -= OnChannelJoined;
            VivoxService.Instance.ChannelLeft -= OnChannelLeft;
            VivoxService.Instance.ParticipantAddedToChannel -= OnParticipantAddedToChannel;
            VivoxService.Instance.ParticipantRemovedFromChannel -= OnParticipantRemovedFromChannel;
        }
    }

    private void BindRelayActions(bool bind)
    {
        if(bind)
        {
            RelayConnection.Instance.onClientStarted += StartEnablingVoiceChat;
        }
        else
        {
            RelayConnection.Instance.onClientStarted -= StartEnablingVoiceChat;
        }
    }

    public async void StartEnablingVoiceChat()
    {
        try
        {
            string _channelName = RelayConnection.Instance.JoinCode;
            string _displayName = UsernameHolder.Username;

            await EnableVoiceChat(_channelName, _displayName);
            Debug.Log("Voice Chat Successfully enabled.");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    private async Task EnableVoiceChat(string _channelName, string _displayName)
    {
        await LoginToVivoxAsync(_displayName);
        await JoinChannelAsync(_channelName);
    }

#region Initializing Vivox
    async Task LoginToVivoxAsync(string _displayName)
    {
        if(VivoxService.Instance.IsLoggedIn)
        {
            Debug.Log("Vivox is already logged in.");
            return;
        }

        try
        {
            Debug.Log("Logging into Vivox...");
            LoginOptions options = new LoginOptions();
            options.DisplayName = _displayName;

            await VivoxService.Instance.LoginAsync(options);

            BindVivoxActions(true);

            Debug.Log("Login successful.");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
#endregion

#region Joining and Leaving Channel
    async Task JoinChannelAsync(string channelName)
    {
        try
        {
            Debug.Log("Joining group channel...");
            await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.AudioOnly);
            Debug.Log("Successfully joined group channel.");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
        
    }

    async Task LeaveChannelAsync(string channelName)
    {
        try
        {
            Debug.Log("Leaving group channel...");
            await VivoxService.Instance.LeaveChannelAsync(channelName);
            Debug.Log("Left channel.");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
#endregion

#region Participant Management
    private void OnChannelJoined(string channelName)
    {
        currentChannelParticipants.Clear();
        foreach(var participant in VivoxService.Instance.ActiveChannels[channelName])
            currentChannelParticipants.Add(participant.DisplayName, participant);

        onChannelJoined?.Invoke(channelName);
    }

    private void OnChannelLeft(string channelName)
    {
        onChannelLeft?.Invoke(channelName);
    }

    private void OnParticipantAddedToChannel(VivoxParticipant participant)
    {
        currentChannelParticipants.Add(participant.DisplayName, participant);

        if(!participant.IsSelf)
        {
            GoblinClient goblinClient = FindObjectsByType<GoblinClient>(FindObjectsSortMode.None).First(x => x.GoblinName == participant.DisplayName);
            if(goblinClient == null)
            {
                Debug.LogWarning("Could not find goblin client for audio tap.");
                return;
            }

            AudioSource participantSource = participant.CreateVivoxParticipantTap($"{participant.DisplayName}'s Audio Tap").GetComponent<AudioSource>();

            participantSource.transform.SetParent(goblinClient.GoblinCharacter.transform);
            participantSource.transform.localPosition = Vector3.zero;

            participantSource.outputAudioMixerGroup = audioMixer;
            participantSource.spatialBlend = spatialBlend;
            participantSource.rolloffMode = rolloffMode;
            participantSource.minDistance = rolloffMinDistance;
            participantSource.maxDistance = rolloffMaxDistance;
        }

        onParticipantJoinedChannel?.Invoke(participant);
    }

    private void OnParticipantRemovedFromChannel(VivoxParticipant participant)
    {
        onParticipantLeftChannel?.Invoke(participant);

        currentChannelParticipants.Remove(participant.DisplayName);
    }

    public void ToggleLocalPlayerMute(string displayName, bool muted, Action<bool> onSuccess)
    {
        if(currentChannelParticipants.TryGetValue(displayName, out VivoxParticipant participant))
        {
            if(participant.IsSelf)
            {
                Debug.LogWarning("Cannot mute yourself!");
                return;
            }

            if(muted)
                participant.MutePlayerLocally();
            else
                participant.UnmutePlayerLocally();

            onSuccess?.Invoke(muted);
            Debug.Log("Successfully " + (muted ? "muted" : "unmuted") + $" {displayName}.");
        }
        else
        {
            Debug.LogWarning($"Player not found: {displayName}. Cannot mute/unmute.");
        }
    }

    #endregion

    void OnDestroy()
    {
        BindRelayActions(false);
        BindVivoxActions(false);
    }
}
