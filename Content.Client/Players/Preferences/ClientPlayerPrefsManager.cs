using Content.Client.Interfaces;
using Content.Server.Players.Preferences;
using SS14.Client.Interfaces.ResourceManagement;
using SS14.Client.Player;
using SS14.Shared.IoC;
using SS14.Shared.Serialization;
using SS14.Shared.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Content.Client.Players.Preferences
{
    public class ClientPlayerPrefsManager : IClientPlayerPrefsManager
    {
#pragma warning disable 649
        [Dependency] private readonly IResourceCache _resourceCache;
#pragma warning restore 649

        private static readonly ResourcePath PrefsPath = new ResourcePath("/preferences.yml");

        private PlayerPrefs _prefs;

        public PlayerPrefs Get()
        {
            if (_prefs == null)
            {
                if (_resourceCache.UserData.Exists(PrefsPath))
                {
                    using (var stream = _resourceCache.UserData.Open(PrefsPath, FileMode.Open))
                    {
                        YamlObjectSerializer serializer = YamlObjectSerializer.NewReader(new YamlMappingNode());
                        var yaml = new YamlStream();
                        yaml.Load(new StreamReader(stream));
                        if (yaml.Documents.Count == 0) return _prefs = new PlayerPrefs();
                        return _prefs = (PlayerPrefs)serializer.NodeToType(typeof(PlayerPrefs), yaml.Documents[0].RootNode);
                    }
                }
                else
                {
                    return _prefs = new PlayerPrefs();
                }
            }
            return _prefs;
        }

        public void Save()
        {
            if (_prefs == null)
            {
                return;
            }
            using (var stream = _resourceCache.UserData.Open(PrefsPath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    YamlObjectSerializer serializer = YamlObjectSerializer.NewWriter(new YamlMappingNode());
                    YamlNode serialized = serializer.TypeToNode(_prefs);
                    var yaml = new YamlStream(new YamlDocument(serialized));
                    yaml.Save(writer, assignAnchors: false);
                }
            }
        }
    }
}
