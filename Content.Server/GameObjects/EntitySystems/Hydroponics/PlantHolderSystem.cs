using Content.Server.GameObjects.Components.Hydroponics;
using SS14.Shared.GameObjects;
using SS14.Shared.GameObjects.Systems;

namespace Content.Server.GameObjects.EntitySystems
{
    class PlantHolderSystem : EntitySystem
    {
        public override void Initialize()
        {
            EntityQuery = new TypeEntityQuery(typeof(PlantHolderComponent));
        }

        public override void Update(float frameTime)
        {
            foreach (var entity in RelevantEntities)
            {
                var comp = entity.GetComponent<PlantHolderComponent>();
                comp.OnUpdate(frameTime);
            }
        }
    }
}
