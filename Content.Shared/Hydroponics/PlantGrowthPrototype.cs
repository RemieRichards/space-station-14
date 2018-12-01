using SS14.Shared.Interfaces.Serialization;
using SS14.Shared.Prototypes;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Hydroponics
{
    /// <summary>
    ///     Prototype representing a series of "Stages" in a Plant's growth.
    /// </summary>

    [Prototype("PlantGrowth")]
    public class PlantGrowthPrototype : IPrototype, IIndexedPrototype
    {
        private string id;
        private List<PlantStage> _stages = new List<PlantStage>();
        private PlantStage deathStage;

        public string ID => id;
        public IReadOnlyList<PlantStage> Stages => _stages;
        public PlantStage DeathStage => deathStage;

        public void LoadFrom(YamlMappingNode mapping)
        {
            var ser = YamlObjectSerializer.NewReader(mapping);

            ser.DataField(ref id, "id", string.Empty);

            foreach (var stageMap in mapping.GetNode<YamlSequenceNode>("stages").Cast<YamlMappingNode>())
            {
                var stageSerializer = YamlObjectSerializer.NewReader(stageMap);
                var stage = new PlantStage();
                stage.ExposeData(stageSerializer);
                _stages.Add(stage);
            }

            List<PlantStage> staging = _stages.ShallowClone();
            buildLinkedList(staging, staging[0]);

            var deathStateSerializer = YamlObjectSerializer.NewReader(mapping.GetNode<YamlMappingNode>("death"));
            deathStage = new PlantStage();
            deathStage.ExposeData(deathStateSerializer);

        }

        /// <summary>
        ///     LinkedList of youngest required age -> oldest requried age
        /// </summary>
        /// <param name="stages"></param>
        /// <param name="currStage"></param>
        public void buildLinkedList(List<PlantStage> stages, PlantStage currStage)
        {
            stages.Remove(currStage);
            PlantStage next = null;
            foreach (var stage in stages)
            {
                if(next == null || stage?.Age < next.Age)
                {
                    next = stage;
                }
            }
            currStage.Next = next;

            if (next != null)
            {
                buildLinkedList(stages, next);
            }
        }
    }

    /// <summary>
    ///     A Stage of a Plant's growth.
    /// </summary>
    public sealed class PlantStage : IExposeData
    {
        /// <summary>
        ///     Used to build a LinkedList of stages.
        /// </summary>
        public PlantStage Next = null;

        /// <summary>
        ///     Used to rebuild the LinkedList of stages
        ///     After a serialization.
        /// </summary>
        public int NextIndex = -1;

        /// <summary>
        ///     The Age at which the plant becomes this stage.
        /// </summary>
        public int Age = 0;

        /// <summary>
        ///     The name of the plant at this stage.
        ///     Null means to reuse the current name.
        /// </summary>
        public string Name = null;

        /// <summary>
        ///     The icon of the plant at this stage.
        ///     Null means to reuse the current icon.
        /// </summary>
        public SpriteSpecifier Icon = null;

        /// <summary>
        ///     If the plant is harvestable at this stage.
        ///     Doesn't prevent harvesting, but used for the harvest light on Hydro trays, and whether the plant dies on harvest.
        /// </summary>
        public bool Harvestable = false;

        public PlantStage()
        {
        }

        public void ExposeData(ObjectSerializer serializer)
        {

            serializer.DataField(ref NextIndex, "nextindex", -1);
            serializer.DataField(ref Age, "age", 0);

            serializer.DataField(ref Name, "name", null);
            serializer.DataField(ref Icon, "icon", SpriteSpecifier.Invalid);

            serializer.DataField(ref Harvestable, "harvestable", false);
        }
    }
}
