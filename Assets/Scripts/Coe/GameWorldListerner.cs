namespace Rogue.Coe
{
    public interface IGameWorldListerner
    {
        void OnEntityAdded(GameWorld world, GameEntity entity);

        void OnEntityRemoved(GameWorld world, GameEntity entity);

        void OnComponentAdded(GameWorld world, GameEntity entity, IGameComponent component);

        void OnComponentRemoved(GameWorld world, GameEntity entity, IGameComponent component);

        void OnBehaviourAdded(GameWorld world, GameEntity entity, IGameBehaviour behaviour);

        void OnBehaviourRemoved(GameWorld world, GameEntity entity, IGameBehaviour behaviour);
    }
}
