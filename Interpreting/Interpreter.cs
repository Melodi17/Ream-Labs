﻿using Ream;
using Ream.Lexing;
using Ream.Parsing;
using Ream.Tools;

namespace Ream.Interpreting
{
    public class Interpreter : Expr.Visitor<Object>, Stmt.Visitor<Object>
    {
        private readonly Scope Scope = new(null);
        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Program.RuntimeError(error);
            }
        }
        public void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = Evaluate(expr.left);
            object right = Evaluate(expr.right);

            switch (expr.@operator.Type)
            {
                case TokenType.Plus:
                    if (left is double && right is double)
                        return (double)left + (double)right;
                    else if (left is string && right is string)
                        return (string)left + (string)right;
                    else
                        throw new RuntimeError(expr.@operator, "Operands must be two intergers or two strings");
                    break;

                case TokenType.Minus:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left - (double)right;

                case TokenType.Star:
                    if (left is double && right is double)
                        return (double)left * (double)right;
                    else if (left is string && right is double)
                        return ((string)left).Multiply(((double)right).ToInt());
                    else if (left is double && right is string)
                        return ((string)right).Multiply(((double)left).ToInt());
                    else
                        throw new RuntimeError(expr.@operator, "Operands must be two intergers or a string and an interger");
                    break;

                case TokenType.Slash:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left / (double)right;

                case TokenType.Greater:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left > (double)right;
                case TokenType.Greater_Equal:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.Less:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left < (double)right;
                case TokenType.Less_Equal:
                    CheckIntergerOperands(expr.@operator, left, right);
                    return (double)left <= (double)right;

                case TokenType.Equal_Equal:
                    return IsEqual(left, right);
                case TokenType.Not_Equal:
                    return !IsEqual(left, right);
            }

            return null;
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = Evaluate(expr.right);

            switch (expr.@operator.Type)
            {
                case TokenType.Not:
                    return !IsTruthy(right);
                case TokenType.Minus:
                    CheckIntergerOperand(expr.@operator, right);
                    return -(double)right;
            }

            return null;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is double) return (double)obj > 0;
            return true;
        }

        private bool IsEqual(object left, object right)
        {
            if (left == null && right == null) return true;
            if (left == null) return false;

            return left.Equals(right);
        }

        private void CheckIntergerOperand(Token token, object obj)
        {
            if (obj is double) return;

            throw new RuntimeError(token, "Operand must be an interger");
        }

        private void CheckIntergerOperands(Token token, object left, object right)
        {
            if (left is double && right is double) return;

            throw new RuntimeError(token, "Operands must be an intergers");
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "null";
            if (obj is double)
            {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text[..^2];
                }
                return text;
            }
            if (obj is bool) return obj.ToString().ToLower();

            return obj.ToString();
        }

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object VisitWriteStmt(Stmt.Write stmt)
        {
            object value = Evaluate(stmt.expression);
            Console.WriteLine(value);
            return null;
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            return Scope.Get(expr.name);
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            Scope.Define(stmt.name.Raw, value);
            return null;
        }

        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = Evaluate(expr.value);
            Scope.Set(expr.name, value);
            return value;
        }
    }
}
