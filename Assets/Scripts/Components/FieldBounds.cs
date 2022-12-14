using Unity.Entities;


    [GenerateAuthoringComponent]
    public struct FieldBounds : IComponentData
    {
        public int Width;
        public int Height;
    }

