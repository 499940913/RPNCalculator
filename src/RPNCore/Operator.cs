
using RPNCore.Enum;
using RPNCore.Interface;

namespace RPNCore
{
    public class Operator : IStackObject, IOperator
    {
        public int ParamCount { get; internal set; }

        public int Priority { get; internal set; }

        public MathFunc MathFun { get; internal set; }

        public string Symbol { get;internal set; }

        private Type _type=Type.Fun;
        public Type Type {
            get { return _type; }
            internal set { _type = value; } 
        }
    }
}
