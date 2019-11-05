using SS14.Client.UserInterface;
using SS14.Client.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserInterface.Players.Preferences.Pages
{
    public abstract class BasePreferencesPage : Panel
    {
        public int pagenumber = -1; //set by PreferencesGui when added as a page
    }
}
