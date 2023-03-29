namespace Web.Interfaces
{
    public interface ITempDataSerializer
    {
        byte[] Serialize(object value);
        object? Deserialize(byte[] value);
    }
}
