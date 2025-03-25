using System;
using UnityEngine;

/// <summary>
/// Represents a globally unique identifier (GUID) that is serializable with Unity and usable in game scripts.
/// </summary>
[Serializable]
public struct SerializableGuid : IEquatable<SerializableGuid> {
    [SerializeField] public long Part1;
    [SerializeField] public long Part2;

    public static SerializableGuid Empty => new SerializableGuid(0, 0);

    public SerializableGuid(long part1, long part2) {
        Part1 = part1;
        Part2 = part2;
    }

    public SerializableGuid(Guid guid) {
        byte[] bytes = guid.ToByteArray();
        Part1 = BitConverter.ToInt64(bytes, 0);
        Part2 = BitConverter.ToInt64(bytes, 8);
    }

    public static SerializableGuid NewGuid() => new SerializableGuid(Guid.NewGuid());

    public static SerializableGuid FromHexString(string hexString) {
        if (hexString.Length != 32) {
            return Empty;
        }

        return new SerializableGuid(
            Convert.ToInt64(hexString.Substring(0, 16), 16),
            Convert.ToInt64(hexString.Substring(16, 16), 16)
        );
    }

    public string ToHexString() {
        return $"{Part1:X16}{Part2:X16}";
    }

    public Guid ToGuid() {
        var bytes = new byte[16];
        BitConverter.GetBytes(Part1).CopyTo(bytes, 0);
        BitConverter.GetBytes(Part2).CopyTo(bytes, 8);
        return new Guid(bytes);
    }

    public static implicit operator Guid(SerializableGuid serializableGuid) => serializableGuid.ToGuid();
    public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);

    public override bool Equals(object obj) {
        return obj is SerializableGuid guid && this.Equals(guid);
    }

    public bool Equals(SerializableGuid other) {
        return Part1 == other.Part1 && Part2 == other.Part2;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Part1, Part2);
    }

    public static bool operator ==(SerializableGuid left, SerializableGuid right) => left.Equals(right);
    public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right);
}