using Content.Client.UserInterface.Players.Preferences.Pages;
using Content.Shared.Input;
using SS14.Client.GameObjects.EntitySystems;
using SS14.Client.Interfaces.Input;
using SS14.Client.UserInterface;
using SS14.Client.UserInterface.Controls;
using SS14.Client.UserInterface.CustomControls;
using SS14.Shared.GameObjects;
using SS14.Shared.IoC;
using SS14.Shared.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Client.UserInterface.Players.Preferences
{
    public sealed class PreferencesGui : SS14Window
    {

        private TabContainer _tabs;
        private int _tabCount = 0;

        protected override Vector2? CustomSize => (400, 600);

        protected override void Initialize()
        {
            base.Initialize();

            Title = "Preferences";

            //Containers
            var scrollContainer = new ScrollContainer();
            scrollContainer.SetAnchorPreset(LayoutPreset.Wide, true);
            Contents.AddChild(scrollContainer);

            var vBoxContainer = new VBoxContainer()
            {
                SizeFlagsHorizontal = SizeFlags.FillExpand,
                SizeFlagsVertical = SizeFlags.FillExpand
            };
            scrollContainer.AddChild(vBoxContainer);

            _tabs = new TabContainer();
            scrollContainer.AddChild(_tabs);

            //Content!
            addPage(new PreferencesPage());
            addPage(new CharactersPage());
            addPage(new JobsPage());
        }

        private void addPage(BasePreferencesPage page)
        {
            _tabs.AddChild(page);
            Console.WriteLine("Page: " + page.Name);
            _tabs.SetTabTitle(page.pagenumber = _tabCount++, page.Name);
        }


    }
}
