using System.Text.RegularExpressions;
using Tokenizer;

namespace Parser
{
    public class ParseException : Exception
    {
        string E;
        public ParseException(string err)
        {
            E = err;
        }
    }

    public static class Parser
    {
        public static AST.BlockStmt Parse(string program)
        {
            // Do something with "program" to make it a list of strings
            var lines = new List<string>();

            var symbolTable = new SymbolTable<string, object>();
            return ParseBlockStmt(lines, symbolTable);
        }


        // Initiates parsing of a block. Should be called recursively as needed.
        // Consumes { and eventually }.
        private static AST.BlockStmt ParseBlockStmt(List<string> lines, SymbolTable<string, object> st)
        {
            // Check if program starts with { and ends with }
            // Throw parse exception if it doesn't

            throw new NotImplementedException();
        }


        // Parses a list of statements within a block. 
        // The list of statements may include a new block that should be handled recursively.
        // Consumes all statements until an end to the block: }.
        private static void ParseStmtList(List<string> lines, AST.BlockStmt block)
        {
            throw new NotImplementedException();
        }


        // Parses an assignment statement and adds the variable 
        // as a key to the symbol table (with a null value).
        private static AST.AssignmentStmt ParseAssignmentStmt(List<Token> tokens, SymbolTable<string, object> st)
        {
            throw new NotImplementedException();
        }


        // Parses a return statement.
        private static AST.ReturnStmt ParseReturnStatement(List<Token> tokens)
        {
            throw new NotImplementedException();
        }


        // Determines the type of statement and delegates 
        // to the appropriate parsing method: assignment or return.
        private static AST.Statement ParseStatement(List<Token> tokens)
        {
            throw new NotImplementedException();
        }


        // Parses an expression enclosed in parentheses.
        // Consumes ( and eventually ).
        private static AST.ExpressionNode ParseExpression(List<Token> tokens)
        {


            // if (tokens[0].ToString() != TokenConstants.LEFT_PAREN &
            //     tokens[tokens.Count - 1].ToString() != TokenConstants.RIGHT_PAREN)
            //     throw new ParseException($"Expression syntax invalid. must begin with a ( and end with a ). \n {tokens}");

            return ParseExpressionContent(tokens[1..(tokens.Count - 2)]);
            // throw new NotImplementedException();
        }


        // Parses the content of an expression.
        // Consumes the expression one token at a time.
        private static AST.ExpressionNode ParseExpressionContent(List<Token> tokens)
        {
            throw new NotImplementedException();
        }


        // Handles a single token expression (variable or int / float literal).
        private static AST.ExpressionNode HandleSingleToken(Token token)
        {
            throw new NotImplementedException();
        }


        // Creates the appropriate binary operator node based on the operator
        private static AST.ExpressionNode CreateBinaryOperatorNode(string op, AST.ExpressionNode l, AST.ExpressionNode r)
        {
            throw new NotImplementedException();
        }
        

        // Validates and creates a variable node.
        private static AST.VariableNode ParseVariableNode(string v)
        {
            throw new NotImplementedException();
        }
    }
}