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
            throw new NotImplementedException();
        }


        private static AST.BlockStmt ParseBlockStmt(List<string> lines, SymbolTable<string, object> st)
        {
            throw new NotImplementedException();
        }

        private static void ParseStmtList(List<string> lines, AST.BlockStmt block)
        {
            throw new NotImplementedException();
        }

        private static AST.AssignmentStmt ParseAssignmentStmt(List<Token> tokens, SymbolTable<string, object> st)
        {
            throw new NotImplementedException();
        }

        private static AST.ReturnStmt ParseReturnStatement(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private static AST.Statement ParseStatement(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private static AST.ExpressionNode ParseExpression(List<Token> tokens)
        {
            // if (tokens[0].ToString() != TokenConstants.LEFT_PAREN &
            //     tokens[tokens.Count - 1].ToString() != TokenConstants.RIGHT_PAREN)
            //     throw new ParseException($"Expression syntax invalid. must begin with a ( and end with a ). \n {tokens}");

            return ParseExpressionContent(tokens[1..(tokens.Count - 2)]);
            // throw new NotImplementedException();
        }

        private static AST.ExpressionNode ParseExpressionContent(List<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private static AST.ExpressionNode HandleSingleToken(Token token)
        {
            throw new NotImplementedException();
        }

        private static AST.ExpressionNode CreateBinaryOperatorNode(string op, AST.ExpressionNode l, AST.ExpressionNode r)
        {
            throw new NotImplementedException();
        }
        
        private static AST.VariableNode ParseVariableNode(string v)
        {
            throw new NotImplementedException();
        }
    }
}