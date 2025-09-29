/**
* Summary: 
*
* Bugs: 
*
* @author Reza Naqvi and Will Zoeller
* @date 9/28/25
*/
using System;
using System.Runtime.CompilerServices;

namespace Tokenizer
{
    // enum and class definitions
    /// <summary>
    /// </summary>
    /// <typeparam name="">The type of keys in the symbol table</typeparam>
    /// <typeparam name="">The type of values in the symbol table</typeparam>
    // public class SymbolTable<TKey, TValue> : IDictionary<TKey, TValue>
    // {

    // }
    // TokenType enumeration
    // TokenConstants static class
    static class TokenConstants
    {
        // +
        // public static int PLUS(int a, int b)
        // {
        //     return a + b;
        // }

        // (

        // {

        // :=

        // .

    }

    // Token class - should not be complex
    class Token
    {
        protected string _value;
        protected string _type;

        // Should these be properties?
        // private int _line;
        // private int _col;
        // private int _len;

        Token(string val, string T)
        {
            _value = val;
            _type = T;
        }

        // ToString
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        // Equals
        public bool Equals()
        {
            throw new NotImplementedException();
        }
        
        

        // etc.
    }
}
