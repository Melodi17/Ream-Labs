using Ream.Lexing;

namespace Ream.Parsing
{
   public abstract class Expr
   {
     public abstract T Accept<T>(Visitor<T> visitor);
     public interface Visitor<T>
     {
         public T VisitAssignExpr(Assign expr);
         public T VisitBinaryExpr(Binary expr);
         public T VisitGroupingExpr(Grouping expr);
         public T VisitLiteralExpr(Literal expr);
         public T VisitUnaryExpr(Unary expr);
         public T VisitVariableExpr(Variable expr);
     }
     public class Assign : Expr
      {
     public readonly Token name;
     public readonly Expr value;

         public Assign(Token name, Expr value)
          {
             this.name = name;
             this.value = value;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitAssignExpr(this);
          }
      }

     public class Binary : Expr
      {
     public readonly Expr left;
     public readonly Token @operator;
     public readonly Expr right;

         public Binary(Expr left, Token @operator, Expr right)
          {
             this.left = left;
             this.@operator = @operator;
             this.right = right;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitBinaryExpr(this);
          }
      }

     public class Grouping : Expr
      {
     public readonly Expr expression;

         public Grouping(Expr expression)
          {
             this.expression = expression;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitGroupingExpr(this);
          }
      }

     public class Literal : Expr
      {
     public readonly Object value;

         public Literal(Object value)
          {
             this.value = value;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitLiteralExpr(this);
          }
      }

     public class Unary : Expr
      {
     public readonly Token @operator;
     public readonly Expr right;

         public Unary(Token @operator, Expr right)
          {
             this.@operator = @operator;
             this.right = right;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitUnaryExpr(this);
          }
      }

     public class Variable : Expr
      {
     public readonly Token name;

         public Variable(Token name)
          {
             this.name = name;
          }

          public override T Accept<T>(Visitor<T> visitor)
          {
             return visitor.VisitVariableExpr(this);
          }
      }

  }
}
