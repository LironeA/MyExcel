﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using Operations;

namespace Parser
{
    static class Parser
    {
        private static readonly Regex DivadersRegex = new Regex(@"(<=|>=|==|<>|[\|\&\\])|([\+\-\*\(\)\^\;\/\%\!\<\>\\])");
        private static readonly Regex ConstantRegex = new Regex(@"^[0-9]+|true|false");
        private static readonly Regex PointerRegex = new Regex(@"^[A-Za-z]+\d+");
        private static readonly Regex OperationsRegex = new Regex(@"([\|\&\\])|([\+\-\*\^\/\!\%\<=\>=\<$\>$\\]|[a-z]+)");
        private static readonly Regex SymbolsRegex = new Regex(@"([\(\)\;\\])");
        private static readonly Dictionary<string , IOperation> OperationsList = new Dictionary<string, IOperation>()
        {
            {"^", new Exponentiate()},
            { "*", new Multiply()},
            { "/", new Divide()},
            { "%", new Mod()},
            { "+", new Add()},
            { "-", new Substruct()},
            { "inc", new Increment()},
            { "dec", new Decrement()},
            { "!", new Not()},
            { "<", new SmallerThan()},
            { ">", new GraterThan()},
            { "<=", new SmallerEqualThan()},
            { ">=", new GraterEqualThan()},
            { "==", new Equal()},
            { "<>", new NotEqual()},
            { "&", new And()},
            { "|", new Or()},
            { "max", null},
            { "min", null},
    };
        public static void Parse(ref Formula f)
        {

            GetTokens(ref f);
            UpdatePrioritets(ref f);
            try
            {
                f.BinaryTree = GetBinaryTree(f.Tokens);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            MessageBox.Show("");
        }

        private static void UpdatePrioritets(ref Formula f)
        {
            var tokens = f.Tokens;
            int coef = 0;
            foreach (var token in tokens)
            {
                if (token.RawData == "(") coef += 10;
                if (token.RawData == ")") coef -= 10;
                token.Priority += coef;
            }
            f.Tokens = tokens.Where(val => val.RawData != "(" && val.RawData != ")").ToArray();
        }

        public static void GetTokens(ref Formula formula)
        {
            var data = formula.RawData;
            data = Regex.Replace(data, @"\s+", "");
            string[] strings = DivadersRegex.Split(data);
            strings = strings.Where(val => val != "").ToArray();
            formula.Tokens = new Token[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                formula.Tokens[i] = DetermineTypeOfToken(strings[i]); ;
            }
        }
        
        private static Token DetermineTypeOfToken(string data)
        {
            if (SymbolsRegex.IsMatch(data))
            {
                var t = new SumbolToken(data);
                t.Priority = 22;
                return t;
            }

            if (ConstantRegex.IsMatch(data))
            { 
                if (data == "true" || data == "false" )
                {
                    var t = new BoolValueToken(data);
                    t.Priority = 21;
                    return t;

                }
                else
                {
                    var t = new IntValueToken(data);
                    t.Priority = 21;
                    return t;
                }
            }

            if (PointerRegex.IsMatch(data))
            {
                var t = new PointerToken(data);
                t.Priority = 20;
                return t;
            }
            if (OperationsRegex.IsMatch(data))
            {
                var t = new OperationToken(data);
                DetermineOperator(ref t);
                return t;
            }
            return null;
        }

        private static void DetermineOperator(ref OperationToken token)
        {
            token.TokenOpertion = OperationsList[token.RawData];
            token.Priority = GetIndexOfKey(OperationsList, token.RawData);
        }
        
        private static int GetIndexOfKey(Dictionary<string,IOperation> tempDict, string key)
        {
            int index = 0;
            foreach (string value in tempDict.Keys)
            {
                if (key == value)
                    return index;
                index++;
            }
            return -1;
        }

        private static int FindMinInArray(Token[] tokens)
        {
            int min = 50;
            int index = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Priority < min)
                {
                    min = tokens[i].Priority;
                    index = i;
                }
            }
            return index;

        }

        private static BinaryTree GetBinaryTree(Token[] tokens)
        {
            if (tokens.Length == 0) return null; 
            if(tokens.Length == 1) return new BinaryTree(tokens[0],null, null);
            var index = FindMinInArray(tokens);
            var left = tokens.Skip(0).Take(index).ToArray();
            var right = tokens.Skip(index+1).ToArray();
            return new BinaryTree(tokens[index],GetBinaryTree(left), GetBinaryTree(right));

        }
    }
}
