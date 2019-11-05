using Content.Shared.Interfaces.Players.Appearance;
using SS14.Shared.Maths;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using SS14.Shared.ViewVariables;
using System;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Players.Appearance
{
    /// <summary>
    ///     What a humanoid character looks like.
    ///     Hair style, colour, facial hair, etc.
    /// </summary>
    /// <remarks>
    ///     This class is used both as the Actual/Current/Now representation of a Character's appearance data
    ///     and as the class that gets serialised as part of a Player's preferences.
    /// 
    ///     This is basically human only at the moment.
    /// </remarks>
    public sealed class HumanoidCharacterAppearance : ICharacterAppearance
    {
        /// <summary>
        ///     The colour of your snowflake's eyes.
        /// </summary>
        [ViewVariables]
        public Color EyeColor { get; set; }

        /// <summary>
        ///     The colour of your snowflake's hair.
        ///     No you cannot have it two-toned.
        /// </summary>
        [ViewVariables]
        public Color HairColor { get; set; }

        /// <summary>
        ///     The colour of your snowflake's facial hair.
        /// </summary>
        [ViewVariables]
        public Color FacialHairColor { get; set; }

        /// <summary>
        ///     The colour of your snowflake's skin.
        /// </summary>
        [ViewVariables]
        public Color SkinColor { get; set; }

        /// <summary>
        ///     The name of the HairPrototype we use.
        /// </summary>
        //TODO: HairPrototype
        [ViewVariables]
        public string HairPrototypeName { get; set; }

        /// <summary>
        ///     The name of the FacialHairPrototype we use.
        /// </summary>
        //TODO: FacialHairPrototype
        [ViewVariables]
        public string FacialHairPrototypeName { get; set; }

        #region Type Serializer

        public class TypeSerializer : YamlObjectSerializer.TypeSerializer
        {
            public override object NodeToType(Type type, YamlNode node, YamlObjectSerializer serializer)
            {
                var mapping = (YamlMappingNode)node;
                var profile = new HumanoidCharacterAppearance()
                {
                    EyeColor                = mapping.GetNode("eyecolor").AsColor(),
                    HairColor               = mapping.GetNode("haircolor").AsColor(),
                    FacialHairColor         = mapping.GetNode("facialhaircolor").AsColor(),
                    SkinColor               = mapping.GetNode("skincolor").AsColor(),
                    HairPrototypeName       = mapping.GetNode("hairstyle").AsString(),
                    FacialHairPrototypeName = mapping.GetNode("facialhairstyle").AsString()
                };
                return profile;
            }

            public override YamlNode TypeToNode(object obj, YamlObjectSerializer serializer)
            {
                var appearance = (HumanoidCharacterAppearance)obj;
                return new YamlMappingNode
                {
                    { new YamlScalarNode("type"),            new YamlScalarNode(nameof(HumanoidCharacterAppearance)) },
                    { new YamlScalarNode("eyecolor"),        new YamlScalarNode(appearance.EyeColor.ToHex())         },
                    { new YamlScalarNode("haircolor"),       new YamlScalarNode(appearance.HairColor.ToHex())        },
                    { new YamlScalarNode("facialhaircolor"), new YamlScalarNode(appearance.FacialHairColor.ToHex())  },
                    { new YamlScalarNode("skincolor"),       new YamlScalarNode(appearance.SkinColor.ToHex())        },
                    { new YamlScalarNode("hairstyle"),       new YamlScalarNode(appearance.HairPrototypeName)        },
                    { new YamlScalarNode("facialhairstyle"), new YamlScalarNode(appearance.FacialHairPrototypeName)  }
                };
            }
        }

        #endregion
    }
}
