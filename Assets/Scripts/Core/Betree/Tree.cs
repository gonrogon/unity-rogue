namespace Rogue.Core.Betree
{
    public class Tree
    {
        public Blackboard Blackboard { get; private set; } = new ();

        public Node Root { get; private set; } = null;

        public Tree(Node root)
        {
            Root = root;
            Root.OnAttached(this, null);
        }

        public void Tick() => Root.Tick();
    }
}
