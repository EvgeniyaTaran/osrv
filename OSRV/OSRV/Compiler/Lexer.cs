using OSRV.Compiler.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSRV.Compiler
{
    public class Lexer
    {
        Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
        IList<Sentence> sentences = new List<Sentence>();
        IList<SentenceDefinition> sentenceDefinitions = new List<SentenceDefinition>();
        IList<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();
        IList<SyntaxError> syntaxErrors = new List<SyntaxError>();
        IList<SemanticError> semanticErrors = new List<SemanticError>();
        IList<string> sentencesWithErrors = new List<string>();

        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }
        public void AddSentenceDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }
        /*
        public IEnumerable<Sentence> ParseToSentences(string source)
        {
            
            int currentIndex = 0;
            int currentLine = 1;
            int currentColumn = 0;

            while (currentIndex < source.Length)
            {
                SentenceDefinition matchedDefinition = null;
                int matchLength = 0;

                foreach (var rule in sentenceDefinitions)
                {
                    var match = rule.Regex.Match(source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {

                    //throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
                }
                else
                {
                    var value = source.Substring(currentIndex, matchLength);

                    if (!matchedDefinition.IsIgnored)
                        yield return new Sentence(value, currentLine);

                    var endOfLineMatch = endOfLineRegex.Match(value);
                    if (endOfLineMatch.Success)
                    {
                        currentLine += 1;
                        currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
                    }
                    else
                    {
                        currentColumn += matchLength;
                    }

                    currentIndex += matchLength;
                }
            }

            yield return new Sentence(null, currentLine);
            
        }
         * */

        public IEnumerable<Token> Tokenize(string source)
        {
            int currentIndex = 0;
            int currentLine = 1;
            int currentColumn = 0;

            while (currentIndex < source.Length)
            {
                TokenDefinition matchedDefinition = null;
                int matchLength = 0;

                foreach (var rule in tokenDefinitions)
                {
                    var match = rule.Regex.Match(source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
                }
                else
                {
                    var value = source.Substring(currentIndex, matchLength);

                    if (!matchedDefinition.IsIgnored)
                        yield return new Token(matchedDefinition.Type, value, new TokenPosition(currentIndex, currentLine, currentColumn));

                    var endOfLineMatch = endOfLineRegex.Match(value);
                    if (endOfLineMatch.Success)
                    {
                        currentLine += 1;
                        currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
                    }
                    else
                    {
                        currentColumn += matchLength;
                    }

                    currentIndex += matchLength;
                }
            }

            yield return new Token("(end)", null, new TokenPosition(currentIndex, currentLine, currentColumn));
        }

    }
}
