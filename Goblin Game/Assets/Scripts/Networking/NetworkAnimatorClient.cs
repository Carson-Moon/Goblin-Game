using Unity.Netcode.Components;
using UnityEngine;

public class NetworkAnimatorClient : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
