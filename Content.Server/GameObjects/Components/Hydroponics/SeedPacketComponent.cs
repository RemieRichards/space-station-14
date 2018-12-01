using Content.Shared.Hydroponics;
using SS14.Shared.GameObjects;
using SS14.Shared.Interfaces.GameObjects;
using SS14.Shared.IoC;
using SS14.Shared.Prototypes;
using SS14.Shared.Serialization;

namespace Content.Server.GameObjects.Components.Hydroponics
{
    public class SeedPacketComponent : Component
    {
        public override string Name => "SeedPacket";

        PlantGrowthPrototype growthPrototype;

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                serializer.DataReadFunction("grows", "badplant", protostring =>
                {
                    var protoMan = IoCManager.Resolve<IPrototypeManager>();
                    growthPrototype = protoMan.Index<PlantGrowthPrototype>(protostring);
                });
            }

            if (serializer.Writing)
            {
                serializer.DataWriteFunction("grows", "badplant", () =>
                {
                    return growthPrototype.ID;
                });
            }
        }

        public void PlantInHolder(PlantHolderComponent holder)
        {
            var entityMan = IoCManager.Resolve<IEntityManager>();
            IEntity plant = entityMan.SpawnEntity("PlantObj");
            PlantComponent pc = plant.AddComponent<PlantComponent>();
            pc.SetGrowth(growthPrototype);
            holder.SetPlant(pc);
        }
    }
}
