/**
* General utility methods for various tasks.
* Every method is a public static method.
* 
* Bugs:
*
* @author Charlie Moss and Will Zoeller
* @date 2 September 2025
*/
class GeneralUtils
{
    /// <summary>
    /// Checks if an array contains a specific item using equality comparison.
    /// Uses EqualityComparer<T> for type-safe comparison of elements.
    /// </summary>
    /// <param name="array">The array to search through</param>
    /// <param name="item">The item to search for</param>
    /// <returns>true if the item is found in the array; otherwise, false</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when array is null</exception>
    public static bool Contains<T>(T[] array, T item)
    {
        // If array is null, throw exception
        if (array == null) throw new ArgumentException("Input array cannot be null.");

        // Initialize a comparer variable
        var comparer = EqualityComparer<T>.Default;

        // Loop through array; if item is found, return true
        foreach (var value in array)
        {
            if (comparer.Equals(value, item)) return true;
        }
        return false;
    }

    /// <summary>
    /// Returns a string with spaces for the specified indentation level.
    /// Each indentation level consists of exactly 4 spaces.
    /// </summary>
    /// <param name="level">The indentation level (0+)</param>
    /// <returns>A string containing 4 * level spaces, or empty string if level is 0 or negative</returns>
    public static string GetIndentation(int level)
    {
        string returnString = "";
        // Add four spaces for every level
        for (int i = 0; i < level; i++)
        {
            returnString += "    ";
        }
        return returnString;
    }

    /// <summary>
    /// Checks if all characters in a given string are lowercase.
    /// </summary>
    /// <param name="name">String to search through</param>
    /// <returns>False if uppercase character is present; true if all lowercase</returns>
    public static bool IsValidVariable(string name)
    {
        // Loop through each letter in input string
        foreach (char letter in name)
        {
            // If any uppercase characters, string is invalid
            if (char.IsUpper(letter)) return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if a string is a valid operator (+, -, *, /, //, %, **)
    /// </summary>
    /// <param name="op">String to validate</param>
    /// <returns>True if the string is an operator; false if otherwise</returns>
    /// <exception cref="System.NullReferenceException">Thrown when string is null</exception>
    public static bool IsValidOperator(string op)
    {
        // If input string is null, throw exception
        if (op == null) throw new NullReferenceException("Input string cannot be null.");

        return op == "+" || op == "-" || op == "*" || op == "/" ||
               op == "//" || op == "%" || op == "**";
    }

    // Counts how many times a given character appears in a string
    public static int CountOccurrences(string s, char c)
    {
        int count = 0;
        // Loop through each letter in input string
        foreach (char letter in s)
        {
            // Increase returned count if target letter occurs
            if (letter == c) count++;
        }
        return count;
    }

    /// <summary>
    /// Converts space-separated words to camelCase format.
    /// The first character is lower case, but all subsequent spaces
    /// are removed and the next letter is capitalized.
    /// </summary>
    /// <param name="s">String to convert to camelCase</param>
    /// <returns>String s in camelCase form, or an empty string if s is empty</returns>
    /// <exception cref="System.ArgumentException">Thrown when string is null</exception>
    public static string ToCamelCase(string s)
    {
        // If input string is empty, return empty
        if (s.Length == 0) return "";
        // If input string, is null, throw exception
        if (s == null) throw new ArgumentException("Input string cannot be null.");

        string camel = "";

        // Boolean for updating when letter after space should be capitalized
        bool nextUp = false;
        // Boolean for the first letter to be lowercase even after a space
        bool firstLetter = true;

        //Loop through the string; when there is a space, capitalize and add next letter 
        for (int i = 0; i < s.Length; i++)
        {
            if (char.IsWhiteSpace(s[i]))
            {
                // Mark next non-whitespace letter to be uppercase, skip whitespace
                nextUp = true;
                continue;
            }
            // Next letter is uppercase if after space, lowercase if not
            if (nextUp && !firstLetter)
            {
                camel += char.ToUpper(s[i]);
                nextUp = false;
            }
            else
            {
                camel += char.ToLower(s[i]);
                // Reset boolean variables
                firstLetter = false;
                nextUp = false;
            }
        }
        return camel;
    }

    /// <summary>
    /// Checks if a password meets strength requirements.
    /// Strong password must be at least 8 characters long, contain
    /// at least one uppercase letter, lowercase letter, digit, and special
    /// character (any non-alphanumeric character).
    /// </summary>
    /// <param name="pwd">String to verify strength of</param>
    /// <returns>True if all strength requirements met; false if otherwise</returns>
    public static bool IsPasswordStrong(string pwd)
    {
        // Length must be at least 8
        bool isLong = pwd.Length >= 8;
        // Must have an uppercase and a lowercase character
        bool hasUpper = pwd.Any(char.IsUpper);
        bool hasLower = pwd.Any(char.IsLower);
        // Must have a number
        bool hasDigit = pwd.Any(char.IsDigit);
        // Must have a special character
        bool hasSpecial = pwd.Any(c => !char.IsLetterOrDigit(c));

        // If password has all of these, return true
        return isLong && hasUpper && hasLower && hasDigit && hasSpecial;
    }

    /// <summary>
    /// Takes an input list and puts each unique item (no duplicates)
    /// into a return list.
    /// </summary>
    /// <param name="lst">List of any type T to search through</param>
    /// <returns>Returns list of unique items</returns>
    /// <exception cref="System.ArgumentException">Thrown when lst is null</exception>
    public static List<T> GetUniqueItems<T>(List<T> lst)
    {
        // Throws ArgumentException if list is null
        if (lst == null) throw new ArgumentException("Input list cannot be null.");

        // Initialize return list
        var uniqueList = new List<T>();

        // Loop through input list
        foreach (var item in lst) {
            // If current item is not in uniqueList, add it to it
            if (!uniqueList.Contains(item)) uniqueList.Add(item);
        }
        return uniqueList;
    }

    /// <summary>
    /// Calculates average value of an array of integers.
    /// </summary>
    /// <param name="numbers">Array of integers to average</param>
    /// <returns>Double representing the average of all ints in numbers</returns>
    /// <exception cref="System.ArgumentException">Throws when list is null or empty</exception>
    public static double CalculateAverage(int[]? numbers)
    {
        // Throws ArgumentException if list is null or empty
        if (numbers == null) throw new ArgumentException("Input list cannot be null.");
        if (numbers.Length == 0) throw new ArgumentException("Input list cannot be empty.");

        // Add all numbers from list into average
        double average = 0.0;
        foreach (int num in numbers)
        {
            average += num;
        }

        // Divide by list length and return
        average /= numbers.Length;
        return average;
    }

    /// <summary>
    /// Creates a set of all items that are duplicated.
    /// Implements a dictionary to store duplicated values.
    /// </summary>
    /// <param name="array">Array of any type T to search through</param>
    /// <returns>Array of duplicate values of the input array</returns>
    public static T[] Duplicates<T>(T[] array)
    {
        // Initialize dictionary where array values are stored
        var dupesDict = new Dictionary<int, T>();
        int key = 0;

        // Initialize a comparer variable to deal with nulls
        var comparer = EqualityComparer<T>.Default;

        // Loop through array
        // If dupe is found, add value to dupesDict
        for (int i = 1; i < array.Length; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                // If dupe found and is not already in dict, add current value to dict
                if (comparer.Equals(array[j], array[i]) && !dupesDict.ContainsValue(array[i]))
                {
                    dupesDict.Add(key, array[i]);
                    key++;
                    break;
                }
            }
        }

        // Initialize return array with size of dupesDict
        var dupesArray = new T[dupesDict.Count];

        // Assign values of dupesArray with values of dupesDict
        for (int i = 0; i < dupesArray.Length; i++)
        {
            dupesArray[i] = dupesDict[i];
        }
        return dupesArray;
    }
}