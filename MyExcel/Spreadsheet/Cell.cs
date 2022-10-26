using Parser;

namespace MyExel.Spreadsheet
{
    public class Cell
    {
        public Point index;
        public string value;
        public Formula formula;

        public delegate void Edited();


    }
}