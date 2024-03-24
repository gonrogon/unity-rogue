using Rogue.Core;
using Rogue.Coe;

namespace Rogue.Game
{
    public class InventoryWorldListener : IGameWorldListerner
    {
        private InventorySystem m_inventories;

        public InventoryWorldListener(InventorySystem inventories) 
        {
            m_inventories = inventories;
        }

        #region @@@ WORLD LISTENER @@@

        public void OnTemplateLoaded(GameWorld world, Template template) {}

        public void OnEntityAdded(GameWorld world, GameEntity entity) 
        {
            if (!entity.ContainsComponent<Comp.Inventory>())
            {
                return;
            }

            SetInventory(entity);
        }

        public void OnEntityRemoved(GameWorld world, GameEntity entity) 
        {
            if (!entity.ContainsComponent<Comp.Inventory>())
            {
                return;
            }

            RemoveInventory(entity);
        }

        public void OnComponentAdded(GameWorld world, GameEntity entity, IGameComponent component)
        {
            if (component is not Comp.Inventory inventory) 
            {
                return;
            }

            SetInventory(entity, inventory);
        }

        public void OnComponentRemoved(GameWorld world, GameEntity entity, IGameComponent component)
        {
            if (component is not Comp.Inventory inventory)
            {
                return;
            }

            RemoveInventory(entity, inventory);
        }

        public void OnBehaviourAdded(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        public void OnBehaviourRemoved(GameWorld world, GameEntity entity, IGameBehaviour behaviour) {}

        #endregion

        private void SetInventory(GameEntity entity) => SetInventory(entity, entity.FindFirstComponent<Comp.Inventory>());

        private void SetInventory(GameEntity entity, Comp.Inventory inventory)
        {
            if (!entity.IsRunning)
            {
                return;
            }

            if (inventory.iid.IsZero)
            {
                inventory.iid = m_inventories.Add();
            }
            else
            {
                UnityEngine.Debug.Log(entity.Id.Value + " - " + Query.GetName(entity.Id).value + ": Inventory already asigned");
            }
        }

        private void RemoveInventory(GameEntity entity) => RemoveInventory(entity, entity.FindFirstComponent<Comp.Inventory>());

        private void RemoveInventory(GameEntity entity, Comp.Inventory inventory)
        {
            if (!entity.IsRunning)
            {
                return;
            }

            if (inventory.iid.IsZero)
            {
                return;
            }

            m_inventories.Remove(inventory.iid);
            inventory.iid = Ident.Zero;
        }
    }
}
