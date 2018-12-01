using System;
using SS14.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.Hydroponics
{
    [Serializable, NetSerializable]
    public enum HydroponicsTrayVisuals
    {
        //Power state
        Powered,

        //Alert lights
        LowWater,
        LowHealth,
        LowNutrients,
        Harvestable,
        PestOrToxins
    }
}
