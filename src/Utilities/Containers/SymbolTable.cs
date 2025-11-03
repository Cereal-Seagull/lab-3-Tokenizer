/**
* Symbol table that maintains and manages key-value pairs where keys and values are 
* indivdually stored in double linked lists of keys and values. 
*
* Implements IDictionary<TKey, TValue> and ICollection Interfaces.
* 
* Test cases written by LLM Claude Sonnet 4 but reviewed
* and confirmed by authors.
*
* Bugs: Nullability warning when calling .Front()
*
* @author Charlie Moss and Will Zoeller
* @date 9/8/25
*/
using System.Collections;
using System.Globalization;
using System.Text;

/// <summary>
/// Represents a symbol table that maintains key-value pairs with hierarchical scope support.
/// Implements IDictionary&lt;TKey, TValue&gt; to provide standard dictionary functionality
/// while supporting parent-child relationships for nested scopes.
/// </summary>
/// <typeparam name="TKey">The type of keys in the symbol table</typeparam>
/// <typeparam name="TValue">The type of values in the symbol table</typeparam>
public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    protected DLL<TKey> _keys;
    protected DLL<TValue> _values;
    protected readonly SymbolTable<TKey, TValue>? _parent;

    /// <summary>
    /// Initializes a new instance of the SymbolTable class.
    /// Creates a new symbol table with a parent reference, establishing a scope hierarchy relationship.
    /// </summary>
    public SymbolTable()
    {
        _keys = new DLL<TKey>();
        _values = new DLL<TValue>();
        _parent = null;
    }

    /// <summary>
    /// Initializes a new instance of the SymbolTable class with a parent scope.
    /// Creates a child symbol table that references the specified parent, establishing 
    /// a hierarchical scope relationship for nested symbol resolution.
    /// </summary>
    /// <param name="parent">The parent SymbolTable that represents the enclosing scope</param>
    public SymbolTable(SymbolTable<TKey, TValue> parent)
    {
        _keys = new DLL<TKey>();
        _values = new DLL<TValue>();
        _parent = parent;
    }

    /// <summary>
    /// Gets the parent symbol table for accessing higher scopes in the hierarchy.
    /// This property is read-only and enables scope traversal in nested symbol table structures.
    /// </summary>
    /// <value>The parent SymbolTable instance that represents the enclosing scope</value>
    public SymbolTable<TKey, TValue>? Parent { get { return _parent; } }


    /// <summary>
    /// Creates a new child scope that references this symbol table as its parent.
    /// This method establishes a hierarchical relationship where the child scope can
    /// access symbols from this scope while maintaining its own local symbol definitions.
    /// </summary>
    /// <returns>A new SymbolTable instance with this table set as its parent</returns>
    public SymbolTable<TKey, TValue> CreateNewScope_GivenParent()
    {
        return new SymbolTable<TKey, TValue>(this);
    }

    #region SymbolTable methods

    /// <summary>
    /// Checks for key existence only in the current scope, without traversing up to parent scopes.
    /// This method provides local scope checking functionality for symbol resolution.
    /// </summary>
    /// <param name="key">The key to locate in the current scope</param>
    /// <returns>true if the key exists in the current scope; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null</exception>
    public bool ContainsKeyLocal(TKey key)
    {
        // Throws an ArgumentNullException when the key is null.
        if (key == null) throw new ArgumentNullException("Input key is null.");

        return this._keys.Contains(key);
    }

    /// <summary>
    /// Attempts to retrieve a value only from the current scope without searching parent scopes.
    /// This method provides local scope value retrieval for symbol resolution.
    /// </summary>
    /// <param name="key">The key whose value to get from the current scope</param>
    /// <param name="val">When this method returns, the value associated with the specified key, 
    /// if the key is found in the current scope; otherwise, the default value for the type of the value parameter</param>
    /// <returns>true if the key is found in the current scope; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null</exception>
    public bool TryGetValueLocal(TKey key, out TValue value)
    {
        // "val" is assigned the value corresponding to "key"
        // current = this.
        if (key == null) throw new ArgumentNullException("Key is null.");

        // Gets index of input key
        int idx = _keys.IndexOf(key);
        // If index does exist in CURRENT scope
        if (idx != -1)
        {
            value = _values[idx];
            return true;
        }
        // Not found, set default value and return false
        else
        {
            value = default;
            return false;
        }
    }

    #endregion
    #region IDictionary<TKey, TValue> Properties

    /// <summary>
    /// Gets the number of key-value pairs contained in the symbol table.
    /// </summary>
    /// <returns>The number of key-value pairs in the current scope</returns>
    private int Count()
    {
        return _keys.Count();
    }

    /// <summary>
    /// Gets a value indicating whether the symbol table is read-only.
    /// </summary>
    /// <returns>true if the symbol table is read-only; otherwise, false</returns>
    private bool IsReadOnly()
    {
        return _keys.IsReadOnly && _values.IsReadOnly;
    }

    /// <summary>
    /// Gets or sets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key whose value to get or set</param>
    /// <returns>The value associated with the specified key</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key is not found during get operations</exception>
    /// <exception cref="ArgumentNullException">Thrown when the key is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the key doesn't exist in SymbolTable</exception>
    public TValue this[TKey key]
    {
        get
        {
            int idx = _keys.IndexOf(key);
            if (idx == -1) throw new ArgumentOutOfRangeException("DLL index out of range.");
            return _values[idx];
        }
        set
        {
            int idx = _keys.IndexOf(key);
            if (idx == -1) throw new ArgumentOutOfRangeException("Cannot set value for index out of range.");
            _values[idx] = value;
        }
    }

    /// <summary>
    /// Gets a collection containing the keys in the symbol table.
    /// </summary>
    /// <value>An ICollection&lt;TKey&gt; containing the keys in the symbol table</value>
    public ICollection<TKey> Keys { get { return _keys; } }

    /// <summary>
    /// Gets a collection containing the values in the symbol table.
    /// </summary>
    /// <value>An ICollection&lt;TValue&gt; containing the values in the symbol table</value>
    public ICollection<TValue> Values { get { return _values; } }

    /// <summary>
    /// Gets the number of key-value pairs contained in the symbol table.
    /// </summary>
    /// <value>The number of key-value pairs in the current scope</value>
    int ICollection<KeyValuePair<TKey, TValue>>.Count => Count();

    /// <summary>
    /// Gets a value indicating whether the symbol table is read-only.
    /// </summary>
    /// <value>true if the symbol table is read-only; otherwise, false</value>
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => IsReadOnly();

    #endregion
    #region IDictionary<TKey, TValue> Methods

    /// <summary>
    /// Adds an element with the provided key and value to the symbol table.
    /// </summary>
    /// <param name="key">The key of the element to add</param>
    /// <param name="val">The value of the element to add</param>
    /// <exception cref="ArgumentNullException">Thrown when key is null</exception>
    /// <exception cref="ArgumentException">Thrown when an element with the same key already exists</exception>
    public void Add(TKey key, TValue val)
    {
        // IMPORTANT: what if key is already there? NO DUPLICATE KEYS
        var idx = _keys.IndexOf(key);
        // key doesn't exist
        if (idx == -1)
        {
            _keys.Add(key);
            _values.Add(val);
        }
        // duplicate key, replace value
        else
        {
            // Remove old value and add new value back in
            _values.RemoveAt(idx);
            _values.Insert(idx, val);
        }
    }

    /// <summary>
    /// Removes all key-value pairs from the symbol table.
    /// </summary>
    public void Clear()
    {
        _keys.Clear();
        _values.Clear();
    }

    /// <summary>
    /// Determines whether the symbol table contains a specific value.
    /// </summary>
    /// <param name="item">The value to locate in the symbol table</param>
    /// <returns>true if the value is found in the symbol table; otherwise, false</returns>
    public bool Contains(TValue item)
    {
        // If current scope has item, true
        if (_values.Contains(item)) return true;
        // If parent scope has item, true (recursive)
        else if (Parent != null) return Parent.Contains(item);
        return false;
    }

    /// <summary>
    /// Determines whether the symbol table contains an element with the specified key.
    /// </summary>
    /// <param name="item">The key to locate in the symbol table</param>
    /// <returns>true if the symbol table contains an element with the key; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null</exception>
    public bool ContainsKey(TKey item)
    {
        // If current scope has key, true
        if (_keys.Contains(item)) return true; 
        // If parent scope has key, true (recursive)
        else if (Parent != null) return Parent.ContainsKey(item);
        return false;
    }

    /// <summary>
    /// Removes the element with the specified key from the symbol table.
    /// </summary>
    /// <param name="key">The key of the element to remove</param>
    /// <returns>true if the element is successfully removed; otherwise, false. 
    /// This method also returns false if key was not found in the symbol table</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null</exception>
    public bool Remove(TKey key)
    {
        // Retrieve index of key
        int idx = _keys.IndexOf(key);

        // Index/key does not exist
        if (idx == -1) return false;

        // Remove key and value at same index
        _keys.RemoveAt(idx);
        _values.RemoveAt(idx);
        return true;
    }

    /// <summary>
    /// Returns a string representation of the symbol table showing all key-value pairs.
    /// The format is "{ key1 : value1, key2 : value2, ... }" or "{ }" for empty tables.
    /// </summary>
    /// <returns>A string that represents the current symbol table</returns>
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append("{ ");
        for (int i = 0; i < _keys.Size(); i++)
        {
            str.Append($"{_keys[i]} : {_values[i]}");
            if (i != _keys.Size() - 1) str.Append(", ");
            else str.Append(" ");
        }
        str.Append("}");

        return str.ToString();
    }

    /// <summary>
    /// Gets the value associated with the specified key.
    /// If the key is not found, returns the front value from the values collection as a default.
    /// </summary>
    /// <param name="key">The key whose value to get</param>
    /// <param name="value">When this method returns, the value associated with the specified key, 
    /// if the key is found; otherwise, the front value from the values collection</param>
    /// <returns>true if the symbol table contains an element with the specified key; otherwise, false</returns>
    /// <exception cref="ArgumentNullException">Thrown when key is null</exception>
    public bool TryGetValue(TKey key, out TValue value)
    {
        // Gets index of input key
        int idx = _keys.IndexOf(key);
        // If index does exist in CURRENT scope
        if (idx != -1)
        {
            value = _values[idx];
            return true;
        }
        // Check parent scope (recursive)
        else if (Parent != null) return Parent.TryGetValue(key, out value);
        // Not found; set value to default value
        else
        {
            value = default;
            return false;
        }
    }

    #endregion
    #region ICollection stuff

    /// <summary>
    /// Adds a key-value pair to the symbol table.
    /// </summary>
    /// <param name="item">The KeyValuePair&lt;TKey, TValue&gt; to add to the symbol table</param>
    /// <exception cref="ArgumentNullException">Thrown when the key of the item is null</exception>
    /// <exception cref="ArgumentException">Thrown when an element with the same key already exists</exception>
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    /// <summary>
    /// Determines whether the symbol table contains a specific KeyValuePair.
    /// </summary>
    /// <param name="item">The KeyValuePair&lt;TKey, TValue&gt; to locate in the symbol table</param>
    /// <returns>true if the KeyValuePair is found in the symbol table; otherwise, false</returns>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        for (int i = 0; i < Count(); i++)
        {
            var kvp = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
            // Note: address check vs value check
            // If item found in CURRENT scope, true
            if (EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(item, kvp)) return true;
        }
        // If item found in PARENT scope, true (recursive)
        if (Parent != null) return Parent.Contains(item);

        return false;
    }

    /// <summary>
    /// Copies the key-value pairs of the symbol table to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the key-value pairs copied from the symbol table</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when arrayIndex is less than 0</exception>
    /// <exception cref="ArgumentException">Thrown when the number of elements in the source symbol table 
    /// is greater than the available space from arrayIndex to the end of the destination array</exception>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        // Index must be positive and array can't be null.
        if (array == null) throw new ArgumentNullException("Target array is null");
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Index is negative.");
        // Must be enough space in array to fit list
        if (array.Length - arrayIndex < _keys.Count()) throw new ArgumentException($"Not enough space in array {array} to accomodate all KeyValuePairs in SymbolTable.");


        for (int i = 0; i < Count(); i++)
        {
            var kvp = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
            array[arrayIndex] = kvp;
            arrayIndex++;
        }
        
    }

    /// <summary>
    /// Removes the first occurrence of a specific KeyValuePair from the symbol table.
    /// </summary>
    /// <param name="item">The KeyValuePair&lt;TKey, TValue&gt; to remove from the symbol table</param>
    /// <returns>true if the KeyValuePair was successfully removed; otherwise, false. 
    /// This method also returns false if item is not found in the symbol table</returns>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        for (int i = 0; i < Count(); i++)
        {
            var kvp = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
            if (EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(item, kvp))
            {
                // maybe required like this? - would we want to remove ("abc", 123) when given ("abc", 124)?
                return Remove(kvp.Key);
            }
        }
        return false;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the key-value pairs in the symbol table.
    /// </summary>
    /// <returns>An IEnumerator&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; that can be used to iterate through the symbol table</returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (TKey key in _keys) {
            // is this O(1)?
            yield return new KeyValuePair<TKey, TValue>(key, this[key]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the symbol table.
    /// This is the non-generic version of GetEnumerator for IEnumerable compatibility.
    /// </summary>
    /// <returns>An IEnumerator that can be used to iterate through the symbol table</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}