using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct InputComponent : IComponentData
{
    public KeyCode ForwardKey; 


}
