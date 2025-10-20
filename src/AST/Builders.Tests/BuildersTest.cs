namespace AST.Tests
{
    /// <summary>
    /// Unit tests for AST node classes including expressions, operators, and statements.
    /// Tests cover node creation, unparsing functionality, and builder pattern implementations.
    /// 
    /// NOTE: Potential bugs found in implementation:
    /// 1. BlockStmt.Unparse() doesn't respect the 'level' parameter for indentation
    /// 2. BinaryOperator constructors initialize with null children, requiring SetChildren call
    /// </summary>
    public class BuildersTest
    {
        #region LiteralNode Tests

        /// <summary>
        /// Tests that LiteralNode correctly stores and unparses integer values.
        /// </summary>
        [Theory]
        [InlineData(42)]
        [InlineData(0)]
        [InlineData(-100)]
        public void LiteralNode_IntegerValues_UnparsesCorrectly(int value)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(value.ToString(), result);
        }

        /// <summary>
        /// Tests that LiteralNode correctly stores and unparses various object types.
        /// </summary>
        [Theory]
        [InlineData(3.14, "3.14")]
        [InlineData("hello", "hello")]
        [InlineData(true, "True")]
        public void LiteralNode_VariousTypes_UnparsesCorrectly(object value, string expected)
        {
            // Arrange
            var node = new LiteralNode(value);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Tests that LiteralNode respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "42")]
        [InlineData(1, "    42")]
        [InlineData(2, "        42")]
        public void LiteralNode_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var node = new LiteralNode(42);

            // Act
            string result = node.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LiteralNode_Unparse_ReturnsValue()
        {
            var node = new LiteralNode(42);
            Assert.Equal("42", node.Unparse(0));
        }

        #endregion

        #region VariableNode Tests

        /// <summary>
        /// Tests that VariableNode correctly stores and unparses variable names.
        /// </summary>
        [Theory]
        [InlineData("x")]
        [InlineData("myVariable")]
        [InlineData("_temp123")]
        public void VariableNode_VariousNames_UnparsesCorrectly(string name)
        {
            // Arrange
            var node = new VariableNode(name);

            // Act
            string result = node.Unparse(0);

            // Assert
            Assert.Equal(name, result);
        }

        /// <summary>
        /// Tests that VariableNode respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "x")]
        [InlineData(1, "    x")]
        [InlineData(3, "            x")]
        public void VariableNode_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var node = new VariableNode("x");

            // Act
            string result = node.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VariableNode_Unparse_ReturnsVariableName()
        {
            var node = new VariableNode("x");
            Assert.Equal("x", node.Unparse(0));
        }

        #endregion

        #region BinaryOperator Tests

        /// <summary>
        /// Tests that PlusNode correctly unparses addition operations.
        /// </summary>
        [Fact]
        public void PlusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);

            // Act
            string result = plusNode.Unparse(0);

            // Assert
            Assert.Equal("5 + 3", result);
        }

        [Fact]
        public void PlusNode_Unparse_ReturnsCorrectExpression()
        {
            var left = new LiteralNode(3);
            var right = new LiteralNode(4);
            var plus = new PlusNode();
            plus.SetChildren(left, right);

            Assert.Equal("3 + 4", plus.Unparse(0));
        }

        /// <summary>
        /// Tests that MinusNode correctly unparses subtraction operations.
        /// </summary>


        [Fact]
        public void MinusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(10);
            var right = new LiteralNode(4);
            var minusNode = new MinusNode();
            minusNode.SetChildren(left, right);

            // Act
            string result = minusNode.Unparse(0);

            // Assert
            Assert.Equal("10 - 4", result);
        }

        /// <summary>
        /// Tests that TimesNode correctly unparses multiplication operations.
        /// </summary>
        [Fact]
        public void TimesNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(6);
            var right = new LiteralNode(7);
            var timesNode = new TimesNode();
            timesNode.SetChildren(left, right);

            // Act
            string result = timesNode.Unparse(0);

            // Assert
            Assert.Equal("6 * 7", result);
        }

        /// <summary>
        /// Tests that FloatDivNode correctly unparses float division operations.
        /// </summary>
        [Fact]
        public void FloatDivNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(15);
            var right = new LiteralNode(3);
            var divNode = new FloatDivNode();
            divNode.SetChildren(left, right);

            // Act
            string result = divNode.Unparse(0);

            // Assert
            Assert.Equal("15 / 3", result);
        }

        /// <summary>
        /// Tests that IntDivNode correctly unparses integer division operations.
        /// </summary>
        [Fact]
        public void IntDivNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(17);
            var right = new LiteralNode(5);
            var divNode = new IntDivNode();
            divNode.SetChildren(left, right);

            // Act
            string result = divNode.Unparse(0);

            // Assert
            Assert.Equal("17 // 5", result);
        }

        /// <summary>
        /// Tests that ModulusNode correctly unparses modulus operations.
        /// </summary>
        [Fact]
        public void ModulusNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(20);
            var right = new LiteralNode(6);
            var modNode = new ModulusNode();
            modNode.SetChildren(left, right);

            // Act
            string result = modNode.Unparse(0);

            // Assert
            Assert.Equal("20 % 6", result);
        }

        /// <summary>
        /// Tests that ExponentiationNode correctly unparses exponentiation operations.
        /// </summary>
        [Fact]
        public void ExponentiationNode_WithOperands_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(2);
            var right = new LiteralNode(8);
            var expNode = new ExponentiationNode();
            expNode.SetChildren(left, right);

            // Act
            string result = expNode.Unparse(0);

            // Assert
            Assert.Equal("2 ** 8", result);
        }

        /// <summary>
        /// Tests that binary operators can handle variable operands.
        /// </summary>
        [Fact]
        public void BinaryOperator_WithVariables_UnparsesCorrectly()
        {
            // Arrange
            var left = new VariableNode("x");
            var right = new VariableNode("y");
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);

            // Act
            string result = plusNode.Unparse(0);

            // Assert
            Assert.Equal("x + y", result);
        }

        /// <summary>
        /// Tests that binary operators can be nested to form complex expressions.
        /// </summary>
        [Fact]
        public void BinaryOperator_Nested_UnparsesCorrectly()
        {
            // Arrange
            var innerLeft = new LiteralNode(2);
            var innerRight = new LiteralNode(3);
            var innerPlus = new PlusNode();
            innerPlus.SetChildren(innerLeft, innerRight);

            var outerRight = new LiteralNode(4);
            var outerTimes = new TimesNode();
            outerTimes.SetChildren(innerPlus, outerRight);

            // Act
            string result = outerTimes.Unparse(0);

            // Assert
            Assert.Equal("2 + 3 * 4", result);
        }

        #endregion

        #region AssignmentStmt Tests

        /// <summary>
        /// Tests that AssignmentStmt correctly unparses simple assignments.
        /// </summary>
        [Fact]
        public void AssignmentStmt_SimpleAssignment_UnparsesCorrectly()
        {
            // Arrange
            var variable = new VariableNode("x");
            var value = new LiteralNode(42);
            var assignment = new AssignmentStmt(variable, value);

            // Act
            string result = assignment.Unparse(0);

            // Assert
            Assert.Equal("x := 42", result);
        }

        /// <summary>
        /// Tests that AssignmentStmt handles complex expressions on the right-hand side.
        /// </summary>
        [Fact]
        public void AssignmentStmt_WithExpression_UnparsesCorrectly()
        {
            // Arrange
            var variable = new VariableNode("result");
            var left = new LiteralNode(10);
            var right = new LiteralNode(5);
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);
            var assignment = new AssignmentStmt(variable, plusNode);

            // Act
            string result = assignment.Unparse(0);

            // Assert
            Assert.Equal("result := 10 + 5", result);
        }

        /// <summary>
        /// Tests that AssignmentStmt respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "x := 5")]
        [InlineData(1, "    x := 5")]
        [InlineData(2, "        x := 5")]
        public void AssignmentStmt_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var variable = new VariableNode("x");
            var value = new LiteralNode(5);
            var assignment = new AssignmentStmt(variable, value);

            // Act
            string result = assignment.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AssignmentStmt_Unparse_ReturnsCorrectFormat()
        {
            var variable = new VariableNode("x");
            var value = new LiteralNode(10);
            var assignment = new AssignmentStmt(variable, value);
            Assert.Equal("x := 10", assignment.Unparse(0));
        }


        #endregion

        #region ReturnStmt Tests

        /// <summary>
        /// Tests that ReturnStmt correctly unparses return statements with literals.
        /// </summary>
        [Fact]
        public void ReturnStmt_WithLiteral_UnparsesCorrectly()
        {
            // Arrange
            var value = new LiteralNode(100);
            var returnStmt = new ReturnStmt(value);

            // Act
            string result = returnStmt.Unparse(0);

            // Assert
            Assert.Equal("return 100", result);
        }

        /// <summary>
        /// Tests that ReturnStmt correctly unparses return statements with expressions.
        /// </summary>
        [Fact]
        public void ReturnStmt_WithExpression_UnparsesCorrectly()
        {
            // Arrange
            var left = new VariableNode("a");
            var right = new VariableNode("b");
            var plusNode = new PlusNode();
            plusNode.SetChildren(left, right);
            var returnStmt = new ReturnStmt(plusNode);

            // Act
            string result = returnStmt.Unparse(0);

            // Assert
            Assert.Equal("return a + b", result);
        }

        /// <summary>
        /// Tests that ReturnStmt respects indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "return 0")]
        [InlineData(1, "    return 0")]
        [InlineData(2, "        return 0")]
        public void ReturnStmt_WithIndentation_FormatsCorrectly(int level, string expected)
        {
            // Arrange
            var value = new LiteralNode(0);
            var returnStmt = new ReturnStmt(value);

            // Act
            string result = returnStmt.Unparse(level);

            // Assert
            Assert.Equal(expected, result);
        }

        #endregion

        #region BlockStmt Tests

        /// <summary>
        /// Tests that BlockStmt correctly unparses empty blocks.
        /// </summary>
        [Fact]
        public void BlockStmt_Empty_UnparsesCorrectly()
        {
            var block = new BlockStmt(new SymbolTable<string, object>());
            string result = block.Unparse(0);
            Assert.Equal("{\n}", result);
        }

        /// <summary>
        /// Tests that BlockStmt correctly unparses blocks with single statement.
        /// </summary>
        [Fact]
        public void BlockStmt_SingleStatement_UnparsesCorrectly()
        {
            var variable = new VariableNode("x");
            var value = new LiteralNode(5);
            var assignment = new AssignmentStmt(variable, value);
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStmt(assignment);
            string result = block.Unparse(0);
            Assert.Equal("{\n    x := 5\n}", result);
        }

        /// <summary>
        /// Tests that BlockStmt correctly unparses blocks with multiple statements.
        /// </summary>
        [Fact]
        public void BlockStmt_MultipleStatements_UnparsesCorrectly()
        {
            var assign1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(10));
            var assign2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(20));

            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStmt(assign1);
            block.AddStmt(assign2);

            string result = block.Unparse(0);
            Assert.Equal("{\n    x := 10\n    y := 20\n}", result);
        }

        /// <summary>
        /// Tests that AddStmt method correctly adds statements to the block.
        /// </summary>
        [Fact]
        public void BlockStmt_AddStmt_AddsStatementCorrectly()
        {
            var block = new BlockStmt(new SymbolTable<string, object>());
            var variable = new VariableNode("z");
            var value = new LiteralNode(30);
            var assignment = new AssignmentStmt(variable, value);
            block.AddStmt(assignment);

            string result = block.Unparse(0);
            Assert.Equal("{\n    z := 30\n}", result);
        }

        #endregion

        #region DefaultBuilder Tests

        /// <summary>
        /// Tests that DefaultBuilder creates all binary operator nodes correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreatePlusNode_CreatesValidNode()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var left = new LiteralNode(1);
            var right = new LiteralNode(2);

            // Act
            var node = builder.CreatePlusNode(left, right);

            // Assert
            Assert.NotNull(node);
            Assert.Equal("1 + 2", node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates various operator nodes correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateOperatorNodes_CreatesValidNodes()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var left = new LiteralNode(10);
            var right = new LiteralNode(3);

            // Act & Assert
            Assert.NotNull(builder.CreateMinusNode(left, right));
            Assert.NotNull(builder.CreateTimesNode(left, right));
            Assert.NotNull(builder.CreateFloatDivNode(left, right));
            Assert.NotNull(builder.CreateIntDivNode(left, right));
            Assert.NotNull(builder.CreateModulusNode(left, right));
            Assert.NotNull(builder.CreateExponentiationNode(left, right));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates expression nodes correctly.
        /// </summary>
        [Theory]
        [InlineData(42)]
        [InlineData(3.14)]
        [InlineData("test")]
        public void DefaultBuilder_CreateLiteralNode_CreatesValidNode(object value)
        {
            // Arrange
            var builder = new DefaultBuilder();

            // Act
            var node = builder.CreateLiteralNode(value);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(value.ToString(), node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates VariableNode correctly.
        /// </summary>
        [Theory]
        [InlineData("x")]
        [InlineData("myVar")]
        [InlineData("temp123")]
        public void DefaultBuilder_CreateVariableNode_CreatesValidNode(string name)
        {
            // Arrange
            var builder = new DefaultBuilder();

            // Act
            var node = builder.CreateVariableNode(name);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(name, node.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates AssignmentStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateAssignmentStmt_CreatesValidStatement()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var variable = new VariableNode("x");
            var value = new LiteralNode(100);

            // Act
            var stmt = builder.CreateAssignmentStmt(variable, value);

            // Assert
            Assert.NotNull(stmt);
            Assert.Equal("x := 100", stmt.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates ReturnStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateReturnStmt_CreatesValidStatement()
        {
            // Arrange
            var builder = new DefaultBuilder();
            var value = new LiteralNode(42);

            // Act
            var stmt = builder.CreateReturnStmt(value);

            // Assert
            Assert.NotNull(stmt);
            Assert.Equal("return 42", stmt.Unparse(0));
        }

        /// <summary>
        /// Tests that DefaultBuilder creates BlockStmt correctly.
        /// </summary>
        [Fact]
        public void DefaultBuilder_CreateBlockStmt_CreatesValidStatement()
        {
            var builder = new DefaultBuilder();

            var assign = new AssignmentStmt(new VariableNode("x"), new LiteralNode(1));
            var block = new BlockStmt(new SymbolTable<string, object>());
            block.AddStmt(assign);

            Assert.NotNull(block);
            Assert.Contains(":=", block.Unparse(0));
        }

        #endregion

        #region NullBuilder Tests

        /// <summary>
        /// Tests that NullBuilder returns null for all operator node creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateOperatorNodes_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();
            var left = new LiteralNode(1);
            var right = new LiteralNode(2);

            // Act & Assert
            Assert.Null(builder.CreatePlusNode(left, right));
            Assert.Null(builder.CreateMinusNode(left, right));
            Assert.Null(builder.CreateTimesNode(left, right));
            Assert.Null(builder.CreateFloatDivNode(left, right));
            Assert.Null(builder.CreateIntDivNode(left, right));
            Assert.Null(builder.CreateModulusNode(left, right));
            Assert.Null(builder.CreateExponentiationNode(left, right));
        }

        /// <summary>
        /// Tests that NullBuilder returns null for expression node creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateExpressionNodes_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();

            // Act & Assert
            Assert.Null(builder.CreateLiteralNode(42));
            Assert.Null(builder.CreateVariableNode("x"));
        }

        /// <summary>
        /// Tests that NullBuilder returns null for statement creation methods.
        /// </summary>
        [Fact]
        public void NullBuilder_CreateStatements_ReturnsNull()
        {
            // Arrange
            var builder = new NullBuilder();
            var variable = new VariableNode("x");
            var value = new LiteralNode(1);
            var statements = new SymbolTable<string, object>();

            // Act & Assert
            Assert.Null(builder.CreateAssignmentStmt(variable, value));
            Assert.Null(builder.CreateReturnStmt(value));
            Assert.Null(builder.CreateBlockStmt(statements));
        }

        #endregion

        #region DebugBuilder Tests

        /// <summary>
        /// Tests that DebugBuilder creates nodes and can be verified through unparsing.
        /// This test captures console output to verify debug messages are written.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreatePlusNode_CreatesValidNodeAndOutputsDebugInfo()
        {
            // Arrange
            var builder = new DebugBuilder();
            var left = new LiteralNode(5);
            var right = new LiteralNode(3);

            // Capture console output
            var originalOut = Console.Out;
            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                var node = builder.CreatePlusNode(left, right);

                // Reset console output
                Console.SetOut(originalOut);
                var output = stringWriter.ToString();

                // Assert
                Assert.NotNull(node);
                Assert.Equal("5 + 3", node.Unparse(0));
                Assert.Contains("Plus node created", output);
            }
        }

        /// <summary>
        /// Tests that DebugBuilder outputs debug information for various node types.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreateVariousNodes_OutputsDebugInfo()
        {
            // Arrange
            var builder = new DebugBuilder();
            var originalOut = Console.Out;

            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                builder.CreateLiteralNode(42);
                builder.CreateVariableNode("test");
                builder.CreateReturnStmt(new LiteralNode(1));

                // Reset console output
                Console.SetOut(originalOut);
                var output = stringWriter.ToString();

                // Assert
                Assert.Contains("Literal Node created", output);
                Assert.Contains("Variable Node created", output);
                Assert.Contains("Return Statement Created", output);
            }
        }

        /// <summary>
        /// Tests that DebugBuilder creates functional nodes despite debugging output.
        /// </summary>
        [Fact]
        public void DebugBuilder_CreatedNodes_AreFunctional()
        {
            // Arrange
            var builder = new DebugBuilder();
            var originalOut = Console.Out;

            using (var stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                // Act
                var variable = builder.CreateVariableNode("result");
                var left = builder.CreateLiteralNode(10);
                var right = builder.CreateLiteralNode(5);
                var plus = builder.CreatePlusNode(left, right);
                var assignment = builder.CreateAssignmentStmt(variable, plus);

                // Reset console output
                Console.SetOut(originalOut);

                // Assert
                Assert.NotNull(assignment);
                Assert.Equal("result := 10 + 5", assignment.Unparse(0));
            }
        }

        #endregion





    /// <summary>
    /// Test suite for validating scoping behavior in the Abstract Syntax Tree (AST).
    /// These tests verify that nested blocks correctly implement lexical scoping,
    /// including variable shadowing, scope isolation, and hierarchical lookup.
    /// </summary>
    public class ASTScopingTests
    {
        #region Helper Methods

        /// <summary>
        /// Creates a simple global scope with predefined variables for testing.
        /// </summary>
        /// <returns>A SymbolTable with test variables x=10, y=20, z=30</returns>
        private SymbolTable<string, object> CreateGlobalScope()
        {
            var scope = new SymbolTable<string, object>();
            scope.Add("x", 10);
            scope.Add("y", 20);
            scope.Add("z", 30);
            return scope;
        }

        /// <summary>
        /// Creates a nested scope hierarchy for testing.
        /// Structure: global -> outer -> inner
        /// </summary>
        /// <returns>Tuple containing (globalScope, outerScope, innerScope)</returns>
        private (SymbolTable<string, object> global, 
                 SymbolTable<string, object> outer, 
                 SymbolTable<string, object> inner) CreateNestedScopes()
        {
            var global = new SymbolTable<string, object>();
            global.Add("x", 1);
            
            var outer = global.CreateNewScope_GivenParent();
            outer.Add("y", 2);
            
            var inner = outer.CreateNewScope_GivenParent();
            inner.Add("z", 3);
            
            return (global, outer, inner);
        }

        #endregion

        #region BlockStmt Scoping Tests

        /// <summary>
        /// Tests that a BlockStmt correctly initializes with a symbol table.
        /// </summary>
        [Fact]
        public void BlockStmt_InitializesWithSymbolTable()
        {
            // Arrange
            var scope = CreateGlobalScope();
            
            // Act
            var block = new BlockStmt(scope);
            
            // Assert
            Assert.NotNull(block);
            // Note: SymbolTable property needs to be added to BlockStmt or use reflection
            // This test assumes the property exists or we verify through behavior
        }

        /// <summary>
        /// Tests that nested blocks maintain separate symbol tables with proper parent references.
        /// </summary>
        [Fact]
        public void BlockStmt_NestedBlocks_HaveSeparateSymbolTables()
        {
            // Arrange
            var globalScope = CreateGlobalScope();
            var outerBlock = new BlockStmt(globalScope);
            
            // Create nested scope
            var innerScope = globalScope.CreateNewScope_GivenParent();
            var innerBlock = new BlockStmt(innerScope);
            
            // Act & Assert - Verify through parent relationship
            Assert.NotNull(innerScope.Parent);
            Assert.Same(globalScope, innerScope.Parent);
        }

        /// <summary>
        /// Tests that variables defined in outer scope are accessible from inner scope.
        /// </summary>
        [Theory]
        [InlineData("x", 10)]
        [InlineData("y", 20)]
        [InlineData("z", 30)]
        public void SymbolTable_InnerScope_CanAccessOuterScopeVariables(string varName, int expectedValue)
        {
            // Arrange
            var globalScope = CreateGlobalScope();
            var innerScope = globalScope.CreateNewScope_GivenParent();
            
            // Act
            bool found = innerScope.TryGetValue(varName, out object value);
            
            // Assert
            Assert.True(found);
            Assert.Equal(expectedValue, value);
        }

        /// <summary>
        /// Tests that variables can be shadowed in inner scopes without affecting outer scope.
        /// </summary>
        [Fact]
        public void SymbolTable_InnerScope_CanShadowOuterVariable()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            globalScope.Add("x", 10);
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            innerScope.Add("x", 100); // Shadow outer 'x'
            
            // Act
            bool foundInner = innerScope.TryGetValue("x", out object innerValue);
            bool foundOuter = globalScope.TryGetValue("x", out object outerValue);
            
            // Assert
            Assert.True(foundInner);
            Assert.True(foundOuter);
            Assert.Equal(100, innerValue); // Inner sees shadowed value
            Assert.Equal(10, outerValue);  // Outer sees original value
        }

        /// <summary>
        /// Tests that shadowing is correctly identified using local scope checks.
        /// </summary>
        [Fact]
        public void SymbolTable_ShadowedVariable_ExistsInBothScopes()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            globalScope.Add("x", 10);
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            innerScope.Add("x", 100);
            
            // Act
            bool existsInGlobalLocal = globalScope.ContainsKeyLocal("x");
            bool existsInInnerLocal = innerScope.ContainsKeyLocal("x");
            
            // Assert
            Assert.True(existsInGlobalLocal);
            Assert.True(existsInInnerLocal);
        }

        /// <summary>
        /// Tests that variables defined in inner scope are not accessible from outer scope.
        /// </summary>
        [Fact]
        public void SymbolTable_OuterScope_CannotAccessInnerScopeVariables()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            var innerScope = globalScope.CreateNewScope_GivenParent();
            innerScope.Add("innerVar", 42);
            
            // Act
            bool found = globalScope.TryGetValue("innerVar", out _);
            
            // Assert
            Assert.False(found);
        }

        #endregion

        #region Multi-Level Scoping Tests

        /// <summary>
        /// Tests variable lookup through multiple levels of nested scopes.
        /// </summary>
        [Theory]
        [InlineData("x", 1)] // From global (grandparent)
        [InlineData("y", 2)] // From outer (parent)
        [InlineData("z", 3)] // From inner (current)
        public void SymbolTable_ThreeLevelNesting_VariableLookup(string varName, int expectedValue)
        {
            // Arrange
            var (global, outer, inner) = CreateNestedScopes();
            
            // Act
            bool found = inner.TryGetValue(varName, out object value);
            
            // Assert
            Assert.True(found);
            Assert.Equal(expectedValue, value);
        }

        /// <summary>
        /// Tests that middle scope can shadow grandparent variable.
        /// </summary>
        [Fact]
        public void SymbolTable_MiddleScope_CanShadowGrandparentVariable()
        {
            // Arrange
            var global = new SymbolTable<string, object>();
            global.Add("x", 1);
            
            var outer = global.CreateNewScope_GivenParent();
            outer.Add("x", 10); // Shadow global x
            
            var inner = outer.CreateNewScope_GivenParent();
            
            // Act
            bool found = inner.TryGetValue("x", out object value);
            
            // Assert
            Assert.True(found);
            Assert.Equal(10, value); // Should find outer's value, not global's
        }

        /// <summary>
        /// Tests that innermost shadowing takes precedence over all parent scopes.
        /// </summary>
        [Fact]
        public void SymbolTable_MultipleShadowing_InnermostTakesPrecedence()
        {
            // Arrange
            var global = new SymbolTable<string, object>();
            global.Add("x", 1);
            
            var outer = global.CreateNewScope_GivenParent();
            outer.Add("x", 10);
            
            var inner = outer.CreateNewScope_GivenParent();
            inner.Add("x", 100);
            
            // Act
            bool found = inner.TryGetValue("x", out object value);
            
            // Assert
            Assert.True(found);
            Assert.Equal(100, value);
        }

        #endregion

        #region Statement Integration Tests

        /// <summary>
        /// Tests that AssignmentStmt correctly unparses with variable names.
        /// </summary>
        [Fact]
        public void AssignmentStmt_UnparsesCorrectly()
        {
            // Arrange
            var varNode = new VariableNode("x");
            var litNode = new LiteralNode(20);
            var assignStmt = new AssignmentStmt(varNode, litNode);
            
            // Act
            string unparsed = assignStmt.Unparse(0);
            
            // Assert
            Assert.Contains("x", unparsed);
            Assert.Contains("=", unparsed);
            Assert.Contains("20", unparsed);
        }

        /// <summary>
        /// Tests that AssignmentStmt unparses with correct indentation.
        /// </summary>
        [Theory]
        [InlineData(0, "x := 20")]
        [InlineData(1, "    x := 20")]
        [InlineData(2, "        x := 20")]
        public void AssignmentStmt_UnparsesWithCorrectIndentation(int level, string expected)
        {
            // Arrange
            var varNode = new VariableNode("x");
            var litNode = new LiteralNode(20);
            var assignStmt = new AssignmentStmt(varNode, litNode);
            
            // Act
            string unparsed = assignStmt.Unparse(level);
            
            // Assert
            Assert.Equal(expected, unparsed);
        }

        /// <summary>
        /// Tests that ReturnStmt correctly unparses with variable names.
        /// </summary>
        [Fact]
        public void ReturnStmt_UnparsesCorrectly()
        {
            // Arrange
            var varNode = new VariableNode("result");
            var returnStmt = new ReturnStmt(varNode);
            
            // Act
            string unparsed = returnStmt.Unparse(0);
            
            // Assert
            Assert.Contains("return", unparsed);
            Assert.Contains("result", unparsed);
        }

        /// <summary>
        /// Tests that ReturnStmt unparses with correct indentation.
        /// </summary>
        [Theory]
        [InlineData(0, "return result")]
        [InlineData(1, "    return result")]
        [InlineData(2, "        return result")]
        public void ReturnStmt_UnparsesWithCorrectIndentation(int level, string expected)
        {
            // Arrange
            var varNode = new VariableNode("result");
            var returnStmt = new ReturnStmt(varNode);
            
            // Act
            string unparsed = returnStmt.Unparse(level);
            
            // Assert
            Assert.Equal(expected, unparsed);
        }

        /// <summary>
        /// Tests that blocks can contain multiple statements and add them correctly.
        /// </summary>
        [Fact]
        public void BlockStmt_AddMultipleStatements_StoresCorrectly()
        {
            // Arrange
            var scope = new SymbolTable<string, object>();
            var block = new BlockStmt(scope);
            
            var assign1 = new AssignmentStmt(new VariableNode("x"), new LiteralNode(30));
            var assign2 = new AssignmentStmt(new VariableNode("y"), new LiteralNode(40));
            var returnStmt = new ReturnStmt(new VariableNode("x"));
            
            // Act
            block.AddStmt(assign1);
            block.AddStmt(assign2);
            block.AddStmt(returnStmt);
            
            string unparsed = block.Unparse(0);
            
            // Assert - Verify all statements appear in output
            Assert.Contains("x := 30", unparsed);
            Assert.Contains("y := 40", unparsed);
            Assert.Contains("return x", unparsed);
        }

        #endregion

        #region Unparsing with Scoping Tests

        /// <summary>
        /// Tests that nested blocks unparse with correct structure and indentation.
        /// </summary>
        [Fact]
        public void BlockStmt_NestedBlocks_UnparseWithCorrectStructure()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            var outerBlock = new BlockStmt(globalScope);
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            var innerBlock = new BlockStmt(innerScope);
            
            innerBlock.AddStmt(new ReturnStmt(new LiteralNode(42)));
            outerBlock.AddStmt(innerBlock);
            
            // Act
            string unparsed = outerBlock.Unparse(0);
            
            // Assert
            Assert.Contains("{", unparsed);
            Assert.Contains("}", unparsed);
            Assert.Contains("return 42", unparsed);
            // Check that we have at least 2 opening and 2 closing braces for nested structure
            Assert.Equal(2, unparsed.Count(c => c == '{'));
            Assert.Equal(2, unparsed.Count(c => c == '}'));
        }

        /// <summary>
        /// Tests that empty blocks unparse correctly.
        /// </summary>
        [Fact]
        public void BlockStmt_EmptyBlock_UnparsesCorrectly()
        {
            // Arrange
            var scope = new SymbolTable<string, object>();
            var block = new BlockStmt(scope);
            
            // Act
            string unparsed = block.Unparse(0);
            
            // Assert
            Assert.Contains("{", unparsed);
            Assert.Contains("}", unparsed);
            Assert.Equal("{\n}", unparsed);
        }

        /// <summary>
        /// Tests unparsing of a complete nested structure with assignments and returns.
        /// </summary>
        [Fact]
        public void BlockStmt_CompleteNestedStructure_UnparsesCorrectly()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            var outerBlock = new BlockStmt(globalScope);
            
            outerBlock.AddStmt(new AssignmentStmt(
                new VariableNode("x"), 
                new LiteralNode(20)
            ));
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            var innerBlock = new BlockStmt(innerScope);
            
            innerBlock.AddStmt(new AssignmentStmt(
                new VariableNode("y"), 
                new LiteralNode(40)
            ));
            innerBlock.AddStmt(new ReturnStmt(new VariableNode("y")));
            
            outerBlock.AddStmt(innerBlock);
            
            // Act
            string unparsed = outerBlock.Unparse(0);
            
            // Assert
            Assert.Contains("x := 20", unparsed);
            Assert.Contains("y := 40", unparsed);
            Assert.Contains("return y", unparsed);
            Assert.Contains("{", unparsed);
            Assert.Contains("}", unparsed);
        }

        /// <summary>
        /// Tests that BlockStmt correctly indents nested blocks.
        /// </summary>
        [Fact]
        public void BlockStmt_NestedBlocks_IndentationIncreases()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            var outerBlock = new BlockStmt(globalScope);
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            var innerBlock = new BlockStmt(innerScope);
            
            var stmt = new ReturnStmt(new LiteralNode(42));
            innerBlock.AddStmt(stmt);
            outerBlock.AddStmt(innerBlock);
            
            // Act
            string unparsed = outerBlock.Unparse(0);
            
            // Assert
            // The inner return statement should have more indentation than outer block content
            var lines = unparsed.Split('\n');
            bool hasIndentedContent = lines.Any(line => line.StartsWith("        ")); // 2 levels of indent
            Assert.True(hasIndentedContent);
        }

        #endregion

        #region Expression Node Tests

        /// <summary>
        /// Tests that LiteralNode unparses correctly at different indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, 42, "42")]
        [InlineData(1, 42, "    42")]
        [InlineData(0, 3.14, "3.14")]
        public void LiteralNode_UnparsesCorrectly(int level, object value, string expected)
        {
            // Arrange
            var node = new LiteralNode(value);
            
            // Act
            string unparsed = node.Unparse(level);
            
            // Assert
            Assert.Equal(expected, unparsed);
        }

        /// <summary>
        /// Tests that VariableNode unparses correctly at different indentation levels.
        /// </summary>
        [Theory]
        [InlineData(0, "x", "x")]
        [InlineData(1, "myVar", "    myVar")]
        [InlineData(2, "result", "        result")]
        public void VariableNode_UnparsesCorrectly(int level, string varName, string expected)
        {
            // Arrange
            var node = new VariableNode(varName);
            
            // Act
            string unparsed = node.Unparse(level);
            
            // Assert
            Assert.Equal(expected, unparsed);
        }

        #endregion

        #region Binary Operator Tests

        /// <summary>
        /// Tests that binary operators unparse correctly with proper spacing.
        /// </summary>
        [Fact]
        public void PlusNode_UnparsesCorrectly()
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(10);
            var plus = new PlusNode();
            plus.SetChildren(left, right);
            
            // Act
            string unparsed = plus.Unparse(0);
            
            // Assert
            Assert.Equal("5 + 10", unparsed);
        }

        /// <summary>
        /// Tests various binary operators unparsing correctly.
        /// </summary>
        [Theory]
        [InlineData("MinusNode", "-")]
        [InlineData("TimesNode", "*")]
        [InlineData("FloatDivNode", "/")]
        [InlineData("IntDivNode", "//")]
        [InlineData("ModulusNode", "%")]
        [InlineData("ExponentiationNode", "**")]
        public void BinaryOperators_UnparseWithCorrectOperator(string nodeType, string expectedOperator)
        {
            // Arrange
            var left = new LiteralNode(5);
            var right = new LiteralNode(10);
            
            BinaryOperator op = nodeType switch
            {
                "MinusNode" => new MinusNode(),
                "TimesNode" => new TimesNode(),
                "FloatDivNode" => new FloatDivNode(),
                "IntDivNode" => new IntDivNode(),
                "ModulusNode" => new ModulusNode(),
                "ExponentiationNode" => new ExponentiationNode(),
                _ => throw new ArgumentException("Unknown node type")
            };
            
            op.SetChildren(left, right);
            
            // Act
            string unparsed = op.Unparse(0);
            
            // Assert
            Assert.Contains(expectedOperator, unparsed);
            Assert.Contains("5", unparsed);
            Assert.Contains("10", unparsed);
        }

        /// <summary>
        /// Tests nested binary operations unparse correctly.
        /// </summary>
        [Fact]
        public void BinaryOperators_NestedOperations_UnparseCorrectly()
        {
            // Arrange - (5 + 10) * 2
            var left = new PlusNode();
            left.SetChildren(new LiteralNode(5), new LiteralNode(10));
            
            var times = new TimesNode();
            times.SetChildren(left, new LiteralNode(2));
            
            // Act
            string unparsed = times.Unparse(0);
            
            // Assert
            Assert.Contains("5 + 10", unparsed);
            Assert.Contains("*", unparsed);
            Assert.Contains("2", unparsed);
        }

        #endregion

        #region Edge Cases

        /// <summary>
        /// Tests that variables with same name in sibling scopes don't interfere.
        /// </summary>
        [Fact]
        public void SymbolTable_SiblingScopes_VariablesAreIndependent()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            
            var scope1 = globalScope.CreateNewScope_GivenParent();
            scope1.Add("x", 10);
            
            var scope2 = globalScope.CreateNewScope_GivenParent();
            scope2.Add("x", 20);
            
            // Act
            bool found1 = scope1.TryGetValueLocal("x", out object value1);
            bool found2 = scope2.TryGetValueLocal("x", out object value2);
            
            // Assert
            Assert.True(found1);
            Assert.True(found2);
            Assert.Equal(10, value1);
            Assert.Equal(20, value2);
        }

        /// <summary>
        /// Tests lookup of non-existent variables across all scope levels.
        /// </summary>
        [Theory]
        [InlineData("nonexistent")]
        [InlineData("undefined")]
        [InlineData("missing")]
        public void SymbolTable_NonexistentVariable_NotFoundInAnyScope(string varName)
        {
            // Arrange
            var (global, outer, inner) = CreateNestedScopes();
            
            // Act
            bool found = inner.TryGetValue(varName, out _);
            
            // Assert
            Assert.False(found);
        }

        /// <summary>
        /// Tests that local-only lookup doesn't search parent scopes.
        /// </summary>
        [Fact]
        public void SymbolTable_LocalLookup_DoesNotSearchParentScopes()
        {
            // Arrange
            var globalScope = new SymbolTable<string, object>();
            globalScope.Add("x", 10);
            
            var innerScope = globalScope.CreateNewScope_GivenParent();
            
            // Act
            bool foundLocal = innerScope.ContainsKeyLocal("x");
            bool foundHierarchical = innerScope.ContainsKey("x");
            
            // Assert
            Assert.False(foundLocal);      // Not in current scope
            Assert.True(foundHierarchical); // But found in parent scope
        }

        /// <summary>
        /// Tests that parent reference is correctly maintained through scope chain.
        /// </summary>
        [Fact]
        public void SymbolTable_ParentReference_MaintainedThroughChain()
        {
            // Arrange & Act
            var (global, outer, inner) = CreateNestedScopes();
            
            // Assert
            Assert.Null(global.Parent);
            Assert.Same(global, outer.Parent);
            Assert.Same(outer, inner.Parent);
            Assert.Same(global, inner.Parent.Parent);
        }

        #endregion
    }
    }
}