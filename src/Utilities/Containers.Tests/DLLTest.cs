/**
* Test cases for all methods in Containers.DLL
*
* Test cases written by LLM Claude Sonnet 4 but reviewed
* and confirmed by authors.
*
* Bugs: several "Cannot convert null literal to non-nullable reference type" errors.
*
* @author Charlie Moss and Will Zoeller
* @date 9/8/25
*/
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DLLTests
{


    /// <summary>
    /// Comprehensive test suite for the DLL (Doubly Linked List) class.
    /// Tests cover basic operations, edge cases, and IList/IEnumerable implementations.
    /// </summary>
    public class DLLTests
    {
        #region Constructor Tests

        /// <summary>
        /// Tests that a new DLL is properly initialized with correct default state.
        /// </summary>
        [Fact]
        public void Constructor_CreatesEmptyList_WithCorrectInitialState()

        {
            // Arrange & Act
            var dll = new DLL<int>();

            // Assert
            Assert.True(dll.IsEmpty());
            Assert.Equal(0, dll.Size());
        }

        #endregion

        #region Contains Tests

        /// <summary>
        /// Tests Contains method with various scenarios including empty list and existing/non-existing items.
        /// </summary>
        [Theory]
        [InlineData(new int[] { }, 5, false)]
        [InlineData(new int[] { 1, 2, 3 }, 2, true)]
        [InlineData(new int[] { 1, 2, 3 }, 5, false)]
        public void Contains_WithVariousScenarios_ReturnsExpectedResult(int[] initialItems, int searchItem, bool expected)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act
            bool result = dll.Contains(searchItem);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests Contains method with null values for nullable types.
        /// </summary>
        [Fact]
        public void Contains_WithNullValue_ReturnsTrue()
        {
            // Arrange
            var dll = new DLL<string>();
            dll.Add("test");
            dll.Add(null);
            dll.Add("another");

            // Act & Assert
            Assert.True(dll.Contains(null));
            Assert.True(dll.Contains("test"));
            Assert.False(dll.Contains("missing"));
        }

        #endregion

        #region Size Tests

        /// <summary>
        /// Tests Size method as items are added and removed from the list.
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Size_AfterAddingItems_ReturnsCorrectCount(int itemCount)
        {
            // Arrange
            var dll = new DLL<int>();

            // Act
            for (int i = 0; i < itemCount; i++)
            {
                dll.Add(i);
            }

            // Assert
            Assert.Equal(itemCount, dll.Size());
        }

        #endregion

        #region ToString Tests

        /// <summary>
        /// Tests ToString method formatting with different list states.
        /// </summary>
        [Theory]
        [InlineData(new int[] { }, "[ ]")]
        [InlineData(new int[] { 1 }, "[ 1 ]")]
        [InlineData(new int[] { 1, 2, 3 }, "[ 1, 2, 3 ]")]
        public void ToString_WithVariousListStates_ReturnsExpectedFormat(int[] items, string expected)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in items)
            {
                dll.Add(item);
            }

            // Act
            string result = dll.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region Remove Tests

        /// <summary>
        /// Tests Remove method with existing and non-existing items.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 2, true, 2)]
        [InlineData(new int[] { 1, 2, 3 }, 5, false, 3)]
        [InlineData(new int[] { }, 1, false, 0)]
        [InlineData(new int[] { 1 }, 1, true, 0)]
        [InlineData(new int[] { 1 }, 2, false, 1)]
        public void Remove_WithVariousScenarios_ReturnsExpectedResult(int[] initialItems, int itemToRemove, bool expectedResult, int expectedSize)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act
            bool result = dll.Remove(itemToRemove);

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedSize, dll.Size());
        }

        #endregion

        #region Front/Back Tests

        /// <summary>
        /// Tests Front method returns the first element correctly.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 1)]
        [InlineData(new int[] { 42 }, 42)]
        public void Front_WithItems_ReturnsFirstElement(int[] items, int expected)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in items)
            {
                dll.Add(item);
            }

            // Act & Assert
            Assert.Equal(expected, dll.Front());
        }

        /// <summary>
        /// Tests Front method throws exception when list is empty.
        /// </summary>
        [Fact]
        public void Front_WithEmptyList_ThrowsInvalidOperationException()
        {
            // Arrange
            var dll = new DLL<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dll.Front());
        }

        /// <summary>
        /// Tests Back method returns the last element correctly.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 3)]
        [InlineData(new int[] { 42 }, 42)]
        public void Back_WithItems_ReturnsLastElement(int[] items, int expected)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in items)
            {
                dll.Add(item);
            }

            // Act & Assert
            Assert.Equal(expected, dll.Back());
        }

        /// <summary>
        /// Tests Back method throws exception when list is empty.
        /// </summary>
        [Fact]
        public void Back_WithEmptyList_ThrowsInvalidOperationException()
        {
            // Arrange
            var dll = new DLL<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dll.Back());
        }

        #endregion

        #region Push/Pop Tests

        /// <summary>
        /// Tests PushFront adds elements to the beginning of the list.
        /// </summary>
        [Fact]
        public void PushFront_AddsElementToBeginning()
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(2);
            dll.Add(3);

            // Act
            dll.PushFront(1);

            // Assert
            Assert.Equal(1, dll.Front());
            Assert.Equal(3, dll.Size());
            Assert.Equal("[ 1, 2, 3 ]", dll.ToString());
        }

        /// <summary>
        /// Tests PushBack adds elements to the end of the list.
        /// </summary>
        [Fact]
        public void PushBack_AddsElementToEnd()
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);

            // Act
            dll.PushBack(3);

            // Assert
            Assert.Equal(3, dll.Back());
            Assert.Equal(3, dll.Size());
            Assert.Equal("[ 1, 2, 3 ]", dll.ToString());
        }

        /// <summary>
        /// Tests PopFront removes and returns the first element.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 1, 2)]
        [InlineData(new int[] { 42 }, 42, 0)]
        public void PopFront_RemovesAndReturnsFirstElement(int[] initialItems, int expectedValue, int expectedSize)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act
            var result = dll.PopFront();

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.Equal(expectedSize, dll.Size());
        }

        /// <summary>
        /// Tests PopFront throws exception when list is empty.
        /// </summary>
        [Fact]
        public void PopFront_WithEmptyList_ThrowsInvalidOperationException()
        {
            // Arrange
            var dll = new DLL<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dll.PopFront());
        }

        /// <summary>
        /// Tests PopBack removes and returns the last element.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 3, 2)]
        [InlineData(new int[] { 42 }, 42, 0)]
        public void PopBack_RemovesAndReturnsLastElement(int[] initialItems, int expectedValue, int expectedSize)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act
            var result = dll.PopBack();

            // Assert
            Assert.Equal(expectedValue, result);
            Assert.Equal(expectedSize, dll.Size());
        }

        /// <summary>
        /// Tests PopBack throws exception when list is empty.
        /// </summary>
        [Fact]
        public void PopBack_WithEmptyList_ThrowsInvalidOperationException()
        {
            // Arrange
            var dll = new DLL<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dll.PopBack());
        }

        #endregion

        #region Clear and IsEmpty Tests

        /// <summary>
        /// Tests Clear method removes all elements from the list.
        /// </summary>
        [Fact]
        public void Clear_RemovesAllElements()
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);
            dll.Add(3);

            // Act
            dll.Clear();

            // Assert
            Assert.True(dll.IsEmpty());
            Assert.Equal(0, dll.Size());
        }

        /// <summary>
        /// Tests IsEmpty method with various list states.
        /// </summary>
        [Theory]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 1 }, false)]
        [InlineData(new int[] { 1, 2, 3 }, false)]
        public void IsEmpty_WithVariousStates_ReturnsExpectedResult(int[] items, bool expected)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in items)
            {
                dll.Add(item);
            }

            // Act & Assert
            Assert.Equal(expected, dll.IsEmpty());
        }

        #endregion

        #region IList Implementation Tests


        [Fact]
        public void IsReadOnly()
        {
            var dll = new DLL<int>();
            // Assert
            Assert.False(dll.IsReadOnly);
            dll.Add(1);
            dll.Add(2);
            dll.Add(3);
            Assert.False(dll.IsReadOnly);
            dll.Clear();
            Assert.False(dll.IsReadOnly);
        }

        /// <summary>
        /// Tests Insert method places items at correct positions.
        /// </summary>
        [Theory]
        [InlineData(0, 0)] // Insert at beginning
        [InlineData(1, 2)]
        [InlineData(2, 4)]
        [InlineData(3, 6)] 
        [InlineData(4, 8)] // Insert at end
        public void Insert_AtVariousPositions_PlacesItemCorrectly(int index, int value)
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1); // index 0
            dll.Add(3); // 1
            dll.Add(5); // 2
            dll.Add(7); // 3

            // Act
            dll.Insert(index, value);

            // Assert
            Assert.Equal(value, dll[index]);
            Assert.Equal(5, dll.Size());
        }

        /// <summary>
        /// Tests Insert method throws exception for invalid indices.
        /// </summary>
        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void Insert_WithInvalidIndex_ThrowsArgumentOutOfRangeException(int invalidIndex)
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => dll.Insert(invalidIndex, 99));
        }

        /// <summary>
        /// Tests IndexOf method returns correct indices.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 2, 1)]
        [InlineData(new int[] { 1, 2, 3 }, 5, -1)]
        [InlineData(new int[] { }, 1, -1)]
        public void IndexOf_WithVariousItems_ReturnsExpectedIndex(int[] items, int searchItem, int expectedIndex)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in items)
            {
                dll.Add(item);
            }

            // Act
            int result = dll.IndexOf(searchItem);

            // Assert
            Assert.Equal(expectedIndex, result);
        }

        /// <summary>
        /// Tests indexer property for getting and setting values.
        /// </summary>
        [Fact]
        public void Indexer_GetAndSet_WorksCorrectly()
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);
            dll.Add(3);

            // Act
            int getValue = dll[1]; //get
            dll[1] = 99; //set

            // Assert
            Assert.Equal(2, getValue); //get
            Assert.Equal(99, dll[1]); //set
        }

        /// <summary>
        /// Tests RemoveAt method removes items at correct positions.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, 1, 2)]
        [InlineData(new int[] { 1, 2, 3 }, 0, 2)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 0, 6)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 4, 6)]
        [InlineData(new int[] { 1, 2, 3, 4, 5, 6, 7 }, 6, 6)]
        public void RemoveAt_WithValidIndex_RemovesItemCorrectly(int[] initialItems, int removeIndex, int expectedSize)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act
            dll.RemoveAt(removeIndex);

            // Assert
            Assert.Equal(expectedSize, dll.Size());
        }


        /// <summary>
        /// Tests RemoveAt method throws ArgumentOutOfRangeException for invalid indices.
        /// Tests negative indices and indices that exceed the list size.
        /// </summary>
        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, -1)] // Negative index
        [InlineData(new int[] { 1, 2, 3 }, 3)]  // Index equal to size (out of range)
        [InlineData(new int[] { 1, 2, 3 }, 5)]  // Index greater than size
        [InlineData(new int[] { }, 0)]          // Empty list with index 0
        [InlineData(new int[] { }, 1)]          // Empty list with positive index
        public void RemoveAt_WithInvalidIndex_ThrowsArgumentOutOfRangeException(int[] initialItems, int invalidIndex)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in initialItems)
            {
                dll.Add(item);
            }

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => dll.RemoveAt(invalidIndex));
            
            // Verify that the list size hasn't changed after the exception
            Assert.Equal(initialItems.Length, dll.Size());
        }

        /// <summary>
        /// Tests CopyTo method successfully copies DLL elements to target array.
        /// Tests various scenarios including empty list, full copy, and partial array filling.
        /// </summary>
        [Theory]
        [InlineData(new int[] { }, 0)] // Empty list
        [InlineData(new int[] { 1, 2, 3 }, 0)] // Copy to beginning of array
        [InlineData(new int[] { 1, 2, 3 }, 2)] // Copy to middle of array
        [InlineData(new int[] { 5 }, 1)] // Single element to middle
        public void CopyTo_WithValidParameters_CopiesElementsCorrectly(int[] dllItems, int arrayIndex)
        {
            // Arrange
            var dll = new DLL<int>();
            foreach (var item in dllItems)
            {
                dll.Add(item);
            }
            var targetArray = new int[dllItems.Length + arrayIndex + 2]; // Extra space

            // Act
            dll.CopyTo(targetArray, arrayIndex);

            // Assert
            for (int i = 0; i < dllItems.Length; i++)
            {
                Assert.Equal(dllItems[i], targetArray[arrayIndex + i]);
            }
        }

        /// <summary>
        /// Tests CopyTo method throws appropriate exceptions for invalid parameters.
        /// Covers null array, negative index, and insufficient space scenarios.
        /// </summary>
        [Theory]
        [InlineData(null, 0, typeof(ArgumentNullException))] // Null array
        [InlineData(new int[5] {1, 2, 3, 4, 5}, -1, typeof(ArgumentOutOfRangeException))] // Negative index
        [InlineData(new int[2] {1, 2}, 0, typeof(ArgumentException))] // Insufficient space - array too small
        [InlineData(new int[4] {1, 2, 4, 5}, 3, typeof(ArgumentException))] // Insufficient space - starting index too high
        [InlineData(new int[0], 0, typeof(ArgumentException))] // Empty array (length 0)
        public void CopyTo_WithInvalidParameters_ThrowsExpectedException(int[] targetArray, int arrayIndex, Type expectedExceptionType)
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);
            dll.Add(3); // 3 elements that need to be copied

            // Act & Assert
            var exception = Assert.Throws(expectedExceptionType, () => dll.CopyTo(targetArray, arrayIndex));
            
            // Additional verification for specific exception messages/types
            if (expectedExceptionType == typeof(ArgumentNullException))
            {
                Assert.Contains("Target array is null", exception.Message);
            }
            else if (expectedExceptionType == typeof(ArgumentOutOfRangeException))
            {
                Assert.Contains("Index is negative", exception.Message);
            }
            else if (expectedExceptionType == typeof(ArgumentException))
            {
                Assert.Contains("Not enough space in array", exception.Message);
            }
        }


        #endregion

        #region IEnumerable Tests

        /// <summary>
        /// Tests that the DLL can be enumerated using foreach.
        /// </summary>
        [Fact]
        public void GetEnumerator_CanIterateOverList()
        {
            // Arrange
            var dll = new DLL<int>();
            var expectedItems = new[] { 1, 2, 3, 4 };
            foreach (var item in expectedItems)
            {
                dll.Add(item);
            }

            // Act
            var actualItems = new List<int>();
            foreach (var item in dll)
            {
                actualItems.Add(item);
            }

            // Assert
            Assert.Equal(expectedItems, actualItems);
        }

        /// <summary>
        /// Tests that LINQ operations work with the DLL.
        /// </summary>
        [Fact]
        public void GetEnumerator_WorksWithLinq()
        {
            // Arrange
            var dll = new DLL<int>();
            dll.Add(1);
            dll.Add(2);
            dll.Add(3);
            dll.Add(4);

            // Act
            var evenNumbers = dll.Where(x => x % 2 == 0).ToList();
            var sum = dll.Sum();

            // Assert
            Assert.Equal(new[] { 2, 4 }, evenNumbers);
            Assert.Equal(10, sum);
        }

        #endregion

        #region Edge Cases and Integration Tests

        /// <summary>
        /// Tests complex operations to ensure the list maintains integrity.
        /// </summary>
        [Fact]
        public void IntegrationTest_ComplexOperations_MaintainsListIntegrity()
        {
            // Arrange
            var dll = new DLL<string>();

            // Act - Perform various operations
            dll.PushBack("first");
            dll.PushFront("beginning");
            dll.PushBack("last");
            dll.Insert(1, "middle");

            string popped = dll.PopFront();
            dll.Remove("middle");

            // Assert
            Assert.Equal("beginning", popped);
            Assert.Equal(2, dll.Size());
            Assert.Equal("first", dll.Front());
            Assert.Equal("last", dll.Back());
            Assert.Equal("[ first, last ]", dll.ToString());
        }

        /// <summary>
        /// Tests operations with null values for reference types.
        /// </summary>
        [Fact]
        public void NullHandling_WithReferenceTypes_WorksCorrectly()
        {
            // Arrange
            var dll = new DLL<string>();

            // Act
            dll.Add("test");
            dll.Add(null);
            dll.Add("another");

            // Assert
            Assert.Equal(3, dll.Size());
            Assert.True(dll.Contains(null));
            Assert.Equal(1, dll.IndexOf(null));
            Assert.Null(dll[1]);
        }

        #endregion
    }
}