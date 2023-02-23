using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Views
{
    [CreateAssetMenu(menuName = "Views/Sprite Table")]
    public class SpriteTable : ScriptableObject
    {
        [SerializeField]
        private SpriteDefinition[] m_sprites;

        /// <summary>
        /// Try to get a sprite.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="sprite">Sprite.</param>
        /// <returns>True if the sprite exists; otherwise, null.</returns>
        public bool TryGet(string name, out Sprite sprite)
        {
            sprite = null;

            foreach (var def in m_sprites)
            {
                if (def.name == name)
                {
                    sprite = def.sprite;
                    return true;
                }
            }

            return false;
        }
    }
}
