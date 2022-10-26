using System;
using System.Linq.Expressions;

namespace Operations
{
    public interface IOperation
    {
    }

    public interface IUnaryOperation : IOperation
    {
        T Evaluate<T>(T expresion);
    }

    public interface IBinaryOperation : IOperation
    {
        TR Evaluate<TR, TL>(TR left, TL right);
    }
    
    public interface IBinaryLogicOperation : IOperation
    {
        bool Evaluate<TR, TL>(TR left, TL right);
    }
    
    public interface IMultyOperation : IOperation
    {
        T Evaluate<T>(params T[] list);
    }

    public class Exponentiate : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            dynamic temp = 1;
            for (dynamic i = 0; i < r; i++)
            {
                temp *= l;
                if (temp > Int32.MaxValue) throw new OverflowException();
            }
            return temp;
        }
    }

    public class Multiply : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l * r;
        }
    }
    
    public class Divide : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            try
            {
                return l / r;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Divide by zero, lol");
                return default;
            }
        }
        
    }
    
    public class Mod : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            try
            {
                return l % r;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Divide by zero, lol");
                return default;
            }
        }
    }

    public class Add : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l + r;
        }
    }

    public class Substruct : IBinaryOperation
    {
        public TR Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l - r;
        }
    }

    public class Increment : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            return e = e + 1;
        }
    }

    public class Decrement : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            return e = e - 1;
        }
    }
    
    public class Not : IUnaryOperation
    {
        public T Evaluate<T>(T expresion)
        {
            dynamic e = expresion;
            if (typeof(T) == typeof(bool))
            {
                return !e;
            }

            return -e;
        }
    }
    
    public class SmallerThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l < r;
        }
    }

    public class GraterThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l < r;
        }
    }
    
    public class SmallerEqualThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR,TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l <= r;
        }
    }

    public class GraterEqualThan : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l <= r;
        }
    }
    
    public class Equal : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l == r;
        }
    }
    
    public class NotEqual : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            dynamic l = left;
            dynamic r = right;
            return l != r;
        }
    }
    
    public class And : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            if (typeof(TR) != typeof(bool) && typeof(TR) != typeof(bool)) return default;
            dynamic l = left;
            dynamic r = right;
            return l && r;
        }
    }
    
    public class Or : IBinaryLogicOperation
    {
        public bool Evaluate<TR, TL>(TR left, TL right)
        {
            if (typeof(TR) != typeof(bool) && typeof(TR) != typeof(bool)) throw new Exception();
            dynamic l = left;
            dynamic r = right;
            return l || r;
        }
    }
}
/*
 * ^
 * !, іnc, dec
 * *, /
 * %;
 * +, - 
 * <, > <=, >=, <> , ==
 * and
 * or
 * max(x,y), mіn(x,y)
 */
