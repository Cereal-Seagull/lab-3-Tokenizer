/**
* ParseException class creates a custom exception type thrown when parsing errors occur.
*
* Parser class provides a Parse method to parse through DEC code using several private methods
* to separate pieces of tokenized code into individual AST nodes contained in expression and
* block statements.
*
* Bugs: Unknown format for unparsing parent block statements and where their curly braces should
* lie.
*
* @author Charlie Moss
* @author Will Zoeller
* @date 11/02/25
*/
using AST;
using Tokenizer;

namespace Parser
{
    /// <summary>
    /// Exception thrown when parsing errors occur.
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class.
        /// </summary>
        public ParseException() { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class with a specified error message.
        /// </summary>
        /// <param name="err">The error message that explains the reason for the exception.</param>
        public ParseException(string err) : base(err) { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException"/> class with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="err">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ParseException(string err, Exception inner) : base(err, inner) { }
    }

    /// <summary>
    /// Provides methods for parsing source code into an abstract syntax tree (AST).
    /// </summary>
    public static class Parser
    {
        #region Blocks
        /// <summary>
        /// Parses a program string into a block statement AST.
        /// </summary>
        /// <param name="program">The source code to parse.</param>
        /// <returns>A <see cref="BlockStmt"/> representing the parsed program.</returns>
        public static AST.BlockStmt Parse(string program)
        {
            
            var lines = new List<string>();
            var parentSymbolTable = new SymbolTable<string, object>();

            // Iterate through program, adding line to string list every new line (\n)
            int startIdx = 0;
            for (int i = 0; i < program.Length; i++)
            {
                // If new line, add line of code (removing trailing whitespace)
                if (program[i] == '\n')
                {
                    lines.Add(program[startIdx..i].Trim());
                    startIdx = i;
                }
            }
            // Add last line of code
            lines.Add(program[startIdx..program.Length].Trim());

            // Parse parent block statement
            return ParseBlockStmt(lines, parentSymbolTable);
        }


        /// <summary>
        /// Initiates parsing of a block statement. Should be called recursively as needed.
        /// Consumes opening and closing curly braces.
        /// </summary>
        /// <param name="lines">The list of code lines to parse.</param>
        /// <param name="st">The symbol table for the current scope.</param>
        /// <returns>A <see cref="BlockStmt"/> representing the parsed block.</returns>
        /// <exception cref="ParseException">Thrown if the block does not begin with '{' and end with '}'.</exception>
        private static AST.BlockStmt ParseBlockStmt(List<string> lines, SymbolTable<string, object> st)
        {
            // Create returning block statement
            AST.BlockStmt Block = new AST.BlockStmt(st);

            // If not parent scope (or is parent scope with brackets), check brackets
            if (st.Parent != null || (st.Parent == null && (lines[0] == TokenConstants.LEFT_CURLY ||
                                                lines[lines.Count - 1] == TokenConstants.RIGHT_CURLY)))
            {
                // Check if program starts with { and ends with }
                // Throw parse exception if it doesn't
                if (lines[0] != TokenConstants.LEFT_CURLY) throw new ParseException(
                                    "Syntax error: block must begin with single '{'");
                if (lines[lines.Count - 1] != TokenConstants.RIGHT_CURLY) throw new ParseException(
                                    "Syntax error: block must end with single '}'");

                // Parse through lines of code (not including beginning & ending {} )
                ParseStmtList(lines[1..(lines.Count - 1)], Block);
            }

            // If only one scope, parse entire expression (don't check brackets)
            else ParseStmtList(lines, Block);
            
            return Block;
        }


        /// <summary>
        /// Parses a list of statements within a block.
        /// The list of statements may include a new block that should be handled recursively.
        /// Consumes all statements until an end to the block.
        /// </summary>
        /// <param name="lines">The list of code lines to parse.</param>
        /// <param name="Block">The block statement to add parsed statements to.</param>
        /// <exception cref="ParseException">Thrown if the program ends unexpectedly or an invalid character is encountered.</exception>
        private static void ParseStmtList(List<string> lines, AST.BlockStmt Block)
        {
            // Iterate through every line of code
            for (int i = 0; i < lines.Count; i++)
            {
                // Handle nested block stmt
                if (lines[i] == "{")
                {
                    int level = 0;
                    int idx = 0;
                    // Finds last "}" on same scope
                    for (int j = i; j < lines.Count; j++)
                    {
                        if (lines[j] == "{") level++;
                        else if (lines[j] == "}") level--;
                        idx++;

                        // Found "}" on same scope
                        if (level == 0) break;
                    }
                    // "}" not found on same scope, missing '}'
                    if (level != 0) throw new ParseException(
                        "Invalid syntax: nested block stmt is missing '}'");

                    // Add nested block to block stmt with its own symbol table
                    Block.AddStatement(ParseBlockStmt(lines[i..(i + idx)],
                                                    new SymbolTable<string, object>(Block.SymbolTable)));
                    i += idx - 1;

                }
                else
                {
                    // Tokenize current line of code
                    var t = new TokenizerImpl();
                    List<Token> line = t.Tokenize(lines[i]);
                    
                    // Add parsed statement to block stmt
                    Block.AddStatement(ParseStatement(line, Block.SymbolTable));
                }
                
            }
        }

        #endregion

        #region Individual Statements
        /// <summary>
        /// Determines the type of statement and delegates to the appropriate parsing method.
        /// </summary>
        /// <param name="tokens">The list of tokens representing the statement.</param>
        /// <param name="st">The symbol table for the current scope.</param>
        /// <returns>A <see cref="Statement"/> representing the parsed statement.</returns>
        /// <exception cref="ParseException">Thrown if an unknown statement is encountered.</exception>
        private static AST.Statement ParseStatement(List<Token> tokens, SymbolTable<string,object> st)
        {
            // First token is 'return'; hand to ParseReturnStatement
            if (tokens[0].Type.Equals(TokenType.RETURN))
                return ParseReturnStatement(tokens);

            // Second token is ':='; hand to ParseAssignmentStatement
            else if (tokens[1].Type.Equals(TokenType.ASSIGNMENT)) return ParseAssignmentStmt(tokens, st);

            // No conditions met, throw exception
            else throw new ParseException("Statement syntax invalid;" +
                                $"must either be a return or assignment statement. \n {tokens}");
        }


        /// <summary>
        /// Parses an assignment statement and adds the variable as a key to the symbol table.
        /// </summary>
        /// <param name="tokens">The list of tokens representing the assignment statement.</param>
        /// <param name="st">The symbol table for the current scope.</param>
        /// <returns>An <see cref="AssignmentStmt"/> representing the parsed assignment.</returns>
        /// <exception cref="ParseException">Thrown if the assignment operator or syntax is invalid.</exception>
        private static AST.AssignmentStmt ParseAssignmentStmt(List<Token> tokens,
                                                                    SymbolTable<string, object> st)
        {
            // Expression is missing after := ; throw exception
            if (tokens.Count <= 2) throw new ParseException($"Missing expression: {tokens}");
            // Must have a variable before := ; throw exception
            if (!tokens[0].Type.Equals(TokenType.VARIABLE))
                throw new ParseException($"Invalid variable name: {tokens[0]}");
            // Second token must be := ; throw exception
            if (!tokens[1].Type.Equals(TokenType.ASSIGNMENT)) 
                throw new ParseException($"Expected assignment operator; Got {tokens[1]}");
            
            // Add variable to symbol table (first token)
            st.Add(tokens[0].Value, null); // may add right side of assignment stmt instead of null

            // Create and return assignment node (variable, expr)
            return new AST.AssignmentStmt(ParseVariableNode(tokens[0].Value),
                                    ParseExpression(tokens[2..tokens.Count]));
        }


        /// <summary>
        /// Parses a return statement.
        /// </summary>
        /// <param name="tokens">The list of tokens representing the return statement.</param>
        /// <returns>A <see cref="ReturnStmt"/> representing the parsed return statement.</returns>
        /// <exception cref="ParseException">Thrown if the return statement contains an empty expression.</exception>
        private static AST.ReturnStmt ParseReturnStatement(List<Token> tokens)
        {
            // 'return' is the only token given; throw exception
            if (tokens.Count <= 1) throw new ParseException("Syntax error: " +
                                "missing expression");
            // Create and return a return statement (return, expr)
            return new AST.ReturnStmt(ParseExpression(tokens[1..tokens.Count]));
        }

        #endregion

        #region Expressions
        /// <summary>
        /// Parses an expression enclosed in parentheses.
        /// Consumes opening and closing parentheses.
        /// </summary>
        /// <param name="tokens">The list of tokens representing the expression.</param>
        /// <returns>An <see cref="ExpressionNode"/> representing the parsed expression.</returns>
        /// <exception cref="ParseException">Thrown if the expression syntax is invalid or does not start with '(' and end with ')'.</exception>
        private static AST.ExpressionNode ParseExpression(List<Token> tokens)
        {
            // If just a variable or literal, hand off to HandleSingleToken
            if (tokens.Count == 1) return HandleSingleToken(tokens[0]);

            // First token is not "(", throw exception
            if (!tokens[0].Type.Equals(TokenType.LEFT_PAREN))
                throw new ParseException($"Expression syntax invalid; must begin with a (. \n {tokens}");

            // Last token is not ")", throw exception
            if (!tokens[tokens.Count - 1].Type.Equals(TokenType.RIGHT_PAREN))
                throw new ParseException($"Expression syntax invalid; must end with a ). \n {tokens}");

            // Parse content
            return ParseExpressionContent(tokens[1..(tokens.Count - 1)]);
        }

        /// <summary>
        /// Parses the content of an expression.
        /// Consumes the expression one token at a time.
        /// </summary>
        /// <param name="tokens">The list of tokens representing the expression content.</param>
        /// <returns>An <see cref="ExpressionNode"/> representing the parsed expression content.</returns>
        /// <exception cref="ParseException">Thrown if the expression syntax is invalid.</exception>
        private static AST.ExpressionNode ParseExpressionContent(List<Token> tokens)
        {
            if (tokens.Count == 0) throw new ParseException("Syntax error: empty expression");

            // If just a variable or literal, hand off to HandleSingleToken
            if (tokens.Count == 1) return HandleSingleToken(tokens[0]);

            else
            {
                // Handle redundant nesting (((x)))
                if (tokens[0].Type == TokenType.LEFT_PAREN && tokens[^1].Type == TokenType.RIGHT_PAREN)
                {
                    int level = 0;
                    bool nested = true;

                    // Make sure nesting is redundant 
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i].Type == TokenType.LEFT_PAREN) level++;
                        else if (tokens[i].Type == TokenType.RIGHT_PAREN) level--;

                        if (level == 0 && i < tokens.Count - 1)
                        {
                            nested = false;
                            break;
                        }
                    }

                    if (nested) return ParseExpression(tokens);
                }

                // Handles binary operators

                int opIdx = -1;
                // Find index of operator
                for (int i = 0; i < tokens.Count; i++)
                {
                    // Skip nested expressions
                    if (tokens[i].Type == TokenType.LEFT_PAREN)
                    {
                        // Initialize parentheses counts for nested expressions
                        int level = 0;
                        int idx = 0;

                        // Iterate through nested expression
                        // Separate for loop to skip nested expression's operators
                        for (int j = i; j < tokens.Count; j++)
                        {
                            // Add parentheses count appropriately
                            if (tokens[j].Type == TokenType.LEFT_PAREN) level++;
                            else if (tokens[j].Type == TokenType.RIGHT_PAREN) level--;
                            idx++;

                            // End of nested expression
                            if (level == 0) break;
                        }
                        i += idx - 1;
                    }

                    // Unknown operator found; throw exception
                    if (tokens[i].Type == TokenType.UNKNOWN) throw new ParseException(
                        $"Unknown token type in token list: {tokens}");

                    // Found operator
                    if (tokens[i].Type == TokenType.OPERATOR)
                    {
                        // If operator already found, throw exception
                        if (opIdx != -1) throw new ParseException("Syntax error: " +
                            "more than one operator in expression; missing parentheses?");
                        // Set operator's index to current position
                        else opIdx = i;
                    }
                }
                // Operator not found, throw exception
                if (opIdx == -1) throw new ParseException(
                    $"Invalid syntax: operator not found in token list: {tokens}");


                ExpressionNode l;
                // Left operand is another expression
                if (tokens[0].Type == TokenType.LEFT_PAREN) l = ParseExpression(tokens[0..opIdx]);
                // Left operand is a variable or literal
                else l = HandleSingleToken(tokens[0]);

                ExpressionNode r;
                // check if there is an right expression here
                if (opIdx + 1 >= tokens.Count) throw new ParseException($"Invalid syntax: missing right-hand size of operator: {tokens}");
                // Right operand is another expression
                if (tokens[opIdx + 1].Type == TokenType.LEFT_PAREN)
                    r = ParseExpression(tokens[(opIdx + 1)..tokens.Count]);
                // Right operand is a variable or literal
                else r = HandleSingleToken(tokens[opIdx + 1]);

                // Return binary operator node
                return CreateBinaryOperatorNode(tokens[opIdx].Value, l, r);
            }
        }

        /// <summary>
        /// Handles a single token expression (variable or numeric literal).
        /// </summary>
        /// <param name="token">The token to handle.</param>
        /// <returns>An <see cref="ExpressionNode"/> representing the variable or literal.</returns>
        /// <exception cref="ParseException">Thrown if the token is invalid.</exception>
        private static AST.ExpressionNode HandleSingleToken(Token token)
        {
            // If variable, pass to ParseVariableNode
            if (token.Type.Equals(TokenType.VARIABLE)) return ParseVariableNode(token.Value);

            // If float or int, return Literal Node
            else if (token.Type.Equals(TokenType.INTEGER))
                return new AST.LiteralNode(int.Parse(token.Value));
            else if (token.Type.Equals(TokenType.FLOAT))
                return new AST.LiteralNode(float.Parse(token.Value));

            // None of these; invalid token
            else throw new ParseException($"Invalid token: {token.Value}");
        }


        /// <summary>
        /// Creates the appropriate binary operator node based on the operator string.
        /// </summary>
        /// <param name="op">The operator string.</param>
        /// <param name="l">The left operand expression node.</param>
        /// <param name="r">The right operand expression node.</param>
        /// <returns>An <see cref="ExpressionNode"/> representing the binary operation.</returns>
        /// <exception cref="ParseException">Thrown if the operator is invalid.</exception>
        private static AST.ExpressionNode CreateBinaryOperatorNode(string op, AST.ExpressionNode l,
                                                                   AST.ExpressionNode r)
        {
            if (op == TokenConstants.PLUS) return new PlusNode(l, r);
            else if (op == TokenConstants.SUBTRACTION) return new MinusNode(l, r);
            else if (op == TokenConstants.TIMES) return new TimesNode(l, r);
            else if (op == TokenConstants.FLOAT_DIVISION) return new FloatDivNode(l, r);
            else if (op == TokenConstants.INT_DIVISION) return new IntDivNode(l, r);
            else if (op == TokenConstants.MODULUS) return new ModulusNode(l, r);
            else if (op == TokenConstants.EXPONENTIATE) return new ExponentiationNode(l, r);
            else throw new ParseException($"Invalid operator: {op}");
        }


        /// <summary>
        /// Validates and creates a variable node.
        /// </summary>
        /// <param name="v">The variable name string.</param>
        /// <returns>A <see cref="VariableNode"/> representing the variable.</returns>
        /// <exception cref="ParseException">Thrown if the variable name is invalid.</exception>
        private static AST.VariableNode ParseVariableNode(string v)
        {
            if (GeneralUtils.IsValidVariable(v)) return new AST.VariableNode(v);
            else throw new ParseException($"Variable name is invalid: {v}");
        }
    }
    
    #endregion
}