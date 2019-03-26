using System;
using Content.Shared.Interfaces.Players.Appearance;
using Content.Shared.Interfaces.Players.Preferences;
using Content.Shared.Players.Appearance;
using SS14.Shared.Interfaces.Serialization;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using SS14.Shared.ViewVariables;
using YamlDotNet.RepresentationModel;

namespace Content.Shared.Players.Preferences.Profiles
{
    /// <summary>
    ///     An <see cref="ICharacterProfile"/> for a Human(oid) Character.
    /// </summary>
    ///
    public sealed class HumanoidCharacterProfile : ICharacterProfile
    {
        /// <summary>
        ///     What your snowflake is called.
        /// </summary>
        [ViewVariables]
        public string Name { get; set; }

        /// <summary>
        ///     How old your snowflake is.
        /// </summary>
        [ViewVariables]
        public int Age { get; set; }

        /// <summary>
        ///     What your snowflake looks like.
        /// </summary>
        [ViewVariables]
        public HumanoidCharacterAppearance Appearance { get; set; }

        #region CharacterProfile impl

        string ICharacterProfile.Name() => Name;

        ICharacterAppearance ICharacterProfile.CharacterAppearance() => Appearance;

        public YamlNode ToYamlNode()
        {
            return YamlObjectSerializer.TypeSerializers[typeof(HumanoidCharacterProfile)].TypeToNode(this, YamlObjectSerializer.NewWriter(new YamlMappingNode()));
        }

        #endregion

        #region Type Serializer

        static HumanoidCharacterProfile()
        {
            YamlObjectSerializer.RegisterTypeSerializer(typeof(HumanoidCharacterProfile), new HumanoidCharacterProfileTypeSerializer());
        }

        class HumanoidCharacterProfileTypeSerializer : YamlObjectSerializer.TypeSerializer
        {
            public override object NodeToType(Type type, YamlNode node, YamlObjectSerializer serializer)
            {
                var mapping = (YamlMappingNode)node;
                var profile = new HumanoidCharacterProfile()
                {
                    Name       = mapping.GetNode("name").AsString(),
                    Age        = mapping.GetNode("age").AsInt(),
                    Appearance = (HumanoidCharacterAppearance)serializer
                        .NodeToType(typeof(HumanoidCharacterAppearance), mapping.GetNode("appearance"))
                };
                return profile;
            }

            public override YamlNode TypeToNode(object obj, YamlObjectSerializer serializer)
            {
                var profile = (HumanoidCharacterProfile)obj;
                return new YamlMappingNode
                {
                    { new YamlScalarNode("type"),       new YamlScalarNode(nameof(HumanoidCharacterProfile)) },
                    { new YamlScalarNode("name"),       new YamlScalarNode(profile.Name)                     },
                    { new YamlScalarNode("age"),        new YamlScalarNode(profile.Age.ToString())           },
                    { new YamlScalarNode("appearance"), serializer.TypeToNode(profile.Appearance)            }
                };
            }
        }

        #endregion
    }
}
