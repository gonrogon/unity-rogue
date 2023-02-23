namespace Rogue.Coe
{
    public interface IGameView
    {
        bool OnSetup(GameWorld world, GameEntity entity);

        void OnInit();

        void OnQuit();

        void OnMessage(IGameMessage message);
    }
}
