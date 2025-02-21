using System.Collections.Generic;
using System.Linq;

namespace AI.Basic_Node.Control_Node
{
    public class RandomSelector : PrioritySelector
    {
        protected override List<Node> SortChildren() => children.Shuffle().ToList();

        public RandomSelector(string name, int priority = 0) : base(name, priority){}
    
    }
}
