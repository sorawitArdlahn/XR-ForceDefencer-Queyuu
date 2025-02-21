namespace AI.Basic_Node.Control_Node
{
    public class Selector : Node
    {
        public Selector(string name, int priority = 0) : base(name, priority){}

        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        currentChild++;
                        return Status.Running;
                }
            }
            Reset();
            return Status.Failure;
        }
    }
}
