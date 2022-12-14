using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Boolean 
{
    public byte boolValue;
 
    public Boolean(bool value)
    {
        boolValue = (byte)(value ? 1 : 0);
    }
 
    public static implicit operator bool(Boolean value)
    {
        return value.boolValue == 1;
    }
 
    public static implicit operator Boolean(bool value)
    {
        return new Boolean(value);
    }
 
    public override string ToString()
    {
        if (boolValue == 1)
            return "true";
 
        return "false";
    }
}
