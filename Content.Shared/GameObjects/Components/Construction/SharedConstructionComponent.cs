using System;
using SS14.Shared.GameObjects;

namespace Content.Shared.GameObjects
{
    public abstract class SharedConstructionComponent : Component
    {
        public override string Name => "Construction";
        public override uint? NetID => ContentNetIDs.CONSTRUCTION;
        public override Type StateType => typeof(ConstructionComponentState);
    }

    [Serializable]
    public class ConstructionComponentState : ComponentState
    {
        public readonly string Sprite;

        public ConstructionComponentState(string Sprite) : base(ContentNetIDs.CONSTRUCTION)
        {
            this.Sprite = Sprite;
        }
    }
}
