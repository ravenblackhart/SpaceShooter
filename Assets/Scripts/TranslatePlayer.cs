using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct TranslatePlayer : IComponentData
{
   public float Speed;
   public Vector2 Direction;

}
