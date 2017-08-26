using System;
using System.Collections.Generic;
using Type = RPNCore.Enum.Type;

namespace RPNCore
{
    internal static class DefaultOperators
    {
        internal static readonly Dictionary<string, Operator> Operators;

        static DefaultOperators()
        {
            #region 默认运算符
            Operators = new Dictionary<string, Operator>
           {
               {
                   "+", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Plus(p[0], p[1]),
                       Type = Type.Operator
                   }
               },
               {
                   "-", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Minus(p[0], p[1]),
                          Type = Type.Operator
                   }
               },
               {
                   "negate", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math2.Negate(p[0]),
                          Type = Type.Operator
                   }
               },
               {
                   "*", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Mul(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "/", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Div(p[0], p[1]),
                          Type = Type.Operator
                   }
               },
               {
                   "%", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Mod(p[0], p[1]),
                          Type = Type.Operator
                   }
               },
               {
                   ">=", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.GreaterThanEqual(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   ">", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.GreaterThan(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "<", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.LessThan(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "<=", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.LessThanEqual(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "!=", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.NotEqual(p[0], p[1]),
                     Type = Type.Operator
                   }
               }
               ,
               {
                   "<>", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.NotEqual(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "==", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Equal(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "&&", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.And(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "||", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math2.Or(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "^", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math.Pow(p[0], p[1]),
                          Type = Type.Operator
                   }
               }
               ,
               {
                   "pow", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math.Pow(p[0], p[1])
                   }
               }
               ,
               {
                   "abs", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Abs(p[0]),

                   }
               }
               ,
               {
                   "ceil", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Ceiling(p[0]),

                   }
               }
               ,
               {
                   "floor", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Floor(p[0]),

                   }
               }
               ,
               {
                   "trunc", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Truncate(p[0]),

                   }
               }
               ,
               {
                   "sqrt", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Sqrt(p[0]),

                   }
               }
               ,
               {
                   "sin", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Sin(p[0]),

                   }
               }
               ,
               {
                   "cos", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Cos(p[0]),

                   }
               }
               ,
               {
                   "tan", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Tan(p[0]),

                   }
               }
               ,
               {
                   "asin", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Asin(p[0]),

                   }
               }
               ,
               {
                   "acos", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Acos(p[0]),

                   }
               }
               ,
               {
                   "atan", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Atan(p[0]),

                   }
               }
               ,
               {
                   "asinh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math2.Asinh(p[0]),

                   }
               }
               ,
               {
                   "acosh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math2.Acosh(p[0]),

                   }
               }
               ,
               {
                   "atanh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math2.Atanh(p[0]),

                   }
               }
               ,
               {
                   "sinh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Sinh(p[0]),

                   }
               }
               ,
               {
                   "cosh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Cosh(p[0]),

                   }
               }
               ,
               {
                   "tanh", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Tanh(p[0]),

                   }
               }
               ,
               {
                   "exp", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Exp(p[0]),

                   }
               }
               ,
               {
                   "log", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Log(p[0]),

                   }
               }
               ,
               {
                   "log10", new Operator
                   {
                       ParamCount = 1,
                       MathFun = p => Math.Log10(p[0]),

                   }
               }
               ,
               {
                   "max", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math.Max(p[0], p[1]),

                   }
               }
               ,
               {
                   "min", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math.Min(p[0], p[1]),

                   }
               }
               ,
               {
                   "round", new Operator
                   {
                       ParamCount = 2,
                       MathFun = p => Math.Round(p[0], (int) p[1]),

                   }
               }
           };
            #endregion

            foreach (var kv in Operators)
            {
                kv.Value.Symbol = kv.Key;
                kv.Value.Priority = GetPriority(kv.Key);
            }
        }

        /// <summary>
        /// 获取运算符优先级
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal static int GetPriority(string symbol)
        {
            switch (symbol)
            {
                case "&&":
                case "||":
                    return 0;
                case "==":
                case "!=":
                case "<>":
                    return 1;
                case ">":
                case ">=":
                case "<":
                case "<=":
                    return 2;
                case "+":
                case "-":
                    return 3;
                case "*":
                case "/":
                case "%":
                    return 4;
                case "^":
                    return 5;
                case "abs":
                case "pow":
                case "ceil":
                case "floor":
                case "trunc":
                case "sqrt":
                case "sin":
                case "cos":
                case "tan":
                case "asin":
                case "acos":
                case "atan":
                case "sinh":
                case "cosh":
                case "tanh":
                case "asinh":
                case "acosh":
                case "atanh":
                case "exp":
                case "log":
                case "max":
                case "min":
                case "round":
                    return 6;
                case "negate":
                    return 7;
            }
            return 0;
        }
    }
}

