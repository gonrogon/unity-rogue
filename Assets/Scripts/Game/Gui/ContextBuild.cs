using UnityEngine;
using GG.Mathe;

namespace Rogue.Game.Gui
{
    public class ContextBuild : Rogue.Gui.Context
    {
        private enum State { Start, Select, Finish }

        private State mState = State.Start;

        private Vec2i mStart;

        private Rect2i mArea;

        private MapView mMap;

        private Input mInput;

        public Rect2i Selection => mArea;

        public ContextBuild(Input input, MapView map)
        {
            mInput = input;
            mMap   = map;
        }

        protected override void OnStart()
        {
            mState = State.Start;
        }

        protected override void OnAction(string action)
        {
            switch (mState)
            {
                case State.Start:
                {
                    if (action == "press")
                    {
                        mStart = GetMapCoord();
                        mState = State.Select;
                    }
                }
                break;

                case State.Select: 
                {
                    if (action == "release")
                    {
                        mState = State.Finish;
                    }
                }
                break;
            }
        }

        protected override bool OnUpdate(float time)
        {
            switch (mState)
            {
                case State.Select:
                {
                    mArea.SetFromPointsEncompassed(mStart, GetMapCoord());
                    Notify();
                }
                break;
            }

            return mState == State.Finish;
        }

        private Vec2i GetMapCoord() => GetMapCoord(mInput.Pointer);

        private Vec2i GetMapCoord(Vector2 cursor)
        {
            Vector2 world = Camera.main.ScreenToWorldPoint(new Vector3(cursor.x, cursor.y, -10.0f));
            Vec2i   map   = mMap.GetCursorCell(world);

            return map;
        }
    }
}
