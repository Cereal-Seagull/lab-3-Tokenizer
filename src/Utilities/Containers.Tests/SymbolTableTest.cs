/**
* Test cases for all methods in Containers.SymbolTable
*
* Test cases written by LLM Claude Sonnet 4 but reviewed
* and confirmed by authors.
*
* Bugs: 
*
* @author Charlie Moss and Will Zoeller
* @date 9/19/25
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SymbolTableTests
{
    /// <summary>
    /// Contains comprehensive test cases for the SymbolTable&lt;TKey, TValue&gt; class.
    /// Tests all public methods, properties, and interfaces implemented by the SymbolTable.
    /// </summary>
    public class SymbolTableTests
    {
        #region Constructor Tests

        /// <summary>
        /// Tests that the SymbolTable constructor properly initializes a new instance
        /// with a valid parent reference.
        /// </summary>
        [Fact]
        public void Constructor_CreateSymbolTableWithParentReference()
        {
            // Arrange & Act
            var symbolTable = new SymbolTable<string, int>();

            // Assert
            Assert.NotNull(symbolTable);
            Assert.NotNull(symbolTable.Parent);
        }

        #endregion

        #region Parent Property Tests

        /// <summary>
        /// Tests that the Parent property returns a valid SymbolTable instance
        /// that serves as the parent scope for symbol resolution.
        /// </summary>
        [Fact]
        public void Parent_ReturnParentSymbolTable()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act
            var parent = symbolTable.Parent;

            // Assert
            Assert.Null(parent);
            // Important: generate some tests for multi-scope
            var symbolTableChild = new SymbolTable<string, int>(symbolTable);

            // Equivalent constructor function
            var symbolTableChild2 = symbolTable.CreateNewScope_GivenParent();

            Assert.NotNull(symbolTableChild.Parent);
            Assert.Equal(symbolTableChild.Parent, symbolTable);

            Assert.NotNull(symbolTableChild2.Parent);
            Assert.Equal(symbolTableChild2.Parent, symbolTable);
        }

        #endregion

        #region ContainsKeyLocal Tests

        /// <summary>
        /// Tests that ContainsKeyLocal correctly identifies whether a key exists
        /// in the current scope only (not checking parent scopes).
        /// </summary>
        /// <param name="key">The key to search for in the current scope</param>
        /// <param name="expectedResult">Expected boolean result of the key existence check</param>
        [Theory]
        [InlineData("existingKey1", true)]
        [InlineData("existingKey2", true)]
        [InlineData("existingKey3", true)]
        [InlineData("nonExistentKey", false)]
        public void ContainsKeyLocal_CheckKeyExistenceOnlyInCurrentScope(string key, bool expectedResult)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("existingKey1", 10);
            symbolTable.Add("existingKey2", 0);
            symbolTable.Add("existingKey3", -1);


            // Act
            bool result = symbolTable.ContainsKeyLocal(key);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Tests that ContainsKeyLocal throws ArgumentNullException when passed a null key.
        /// </summary>
        [Fact]
        public void ContainsKeyLocal_WithNullKey_ThrowArgumentNullException()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => symbolTable.ContainsKeyLocal(null));
        }

        #endregion

        #region TryGetValueLocal Tests

        /// <summary>
        /// Tests that TryGetValueLocal correctly retrieves values from the current scope only
        /// and returns appropriate success/failure indicators.
        /// </summary>
        /// <param name="key">The key to search for in the current scope</param>
        /// <param name="expectedValue">Expected value associated with the key</param>
        /// <param name="expectedFound">Expected boolean indicating if the key was found</param>
        [Theory]
        [InlineData("existingKey", 100, true)]
        [InlineData("existingKey2", 200, true)]
        [InlineData("existingKey3", -1, true)]
        [InlineData("nonExistentKey", 0, false)]
        public void TryGetValueLocal_RetrieveValueOnlyFromCurrentScope(string key, int expectedValue, bool expectedFound)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("existingKey", 100);
            symbolTable.Add("existingKey2", 200);
            symbolTable.Add("existingKey3", -1);

            // Act
            bool found = symbolTable.TryGetValueLocal(key, out int actualValue);

            // Assert
            Assert.Equal(expectedFound, found);
            if (expectedFound)
            {
                Assert.Equal(expectedValue, actualValue);
            }
        }

        /// <summary>
        /// Tests that TryGetValueLocal throws ArgumentNullException when passed a null key.
        /// </summary>
        [Fact]
        public void TryGetValueLocal_WithNullKey_ThrowArgumentNullException()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => symbolTable.TryGetValueLocal(null, out int value));
        }

        #endregion

        #region Count Property Tests

        /// <summary>
        /// Tests that the Count property correctly returns the number of key-value pairs
        /// in the current scope of the SymbolTable.
        /// </summary>
        /// <param name="expectedCount">The expected number of elements in the collection</param>
        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void Count_ReturnElementCountInCollection(int expectedCount)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            for (int i = 0; i < expectedCount; i++)
            {
                symbolTable.Add($"key{i}", i * 10);
            }

            // Act
            int actualCount = symbolTable.Count();

            // Assert
            Assert.Equal(expectedCount, actualCount);
        }

        #endregion

        #region IsReadOnly Property Tests

        /// <summary>
        /// Tests that the IsReadOnly property returns a valid boolean value
        /// indicating whether the collection is read-only.
        /// </summary>
        [Fact]
        public void IsReadOnly_IndicateCollectionIsReadOnly()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act
            bool isReadOnly = ((ICollection<KeyValuePair<string, int>>)symbolTable).IsReadOnly;

            // Assert
            Assert.IsType<bool>(isReadOnly);
            Assert.False(isReadOnly);
        }

        #endregion

        #region Indexer Tests

        /// <summary>
        /// Tests that the indexer getter correctly retrieves values for string keys.
        /// </summary>
        /// <param name="key">The string key to retrieve the value for</param>
        /// <param name="expectedValue">The expected integer value associated with the key</param>
        [Theory]
        [InlineData("key1", 100)]
        [InlineData("key2", 200)]
        [InlineData("key3", 100000)]
        [InlineData("key4", -1342)]
        public void Indexer_Get_ReturnValueForGivenStringKey(string key, int expectedValue)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add(key, expectedValue);

            // Act
            int actualValue = symbolTable[key];

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that the indexer getter correctly retrieves values for integer keys.
        /// </summary>
        /// <param name="key">The integer key to retrieve the value for</param>
        /// <param name="expectedValue">The expected string value associated with the key</param>
        [Theory]
        [InlineData(1, "blah")]
        [InlineData(2, "bleh")]
        [InlineData(3, "naughty")]
        [InlineData(4, "MASTER THEOREM")]
        public void Indexer_Get_ReturnValueForGivenIntKey(int key, string expectedValue)
        {
            // Arrange
            var symbolTable = new SymbolTable<int, string>();
            symbolTable.Add(key, expectedValue);

            // Act
            string actualValue = symbolTable[key];

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that the indexer setter correctly updates values for existing keys.
        /// </summary>
        /// <param name="key">The integer key to update the value for</param>
        /// <param name="newValue">The new double value to assign to the key</param>
        [Theory]
        [InlineData(0, 5.0)]
        [InlineData(1, 10.5)]
        public void Indexer_Set_UpdateValueForGivenKey(int key, double newValue)
        {
            // Arrange
            var symbolTable = new SymbolTable<int, double>();
            symbolTable.Add(key, 100.0);

            // Act
            symbolTable[key] = newValue;

            // Assert
            Assert.Equal(newValue, symbolTable[key]);
        }

        #endregion

        #region Keys and Values Properties Tests

        /// <summary>
        /// Tests that the Keys property returns a collection containing all keys
        /// from the current scope of the SymbolTable.
        /// </summary>
        [Fact]
        public void Keys_ReturnCollectionOfAllKeys()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);

            // Act
            var keys = symbolTable.Keys;

            // Assert
            Assert.NotNull(keys);
            Assert.Contains("key1", keys);
            Assert.Contains("key2", keys);
        }

        /// <summary>
        /// Tests that the Values property returns a collection containing all values
        /// from the current scope of the SymbolTable.
        /// </summary>
        [Fact]
        public void Values_ReturnCollectionOfAllValues()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);

            // Act
            var values = symbolTable.Values;

            // Assert
            Assert.NotNull(values);
            Assert.Contains(100, values);
            Assert.Contains(200, values);
        }

        #endregion

        #region Add Method Tests

        /// <summary>
        /// Tests that the Add method correctly adds key-value pairs to the SymbolTable.
        /// </summary>
        /// <param name="key">The string key to add</param>
        /// <param name="value">The integer value to associate with the key</param>
        [Theory]
        [InlineData("key1", 100)]
        [InlineData("key2", 200)]
        [InlineData("Big O", 123123)]
        [InlineData("Little Omega", -40)]
        public void Add_AddSpecifiedKeyAndValue(string key, int value)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act
            symbolTable.Add(key, value);

            // Assert
            Assert.True(symbolTable.ContainsKey(key));
            Assert.Equal(value, symbolTable[key]);
        }

        /// <summary>
        /// Tests that the Add method correctly adds a KeyValuePair to the SymbolTable.
        /// </summary>
        [Fact]
        public void Add_KeyValuePair_AddElementFromKeyValuePair()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            var kvp = new KeyValuePair<string, int>("testKey", 300);

            // Act
            symbolTable.Add(kvp);

            // Assert
            Assert.True(symbolTable.ContainsKey("testKey"));
            Assert.Equal(300, symbolTable["testKey"]);
        }

        #endregion

        #region Clear Method Tests

        /// <summary>
        /// Tests that the Clear method removes all key-value pairs from the SymbolTable.
        /// </summary>
        [Fact]
        public void Clear_RemoveAllItemsFromCollection()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("balvin", 1);
            symbolTable.Add("malvin (evil alvin)", -1);
            symbolTable.Add("mrs dr alvin", 261);
            symbolTable.Add("alvin", 10000);
            symbolTable.Add("will", 21);
            symbolTable.Add("charlie", 19);

            // Act
            symbolTable.Clear();

            // Assert
            Assert.Empty(symbolTable.Keys);
            Assert.Empty(symbolTable.Values);
        }

        #endregion

        #region Contains Method Tests

        /// <summary>
        /// Tests that the Contains method correctly determines if the SymbolTable
        /// contains a specific value.
        /// </summary>
        /// <param name="value">The integer value to search for</param>
        /// <param name="expectedResult">Expected boolean result indicating if the value exists</param>
        [Theory]
        [InlineData(100, true)]
        [InlineData(200, true)]
        [InlineData(300, true)]
        [InlineData(500, true)]
        [InlineData(0, false)]
        [InlineData(999, false)]
        public void Contains_DetermineIfCollectionContainsValue(int value, bool expectedResult)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);
            symbolTable.Add("In the middle of the party:", 300);
            symbolTable.Add("I just farted!!!!", 500);

            // Act
            bool result = symbolTable.Contains(value);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Tests that the Contains method correctly determines if the SymbolTable
        /// contains a specific KeyValuePair.
        /// </summary>
        /// <param name="key">The key portion of the KeyValuePair to search for</param>
        /// <param name="expectedResult">Expected boolean result indicating if the pair exists</param>
        [Theory]
        [InlineData("key1", true)]
        [InlineData("keydeux", true)]
        [InlineData("nonExistentKey", false)]
        public void Contains_KeyValuePair_DetermineIfCollectionContainsKeyValuePair(string key, bool expectedResult)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            var kvpInsert = new KeyValuePair<string, int>("keydeux", 100);

            symbolTable.Add(kvpInsert);
            var kvp = new KeyValuePair<string, int>(key, 100);

            // Act
            bool result = symbolTable.Contains(kvp);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region ContainsKey Method Tests

        /// <summary>
        /// Tests that the ContainsKey method correctly determines if the SymbolTable
        /// contains an element with the specified key (searching through parent scopes).
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="expectedResult">Expected boolean result indicating if the key exists</param>
        [Theory]
        [InlineData("key1", true)]
        [InlineData("key2", true)]
        [InlineData("blingle blongle", true)]
        [InlineData("single songle", false)]
        [InlineData("nonExistentKey", false)]
        public void ContainsKey_DetermineIfDictionaryContainsElementWithSpecifiedKey(string key, bool expectedResult)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);
            symbolTable.Add("blingle blongle", 400);

            // Act
            bool result = symbolTable.ContainsKey(key);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region Remove Method Tests

        /// <summary>
        /// Tests that the Remove method correctly removes elements with the specified key
        /// and returns appropriate success/failure indicators.
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <param name="expectedResult">Expected boolean indicating if the removal was successful</param>
        [Theory]
        [InlineData("key1", true)]
        [InlineData("key2", true)]
        [InlineData("key3", true)]
        [InlineData("key4", false)]
        public void Remove_RemovesElementWithSpecifiedKey(string key, bool expectedResult)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);
            symbolTable.Add(new KeyValuePair<string, int>("key3", 300));

            // Act
            int initSize = symbolTable.Count();
            bool result = symbolTable.Remove(key);


            // Assert
            Assert.Equal(expectedResult, result);
            if (result != false) Assert.Equal(initSize, symbolTable.Count() + 1);
            else Assert.Equal(initSize, symbolTable.Count());
        }

        #endregion

        #region ToString Method Tests

        /// <summary>
        /// Tests that the ToString method returns a properly formatted string representation
        /// of the SymbolTable contents, including empty table handling.
        /// </summary>
        [Fact]
        public void ToString_ReturnStringRepresentationOfSymbolTable()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // ToString() when list is empty
            Assert.Equal("{ }", symbolTable.ToString());

            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);
            symbolTable.Add("idk why claude only adds single tests for theories but it sUCKs!", 300);

            // Act
            string result = symbolTable.ToString();

            // Assert
            Assert.Equal("{ key1 : 100, key2 : 200, " +
                         "idk why claude only adds single tests for theories but it sUCKs! : 300 }", result);
        }

        #endregion

        #region TryGetValue Method Tests

        /// <summary>
        /// Tests that the TryGetValue method correctly attempts to retrieve values
        /// associated with specific keys and returns appropriate success/failure indicators.
        /// </summary>
        /// <param name="key">The key to search for</param>
        /// <param name="expectedValue">Expected value associated with the key</param>
        /// <param name="expectedFound">Expected boolean indicating if the key was found</param>
        [Theory]
        [InlineData("key1", 100, true)]
        [InlineData("key_fifty", -200, true)]
        [InlineData("key_i love doctor alvin!!", 300, true)]
        [InlineData("from 261!", 261, true)]
        [InlineData("...", 0, true)]
        [InlineData("AND 223!", 223, false)]
        [InlineData("nonExistentKey", 0, false)] // Will get default value when not found
        public void TryGetValue_TryValueAssociatedWithSpecificKey(string key, int expectedValue, bool expectedFound)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key_fifty", -200);
            symbolTable.Add("key_i love doctor alvin!!", 300);
            symbolTable.Add("from 261!", 261);
            symbolTable.Add("...", 0);

            // Act
            bool found = symbolTable.TryGetValue(key, out int actualValue);

            // Assert
            Assert.Equal(expectedFound, found);
            // Note: Implementation returns _values.Front() as default, so we can't predict the exact default value
            if (expectedFound)
            {
                Assert.Equal(expectedValue, actualValue);
            }
        }

        #endregion

        #region CopyTo Method Tests

        /// <summary>
        /// Tests that the CopyTo method correctly copies KeyValuePairs to a target array
        /// starting at the specified index.
        /// </summary>
        /// <param name="keys">Array of keys to add to the SymbolTable</param>
        /// <param name="values">Array of values corresponding to the keys</param>
        /// <param name="arrayIndex">Starting index in the target array for copying</param>
        [Theory]
        [InlineData(new string[] { "key1", "key2", "key3" }, new int[] { 1, 2, 3 }, 0)]
        [InlineData(new string[] { "a", "b" }, new int[] { 10, 20 }, 1)]
        [InlineData(new string[] { "single" }, new int[] { 100 }, 2)]
        public void CopyTo_ValidInputs_CopiesKeyValuePairsCorrectly(string[] keys, int[] values, int arrayIndex)
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Add test data to the symbol table
            for (int i = 0; i < keys.Length; i++)
            {
                symbolTable.Add(keys[i], values[i]);
            }

            var targetArray = new KeyValuePair<string, int>[arrayIndex + keys.Length];

            // Act
            symbolTable.CopyTo(targetArray, arrayIndex);

            // Assert
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.Equal(keys[i], targetArray[arrayIndex + i].Key);
                Assert.Equal(values[i], targetArray[arrayIndex + i].Value);
            }
        }

        /// <summary>
        /// Tests that the CopyTo method throws ArgumentNullException when passed a null array.
        /// </summary>
        [Fact]
        public void CopyTo_WithNullArray_ThrowArgumentNullException()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => symbolTable.CopyTo(null, 0));
        }

        /// <summary>
        /// Tests that the CopyTo method throws ArgumentOutOfRangeException when passed a negative index.
        /// </summary>
        [Fact]
        public void CopyTo_WithNegativeIndex_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            var array = new KeyValuePair<string, int>[5];

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => symbolTable.CopyTo(array, -1));
        }

        /// <summary>
        /// Tests that the CopyTo method throws ArgumentException when the destination array
        /// has insufficient space to accommodate all elements.
        /// </summary>
        [Fact]
        public void CopyTo_WithInsufficientSpace_ThrowArgumentException()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);
            var array = new KeyValuePair<string, int>[2]; // Not enough space starting at index 1

            // Act & Assert
            Assert.Throws<ArgumentException>(() => symbolTable.CopyTo(array, 1));
        }

        #endregion

        #region GetEnumerator Method Tests

        /// <summary>
        /// Tests that the generic GetEnumerator method returns a valid enumerator
        /// that can iterate through all KeyValuePairs in the SymbolTable.
        /// </summary>
        [Fact]
        public void GetEnumerator_ReturnEnumeratorForKeyValuePairs()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);
            symbolTable.Add("key2", 200);

            // Act
            var enumerator = symbolTable.GetEnumerator();
            var items = new List<KeyValuePair<string, int>>();

            while (enumerator.MoveNext())
            {
                items.Add(enumerator.Current);
            }

            // Assert
            Assert.Equal(2, items.Count);
            Assert.Contains(new KeyValuePair<string, int>("key1", 100), items);
            Assert.Contains(new KeyValuePair<string, int>("key2", 200), items);
        }

        /// <summary>
        /// Tests that the non-generic GetEnumerator method returns a valid enumerator
        /// for compatibility with IEnumerable interface.
        /// </summary>
        [Fact]
        public void GetEnumerator_NonGeneric_ReturnEnumeratorForKeyValuePairs()
        {
            // Arrange
            var symbolTable = new SymbolTable<string, int>();
            symbolTable.Add("key1", 100);

            // Act
            var enumerator = ((System.Collections.IEnumerable)symbolTable).GetEnumerator();

            // Assert
            Assert.NotNull(enumerator);
            Assert.True(enumerator.MoveNext());
        }

        #endregion
        #region Multi-Scope TryGetValue Tests

        /// <summary>
        /// Tests that TryGetValue correctly searches through parent scopes when a key
        /// is not found in the current scope, verifying hierarchical symbol resolution.
        /// </summary>
        /// <param name="searchKey">The key to search for across scopes</param>
        /// <param name="expectedValue">Expected value from the scope hierarchy</param>
        /// <param name="expectedFound">Expected boolean indicating if key was found in hierarchy</param>
        [Theory]
        [InlineData("parentKey1", 1000, true)]
        [InlineData("parentKey2", 2000, true)]
        [InlineData("childKey1", 100, true)]
        [InlineData("sharedKey", 5555, true)] // Child value should take precedence
        [InlineData("nonExistentKey", 0, false)]
        public void TryGetValue_MultiScope_SearchesThroughParentScopes(string searchKey, int expectedValue, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("parentKey2", 2000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("childKey2", 200);
            childScope.Add("sharedKey", 5555); // Should shadow parent value

            // Act
            bool found = childScope.TryGetValue(searchKey, out int actualValue);

            // Assert
            Assert.Equal(expectedFound, found);
            if (expectedFound)
            {
                Assert.Equal(expectedValue, actualValue);
            }
        }

        /// <summary>
        /// Tests that TryGetValue correctly resolves keys that exist in both parent and child scopes,
        /// verifying that child scope values take precedence (shadowing behavior).
        /// </summary>
        /// <param name="key">The key that exists in both scopes</param>
        /// <param name="parentValue">Value in parent scope</param>
        /// <param name="childValue">Value in child scope (should take precedence)</param>
        [Theory]
        [InlineData("shadowedKey1", 999, 111)]
        [InlineData("shadowedKey2", -500, 750)]
        [InlineData("commonVar", 0, 42)]
        public void TryGetValue_MultiScope_ChildScopeShadowsParentScope(string key, int parentValue, int childValue)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add(key, parentValue);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add(key, childValue);

            // Act
            bool parentFound = parentScope.TryGetValue(key, out int parentResult);
            bool childFound = childScope.TryGetValue(key, out int childResult);

            // Assert
            Assert.True(parentFound);
            Assert.True(childFound);
            Assert.Equal(parentValue, parentResult);
            Assert.Equal(childValue, childResult); // Child value should take precedence
            Assert.NotEqual(parentResult, childResult);
        }

        #endregion

        #region Multi-Scope Contains Method Tests

        /// <summary>
        /// Tests that Contains(value) searches through parent scopes to locate values
        /// that exist anywhere in the scope hierarchy.
        /// </summary>
        /// <param name="searchValue">The value to search for across scopes</param>
        /// <param name="expectedFound">Expected boolean indicating if value exists in hierarchy</param>
        [Theory]
        [InlineData(1000, true)]  // From parent scope
        [InlineData(100, true)]   // From child scope
        [InlineData(5555, true)]  // Child's version of shared value
        [InlineData(9999, true)]  // Parent's original value still exists in parent scope
        [InlineData(404, false)]  // Non-existent value
        public void Contains_Value_MultiScope_SearchesThroughParentScopes(int searchValue, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("parentKey2", 2000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("sharedKey", 5555); // Shadows parent value

            // Act
            bool found = childScope.Contains(searchValue);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        /// <summary>
        /// Tests that Contains(KeyValuePair) searches through parent scopes to locate
        /// exact key-value pairs that exist anywhere in the scope hierarchy.
        /// </summary>
        /// <param name="key">Key portion of the KeyValuePair</param>
        /// <param name="value">Value portion of the KeyValuePair</param>
        /// <param name="expectedFound">Expected boolean indicating if exact pair exists in hierarchy</param>
        [Theory]
        [InlineData("childKey1", 100, true)]    // Exists in child scope
        [InlineData("parentKey1", 1000, true)]  // Exists in parent scope
        [InlineData("sharedKey", 5555, true)]   // Child's version of shared key
        [InlineData("sharedKey", 9999, true)]   // Parent's version should also be found
        [InlineData("childKey1", 999, false)]   // Wrong value for existing key
        [InlineData("fakeKey", 123, false)]     // Non-existent pair
        public void Contains_KeyValuePair_MultiScope_SearchesThroughParentScopes(string key, int value, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("sharedKey", 5555);

            var kvp = new KeyValuePair<string, int>(key, value);

            // Act
            bool found = childScope.Contains(kvp);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        #endregion

        #region Multi-Scope ContainsKey Tests

        /// <summary>
        /// Tests that ContainsKey searches through parent scopes to locate keys
        /// that exist anywhere in the scope hierarchy.
        /// </summary>
        /// <param name="searchKey">The key to search for across scopes</param>
        /// <param name="expectedFound">Expected boolean indicating if key exists in hierarchy</param>
        [Theory]
        [InlineData("parentKey1", true)]     // In parent scope only
        [InlineData("parentKey2", true)]     // In parent scope only
        [InlineData("childKey1", true)]      // In child scope only
        [InlineData("childKey2", true)]      // In child scope only
        [InlineData("sharedKey", true)]      // In both scopes
        [InlineData("ghostKey", false)]      // Non-existent key
        public void ContainsKey_MultiScope_SearchesThroughParentScopes(string searchKey, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("parentKey2", 2000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("childKey2", 200);
            childScope.Add("sharedKey", 5555);

            // Act
            bool found = childScope.ContainsKey(searchKey);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        /// <summary>
        /// Tests ContainsKey behavior across multiple nested scope levels,
        /// verifying that search traverses through all ancestor scopes.
        /// </summary>
        /// <param name="searchKey">The key to search for across nested scopes</param>
        /// <param name="expectedFound">Expected boolean indicating if key exists in hierarchy</param>
        [Theory]
        [InlineData("grandparentKey", true)]   // Three levels up
        [InlineData("parentKey", true)]        // Two levels up
        [InlineData("childKey", true)]         // One level up
        [InlineData("grandchildKey", true)]    // Current scope
        [InlineData("phantomKey", false)]      // Non-existent across all levels
        public void ContainsKey_MultiScope_SearchesAcrossMultipleLevels(string searchKey, bool expectedFound)
        {
            // Arrange
            var grandparentScope = new SymbolTable<string, int>();
            grandparentScope.Add("grandparentKey", 10000);
            
            var parentScope = grandparentScope.CreateNewScope_GivenParent();
            parentScope.Add("parentKey", 1000);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey", 100);
            
            var grandchildScope = childScope.CreateNewScope_GivenParent();
            grandchildScope.Add("grandchildKey", 10);

            // Act
            bool found = grandchildScope.ContainsKey(searchKey);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        #endregion

        #region Multi-Scope ContainsKeyLocal Tests

        /// <summary>
        /// Tests that ContainsKeyLocal only checks the current scope and does not
        /// search through parent scopes, providing strict local scope verification.
        /// </summary>
        /// <param name="searchKey">The key to search for in current scope only</param>
        /// <param name="expectedFound">Expected boolean indicating if key exists in current scope</param>
        [Theory]
        [InlineData("childKey1", true)]       // Exists in current (child) scope
        [InlineData("childKey2", true)]       // Exists in current (child) scope
        [InlineData("sharedKey", true)]       // Exists in current scope (shadows parent)
        [InlineData("parentKey1", false)]     // Exists in parent but not current scope
        [InlineData("parentKey2", false)]     // Exists in parent but not current scope
        [InlineData("nonExistentKey", false)] // Doesn't exist anywhere
        public void ContainsKeyLocal_MultiScope_ChecksOnlyCurrentScope(string searchKey, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("parentKey2", 2000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("childKey2", 200);
            childScope.Add("sharedKey", 5555); // Shadows parent

            // Act
            bool found = childScope.ContainsKeyLocal(searchKey);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        /// <summary>
        /// Tests ContainsKeyLocal behavior to verify it provides different results
        /// than ContainsKey when keys exist in parent scopes.
        /// </summary>
        /// <param name="key">The key to test for local vs hierarchical containment</param>
        /// <param name="expectedLocal">Expected result for local-only search</param>
        /// <param name="expectedHierarchical">Expected result for hierarchical search</param>
        [Theory]
        [InlineData("parentOnlyKey", false, true)]  // In parent scope only
        [InlineData("childOnlyKey", true, true)]    // In child scope only
        [InlineData("sharedKey", true, true)]       // In both scopes
        [InlineData("nowhereKey", false, false)]    // In neither scope
        public void ContainsKeyLocal_Vs_ContainsKey_MultiScope_ShowsDifference(string key, bool expectedLocal, bool expectedHierarchical)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentOnlyKey", 1000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childOnlyKey", 100);
            childScope.Add("sharedKey", 5555);

            // Act
            bool localResult = childScope.ContainsKeyLocal(key);
            bool hierarchicalResult = childScope.ContainsKey(key);

            // Assert
            Assert.Equal(expectedLocal, localResult);
            Assert.Equal(expectedHierarchical, hierarchicalResult);
        }

        #endregion

        #region Multi-Scope TryGetValueLocal Tests

        /// <summary>
        /// Tests that TryGetValueLocal only retrieves values from the current scope
        /// and does not search through parent scopes.
        /// </summary>
        /// <param name="searchKey">The key to search for in current scope only</param>
        /// <param name="expectedValue">Expected value if found in current scope</param>
        /// <param name="expectedFound">Expected boolean indicating if key exists in current scope</param>
        [Theory]
        [InlineData("childKey1", 100, true)]    // Exists in current scope
        [InlineData("childKey2", 200, true)]    // Exists in current scope  
        [InlineData("sharedKey", 5555, true)]   // Child's version (shadows parent)
        [InlineData("parentKey1", 0, false)]    // Exists in parent but not current scope
        [InlineData("parentKey2", 0, false)]    // Exists in parent but not current scope
        [InlineData("ghostKey", 0, false)]      // Doesn't exist anywhere
        public void TryGetValueLocal_MultiScope_RetrievesOnlyFromCurrentScope(string searchKey, int expectedValue, bool expectedFound)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentKey1", 1000);
            parentScope.Add("parentKey2", 2000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey1", 100);
            childScope.Add("childKey2", 200);
            childScope.Add("sharedKey", 5555); // Shadows parent value

            // Act
            bool found = childScope.TryGetValueLocal(searchKey, out int actualValue);

            // Assert
            Assert.Equal(expectedFound, found);
            if (expectedFound)
            {
                Assert.Equal(expectedValue, actualValue);
            }
        }

        /// <summary>
        /// Tests TryGetValueLocal behavior to verify it provides different results
        /// than TryGetValue when keys exist in parent scopes.
        /// </summary>
        /// <param name="key">The key to test for local vs hierarchical retrieval</param>
        /// <param name="expectedLocalFound">Expected result for local-only search</param>
        /// <param name="expectedHierarchicalFound">Expected result for hierarchical search</param>
        /// <param name="localValue">Expected value from local search (if found)</param>
        /// <param name="hierarchicalValue">Expected value from hierarchical search (if found)</param>
        [Theory]
        [InlineData("parentOnlyKey", false, true, 0, 1000)]     // Parent scope only
        [InlineData("childOnlyKey", true, true, 100, 100)]      // Child scope only  
        [InlineData("sharedKey", true, true, 5555, 5555)]       // Both scopes (child wins)
        [InlineData("nowhereKey", false, false, 0, 0)]          // Neither scope
        public void TryGetValueLocal_Vs_TryGetValue_MultiScope_ShowsDifference(string key, bool expectedLocalFound, bool expectedHierarchicalFound, int localValue, int hierarchicalValue)
        {
            // Arrange
            var parentScope = new SymbolTable<string, int>();
            parentScope.Add("parentOnlyKey", 1000);
            parentScope.Add("sharedKey", 9999);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childOnlyKey", 100);
            childScope.Add("sharedKey", 5555);

            // Act
            bool localFound = childScope.TryGetValueLocal(key, out int actualLocalValue);
            bool hierarchicalFound = childScope.TryGetValue(key, out int actualHierarchicalValue);

            // Assert
            Assert.Equal(expectedLocalFound, localFound);
            Assert.Equal(expectedHierarchicalFound, hierarchicalFound);
            
            if (expectedLocalFound)
            {
                Assert.Equal(localValue, actualLocalValue);
            }
            if (expectedHierarchicalFound)
            {
                Assert.Equal(hierarchicalValue, actualHierarchicalValue);
            }
        }

        /// <summary>
        /// Tests TryGetValueLocal with multiple nested scopes to ensure it only
        /// checks the immediate scope and not any ancestor scopes.
        /// BUG: Will fail because implementation searches through all parent scopes.
        /// </summary>
        /// <param name="searchKey">The key to search for in current scope only</param>
        /// <param name="expectedFound">Expected boolean for strict local-only search</param>
        [Theory]
        [InlineData("grandparentKey", false)]   // Should not find in grandparent
        [InlineData("parentKey", false)]        // Should not find in parent
        [InlineData("childKey", false)]         // Should not find in child
        [InlineData("grandchildKey", true)]     // Should find in current scope
        [InlineData("phantomKey", false)]       // Should not find anywhere
        public void TryGetValueLocal_MultiScope_StrictlyLocalOnly(string searchKey, bool expectedFound)
        {
            // Arrange
            var grandparentScope = new SymbolTable<string, int>();
            grandparentScope.Add("grandparentKey", 10000);
            
            var parentScope = grandparentScope.CreateNewScope_GivenParent();
            parentScope.Add("parentKey", 1000);
            
            var childScope = parentScope.CreateNewScope_GivenParent();
            childScope.Add("childKey", 100);
            
            var grandchildScope = childScope.CreateNewScope_GivenParent();
            grandchildScope.Add("grandchildKey", 10);

            // Act
            bool found = grandchildScope.TryGetValueLocal(searchKey, out int actualValue);

            // Assert
            Assert.Equal(expectedFound, found);
        }

        #endregion
    }
}