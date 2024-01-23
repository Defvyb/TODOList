namespace TODOListProject.Rubens;

public struct AtomID :
    IEquatable<AtomID>
{
    private AtomID_Data _atomId;

    public AtomID()
    {
        _atomId.AtomId = 0;
    }
    public AtomID(ulong value)
    {
        _atomId.AtomId = value;
    }

    public AtomID(uint initial, uint sequence)
    {
        _atomId.AtomId = ((ulong)initial << 32) | sequence;
    }

    public bool IsValid => _atomId.AtomId != 0;

    public ulong Value => _atomId.AtomId;

    public uint Initial => (uint)((_atomId.AtomId & 0xFFFFFFFF00000000) >> 32);
    public uint Sequence => (uint)(_atomId.AtomId & 0xFFFFFFFF);

    public bool Equals(AtomID other)
    {
        return _atomId.AtomId == other._atomId.AtomId;
    }

    public override bool Equals(object other)
    {
        return other is AtomID && Equals((AtomID)other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{{{Initial.ToString()}:{Sequence.ToString()}}}";
    }

    public static bool operator ==(AtomID left, AtomID right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AtomID left, AtomID right)
    {
        return !left.Equals(right);
    }
}