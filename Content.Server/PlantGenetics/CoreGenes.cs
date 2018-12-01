using SS14.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;


namespace Content.Server.PlantGenetics
{
    /// <summary>
    /// Genes that all plants must have should be placed in the <see cref="AddCoreGenes(PlantDna)"/> method
    /// <para>This method is called by <see cref="PlantDnaSerializer.NodeToType(Type, YamlNode, SS14.Shared.Serialization.YamlObjectSerializer)"/></para>
    /// </summary>

    //TODO: good idea? should perhaps just throw errors/have a unit test validating that they exist.
    //The point is often these values won't need changing, so this acts as a "default" if you don't
    //manually add these genes.
    public static class CoreGenes
    {
        /// <summary>
        /// Add 'Core' genes to the plant DNA, only if they don't already exist
        /// </summary>
        /// <param name="dna"></param>
        public static void AddCoreGenes(PlantDna dna)
        {
            dna.AddIfMissing<YieldGene>();
            dna.AddIfMissing<HealthGene>();
            dna.AddIfMissing<AgeGene>();
            dna.AddIfMissing<HungerGene>();
            dna.AddIfMissing<ThirstGene>();
        }
    }


    /// <summary>
    /// A <see cref="IPlantGene"/> that stores the plant's harvest yield
    /// </summary>
    public class YieldGene : IntValueGene
    {
        public override string Name => "Yield";

        public YieldGene()
        {
            Value = 0;
            MaxValue = 100;
            MinValue = 0;
        }
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores the plant's health
    /// </summary>
    public class HealthGene : IntValueGene
    {
        public override string Name => "Endurance";

        public HealthGene()
        {
            Value = 100;
            MaxValue = 100;
            MinValue = 0;
        }
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores the plant's age
    /// </summary>
    public class AgeGene : IPlantGene
    {
        public string Name => "Longevity";

        public int Age = 0;
        public int DyingAge = 100; //abvoe this age, it starts to die

        public void FromYAML(YamlMappingNode node)
        {
            Age = node.GetNode("age").AsInt();
            DyingAge = node.GetNode("dyingAge").AsInt();
        }

        public void ToYAML(YamlMappingNode node)
        {
            node.Add(new YamlScalarNode("age"), new YamlScalarNode(Age.ToString()));
            node.Add(new YamlScalarNode("dyingAge"), new YamlScalarNode(DyingAge.ToString()));
        }
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores how many nutrients the plant wishes to consume per update
    /// </summary>
    public class HungerGene : IPlantGene
    {
        public string Name => "Hunger";

        public int MinHunger = 1;
        public int MaxHunger = 3;

        public void FromYAML(YamlMappingNode node)
        {
            MinHunger = node.GetNode("minHunger").AsInt();
            MaxHunger = node.GetNode("maxHunger").AsInt();
        }

        public void ToYAML(YamlMappingNode node)
        {
            node.Add(new YamlScalarNode("minHunger"), new YamlScalarNode(MinHunger.ToString()));
            node.Add(new YamlScalarNode("maxHunger"), new YamlScalarNode(MaxHunger.ToString()));
        }
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores how much water the plant wishes to consume per update as a min max random number
    /// </summary>
    public class ThirstGene : IPlantGene
    {
        public string Name => "Thirst";

        public int MinThirst = 1;
        public int MaxThirst = 6;

        public void FromYAML(YamlMappingNode node)
        {
            MinThirst = node.GetNode("minThirst").AsInt();
            MaxThirst = node.GetNode("maxThirst").AsInt();
        }

        public void ToYAML(YamlMappingNode node)
        {
            node.Add(new YamlScalarNode("minThirst"), new YamlScalarNode(MinThirst.ToString()));
            node.Add(new YamlScalarNode("maxThirst"), new YamlScalarNode(MaxThirst.ToString()));
        }
    }
}
