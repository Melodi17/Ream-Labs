using Ream.Lexing;

namespace Ream.Parsing
{
   public abstract class Stmt
   {
     public abstract T Accept<T>(Visitor<T> visitor);
     public interface Visitor<T>
     {
         public T VisitExpressionStmt(Expression stmt);
         public T VisitWriteStmt(Write stmt);
         public T VisitVarStmt(Var stmt);
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
