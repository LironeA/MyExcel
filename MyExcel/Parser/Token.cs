using System;
using System.Collections.Generic;
using Operations;

namespace Parser
{
    public class Token
    {
        public string Value { get; set; }
        public ValueType ValueType { get; set; }
        public TokenType Type { get; set; }
        private int valueLenth => Value.Length;
        public dynamic TokenOpertion;
        public int Priority;
        

        public Token(string value)
        {
            this.Value = value;
        }
        
        public dynamic ToValue()
        {
            dynamic numVal = this.ValueType == ValueType.Int ? Int32.Parse(Value) : Boolean.Parse(Value);
            return numVal;
        }

    }

    public enum TokenType
    {
        Symbol, // 22
        Constant,// 21
        Pointer, // 20
        Operator,// 0 -  19
        Invalid
    }

    public enum ValueType
    {
        Int,
        Bool
    }
}