

namespace RPNCore.Interface
{
   public interface IOperator
    {
        int ParamCount { get;}

        int Priority { get; }

        MathFunc MathFun { get; }

    }
}
