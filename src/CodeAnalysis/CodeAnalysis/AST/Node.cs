namespace CodeAnalysis
{
    public abstract class Node
    {
        public int lineNumber { get; set; }

        public Node (int lineNumber)
        {
            this.lineNumber = lineNumber;
        }

        public override string ToString()
        {
            return "Line " + lineNumber.ToString() + ": ";
        }
    }
}
