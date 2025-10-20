using Tokenizer;

namespace Parser
{
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
            throw new NotImplementedException();
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