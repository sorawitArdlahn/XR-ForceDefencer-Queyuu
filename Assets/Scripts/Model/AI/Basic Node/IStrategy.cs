namespace AI.Basic_Node
{
        public interface IStrategy
        {
                Node.Status Process();

                void Reset()
                {
                        // No-Op
                }
        }
}
