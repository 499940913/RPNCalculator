

using RPNCore.Enum;
using RPNCore.Interface;

namespace RPNCore
{
   public class Operand:IStackObject
    {
        public string Symbol { get; set; }

        public Type Type
        {
            get
            {
                return Type.Operand;
            }
        }

        public double Value { get;internal set;}
    }
}
