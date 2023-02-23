using Rogue;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rogue.Data {

[CreateAssetMenu]
public class ViewTable : ScriptableObject, Coe.IGameViewFactory
{
    public SimpleView[] views;

    public UnityEngine.Grid grid;

    public bool Contains(string name)
    {
        foreach (var view in views)
        {
            if (view.GetType().Name == name)
            {
                return true;
            }
        }
        
        return false;
    }

    public Coe.IGameView Create(string type, string name)
    {
        foreach (var view in views)
        {
            if (view.GetType().Name == type)
            {
                SimpleView instance = Instantiate(view);
                instance.grid = grid;

                return instance;
            }
        }

        return null;
    }
}

}