namespace Hierarchy.Core
{
    using System.Collections.Generic;

    public class Node<T>
    {
        public Node(T value)
        {
            this.Value = value;
            this.Children = new List<Node<T>>();
        }

        public T Value { get; set; }

        public Node<T> Parent { get; set; }

        public IList<Node<T>> Children { get; }

        public void AddChild(Node<T> child)
        {
            child.Parent = this;
            this.Children.Add(child);
        }
    }
}