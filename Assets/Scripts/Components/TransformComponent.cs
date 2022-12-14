using Unity.Entities;
using Unity.Mathematics;


   [GenerateAuthoringComponent]
   public struct TransformComponent : IComponentData
   {
      public float Speed;
      public float2 Direction;
      public float2 Rotation; 
      public bool SwitchDirection; 
   }

