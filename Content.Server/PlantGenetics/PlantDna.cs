using SS14.Shared.Interfaces.Reflection;
using SS14.Shared.IoC;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.RepresentationModel;
using static SS14.Shared.Serialization.YamlObjectSerializer;

namespace Content.Server.PlantGenetics
{
    /// <summary>
    /// Plant Dna, unique to each plant, contains the plants genes
    /// and various methods to access/check them
    /// </summary>
    public class PlantDna
    {
        Dictionary<Type, IPlantGene> genes;
        public IReadOnlyDictionary<Type, IPlantGene> Genes => genes;

        public void AddGene(IPlantGene gene)
        {
            genes.Add(gene.GetType(), gene);
        }

        public void AddGene<T>() where T : IPlantGene, new()
        {
            AddGene(new T());
        }

        public void AddIfMissing<T>() where T : IPlantGene, new()
        {
            if (!HasGene<T>())
            {
                AddGene<T>();
            }
        }

        public bool RemoveGene(IPlantGene gene)
        {
            return genes.Remove(gene.GetType());
        }

        public bool RemoveGene<T>() where T : IPlantGene, new()
        {
            return genes.Remove(typeof(T));
        }

        public bool HasGene<T>() where T : IPlantGene, new()
        {
            return genes[typeof(T)] != null;
        }

        public T GetGene<T>() where T : IPlantGene, new()
        {
            return (T)genes[typeof(T)];
        }

        public IEnumerable<T> GetGenes<T>() where T : IPlantGene, new()
        {
            return genes.OfType<T>();
        }

        public void OnUpdate()
        {
            foreach (IUpdatingGene g in genes.Values)
            {
                g.OnUpdate();
            }
        }

        internal void ExamineText(StringBuilder sb)
        {
            foreach (IDescriptiveGene g in genes.Values)
            {
                g.ExamineText(sb);
            }
        }
    }

    public class PlantDnaSerializer : TypeSerializer
    {
        public override object NodeToType(Type type, YamlNode node, YamlObjectSerializer serializer)
        {
            var refl = IoCManager.Resolve<IReflectionManager>();
            var mapping = (YamlMappingNode)node;
            var seq = mapping.GetNode<YamlSequenceNode>("genes");

            PlantDna dna = new PlantDna();

            foreach (YamlNode child in seq.Children)
            {
                var childMapping = (YamlMappingNode)child;
                var typeNode = childMapping.GetNode("type");

                var geneType = refl.LooseGetType(typeNode.AsString());
                if (!typeof(IPlantGene).IsAssignableFrom(geneType))
                {
                    throw new InvalidOperationException();
                }

                var gene = (IPlantGene)Activator.CreateInstance(geneType);
                gene.FromYAML(childMapping);
                dna.AddGene(gene);
            }

            CoreGenes.AddCoreGenes(dna);

            return dna;
        }

        public override YamlNode TypeToNode(object obj, YamlObjectSerializer serializer)
        {

            PlantDna dna = (PlantDna)obj;

            var mapping = new YamlMappingNode();
            var seq = new YamlSequenceNode();

            foreach (IPlantGene gene in dna.Genes.Values)
            {
                var childMapping = new YamlMappingNode();
                childMapping.Add(new YamlScalarNode("type"), new YamlScalarNode(gene.GetType().ToString()));
                gene.ToYAML(childMapping);
                seq.Add(childMapping);
            }

            mapping.Add(new YamlScalarNode("genes"), seq);

            return mapping;
        }
    }


    /// <summary>
    /// A <see cref="PlantDna"/>'s gene
    /// </summary>
    public interface IPlantGene
    {
        string Name { get; }
        void FromYAML(YamlMappingNode node);
        void ToYAML(YamlMappingNode node);
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that updates whenever its DNA/Plant does
    /// </summary>
    public interface IUpdatingGene : IPlantGene
    {
        void OnUpdate();
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that provides examine text
    /// </summary>
    public interface IDescriptiveGene : IPlantGene
    {
        void ExamineText(StringBuilder sb);
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores a single Value of T, with a maximum and minimum bound
    /// <para>Do not use directly, instead use one of the abstract implementations (ie: <see cref="IntValueGene"/>)
    /// so you get YAML serialization support</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueGene<T> : IPlantGene
    {
        T Value { get; }
        T MaxValue { get; }
        T MinValue { get; }
        void Set(T value);
        void Adjust(T value);
    }

    /// <summary>
    /// A <see cref="IPlantGene"/> that stores a single Value of <see cref="int"/>, with support for loading/saving YAML
    /// </summary>
    public abstract class IntValueGene : IValueGene<int>
    {
        public abstract string Name { get; }

        public int Value { get; protected set; }
        public int MaxValue { get; protected set; }
        public int MinValue { get; protected set; }

        public void Set(int value)
        {
            Value = Math.Max(Math.Min(MaxValue, value), MinValue);
        }

        public void Adjust(int adjust)
        {
            Set(Value + adjust);
        }

        public void FromYAML(YamlMappingNode node)
        {
            Value    = node.GetNode("value").AsInt();
            MaxValue = node.GetNode("maxValue").AsInt();
            MinValue = node.GetNode("minValue").AsInt();
        }

        public void ToYAML(YamlMappingNode node)
        {
            node.Add(new YamlScalarNode("value"),    new YamlScalarNode(Value.ToString()));
            node.Add(new YamlScalarNode("maxValue"), new YamlScalarNode(MaxValue.ToString()));
            node.Add(new YamlScalarNode("minValue"), new YamlScalarNode(MinValue.ToString()));
        }
    }
}
