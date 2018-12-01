using Content.Server.GameObjects.Components.Hydroponics;
using SS14.Shared.GameObjects;
using SS14.Shared.GameObjects.Systems;

namespace Content.Server.GameObjects.EntitySystems
{
    class HydroponicsTraySystem : EntitySystem
    {
        public override void Initialize()
        {
            EntityQuery = new TypeEntityQuery(typeof(HydroponicsTrayComponent));
        }

        public override void Update(float frameTime)
        {
            foreach (var entity in RelevantEntities)
            {
                var comp = entity.GetComponent<HydroponicsTrayComponent>();
                comp.OnUpdate(frameTime);
            }
        }
    }
}
