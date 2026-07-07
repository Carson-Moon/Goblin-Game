using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerProfile : INetworkSerializable, IEquatable<PlayerProfile>
{
    public FixedString32Bytes DisplayName;
    public Color GoblinColor;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref DisplayName);
        serializer.SerializeValue(ref GoblinColor);
    }

    public bool Equals(PlayerProfile other)
    {
        return  DisplayName == other.DisplayName &&
                GoblinColor == other.GoblinColor;
    }
}
