using Content.Shared.Interfaces.Players.Preferences;
using SS14.Shared.Interfaces.Reflection;
using SS14.Shared.IoC;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Content.Server.Players.Preferences
{
    /// <summary>
    ///     A Single player's preferences.
    /// </summary>
    public sealed class PlayerPrefs
    {
        /// <summary>
        ///     What Characters this player has.
        ///     Urist McRobust, Joe Genero, etc.
        /// </summary>
        public List<ICharacterProfile> Characters { get; set; }

        //TODO: more preferences!

        #region Type Serializer

        static PlayerPrefs()
        {
            YamlObjectSerializer.RegisterTypeSerializer(typeof(PlayerPrefs), new PlayerPrefsTypeSerializer());
        }

        class PlayerPrefsTypeSerializer : YamlObjectSerializer.TypeSerializer
        {
            public override object NodeToType(Type type, YamlNode node, YamlObjectSerializer serializer)
            {
                var mapping = (YamlMappingNode)node;
                var charactersSeq = mapping.GetNode<YamlSequenceNode>("characters");
                var charactersList = new List<ICharacterProfile>();

                var refl = IoCManager.Resolve<IReflectionManager>();

                foreach(YamlMappingNode characterNode in charactersSeq.Children)
                {
                    Type t = refl.LooseGetType(characterNode.GetNode("type").AsString());
                    var characterSerializer = YamlObjectSerializer.TypeSerializers[t];
                    charactersList.Add((ICharacterProfile)characterSerializer.NodeToType(t, characterNode, serializer));
                }

                var prefs = new PlayerPrefs()
                {
                    Characters = charactersList
                };
                return prefs;
            }

            public override YamlNode TypeToNode(object obj, YamlObjectSerializer serializer)
            {
                var prefs = (PlayerPrefs)obj;
                var charactersSeq = new YamlSequenceNode();

                foreach(ICharacterProfile characterProf in prefs.Characters)
                {
                    charactersSeq.Add(characterProf.ToYamlNode());
                }

                return new YamlMappingNode {
                    { new YamlScalarNode("characters"), charactersSeq }
                };
            }
        }

        #endregion
    }
}
