namespace DEiXTo.Models
{
    public interface IConstraint
    {
        bool Evaluate(NodeInfo instance);
        string Value { get; }
    }
}
