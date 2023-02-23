using UnityEngine;
using Rogue.Coe;

namespace Rogue
{
    public class SimpleView : MonoBehaviour, IGameView
    {
        public Data.SpriteTable spriteTable;

        public Grid grid;

        public Sprite sprite;

        private SpriteRenderer m_renderer;

        private Game.Comp.Location m_loc;

        private Game.Comp.View m_view;

        GameWorld m_world;

        GameEntity m_entity;



        private void Awake()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            m_renderer.sprite = sprite;
        }

        // ---------
        // GAME VIEW
        // ---------

        public bool OnSetup(GameWorld world, GameEntity entity)
        {
            m_world  = world;
            m_entity = entity;
            m_loc    = entity.FindFirstComponent<Game.Comp.Location>();
            m_view   = entity.FindFirstComponent<Game.Comp.View>();

            if (m_view != null)
            {
                if (spriteTable.TryFind(m_view.sprite, out Sprite entitySprite))
                {
                    m_renderer.sprite = sprite = entitySprite;
                }

            }

            var name  = entity.FindFirstComponent<Game.Comp.Name>();
            if (name != null)
            {
                gameObject.name = name.name;
            }

            return true;
        }

        public void OnInit()
        {
            UpdateWorldPosition();
        }

        public void OnQuit()
        {
            Destroy(gameObject);
        }

        public void OnMessage(IGameMessage message)
        {
            UpdateWorldPosition();

            if (message is Game.Msg.PickUp pm)
            {
                if (pm.what == m_entity.Id)
                {
                    m_renderer.enabled = false;
                }
            }

            if (message is Game.Msg.Drop)
            {
                m_renderer.enabled = true;
            }
        }

        private void UpdateWorldPosition()
        {
            transform.position   = grid.GetCellCenterWorld(new Vector3Int(m_loc.position.x, m_loc.position.y, 0));
            transform.localScale = grid.transform.lossyScale;
        }
    }
}
