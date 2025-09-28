/**
* DLL Methods for various tasks.
* Private DNode method for implementation.
*
* Test cases written by LLM Claude Sonnet 4 but reviewed
* and confirmed by authors.
*
* Bugs: several "Cannot convert null literal to non-nullable reference type" errors.
*
* @author Charlie Moss and Will Zoeller
* @date 9/8/25
*/
using System.Collections;
using System.Text;
public class DLL<T> : IEnumerable<T>, IList<T>
{
    private class DNode
    {
        // Instance variables can be accessed by anyone
        // but modified only by DLL class
        public DNode? Prev
        { get; internal set; }
        public T? Value
        { get; internal set; }
        public DNode? Next
        { get; internal set; }

        public DNode(DNode? left, T? val, DNode? right)
        {
            Prev = left;
            Value = val;
            Next = right;
        }
    }

    private DNode _head;
    private DNode _tail;
    private int _size;

    /// <summary>
    /// Constructor that properly initializes an instance of this class with sentinel nodes.
    /// </summary>
    public DLL()
    {
        _head = new DNode(null, default, null);
        _tail = new DNode(_head, default, null);
        _head.Next = _tail;
        _size = 0;
    }

    /// <summary>
    /// Private helper method that inserts a new node with the specified item BEFORE the given node.
    /// </summary>
    /// <param name="node">The node before which to insert the new item</param>
    /// <param name="item">The item to insert</param>
    /// <exception cref="ArgumentException">Thrown when the node is a sentinel node</exception>
    private void Insert(DNode node, T item)
    {
        // Node to be inserted needs to be inside the list
        if (node.Prev == null) throw new ArgumentException($"Node {node} must be valid: " +
                                                            "cannot be sentinel node.");

        // Create new node; placed before the given node
        var itemNode = new DNode(node.Prev, item, node);

        // Redirect pointers to the new node
        node.Prev.Next = itemNode;
        node.Prev = itemNode;

        // Increase size
        _size++;
    }


    /// <summary>
    /// Private helper method that removes the specified node from the list.
    /// </summary>
    /// <param name="node">The node to remove from the list</param>
    /// <exception cref="ArgumentException">Thrown when the node is a sentinel node</exception>
    private void Remove(DNode node)
    {
        // Nodes to be removed must be inside list
        if (node.Prev == null || node.Next == null)
            throw new ArgumentException($"Node {node} must be valid: cannot be sentinel node.");

        // Redirect nodes around removed node to make it inaccessible
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;

        // Decrease size
        _size--;
    }

    /// <summary>
    /// Private helper method that returns the node at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the node to retrieve</param>
    /// <returns>The node at the specified index</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative or out of range</exception>
    /// <exception cref="ArgumentException">Thrown when the list is empty</exception>
    private DNode? GetNode(int index)
    {
        // Index must be between 0 and list size
        if (index < 0) throw new ArgumentOutOfRangeException("Index is negative.");
        if (index >= Size()) throw new ArgumentOutOfRangeException("Index out of range.");
        // Useful for debugging
        if (IsEmpty()) throw new ArgumentException("List is empty.");

        // Current node starts at 1st valid list item
        int currIdx = 0;
        DNode? curr = _head.Next;

        // Stop iterating when target index is reached or when at end of list
        while (currIdx != index)
        {
            curr = curr.Next;
            currIdx++;
        }

        return curr;
    }

    // ADD TEST CASES
    /// <summary>
    /// Determines whether the list contains the specified item.
    /// </summary>
    /// <param name="item">The item to search for in the list</param>
    // /// <returns>True if the item is found in the list; otherwise, false</returns>
    // public int GetIndex(T item)
    // {
    //     var curr = _head.Next;
    //     for (int i=0; i < Size(); i++)
    //     {
    //         if (EqualityComparer<T>.Default.Equals(item, curr.Value)) return i;
    //         curr = curr.Next;
    //     }
    //     return -1;
    // }

    /// <summary>
    /// Determines whether the list contains the specified item.
    /// </summary>
    /// <param name="item">The item to search for in the list</param>
    /// <returns>True if the item is found in the list; otherwise, false</returns>
    public bool Contains(T item)
    {
        // Start checking at first node
        // var curr = _head.Next;

        // // Iterate until end of list is reached
        // for (int i = 0; i < Size(); i++)
        // {
        //     // Compare current node's value to input value
        //     // Return true if match, iterate through next node if false
        //     if (EqualityComparer<T>.Default.Equals(item, curr.Value)) return true;
        //     curr = curr.Next;
        // }
        // // Not found
        // return false;
        return IndexOf(item) != -1;
    }

    /// <summary>
    /// Returns the number of items in the list.
    /// </summary>
    /// <returns>The number of items in the list</returns>
    public int Size()
    {
        return _size;
    }

    /// <summary>
    /// Returns a string representation of the list for debugging.
    /// </summary>
    /// <returns>A string representation of the list in the format "[ item1, item2, ... ]"</returns>
    public override string ToString()
    {
        // Initialize new StringBuilder
        var str = new StringBuilder();

        // Start building the string node by node
        // Format: "[ 1, 2, 3, 4, 5 ]"
        str.Append("[ ");
        DNode? curr = _head.Next;
        while (curr != _tail)
        {
            // Value and no comma for last value in the list
            if (curr == _tail.Prev) str.Append(curr.Value + " ");

            // Value and comma
            else str.Append(curr.Value + ", ");

            curr = curr.Next;
        }

        str.Append(']');
        // Converts final StringBuilder construction to string
        return str.ToString();
    }

    /// <summary>
    /// Deletes the first occurrence of item in the list; returns whether deletion occurred or not.
    /// </summary>
    /// <param name="item">The item to remove from the list</param>
    /// <returns>True if the item was found and removed; otherwise, false</returns>
    public bool Remove(T item)
    {
        // False when list is empty
        if (IsEmpty()) return false;

        // // Iterate through list using current
        // DNode? current = _head.Next;
        // while (current != _tail)
        // {
        //     // If values match, remove the node and return
        //     if (EqualityComparer<T>.Default.Equals(current.Value, item))
        //     {
        //         Remove(current);
        //         return true;
        //     }
        //     current = current.Next;
        // }
        // // Couldn't find and remove element
        // return false;
        int idx = IndexOf(item);
        if (idx != -1)
        {
            Remove(GetNode(idx));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the element stored at the first valid node in the list.
    /// </summary>
    /// <returns>The element at the front of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty</exception>
    public T? Front()
    {
        // List can't be empty
        if (IsEmpty()) throw new InvalidOperationException("No 'initial' element in empty list.");

        // Return value after head node
        return _head.Next.Value;
    }

    /// <summary>
    /// Returns the element stored at the last valid node in the list.
    /// </summary>
    /// <returns>The element at the back of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty</exception>
    public T? Back()
    {
        // List can't be empty
        if (IsEmpty()) throw new InvalidOperationException("No 'final' element in empty list.");

        // Return value before tail node
        return _tail.Prev.Value;
    }

    /// <summary>
    /// Adds the element to the beginning of the list.
    /// </summary>
    /// <param name="item">The item to add to the front of the list</param>
    public void PushFront(T item)
    {
        Insert(_head.Next, item);
    }

    /// <summary>
    /// Adds the element to the end of the list.
    /// </summary>
    /// <param name="item">The item to add to the back of the list</param>
    public void PushBack(T item)
    {
        Insert(_tail, item);
    }

    /// <summary>
    /// Removes and returns the element stored at the first valid DNode in the list.
    /// </summary>
    /// <returns>The element that was removed from the front of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty</exception>
    public T? PopFront()
    {
        // Throw exception if list is empty
        if (IsEmpty()) throw new InvalidOperationException("No element to remove in empty list.");

        // Get first node & value in list and remove node
        var node = GetNode(0);
        var nodeVal = node.Value;
        Remove(node);

        return nodeVal;
    }

    /// <summary>
    /// Removes and returns the element stored at the last valid DNode in the list.
    /// </summary>
    /// <returns>The element that was removed from the back of the list</returns>
    /// <exception cref="InvalidOperationException">Thrown when the list is empty</exception>
    public T? PopBack()
    {
        // Throw exception if list is empty
        if (IsEmpty()) throw new InvalidOperationException("No element to remove in empty list.");

        // Get last node & value in list and remove node
        var nodeVal = _tail.Prev.Value;
        Remove(_tail.Prev);

        return nodeVal;
    }

    /// <summary>
    /// Removes all nodes in the list (except the sentinel nodes).
    /// </summary>
    public void Clear()
    {
        // Redirect head and tail pointers to each other
        // Garbage collection will remove all other nodes
        _head.Next = _tail;
        _tail.Prev = _head;
        // Set size to 0
        _size = 0;
    }

    /// <summary>
    /// Returns if this list contains any values (or not).
    /// </summary>
    /// <returns>True if the list is empty; otherwise, false</returns>
    public bool IsEmpty()
    {
        return _size == 0;
    }



    #region IList<T> Properties



    /// <summary>
    /// A property that returns the current number of elements in the linked list.
    /// </summary>
    public int Count { get { return _size; } }

    /// <summary>
    /// This property returns false in this implementation, indicating that the linked list 
    /// allows adding, removing, and modifying elements.
    /// </summary>
    public bool IsReadOnly { get { return false; } }

    /// <summary>
    /// Appends an item to the end of the list.
    /// </summary>
    /// <param name="item">The item to add to the list</param>
    public void Add(T item)
    {
        // Add the item to end of list
        PushBack(item);
    }

    /// <summary>
    /// Places a new item at a specified index position, 'shifting' subsequent elements 
    /// down the list.
    /// </summary>
    /// <param name="index">The zero-based index at which to insert the item</param>
    /// <param name="item">The item to insert</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index 
    /// is negative or out of range</exception>
    public void Insert(int index, T item)
    {
        // Index must be valid
        if (index < 0) throw new ArgumentOutOfRangeException("Index is negative.");
        if (index > Size() + 1) throw new ArgumentOutOfRangeException("Index out of range.");

        // Get node at index and insert it there
        DNode? itemNode;
        if (index == Size()) itemNode = _tail;
        else itemNode = GetNode(index);

        Insert(itemNode, item);
    }

    /// <summary>
    /// Returns the index of the first occurrence of the specified item, or -1 if not found.
    /// </summary>
    /// <param name="item">The item to locate in the list</param>
    /// <returns>The zero-based index of the first occurrence of the item, or -1 if not 
    /// found</returns>
    public int IndexOf(T item)
    {
        var curr = _head.Next;
        // Loop through DLL
        for (int i = 0; i < Size(); i++)
        {
            // Compare current node's value to input value
            // Return iterator index if a match is found
            if (EqualityComparer<T>.Default.Equals(item, curr.Value)) return i;
            curr = curr.Next;
        }
        return -1;
    }

    /// <summary>
    /// Indexer property that gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set</param>
    /// <returns>The element at the specified index</returns>
    public T this[int index]
    {
        get
        {
            return GetNode(index).Value;
        }
        set
        {
            GetNode(index).Value = value;
        }
    }

    /// <summary>
    /// Deletes the element stored at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is negative or out of range</exception>
    public void RemoveAt(int index)
    {
        // Index must be valid
        if (index < 0) throw new ArgumentOutOfRangeException("Index is negative.");
        if (index > Size()) throw new ArgumentOutOfRangeException("Index out of range.");

        // Get node at specified index then remove it
        var node = GetNode(index);
        Remove(node);
    }

    /// <summary>
    /// Transfers elements from the doubly-linked list directly into a target array starting at a specified 
    /// index, providing a way to efficiently populate arrays with the list's contents.
    /// </summary>
    /// <param name="array">The target array to copy elements to</param>
    /// <param name="arrayIndex">The index of the array at which copying begins</param>
    /// <exception cref="ArgumentNullException">Thrown when the target array is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the array index is negative</exception>
    /// <exception cref="ArgumentException">Thrown when there is not enough 
    /// space in the array to accommodate all elements</exception>
    public void CopyTo(T[] array, int arrayIndex)
    {
        // Index must be positive and array can't be null.
        if (array == null) throw new ArgumentNullException("Target array is null");
        if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Index is negative.");
        // Must be enough space in array to fit list
        if (array.Length - arrayIndex < Size()) throw new ArgumentException(
            $"Not enough space in array {array} to accomodate all elements in DLL.");

        var curr = _head.Next;

        // Iterate starting at given array index, stop iterating at end of DLL
        for (int i = arrayIndex; curr != _tail; i++)
        {
            // Set current array value to DLL value, iterate to next DLL node
            array[i] = curr.Value;
            curr = curr.Next;
        }
    }




    #endregion
    #region IEnumerable<T> properties



    /// <summary>
    /// This method returns a typed iterator that yields each data element in the linked list in sequence.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection</returns>
    public IEnumerator<T> GetEnumerator()
    {
        DNode? curr = _head.Next;

        // Simple loop that goes through list yield returns
        while (curr != _tail)
        {
            if (curr != null) yield return curr.Value;
            curr = curr.Next;
        }
    }

    /// <summary>
    /// Old school method
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion
}