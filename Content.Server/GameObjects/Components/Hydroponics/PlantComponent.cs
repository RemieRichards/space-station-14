using System;
using System.Text;
using Content.Server.GameObjects.EntitySystems;
using Content.Server.PlantGenetics;
using Content.Shared.Hydroponics;
using SS14.Server.GameObjects;
using SS14.Server.Interfaces.Chat;
using SS14.Shared.GameObjects;
using SS14.Shared.Interfaces.GameObjects;
using SS14.Shared.IoC;
using SS14.Shared.Prototypes;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;

namespace Content.Server.GameObjects.Components.Hydroponics
{
    /// <summary>
    /// A Growable Plant
    /// </summary>
    ///
    public class PlantComponent : Component, IUse, IExamine
    {
        public override string Name => "Plant";

        private string displayName = "Plant";
        public string DisplayName => displayName;

        Random random;

        SpriteComponent spriteComponent;

        PlantGrowthPrototype growthPrototype;
        public PlantGrowthPrototype GrowthPrototype
        {
            get => growthPrototype;
            set
            {
                if (value.Stages.Count < 1)
                {
                    throw new InvalidOperationException($"{nameof(PlantGrowthPrototype)} '{value.ID}' does not have enough stages.");
                }
                growthPrototype = value;
                spriteComponent.AddLayerWithSprite(growthPrototype.Stages[0].Icon);
                Stage = growthPrototype.Stages[0];
            }
        }

        PlantStage stage;
        public PlantStage Stage
        {
            get => stage;
            set
            {
                stage = value;

                SpriteSpecifier sprite = stage.Icon;
                if (sprite != null)
                {
                    spriteComponent.LayerSetSprite(0, sprite);
                }

                string name = stage.Name;
                if (name != null)
                {
                    displayName = name;
                }
                harvestable = stage.Harvestable;
            }
        }

        PlantDna dna;
        public PlantDna DNA => dna;

        public PlantHolderComponent Holder;

        //Core genes that must always exist, so provide easy of access props
        public ThirstGene Thirst => dna.GetGene<ThirstGene>();
        public HungerGene Hunger => dna.GetGene<HungerGene>();
        public AgeGene Age => dna.GetGene<AgeGene>();
        public HealthGene Health => dna.GetGene<HealthGene>();

        public bool IsLowHealth
        {
            get
            {
                HealthGene healthg = Health;
                return healthg.Value < healthg.MaxValue / 2;
            }
        }

        //Harvestable
        private bool harvestable;
        public bool Harvestable => harvestable;

        public override void Initialize()
        {
            base.Initialize();

            random = new Random();

            spriteComponent = Owner.GetComponent<SpriteComponent>();
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                serializer.DataReadFunction("growth", "badplant", protostring =>
                {
                    var protoMan = IoCManager.Resolve<IPrototypeManager>();
                    GrowthPrototype = protoMan.Index<PlantGrowthPrototype>(protostring);
                });
                serializer.DataField(ref stage, "stage", new PlantStage());
                serializer.DataField(ref dna, "dna", new PlantDna());
            }

            if (serializer.Writing)
            {
                serializer.DataWriteFunction("growthpath", "badplant", () =>
                {
                    return growthPrototype.ID;
                });
                serializer.DataField(ref stage, "stage", new PlantStage());
                serializer.DataField(ref dna, "dna", new PlantDna());
            }
        }

        float UpdateLimiter = 0;
        public void OnUpdate(float frameTime)
        {
            UpdateLimiter += frameTime;
            if (UpdateLimiter < 2) return;
            UpdateLimiter = 0;

            dna.OnUpdate();

            if (Holder == null)
            {

                int thirst = random.Next(Thirst.MinThirst, Thirst.MaxThirst);
                int hunger = random.Next(Hunger.MinHunger, Hunger.MaxHunger);

                bool quenched = Holder.UseWater(thirst);
                bool fed = Holder.UseNutrients(hunger);


                if (quenched && fed)
                {
                    HandleAging();
                }
                else
                {
                    int hurt = 0;
                    if (!quenched)
                    {
                        hurt++;
                    }
                    if (!fed)
                    {
                        hurt++;
                    }
                    Health.Adjust(-hurt);
                }

                HandlePestsAndWeeds();
            }
        }

        public void HandlePestsAndWeeds()
        {
            int hurt = 0;

            if (Holder.IsHighPests)
            {
                hurt++;
            }

            if (Holder.IsHighWeeds)
            {
                hurt++;
            }

            Health.Adjust(-hurt);
        }

        public void HandleAging()
        {
            Age.Age++;

            if (stage.Next != null)
            {
                if(Age.Age >= stage.Next.Age)
                {
                    Stage = stage.Next;
                }
            }

            if(Age.Age > Age.DyingAge)
            {
                Health.Adjust(-3);
            }
        }

        public string Examine()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("A " + DisplayName + "\n");

            string healthText;
            if(Health.Value == 0)
            {
                healthText = "It's dead (jim).\n";
            }
            else if(Health.Value < (Health.MaxValue/4))
            {
                healthText = "It's nearly dead.\n";
            }
            else if(Health.Value < (Health.MaxValue / 2))
            {
                healthText = "It's unhealthy.\n";
            }
            else if(Health.Value == Health.MaxValue)
            {
                healthText = "It's perfectly healthy.\n";
            }
            else
            {
                healthText = "It's quite healthy.\n";
            }
            sb.Append(healthText);

            string ageText;
            if (Age.Age > Age.DyingAge)
            {
                ageText = "It's dying because it's too old.\n";
            }
            else if (Age.Age > Age.DyingAge / 2)
            {
                ageText = "It's past its prime.\n";
            }
            else
            {
                ageText = "It's quite young.\n";
            }
            sb.Append(ageText);

            dna.ExamineText(sb);
            return sb.ToString();
        }

        //TODO
        bool IUse.UseEntity(IEntity user)
        {
            IoCManager.Resolve<IChatManager>().DispatchMessage(SS14.Shared.Console.ChatChannel.Visual, "No touch-y");

            return true;
        }
    }
}
