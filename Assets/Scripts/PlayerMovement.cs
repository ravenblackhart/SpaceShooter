using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerMovement : SystemBase
{

    
    protected override void OnUpdate()
    {
        
        
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, in TranslatePlayer translatePlayer) => {
                
            }).ScheduleParallel();
    }
}
