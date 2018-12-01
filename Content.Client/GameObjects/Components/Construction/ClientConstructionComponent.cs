using Content.Shared.GameObjects;
using Lidgren.Network;
using SS14.Client.GameObjects;
using SS14.Shared.GameObjects;
using SS14.Shared.GameObjects.Serialization;
using SS14.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Client.GameObjects
{
    class ClientConstructionComponent : SharedConstructionComponent
    {
        private SpriteComponent spriteComponent;

        public override void Initialize()
        {
            base.Initialize();
            spriteComponent = Owner.GetComponent<SpriteComponent>();
        }

        public override void OnRemove()
        {
            spriteComponent = null;
        }

        public override void HandleComponentState(ComponentState state)
        {
            var castState = (ConstructionComponentState)state;
            if(!spriteComponent.HasSprite(castState.Sprite)){
                spriteComponent.AddSprite(castState.Sprite);
            }
            spriteComponent.SetSpriteByKey(castState.Sprite);
        }

    }
}
