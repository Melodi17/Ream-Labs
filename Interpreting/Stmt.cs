using Ream.Lexing;

namespace Ream.Parsing
{
   public abstract class Stmt
   {
     public abstract T Accept<T>(Visitor<T> visitor);
     public interface Visitor<T>
     {
         public T VisitBlockStmt(Block stmt);
         public T VisitExpressionStmt(Expression stmt);
         public T VisitIfStmt(If stmt);
         public T VisitWriteStmt(Write stmt);
         public T VisitVarStmt(Var stmt);
     }
     public class Block : Stmt
      {
     public readonly List<Stmt> statements;

         public Block(List<Stmt> statements)
          {
             this.statements = statements;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitBlockStmt(this);
          }
      }

     public class Expression : Stmt
      {
     public readonly Expr expression;

         public Expression(Expr expression)
          {
             this.expression = expression;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitExpressionStmt(this);
          }
      }

     public class If : Stmt
      {
     public readonly Expr condition;
     public readonly Stmt thenBranch;
     public readonly Stmt elseBranch;

         public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
          {
             this.condition = condition;
             this.thenBranch = thenBranch;
             this.elseBranch = elseBranch;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitIfStmt(this);
          }
      }

     public class Write : Stmt
      {
     public readonly Expr expression;

         public Write(Expr expression)
          {
             this.expression = expression;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitWriteStmt(this);
          }
      }

     public class Var : Stmt
      {
     public readonly Token name;
     public readonly Expr initializer;

         public Var(Token name, Expr initializer)
          {
             this.name = name;
             this.initializer = initializer;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitVarStmt(this);
          }
      }

  }
}
