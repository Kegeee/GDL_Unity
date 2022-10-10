/*
 * Code taken from here : 
 * https://forum.unity.com/threads/vector3-is-not-marked-serializable.435303/
 * It follows this Micrososft guide on serialization : 
 * https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.surrogateselector?redirectedfrom=MSDN&view=net-6.0
 * 
 * Used to make sure Vector3 are serializable.
 */

using UnityEngine;
using System.Runtime.Serialization;
using System.Collections;

public class Vector3SerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Vector3 object.
    // System.Object necessary, otherwise it serializes as Unity.Object instead, making this whole script pointless.
    public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
    {
        Vector3 v3 = (Vector3)obj;
        info.AddValue("x", v3.x);
        info.AddValue("y", v3.y);
        info.AddValue("z", v3.z);
    }
    // Method called to deserialize a Vector3 object.
    // System.Object necessary, otherwise it deserializes as Unity.Object instead, making this whole script pointless.
    public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 v3 = (Vector3)obj;
        v3.x = (float)info.GetValue("x", typeof(float));
        v3.y = (float)info.GetValue("y", typeof(float));
        v3.z = (float)info.GetValue("z", typeof(float));
        obj = v3;
        return obj;
    }
}
