using Content.Server.GameObjects.Components.Power;
using Content.Shared.GameObjects.Components.Hydroponics;
using SS14.Server.GameObjects;
using SS14.Shared.GameObjects;

namespace Content.Server.GameObjects.Components.Hydroponics
{
    /// <summary>
    /// Hydroponics Tray, a type of <see cref="PlantHolderComponent"/> but requires power in exchange for providing "alert" lights about its plant's condition
    /// </summary>
    public class HydroponicsTrayComponent : PlantHolderComponent
    {
        public override string Name => "HydroponicsTray";

        PowerDeviceComponent Power;
        AppearanceComponent Appearance;

        public override void Initialize()
        {
            base.Initialize();

            Power = Owner.GetComponent<PowerDeviceComponent>();
            Appearance = Owner.GetComponent<AppearanceComponent>();

        }

        public override void OnUpdate(float frameTime)
        {
            base.OnUpdate(frameTime);
            if (Power.Powered)
            {
                Appearance.SetData(HydroponicsTrayVisuals.Powered, true);

                if(Plant != null)
                {
                    Appearance.SetData(HydroponicsTrayVisuals.LowHealth, Plant.IsLowHealth);
                    Appearance.SetData(HydroponicsTrayVisuals.Harvestable, Plant.Harvestable);
                }
                else
                {
                    Appearance.SetData(HydroponicsTrayVisuals.LowHealth, false);
                    Appearance.SetData(HydroponicsTrayVisuals.Harvestable, false);
                }

                Appearance.SetData(HydroponicsTrayVisuals.LowWater, IsLowWater);
                Appearance.SetData(HydroponicsTrayVisuals.LowNutrients, IsLowNutrients);

                bool warning = IsHighPests || IsHighWeeds;
                Appearance.SetData(HydroponicsTrayVisuals.PestOrToxins, warning);
            }
            else
            {
                Appearance.SetData(HydroponicsTrayVisuals.Powered, false);
            }
        }
    }
}
