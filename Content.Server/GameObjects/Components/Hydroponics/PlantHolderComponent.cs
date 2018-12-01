using Content.Server.GameObjects.EntitySystems;
using SS14.Shared.GameObjects;
using SS14.Shared.Interfaces.GameObjects;
using SS14.Shared.Serialization;
using System;

namespace Content.Server.GameObjects.Components.Hydroponics
{

    /*
     * 'TODO: Really need reagents.
     */ 

    public class PlantHolderComponent : Component, IAttackby, IExamine
    {
        public override string Name => "PlantHolder";

        private PlantComponent plant;
        public PlantComponent Plant
        {
            get => plant;
            set
            {
                plant = value;
                plant.Holder = this;
                plant.Owner.Transform.LocalPosition = Owner.Transform.LocalPosition;

                //plant.Owner.Transform.VisibleWhileParented = true;
                //plant.Owner.Transform.AttachParent(Owner.Transform);
            }
        }

        //0-10
        private float pests = 0;
        public int Pests => (int)pests;
        public bool IsHighPests => Pests >= 5;

        //0-10
        private float weeds = 0;
        public int Weeds => (int)weeds;
        public bool IsHighWeeds => Weeds >= 5; //Is your child 'High Weeds'? find out on our next episode

        //TODO: Reagents, until then, 0-100
        private int water = 0;
        public int Water => water;
        public bool IsLowWater => water <= 10;

        //TODO: Reagents, until then, 0-100
        private int nutrients = 0;
        public int Nutrients => nutrients;
        public bool IsLowNutrients => nutrients <= 20;

        //TODO: Reagents
        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            serializer.DataField(ref pests, "pests", 0);
            serializer.DataField(ref weeds, "weeds", 0);
            serializer.DataField(ref water, "water", 0);
            serializer.DataField(ref nutrients, "nutrients", 0);

            //TODO: serialize plant

        }

        float UpdateLimiter = 0;
        public virtual void OnUpdate(float frameTime)
        {
            UpdateLimiter += frameTime;
            if (UpdateLimiter < 2) return;
            UpdateLimiter = 0;

            pests = Math.Min(10, pests + frameTime);
            weeds = Math.Min(10, weeds + frameTime);
        }

        /// <summary>
        ///     Tries to take 'request' 'magical bullshit units' of water (TODO: Reagents)
        ///     Returns if it took the requested amount.
        ///     Plants use this to take water.
        /// </summary>
        /// <param name="request">Amount of 'magical bullshit units' of water to take</param>
        /// <returns></returns>
        public bool UseWater(int request)
        {
            int take = Math.Min(water, request);
            water -= take;
            return take == request;
        }

        /// <summary>
        ///     Tries to take 'request' 'magical bullshit units' of nutrients (TODO: Reagents)
        ///     Returns if it took the requested amount.
        ///     Plants use this to take nutrients.
        /// </summary>
        /// <param name="request">Amount of 'magical bullshit units' of nutrients to take</param>
        /// <returns></returns>
        public bool UseNutrients(int request)
        {
            int take = Math.Min(nutrients, request);
            nutrients -= take;
            return take == request;
        }

        bool IAttackby.Attackby(IEntity user, IEntity attackwith)
        {
            if (attackwith.TryGetComponent(out SeedPacketComponent component))
            {
                component.PlantInHolder(this);
                var handsComp = user.GetComponent<HandsComponent>();
                handsComp.Drop(handsComp.ActiveIndex);
                component.Owner.Delete();
                return true;
            }



            //TEMPORARY
            if (attackwith.TryGetComponent(out WeedkillerComponent component2))
            {
                weeds = Math.Max(0, weeds - 2);
                return true;
            }
            if (attackwith.TryGetComponent(out PestkillerComponent component3))
            {
                pests = Math.Max(0, pests - 2);
                return true;
            }
            if (attackwith.TryGetComponent(out WaterBucketComponent component4))
            {
                water = Math.Min(100,water+20);
                return true;
            }
            if (attackwith.TryGetComponent(out NutrientsComponent component5))
            {
                nutrients = Math.Min(100, nutrients + 20);
                return true;
            }
            //TEMPORARY

            return false;
        }

        public string Examine()
        {
            if (plant != null)
            {
                string text = "It's growing a plant:\n";
                text += Plant.Examine();
                return text;
            } else
            {
                return "It's empty";
            }
        }
    }

    //TEMPORARY
    public class WeedkillerComponent : Component
    {
        public override string Name => "Weedkiller";
    }

    public class PestkillerComponent : Component
    {
        public override string Name => "Pestkiller";
    }

    public class WaterBucketComponent: Component
    {
        public override string Name => "WaterBucket";
    }

    public class NutrientsComponent : Component
    {
        public override string Name => "Nutrients";
    }
    //TEMPORARY
}
