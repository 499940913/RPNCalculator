using System.Collections.Generic;
using System.Text.RegularExpressions;
using RPNCore.Enum;
using System.Linq;

namespace RPNCore
{
    public class Word
    {
        /// <summary>
        /// 数字校验
        /// </summary>
        private const string NumberPattern = @"(\d+(\.\d+)?)";

        private  string _symbol;

        public override string ToString()
        {
            return _symbol;
        }

        private Word(string symbol)
        {
            _symbol = symbol;
       
        }

        public string Symbol => _symbol;

        public int Index { get; private set; }

        public int Length => _symbol.Length;

        public bool IsParamGroup { get; set; }

        private static bool IsContainedWord(Match match, List<Word> blocks)
        {
            var becontained = blocks.FirstOrDefault(p => p.Index < match.Index && match.Index < p.Index + p.Length);
            return becontained != null;
        }

        public static string GetEscapeString(string input)
        {
            foreach (var str in RegexEscapecharacters)
            {
                if (!input.Contains(str)) continue;
                input = Regex.Replace(input, @"\" + str, m => m.Value.Replace(m.Value, $@"\{m.Value}"));
            }
            return input;
        }

        private static readonly string[] RegexEscapecharacters = { "^", "$", ".", "(", ")", "*", "?", "+", "[", "]", "{", "}", "|" };

        public static List<Word> ExpressionToWords(string expression)
        {
            var blocks = new List<Word>();
            var operators = DefaultOperators.Operators.OrderByDescending(p => p.Key.Length);
            foreach (var op in operators)
            {
                var matches = Regex.Matches(expression, GetEscapeString(op.Key));
                if (matches.Count == 0) continue;
                foreach (var item in matches)
                {
                    var match = item as Match;
                    if (match == null) continue;
                    if (IsContainedWord(match, blocks)) continue;
                    var single = blocks.SingleOrDefault(p => p.Index == match.Index);
                    if (single == null)
                    {
                        blocks.Add(new Word(match.Value)
                        {
                            Index = match.Index,
                            Type = op.Value.Type
                        });
                    }
                    else
                    {
                        if (single.Length >= match.Length) continue;
                        single._symbol = match.Value;
                        single.Type = op.Value.Type;
                    }
                }
            }
            var groupoperators = new[] { "(", ")", "," };
            foreach (var group in groupoperators)
            {
                var matches = Regex.Matches(expression, GetEscapeString(group));
                if (matches.Count == 0) continue;
                foreach (var item in matches)
                {
                    var match = item as Match;
                    if (match == null) continue;
                    if (IsContainedWord(match, blocks)) continue;
                    var single = blocks.SingleOrDefault(p => p.Index == match.Index);
                    if (single == null)
                    {
                        blocks.Add(new Word(match.Value)
                        {
                            Index = match.Index,
                            Type = Type.Group
                        });
                    }
                    else
                    {
                        if (single.Length >= match.Length) continue;
                        single._symbol = match.Value;
                        single.Type = Type.Group;
                    }
                }
            }
            var numericmatches = Regex.Matches(expression, NumberPattern);
            foreach (var item in numericmatches)
            {
                var match = item as Match;
                if (match == null) continue;
                if (IsContainedWord(match, blocks)) continue;
                var single = blocks.SingleOrDefault(p => p.Index == match.Index);
                if (single == null)
                {
                    blocks.Add(new Word(match.Value)
                    {
                        Index = match.Index,
                        Type =Type.Operand
                    });
                }
                else
                {
                    if (single.Length >= match.Length) continue;
                    single._symbol = match.Value;
                    single.Type = Type.Operand;
                }
            }
            return blocks.OrderBy(p => p.Index).ToList();
        }

        internal Type Type { get; private set;}
    }

    public static class WordExtension
    {
        public static Word GetPreWord(this IEnumerable<Word> words, Word current)
        {
            if (words == null)
                return null;
            if (current == null)
                return null;
            return current.Index == 0 ? null : words.LastOrDefault(p => p.Index < current.Index);
        }

        public static Word GetNextWord(this IEnumerable<Word> words, Word current)
        {
            if (words == null)
                return null;
            return current == null ? null : words.FirstOrDefault(p => p.Index > current.Index);
        }
    }
}
