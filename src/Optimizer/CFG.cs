using AST;

public class CFG : DiGraph<Statement>
{
    public Statement? Start { get; set; }

    // I think all your base are belong to us
    public CFG() : base() { }

}