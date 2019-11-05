using Content.Shared.Players.Appearance;
using Content.Shared.Players.Preferences.Profiles;
using NUnit.Framework;
using SS14.Shared.Maths;
using SS14.Shared.Serialization;
using System;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Players.Preferences.Profiles
{
    [Parallelizable]
    [TestFixture]
    public class CharacterProfile_Serialization_Tests
    {
        [Parallelizable]
        [Test]
        public void Test_Serialize_HumanoidCharacterProfile()
        {
            var data = new HumanoidCharacterProfile()
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
            };

            var dataSerialized = "profile:\n  type: HumanoidCharacterProfile\n  name: Joe Genero\n  age: 25\n  appearance:\n    type: HumanoidCharacterAppearance\n    eyecolor: '#FF0000FF'\n    haircolor: '#800080FF'\n    facialhaircolor: '#008000FF'\n    skincolor: '#F5F5DCFF'\n    hairstyle: Mohawk\n    facialhairstyle: Bald\n...\n";

            var mapping = new YamlMappingNode();
            var serializer = YamlObjectSerializer.NewWriter(mapping);
            serializer.DataField(ref data, "profile", data, alwaysWrite: true);

            var text = NodeToYamlText(mapping);

            Console.WriteLine(text);

            Assert.AreEqual(text, dataSerialized);
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
