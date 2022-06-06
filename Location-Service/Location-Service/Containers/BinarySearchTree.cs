namespace Location_Service.Containers;

public class BinarySearchTree<TKey, TValue> : ISearchTree<TKey, TValue> where TKey : IComparable<TKey>
{
    private int _size = 0;
    private Node? _root;

    public class Node
    {
        public TKey Key { get; }
        public TValue Value { get; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public bool Contains(TKey key)
    {
        return Get(key) != null;
    }

    public int Size()
    {
        return _size;
    }

    public TValue? Get(TKey key)
    {
        return Get(_root, key);
    }
    
    public void Insert(TKey key, TValue value)
    {
        if (_root == null)
        {
            _root = new Node(key, value);
            _size++;
        }

        var curr = _root;
        var parent = _root;

        while (curr != null)
        {
            parent = curr;
            
            int cmp = key.CompareTo(curr.Key);

            if (cmp < 0)
                curr = curr.Left;
            else if (cmp > 0)
                curr = curr.Right;
            else
                return;
        }
        
        if (key.CompareTo(parent.Key) < 0)
            parent.Left = new Node(key, value);
        else if (key.CompareTo(parent.Key) > 0)
            parent.Right = new Node(key, value);

        _size++;
    }
    
    public void Remove(TKey key)
    {
        if (_root == null)
            return;
        
        var current = _root;
        var parent = _root;
        
        bool isLeftChild = false;

        while (key.CompareTo(current.Key) != 0)
        {
            parent = current;
            if (key.CompareTo(current.Key) < 0)
            {
                current = current.Left;
                isLeftChild = true;
            }
            else
            {
                current = current.Right;
                isLeftChild = false;
            }

            if (current == null)
                return;
        }

        if (current.Left == null && current.Right == null)
        {
            if (current == _root)
                _root = null;
            else if (isLeftChild)
                parent.Left = null;
            else
                parent.Right = null;
        }
        else if (current.Left == null)
        {
            if (current == _root)
                _root = current.Right;
            else if (isLeftChild)
                parent.Left = current.Right;
            else
                parent.Right = current.Right;
        }
        else if (current.Right == null)
        {
            if (current == _root)
                _root = current.Left;
            else if (isLeftChild)
                parent.Left = current.Left;
            else
                parent.Right = current.Left;
        }
        else
        {
            var successor = FindSuccessor(current);
            if (current == _root)
                _root = successor;
            else if (isLeftChild)
                parent.Left = successor;
            else
                parent.Right = successor;

            successor.Left = current.Left;
        }
    }

    private static TValue? Get(Node? node, TKey key)
    {
        while (true)
        {
            if (node == null)
                return default;

            int cmp = key.CompareTo(node.Key);

            switch (cmp)
            {
                case < 0:
                    node = node.Left;
                    break;
                case > 0:
                    node = node.Right;
                    break;
                default:
                    return node.Value;
            }
        }
    }

    private static Node FindSuccessor(Node node)
    {
        Node successor = node;
        Node successorParent = node;

        Node? current = node.Right;
        while(current != null){
            successorParent = successor;
            successor = current;
            current = current.Left;
        }

        if(successor != node.Right){
            successorParent.Left = successor.Right;
            successor.Right = node.Right;
        }
        
        return successor;
    }
}