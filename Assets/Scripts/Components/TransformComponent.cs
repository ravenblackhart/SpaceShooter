using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[GenerateAuthoringComponent]
   public struct TransformComponent : IComponentData
   {
      public float Speed;
      public float2 Direction;
      public float RotationSpeed; 
      public bool SwitchDirectionX;
      public bool SwitchDirectionY; 
   }

