using System;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerInformation : INetworkSerializable, IEquatable<PlayerInformation>
{
    public FixedString32Bytes Username;


    public PlayerInformation(FixedString32Bytes username)
    {
        Username = username;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Username);
    }

    public bool Equals(PlayerInformation other)
    {
        return Username == other.Username;
    }
}

