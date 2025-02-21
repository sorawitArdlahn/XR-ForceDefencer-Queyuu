namespace AI.Basic_Node.Control_Node
{
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority){}

        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    default:
                        currentChild++;
                        return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }
            Reset();
            return Status.Success;
        }
    }
}
