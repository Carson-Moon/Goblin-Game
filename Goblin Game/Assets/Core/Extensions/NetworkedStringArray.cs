using Unity.Netcode;
using UnityEngine;

public struct NetworkedStringArray : INetworkSerializable
{
    public string[] StringArray;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if(serializer.IsWriter)
        {
            var writer = serializer.GetFastBufferWriter();
            int length = StringArray != null ? StringArray.Length : 0;
            writer.WriteValueSafe(length);

            for(int i=0; i<length; i++)
            {
                writer.WriteValueSafe(StringArray[i]);
            }
        }
        else
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out int length);
            StringArray = new string[length];

            for(int i=0; i<length; i++)
            {
                reader.ReadValueSafe(out StringArray[i]);
            }
        }
    }
}
