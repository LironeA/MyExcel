using System;

namespace Parser
{
    public class Formula
    {
        public string RawData { get; set; }
        public Token[] Tokens { get; set; }
        public BinaryTree BinaryTree { get; set; }
        public string Result { get; set; }
        public Formula(string rawData)
        {
            this.RawData = rawData ?? throw new ArgumentNullException(nameof(rawData));
        }
        
    }
}