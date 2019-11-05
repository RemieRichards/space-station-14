using Content.Server.Players.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.Interfaces
{
    public interface IClientPlayerPrefsManager
    {
        PlayerPrefs Get();
        void Save();
    }
}
