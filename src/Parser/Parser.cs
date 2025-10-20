using System.Reflection.Metadata;
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
            if (!tokens[0].Type.Equals(TokenType.LEFT_PAREN))
                throw new ParseException($"Expression syntax invalid. must begin with a (. \n {tokens}");
            if (!tokens[tokens.Count - 1].Type.Equals(TokenType.RIGHT_PAREN))
                throw new ParseException($"Expression syntax invalid. must end with a ). \n {tokens}");
            
            return ParseExpressionContent(tokens[1..tokens.Count]);
        }


        // Parses the content of an expression.
        // Consumes the expression one token at a time.
        private static AST.ExpressionNode ParseExpressionContent(List<Token> tokens)
        {
            Token left = tokens[0];
            Token middle;
            if (tokens.Count > 1) middle = tokens[1];
            else return HandleSingleToken(left);

            if (left.Type.Equals(TokenType.UNKNOWN)) throw new ParseException($"Invalid Token: {left.Value}");
            else if (left.Type.Equals(TokenType.LEFT_PAREN)) return ParseExpression(tokens);
            else if (middle.Type.Equals(TokenType.OPERATOR))
            {
                if (tokens.Count < 3) throw new ParseException($"Invalid Syntax: {left.Value + middle.Value}");
                CreateBinaryOperatorNode(middle.Value, HandleSingleToken(left), HandleSingleToken(tokens[2]));
            }
            else
            {
                HandleSingleToken(left);
                ParseExpressionContent(tokens[1..tokens.Count]);
                // find out what tf to return
            }
            throw new NotImplementedException();
        }

        // Handles a single token expression (variable or int / float literal).
        private static AST.ExpressionNode HandleSingleToken(Token token)
        {
            // If variable, pass to ParseVariableNode
            if (token.Type.Equals(TokenType.VARIABLE)) return ParseVariableNode(token.Value);

            // If float or int, return Literal Node
            else if (token.Type.Equals(TokenType.FLOAT) || token.Type.Equals(TokenType.INTEGER))
                return new AST.LiteralNode(token.Value);

            // None of these; invalid token
            else throw new ParseException($"Invalid token: {token.Value}");
        }


        // Creates the appropriate binary operator node based on the operator
        private static AST.ExpressionNode CreateBinaryOperatorNode(string op, AST.ExpressionNode l, AST.ExpressionNode r)
        {
            throw new NotImplementedException();
        }
        

        // Validates and creates a variable node.
        private static AST.VariableNode ParseVariableNode(string v)
        {
            if (GeneralUtils.IsValidOperator(v)) return new AST.VariableNode(v);
            else throw new ParseException($"Variable name is invalid: {v}");
        }
    }
}