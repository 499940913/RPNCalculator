using RPNCore.Enum;
using RPNCore.Interface;

namespace RPNCore
{
    public class GroupOperator:IStackObject
    {
       
      

        /// <summary>
        /// '(' ')' ','
        /// </summary>
        public string Symbol { get; set; }

        public Type Type
        {
            get
            {
                return Type.Group;
            }
        }
    }
}
