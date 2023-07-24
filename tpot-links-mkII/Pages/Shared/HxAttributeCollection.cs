using System.Collections;

public class HxAttributeCollection : ICollection<HxAttribute>
{
    private HxAttributeCollection(params HxAttribute[] attributes)
    {
        foreach (var attribute in attributes.Where(hxAttribute => !Contains(hxAttribute)))
        {
            Add(attribute);
        }
    }

    public static HxAttributeCollection Create(params HxAttribute [] attributes) => new(attributes);

    public IEnumerator<HxAttribute> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(HxAttribute item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(HxAttribute item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(HxAttribute[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(HxAttribute item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
}