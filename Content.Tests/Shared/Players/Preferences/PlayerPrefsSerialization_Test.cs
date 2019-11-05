using Content.Server.Players.Preferences;
using Content.Shared.Interfaces.Players.Preferences;
using Content.Shared.Players.Appearance;
using Content.Shared.Players.Preferences.Profiles;
using NUnit.Framework;
using SS14.Shared.Maths;
using SS14.Shared.Reflection;
using SS14.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Players.Preferences.Profiles
{
    [Parallelizable]
    [TestFixture]
    public class PlayerPrefs_Serialization_Test
    {


        public sealed class ReflectionManagerTestContent : ReflectionManager
        {
            protected override IEnumerable<string> TypePrefixes => new[] {
                "",
                "SS14.UnitTesting.",
                "SS14.Server.",
                "SS14.Shared.",
                "Content.Shared.",
                "Content.Shared.Players.Preferences.Profiles."};
        }

        [Parallelizable]
        [Test]
        public void Test_Serialize_PlayerPrefs()
        {

            var data = new PlayerPrefs()
            {
                Characters = new List<ICharacterProfile>()
                {
                    new HumanoidCharacterProfile()
                    {
                        Name = "Joe Genero",
                        Age = 25,
                        Appearance = new HumanoidCharacterAppearance()
                        {
                            EyeColor = Color.Red,
                            HairColor = Color.Purple,
                            FacialHairColor = Color.Green,
                            SkinColor = Color.Beige,
                            HairPrototypeName = "Mohawk",
                            FacialHairPrototypeName = "Bald"
                        }
                    },
                    new HumanoidCharacterProfile()
                    {
                        Name = "Sibyl Secutie",
                        Age = 21,
                        Appearance = new HumanoidCharacterAppearance()
                        {
                            EyeColor = Color.Blue,
                            HairColor = Color.Yellow,
                            FacialHairColor = Color.White,
                            SkinColor = Color.Beige,
                            HairPrototypeName = "Long Hair",
                            FacialHairPrototypeName = "Bald"
                        }
                    }
                }
            };

            //hate is not a strong enough word
            var dataSerialized = "prefs:\n  characters:\n  - type: HumanoidCharacterProfile\n    name: Joe Genero\n    age: 25\n    appearance:\n      type: HumanoidCharacterAppearance\n      eyecolor: '#FF0000FF'\n      haircolor: '#800080FF'\n      facialhaircolor: '#008000FF'\n      skincolor: '#F5F5DCFF'\n      hairstyle: Mohawk\n      facialhairstyle: Bald\n  - type: HumanoidCharacterProfile\n    name: Sibyl Secutie\n    age: 21\n    appearance:\n      type: HumanoidCharacterAppearance\n      eyecolor: '#0000FFFF'\n      haircolor: '#FFFF00FF'\n      facialhaircolor: '#FFFFFFFF'\n      skincolor: '#F5F5DCFF'\n      hairstyle: Long Hair\n      facialhairstyle: Bald\n...\n";

            var mapping = new YamlMappingNode();
            var serializer = YamlObjectSerializer.NewWriter(mapping);
            serializer.DataField(ref data, "prefs", data, alwaysWrite: true);

            var text = NodeToYamlText(mapping);

            Console.WriteLine(text);

            Assert.AreEqual(text, dataSerialized);

            SS14.Shared.IoC.IoCManager.Register<SS14.Shared.Interfaces.Reflection.IReflectionManager, ReflectionManagerTestContent>(overwrite: true);
            SS14.Shared.IoC.IoCManager.BuildGraph();
            SS14.Shared.IoC.IoCManager.Resolve<SS14.Shared.Interfaces.Reflection.IReflectionManager>().LoadAssemblies(
                AppDomain.CurrentDomain.GetAssemblies()
            );

            serializer = YamlObjectSerializer.NewReader(mapping);
            serializer.DataField(ref data, "prefs", data, alwaysWrite: true);

            Console.WriteLine(data.ToString());
            Console.WriteLine(data.Characters.ToString());
            foreach(ICharacterProfile prof in data.Characters)
            {
                Console.WriteLine(NodeToYamlText(prof.ToYamlNode()));
            }
        }


        //"Borrowed" from engine's Yaml object serialization tests.
        // serializes a node tree into text
        private static string NodeToYamlText(YamlNode root)
        {
            var document = new YamlDocument(root);

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.NewLine = "\n";
                    var yamlStream = new YamlStream(document);
                    yamlStream.Save(writer, false);
                    writer.Flush();
                    return System.Text.Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }

    }

}
