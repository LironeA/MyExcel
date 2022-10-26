using System;
using System.Collections.Generic;
using Operations;
using MyExcel;
using MyExel.Spreadsheet;

namespace Parser
{
    public abstract class Token
    {
        public string RawData { get; set; }
        
        private int valueLenth => RawData.Length;
        
        public int Priority;

        public Token(string value)
        {
            RawData = value;
        }

        public abstract dynamic ToValue();
    }

    public class IntValueToken : Token
    {
        public IntValueToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Int64.Parse(RawData);
        }
    }

    public class BoolValueToken : Token
    {
        public BoolValueToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Boolean.Parse(RawData); ;
        }
    }

    public class SumbolToken : Token
    {
        public SumbolToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return Char.Parse(RawData);
        }
    }

    public class PointerToken : Token
    {
        public PointerToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            int columnIndex = 0;
            string row = "";
            foreach(var c in RawData)
            {
                if((int)c >= 65 && (int)c <= 90)
                {
                    columnIndex += ((int)c - 65) * 26;
                }
                else
                {
                    row += c;
                }
            }
            var cell = SpreadSheet.cells[Int32.Parse(row) - 1, columnIndex];
            if (cell.formula == null) return cell.value;
            else return cell.formula.GetResult();
        }
    }

    public class OperationToken : Token
    {

        public dynamic TokenOpertion;

        public OperationToken(string value) : base(value)
        {
        }

        public override dynamic ToValue()
        {
            return TokenOpertion;
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
}