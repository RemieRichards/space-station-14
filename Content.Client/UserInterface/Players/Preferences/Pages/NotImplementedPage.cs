using SS14.Client.UserInterface.Controls;
using SS14.Shared.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserInterface.Players.Preferences.Pages
{
    public class NotImplementedPage : BasePreferencesPage
    {
        public NotImplementedPage()
        {
            var cont = new VBoxContainer();

            var labelA = new Label();
            labelA.Text = "!! NOT IMPLEMENTED !!";
            labelA.FontColorOverride = Color.Red;
            labelA.FontColorShadowOverride = Color.DarkRed;
            cont.AddChild(labelA);

            var labelB = new Label();
            labelB.Text = "Be the change you want to see in the world!";
            labelB.FontColorOverride = Color.Red;
            labelB.FontColorShadowOverride = Color.DarkRed;
            cont.AddChild(labelB);

            AddChild(cont);
        }
    }
}
