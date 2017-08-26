using System;
using System.Collections.Generic;
using System.Linq;
using RPNCore.Interface;
namespace RPNCore
{
    public class Calculator
    {
        private static readonly ExpressBuilder ExpressBuilder = new ExpressBuilder();
        private double Compute(List<IStackObject> rpnlist)
        {
            if (rpnlist.Count == 0)
                throw new Exception("无算数因子！");
            if (rpnlist[0].Symbol != "#")
                throw new Exception("缺少开始运算符，请确认该表达式式是否已经使用！");
            rpnlist.RemoveAt(0);
            if (rpnlist.Count == 0)
                throw new Exception("无算数因子！");
            var run = rpnlist.FirstOrDefault(p => p is Operator);
            while (true)
            {
                if (run == null)
                    break;
                var index = rpnlist.IndexOf(run);
                var op = run as Operator;
                var toInsertIndex = index - op.ParamCount;
                if (toInsertIndex < 0)
                    throw new Exception("运算错误！");
                var arguments = new List<Operand>();
                for (int i = toInsertIndex; i < index; i++)
                {
                    var operand = rpnlist[i] as Operand;
                    if (operand == null)
                        throw new Exception($"运算符{op.Symbol}计算的对象不应为{rpnlist[i].Symbol}");
                    arguments.Add(operand);
                }
                var value = op.MathFun(arguments.Select(p => p.Value).ToArray());
                (rpnlist[toInsertIndex] as Operand).Value = value;
                int removeCount = index - toInsertIndex;
                while (removeCount > 0)
                {
                    rpnlist.RemoveAt(toInsertIndex + 1);
                    removeCount--;
                }
                run = rpnlist.FirstOrDefault(p => p is Operator);
            }
            if (rpnlist.Count != 1)
                throw new Exception("运算出错！");
            var result = rpnlist[0] as Operand;
            if (result == null)
                throw new Exception("运算出错！");
            return result.Value;
        }
        public double Compute(ExpressBuilder builder, out bool isOk, out string msg)
        {
            msg = string.Empty;
            isOk = true;
            try
            {
                return Compute(builder.RpnList);
            }
            catch (Exception e)
            {
                isOk = false;
                msg = e.Message;
            }
            return 0;
        }
        public double Compute(string expression, out bool isOk, out string msg)
        {
            isOk = ExpressBuilder.Parse(expression, out msg);
            return !isOk ? 0 : Compute(ExpressBuilder, out isOk, out msg);
        }
    }
}
