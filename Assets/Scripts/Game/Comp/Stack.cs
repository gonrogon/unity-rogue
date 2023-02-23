using UnityEngine;
using Rogue.Coe;
using Newtonsoft.Json;

namespace Rogue.Game.Comp
{
    public class Stack : GameComponent<Stack>
    {
        public int amount = 1;

        public int size = 100;
    }
}
