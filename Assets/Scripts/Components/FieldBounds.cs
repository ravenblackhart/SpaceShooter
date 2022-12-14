using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[GenerateAuthoringComponent]
    public struct FieldBounds : IComponentData
    {
        public float BorderLeft;
        public float BorderRight;
        public float BorderTop;
        public float BorderBottom;
    }

