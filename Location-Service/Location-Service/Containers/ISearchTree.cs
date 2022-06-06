namespace Location_Service.Containers;

public interface ISearchTree<in TKey, TValue> where TKey : IComparable<TKey>
{
    public bool Contains(TKey key);
    public int Size();
    public TValue? Get(TKey key);
    public void Insert(TKey key, TValue value);
    public void Remove(TKey key);
}