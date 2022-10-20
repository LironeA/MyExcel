using System;
using Operations;

namespace Parser
{
    public class BinaryTree
    {
        public Token value;
        public BinaryTree[] Next;

        public BinaryTree(Token value, BinaryTree first, BinaryTree second = null)
        {
            Console.WriteLine(value.Value + "><" + (first!=null ? first.value.Value : null)  + "><" + (second!=null ? second.value.Value : null));
            this.value = value;
            if (first == null && second == null)
            {
                Next=null;
                return;
            }
            Next = new BinaryTree[2] {first, second};
        }
        
        public dynamic GetResult()
        {
            if (Next == null && value.Type == TokenType.Constant) return value.ToValue();
            if (value.Type == TokenType.Pointer) return 1;
            if (value.TokenOpertion.GetType().GetInterface("IBinaryOperation") != null)
            {
                IBinaryOperation t = value.TokenOpertion;
                return t.Evaluate(Next[0].GetResult(), Next[1].GetResult());
            }
            else if (value.TokenOpertion.GetType().GetInterface("IUnaryOperation") != null)
            {
                var t = value.TokenOpertion as IUnaryOperation;
                return t.Evaluate(Next[1].GetResult());
            }
            else
            {
                var t = value.TokenOpertion as IBinaryLogicOperation;
                return t.Evaluate(Next[0].GetResult(), Next[1].GetResult());
            }
        }
    }
}