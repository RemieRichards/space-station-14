using Content.Shared.GameObjects.Components.Hydroponics;
using SS14.Client.GameObjects;
using SS14.Client.Interfaces.GameObjects.Components;
using SS14.Shared.Interfaces.GameObjects;

namespace Content.Client.GameObjects.Components.Hydroponics
{
    public class HydroponicsTrayVisualizer2D : AppearanceVisualizer
    {
        public override void InitializeEntity(IEntity entity)
        {
            base.InitializeEntity(entity);

            var sprite = entity.GetComponent<ISpriteComponent>();

            sprite.LayerMapSet(Layers.LowHealth,        sprite.AddLayerState("blank"));
            sprite.LayerMapSet(Layers.LowWater,         sprite.AddLayerState("blank"));
            sprite.LayerMapSet(Layers.LowNutrients,     sprite.AddLayerState("blank"));
            sprite.LayerMapSet(Layers.Harvestable,      sprite.AddLayerState("blank"));
            sprite.LayerMapSet(Layers.PestsOrToxins,    sprite.AddLayerState("blank"));

            sprite.LayerSetShader(Layers.LowHealth,     "unshaded");
            sprite.LayerSetShader(Layers.LowWater,      "unshaded");
            sprite.LayerSetShader(Layers.LowNutrients,  "unshaded");
            sprite.LayerSetShader(Layers.Harvestable,   "unshaded");
            sprite.LayerSetShader(Layers.PestsOrToxins, "unshaded");

        }

        public override void OnChangeData(AppearanceComponent component)
        {
            base.OnChangeData(component);

            var sprite = component.Owner.GetComponent<ISpriteComponent>();

            if (component.TryGetData(HydroponicsTrayVisuals.Powered, out bool powered) && powered)
            {

                if (component.TryGetData(HydroponicsTrayVisuals.LowHealth, out bool tru))
                {
                    if (tru)
                    {
                        sprite.LayerSetState(Layers.LowHealth, "alert-lowhealth");
                    }
                    else
                    {
                        sprite.LayerSetState(Layers.LowHealth, "blank");
                    }
                }

                if (component.TryGetData(HydroponicsTrayVisuals.LowWater, out tru))
                {
                    if (tru)
                    {
                        sprite.LayerSetState(Layers.LowWater, "alert-lowwater");
                    }
                    else
                    {
                        sprite.LayerSetState(Layers.LowWater, "blank");
                    }
                }

                if (component.TryGetData(HydroponicsTrayVisuals.LowNutrients, out tru))
                {
                    if (tru)
                    {
                        sprite.LayerSetState(Layers.LowNutrients, "alert-lownutrients");
                    }
                    else
                    {
                        sprite.LayerSetState(Layers.LowNutrients, "blank");
                    }
                }

                if (component.TryGetData(HydroponicsTrayVisuals.Harvestable, out tru))
                {
                    if (tru)
                    {
                        sprite.LayerSetState(Layers.Harvestable, "alert-harvestable");
                    }
                    else
                    {
                        sprite.LayerSetState(Layers.Harvestable, "blank");
                    }
                }

                if (component.TryGetData(HydroponicsTrayVisuals.PestOrToxins, out tru))
                {
                    if (tru)
                    {
                        sprite.LayerSetState(Layers.PestsOrToxins, "alert-pestsortoxins");
                    }
                    else
                    {
                        sprite.LayerSetState(Layers.PestsOrToxins, "blank");
                    }
                }
            }
            else
            {
                sprite.LayerSetState(Layers.LowHealth, "blank");
                sprite.LayerSetState(Layers.LowWater, "blank");
                sprite.LayerSetState(Layers.LowNutrients, "blank");
                sprite.LayerSetState(Layers.Harvestable, "blank");
                sprite.LayerSetState(Layers.PestsOrToxins, "blank");
            }
        }

        enum Layers
        {
            //Alert lights
            LowHealth,
            LowWater,
            LowNutrients,
            Harvestable,
            PestsOrToxins
        }
    }
}
