using System.Linq;
using UnityEngine;

namespace Rogue.Data {

[CreateAssetMenu]
public class SpriteTable : ScriptableObject
{
    public SpriteDefinition[] sprites;

    public bool TryFind(string name, out Sprite sprite)
    {
        sprite = null;

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].name == name)
            {
                sprite = sprites[i].sprite;
                return true;
            }
        }

        return false;
    }
}

}