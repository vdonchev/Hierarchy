namespace Hierarchy.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Hierarchy<T> : IHierarchy<T>
    {
        private readonly Dictionary<T, Node<T>> nodes;
        private readonly Node<T> root;

        public Hierarchy(T root)
        {
            this.root = new Node<T>(root);
            this.nodes = new Dictionary<T, Node<T>>
            {
                { root, this.root }
            };
        }

        public int Count
        {
            get
            {
                return this.nodes.Count;
            }
        }

        public void Add(T element, T child)
        {
            if (!this.nodes.ContainsKey(element) ||
                this.nodes.ContainsKey(child))
            {
                throw new ArgumentException();
            }

            var parent = this.nodes[element];
            var newNode = new Node<T>(child);

            parent.AddChild(newNode);
            this.nodes.Add(child, newNode);
        }

        public void Remove(T element)
        {
            Node<T> elementNode;
            if (!this.nodes.TryGetValue(element, out elementNode))
            {
                throw new ArgumentException();
            }

            if (elementNode.Parent == null)
            {
                throw new InvalidOperationException();
            }

            elementNode.Parent.Children.Remove(elementNode);
            foreach (var child in elementNode.Children)
            {
                elementNode.Parent.AddChild(child);
            }

            this.nodes.Remove(element);
        }

        public IEnumerable<T> GetChildren(T item)
        {
            Node<T> elementNode;
            if (!this.nodes.TryGetValue(item, out elementNode))
            {
                throw new ArgumentException();
            }

            return elementNode.Children.Select(e => e.Value);
        }

        public T GetParent(T item)
        {
            Node<T> elementNode;
            if (!this.nodes.TryGetValue(item, out elementNode))
            {
                throw new ArgumentException();
            }

            if (elementNode.Parent == null)
            {
                return default(T);
            }

            return elementNode.Parent.Value;
        }

        public bool Contains(T value)
        {
            return this.nodes.ContainsKey(value);
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            var set = new HashSet<T>(this);
            set.IntersectWith(other);

            return set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var bfsQueue = new Queue<Node<T>>();
            bfsQueue.Enqueue(this.root);
            while (bfsQueue.Count > 0)
            {
                var node = bfsQueue.Dequeue();
                foreach (var child in node.Children)
                {
                    bfsQueue.Enqueue(child);
                }

                yield return node.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}