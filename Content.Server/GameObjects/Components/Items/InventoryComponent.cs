using Content.Server.Interfaces.GameObjects;
using SS14.Server.GameObjects;
using SS14.Server.GameObjects.Components.Container;
using SS14.Server.Interfaces.GameObjects;
using SS14.Shared.Utility;
using SS14.Shared.GameObjects;
using SS14.Shared.Interfaces.GameObjects;
using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Content.Server.GameObjects
{
    public class InventoryComponent : Component, IInventoryComponent
    {
        public override string Name => "Inventory";

        private Dictionary<string, InventorySlot> slots = new Dictionary<string, InventorySlot>();
        private TransformComponent transform;
        // TODO: Make this container unique per-slot.
        private IContainer container;

        public override void Initialize()
        {
            transform = Owner.GetComponent<TransformComponent>();
            container = Container.Create("inventory", Owner);
            base.Initialize();
        }

        public override void OnRemove()
        {
            foreach (var slot in slots.Keys)
            {
                RemoveSlot(slot);
            }
            transform = null;
            container = null;
            base.OnRemove();
        }

        public override void LoadParameters(YamlMappingNode mapping)
        {
            if (mapping.TryGetNode<YamlSequenceNode>("slots", out var slotsNode))
            {
                foreach (var node in slotsNode)
                {
                    AddSlot(node.AsString());
                }
            }
            base.LoadParameters(mapping);
        }

        public IItemComponent Get(string slot)
        {
            return _GetSlot(slot).Item;
        }

        public IInventorySlot GetSlot(string slot)
        {
            return slots[slot];
        }

        // Private version that returns our concrete implementation.
        private InventorySlot _GetSlot(string slot)
        {
            return slots[slot];
        }

        public bool Insert(string slot, IItemComponent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "An item must be passed. To remove an item from a slot, use Drop()");
            }

            var inventorySlot = _GetSlot(slot);
            if (!CanInsert(slot, item) || !container.Insert(item.Owner))
            {
                return false;
            }

            inventorySlot.Item = item;
            item.EquippedToSlot(inventorySlot);
            return true;
        }

        public bool CanInsert(string slot, IItemComponent item)
        {
            var inventorySlot = _GetSlot(slot);
            return inventorySlot.Item == null && container.CanInsert(item.Owner);
        }

        public bool Drop(string slot)
        {
            if (!CanDrop(slot))
            {
                return false;
            }

            var inventorySlot = _GetSlot(slot);
            var item = inventorySlot.Item;
            if (!container.Remove(item.Owner))
            {
                return false;
            }

            item.RemovedFromSlot();
            inventorySlot.Item = null;

            // TODO: The item should be dropped to the container our owner is in, if any.
            var itemTransform = item.Owner.GetComponent<TransformComponent>();
            itemTransform.LocalPosition = transform.LocalPosition;
            return true;
        }

        public bool CanDrop(string slot)
        {
            var inventorySlot = _GetSlot(slot);
            var item = inventorySlot.Item;
            return item != null && container.CanRemove(item.Owner);
        }

        public IInventorySlot AddSlot(string slot)
        {
            if (HasSlot(slot))
            {
                throw new InvalidOperationException($"Slot '{slot}' already exists.");
            }

            return slots[slot] = new InventorySlot(slot, this);
        }

        public void RemoveSlot(string slot)
        {
            if (!HasSlot(slot))
            {
                throw new InvalidOperationException($"Slow '{slot}' does not exist.");
            }

            if (Get(slot) != null && !Drop(slot))
            {
                // TODO: Handle this potential failiure better.
                throw new InvalidOperationException("Unable to remove slot as the contained item could not be dropped");
            }

            slots.Remove(slot);
        }

        public bool HasSlot(string slot)
        {
            return slots.ContainsKey(slot);
        }

        private class InventorySlot : IInventorySlot
        {
            public IItemComponent Item { get; set; }
            public string Name { get; }
            public IInventoryComponent Owner { get; }

            public InventorySlot(string name, IInventoryComponent owner)
            {
                Name = name;
                Owner = owner;
            }
        }
    }
}
