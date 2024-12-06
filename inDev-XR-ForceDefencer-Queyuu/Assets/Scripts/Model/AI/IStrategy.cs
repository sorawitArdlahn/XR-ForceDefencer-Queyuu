namespace Model.AI
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset();
    }
}