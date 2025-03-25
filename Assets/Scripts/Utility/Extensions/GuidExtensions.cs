using System;

public static class GuidExtensions {
    public static SerializableGuid ToSerializableGuid(this Guid systemGuid) {
        byte[] bytes = systemGuid.ToByteArray();
        return new SerializableGuid(
            BitConverter.ToInt64(bytes, 0),
            BitConverter.ToInt64(bytes, 8)
        );
    }

    public static Guid ToSystemGuid(this SerializableGuid serializableGuid) {
        byte[] bytes = new byte[16];
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part1), 0, bytes, 0, 8);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part2), 0, bytes, 8, 8);
        return new Guid(bytes);
    }
}
