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
            if (!tokens[0].Type.Equals(TokenType.LEFT_PAREN))
                throw new ParseException($"Expression syntax invalid. must begin with a (. \n {tokens}");
            if (!tokens[tokens.Count - 1].Type.Equals(TokenType.RIGHT_PAREN))
                throw new ParseException($"Expression syntax invalid. must end with a ). \n {tokens}");
            
            return ParseExpressionContent(tokens[1..tokens.Count]);
        }

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
                CreateBinaryOperatorNode(middle, left, tokens[2])
            }
            else
            {
                HandleSingleToken(currTk);
                ParseExpressionContent(tokens[1..tokens.Count]);
                return;
            }
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