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
        List<Sentence> sentences = new List<Sentence>();
        List<SentenceDefinition> sentenceDefinitions = new List<SentenceDefinition>();
        List<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();
        List<string> sentencesWithErrors = new List<string>();

        public List<SyntaxError> SyntaxErrors = new List<SyntaxError>();
        public List<SemanticError> SemanticErrors = new List<SemanticError>();
        public List<MyCommand> Commands = new List<MyCommand>();

        // добавляем регекспы для слов 
        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }

        // добавляем регекспы для предложений
        public void AddSentenceDefinition(SentenceDefinition definition)
        {
            sentenceDefinitions.Add(definition);
        }


        // парсинг кода на строки и генерация команд или ошибок
        public void ParseToSentences(string source) 
        {
            int currentLine = 1;
            var lines = source.Replace("\r", "").Replace("\n", "").Split(';').Where(l => !String.IsNullOrEmpty(l)).ToList();
            foreach (var line in lines) 
            {
                foreach (var rule in sentenceDefinitions)
                {
                    var match = rule.Regex.Match(line);

                    if (match.Success && match.Index == 0)
                    {
                        generateCommand(line, currentLine);
                        break;
                    }
                    else
                    {
                        findError(line, currentLine);
                    }
                }
                currentLine++;
            }
        }


        // находим ошибки в словах
        private void findError(string line, int lineNumber)
        {
            var tokens = line.Split(' ').Where(t => !String.IsNullOrEmpty(t)).ToList();
            if (tokens.Count < 3)
            {
                SemanticErrors.Add(new SemanticError
                {
                    Message = String.Format("В предложении {0} в строке {1} не хватает слов", line, lineNumber),
                    Reason = line,
                    LineNumber = lineNumber
                });
            }
            else if (tokens.Count > 3)
            {
                SemanticErrors.Add(new SemanticError
                {
                    Message = String.Format("В предложении {0} в строке {1} больше слов", line, lineNumber),
                    Reason = line,
                    LineNumber = lineNumber
                });
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    var match = tokenDefinitions.ElementAt(i).Regex.Match(tokens.ElementAt(i).ToLower());
                    if (!match.Success || match.Length != tokens.ElementAt(i).Length)
                    {
                        SyntaxErrors.Add(new SyntaxError
                        {
                            Message = String.Format("Ошибка в токене {0}", tokens.ElementAt(i)),
                            Reason = tokens.ElementAt(i),
                            LineNumber = lineNumber,
                            ColumnNumber = i + 1
                        });
                    }
                }
            }
        }


        // создаем команды и ищем ошибки, если где-то не доглядели
        private void generateCommand(string line, int lineNumber) 
        {
            var tokens = line.Split(' ').Where(t => !String.IsNullOrEmpty(t)).ToList();
            if (tokens.Count < 3)
            {
                SemanticErrors.Add(new SemanticError
                {
                    Message = String.Format("В предложении {0} в строке {1} не хватает слов", line, lineNumber),
                    Reason = line,
                    LineNumber = lineNumber
                });
            }
            else if (tokens.Count > 3)
            {
                SemanticErrors.Add(new SemanticError
                {
                    Message = String.Format("В предложении {0} в строке {1} больше слов", line, lineNumber),
                    Reason = line,
                    LineNumber = lineNumber
                });
            }
            else 
            {
                var command = new MyCommand();
                bool noError = true;
                for (int i = 0; i < 3; i++)
                {
                    var match = tokenDefinitions.ElementAt(i).Regex.Match(tokens.ElementAt(i).ToLower());
                    // если регексп не распознан или есть еще лишние символы - ошибка 
                    if (!match.Success || match.Length != tokens.ElementAt(i).Length)
                    {
                        noError = false;
                        SyntaxErrors.Add(new SyntaxError
                        {
                            Message = String.Format("Ошибка в токене {0} - строка {1} терм {2}", tokens.ElementAt(i), lineNumber, (i+1)),
                            Reason = tokens.ElementAt(i),
                            LineNumber = lineNumber,
                            ColumnNumber = i + 1
                        });
                    }
                    else 
                    {
                        switch (i)
                        {
                            case 0:
                                command.ActionType = tokens.ElementAt(i).ToLower() == "move" ? ActionType.Move : ActionType.Rotate;
                                break;
                            case 1:
                                command.RobotName = tokens.ElementAt(i).ToLower() == "r1" ? "R1" : "R2";
                                break;
                            case 2:
                                command.Value = Convert.ToInt32(tokens.ElementAt(i));
                                break;
                        }
                    }
                }
                if (noError) 
                {
                    Commands.Add(command);
                }
            }
        }


        // не юзаем
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
