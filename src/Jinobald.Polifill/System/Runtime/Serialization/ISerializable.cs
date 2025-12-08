#if NET35
namespace System.Runtime.Serialization;

public interface ISerializable
{
    void GetObjectData(SerializationInfo info, StreamingContext context);
}
#endif