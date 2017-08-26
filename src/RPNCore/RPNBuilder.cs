

using System;
using System.Collections.Generic;
using System.Linq;
using RPNCore.Interface;
using Type = RPNCore.Enum.Type;
// ReSharper disable InconsistentNaming

namespace RPNCore
{
    public class ExpressBuilder
    {
        private static string FunMatchPattern = string.Empty;
        private readonly Stack<IStackObject> _operatorStack;
        private readonly List<IStackObject> _rpnList;
        internal List<IStackObject> RpnList => _rpnList;

        public ExpressBuilder()
        {
            if (FunMatchPattern == string.Empty)
            {
                var operatornames = DefaultOperators.Operators.Where(p => p.Value.Type==Type.Fun).Select(p => Word.GetEscapeString(p.Key));
                FunMatchPattern = string.Join("|", operatornames);
            }
            _operatorStack = new Stack<IStackObject>();
            _rpnList = new List<IStackObject>();
        }
        private static void GetFunctionParamGroup(List<Word> expression)
        {
            var funs = expression.Where(p => p.Type == Type.Fun);
            foreach (var fun in funs)
            {
                GetFunctionEndIndex(expression, fun);
            }
        }
        private static void GetFunctionEndIndex(List<Word> expression, Word fun)
        {
            var gplist = new List<string>();
            var level = -1;
            var paramdictionary = new List<Word>();
            var tofind = expression.Where(p => p.Index > fun.Index).ToList();
            var matches = tofind.Where(p => p.Type ==Type.Group).ToList();
            if (!matches.Any() || matches[0].ToString() != "(" || matches[0].Index > fun.Index + fun.Length)
                throw new Exception($"函数{fun}未找到开始'(',位置：{fun.Index + fun.Length}");
            var index = 0;
            matches[0].IsParamGroup = true;
            foreach (var op in matches)
            {
                index = op.Index;
                if (gplist.Count == 0)
                {
                    gplist.Add(op.ToString());
                    level++;
                    continue;
                }
                if (op.ToString() == ",")
                {
                    if (gplist[0] == "(" && level == 0 || op.ToString() == "," && gplist[0] == ")" && level == -1)
                        paramdictionary.Add(op);
                    continue;
                }
                var symbol = gplist[0];
                if (symbol == op.ToString())
                {
                    if (symbol == "(")
                        level--;
                    if (symbol == ")")
                        level++;
                }
                gplist.RemoveAt(0);
                gplist.Add(op.ToString());
                if (level != 0) continue;
                op.IsParamGroup = true;
                break;
            }
            if (level != 0)
                throw new Exception($"函数{fun}未找到结束')',位置：{index}");
            var paramseparators = paramdictionary.Where(p => p.Index < index).Select(p => p.Index).ToList();
            paramseparators.Add(tofind[0].Index);
            paramseparators.Add(index);
            paramseparators = paramseparators.OrderBy(p => p).ToList();
            var group = tofind.Where(p => p.Index <= index).ToList();
            var vaildcount = paramseparators.Count - 1;
            if (vaildcount != DefaultOperators.Operators[fun.ToString()].ParamCount)
                throw new Exception(
                    $"函数{fun}要求的参数与输入不一致，要求{DefaultOperators.Operators[fun.ToString()].ParamCount}个，实际{vaildcount}个");
            CheckParams(group, paramseparators, fun);
        }
        private static void CheckParams(List<Word> group, List<int> separatorIndex, Word fun)
        {
            for (var i = 0; i < separatorIndex.Count - 1; i++)
            {
                var from = separatorIndex[i];
                var to = separatorIndex[i + 1];
                var param = group.Where(p => p.Index < to && p.Index > from && p.Type != Type.Group);
                if (!param.Any())
                    throw new Exception($"函数{fun}参数错误，无参数值或无法推导出有效操作数！位置：{from}~{to}");
            }
        }
        public string RPNExpression { get; private set; }
        public bool Parse(string expression, out string msg)
        {
            msg = string.Empty;
            _operatorStack.Clear();
            _rpnList.Clear();
            RPNExpression = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(expression))
                    throw new Exception("表达式不能为空！");
                expression = expression.Trim().ToLower().Replace(" ", "");
                if (string.IsNullOrEmpty(expression))
                    throw new Exception("表达式不能为空！");
                var expwords = Word.ExpressionToWords(expression);
                GetFunctionParamGroup(expwords);
                if (expwords.Count == 0)
                    throw new FormatException("未解析到支持的字符！");
                var lastword = expwords.Last();
                if (lastword.Index + lastword.Length < expression.Length)
                    throw new FormatException(
                        $"'{lastword}'附近存在不被支持的字符'{expression.Substring(lastword.Index + lastword.Length)}',位置：{lastword.Index + lastword.Length}~{expression.Length - 1}");
                var nextIndex = 0;
                foreach (var word in expwords)
                {
                    var preword = expwords.GetPreWord(word);
                    if (word.Index != nextIndex)
                        throw new FormatException(
                            $"'{word}'附近存在不被支持的字符'{expression.Substring(preword == null ? 0 : preword.Index + preword.Length, word.Index - (preword == null ? 0 : preword.Index + 1))}',位置：{(preword == null ? 0 : preword.Index + preword.Length)}~{word.Index - 1}");
                    nextIndex += word.Length;
                    switch (word.Type)
                    {
                        case Type.Fun:
                        case Type.Operator:
                            {
                                var op = DefaultOperators.Operators[word.ToString()];
                                if (op.Symbol == "-")
                                {
                                    if (preword == null || preword.Type !=Type.Operand && preword.ToString() != ")")
                                    {
                                        op = DefaultOperators.Operators["negate"];
                                    }
                                }
                                var top = _operatorStack.Count > 0 ? _operatorStack.Peek() as Operator : null;
                                if (top == null || op.Priority >top.Priority)
                                {
                                    _operatorStack.Push(op);
                                }
                                else
                                {
                                    top = _operatorStack.Pop() as Operator;
                                    _rpnList.Add(top);
                                    _operatorStack.Push(op);
                                }
                            }
                            break;
                        case Type.Group:
                            switch (word.ToString())
                            {
                                case "(":
                                    {
                                        var count = word.IsParamGroup ? 2 : 1;
                                        while (count > 0)
                                        {
                                            _operatorStack.Push(new GroupOperator { Symbol = "(" });
                                            count--;
                                        }
                                    }
                                    break;
                                case ")":
                                    {
                                        var count = word.IsParamGroup ? 2 : 1;
                                        while (count > 0)
                                        {
                                            if (_operatorStack.Count == 0)
                                                throw new Exception("表达式异常关闭！未找到'(");
                                            var isEnd = false;
                                            var top = _operatorStack.Pop();
                                            while (true)
                                            {
                                                if (top.Type == Type.Group)
                                                {
                                                    isEnd = true;
                                                    break;
                                                }
                                                _rpnList.Add(top);
                                                if (_operatorStack.Count == 0)
                                                    break;
                                                top = _operatorStack.Pop();
                                            }
                                            if (!isEnd)
                                                throw new Exception("表达式异常关闭！未找到'(");
                                            count--;
                                        }
                                    }
                                    break;
                                case ",":
                                    {
                                        if (_operatorStack.Count == 0)
                                            throw new Exception("表达式异常关闭！未找到'(");
                                        var isEnd = false;
                                        var top = _operatorStack.Pop();
                                        while (true)
                                        {
                                            if (top.Type == Type.Group)
                                            {
                                                isEnd = true;
                                                break;
                                            }
                                            _rpnList.Add(top);
                                            if (_operatorStack.Count == 0)
                                                break;
                                            top = _operatorStack.Pop();
                                        }
                                        if (!isEnd)
                                            throw new Exception("表达式异常关闭！未找到'(");
                                    }
                                    _operatorStack.Push(new GroupOperator { Symbol = "(" });
                                    break;
                            }
                            break;
                        case Type.Operand:
                            _rpnList.Add(new Operand
                            {
                                Symbol = word.ToString(),
                                Value = double.Parse(word.ToString())
                            });
                            break;
                    }
                }
                while (_operatorStack.Count > 0)
                {
                    var top = _operatorStack.Pop();
                    if (top.Type == Type.Group)
                        throw new Exception($"意外的操作符'{top.Symbol}'");
                    _rpnList.Add(top);
                }
                RPNExpression = string.Join(" ", _rpnList.Select(p => p.Symbol));
                _rpnList.Insert(0, new Operand { Symbol = "#" });
                return true;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }
    }
}
