/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 ___________________________________________________ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSystems.Interpreter
{
    internal static class Tokenizer
    {
        /// <summary>
        /// Preliminary tokenization of the expression.
        /// Tokenizes numeric values, alpha values, parentheses, commas and other tokens. 
        /// Any whitespace is removed.
        /// </summary>
        public static List<Token> Parse(string expression)
        {
            const char LeftParenthesis = '(';
            const char RightParenthesis = ')';
            const char Comma = ',';
            const char NumericNegative = '-';
            const char DateTimeDelimiter = '#';
	        const char TimeSpanDelimiter = '`';

            var whitespaceCharacters = new[] { ' ', '\t' };
            var numericCharacters = new[] { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var identifierCharacters = new[] { '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            var identifierSecondaryCharacters = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };     // other characters that can be used as identifiers - but cannot be a starting character
            var textDelimiters = new[] { '\"', '\'' };
            bool isNumericNegative = false;
            bool parsingText = false;
            bool parsingDateTime = false;
	        bool parsingTimeSpan = false;

            var tokens = new List<Token>();
            var currentTokenType = TokenType.Other;
            var currentToken = String.Empty;
            char currentTextDelimiter = '\0';
            var characterTokenType = TokenType.Other;
            var expressionEnumerator = new PeekableEnumerator<char>(expression);
            var characterString = String.Empty;

            while (expressionEnumerator.MoveNext())
            {
                var tokenIsSeparateCharacter = false;
                var character = expressionEnumerator.Current;

                // if the character is a '-' and the subsequent character is a numeric character then this is a negative number. 
                // otherwise it is some other character TokenType.Other -- probably a subtraction operator.
                isNumericNegative = character == NumericNegative && expressionEnumerator.CanPeek && numericCharacters.Contains(expressionEnumerator.Peek);

                if (textDelimiters.Contains(character) || parsingText)
                {
                    if (textDelimiters.Contains(character) && !parsingText)         // started parsing
                    {
                        characterTokenType = TokenType.Text;
                        characterString = String.Empty;         // consume character
                        currentTextDelimiter = character;
                        parsingText = true;
                    }
                    else if (character == currentTextDelimiter && parsingText)     // finished parsing
                    {
                        characterString = String.Empty;         // consume character
                        parsingText = false;
                    }
                    else
                        characterString = character.ToString();
                }
                else if (character == DateTimeDelimiter || parsingDateTime)
                {
                    if (!parsingDateTime)                       // started parsing
                    {
                        characterTokenType = TokenType.DateTime;
                        characterString = String.Empty;     // consume character
                        parsingDateTime = true;
                    }
                    else if (character == DateTimeDelimiter)    // finished parsing
                    {
                        characterString = String.Empty;     // consume character
                        parsingDateTime = false;
                    }
                    else 
                        characterString = character.ToString();
                }
				else if (character == TimeSpanDelimiter || parsingTimeSpan)
				{
					if (!parsingTimeSpan)                       // started parsing
					{
						characterTokenType = TokenType.TimeSpan;
						characterString = String.Empty;     // consume character
						parsingTimeSpan = true;
					}
					else if (character == TimeSpanDelimiter)    // finished parsing
					{
						characterString = String.Empty;     // consume character
						parsingTimeSpan = false;
					}
					else
						characterString = character.ToString();
				}
                else if (whitespaceCharacters.Contains(character))
                {
                    characterTokenType = TokenType.Whitespace;
                    characterString = String.Empty;             // consume character
                }
                else if (identifierCharacters.Contains(character) || (currentTokenType == TokenType.Identifier && identifierSecondaryCharacters.Contains(character)))
                {
                    characterTokenType = TokenType.Identifier;
                    characterString = character.ToString();
                }
                else if (numericCharacters.Contains(character) || isNumericNegative)
                {
                    characterTokenType = TokenType.Number;
                    characterString = character.ToString();
                }
                else if (character == LeftParenthesis)
                {
                    characterTokenType = TokenType.LeftParenthesis;
                    characterString = character.ToString();
                    tokenIsSeparateCharacter = true;
                }
                else if (character == RightParenthesis)
                {
                    characterTokenType = TokenType.RightParenthesis;
                    characterString = character.ToString();
                    tokenIsSeparateCharacter = true;
                }
                else if (character == Comma)
                {
                    characterTokenType = TokenType.Comma;
                    characterString = character.ToString();
                    tokenIsSeparateCharacter = true;
                }
                else
                {
                    characterTokenType = TokenType.Other;
                    characterString = character.ToString();
                }

                if (currentTokenType == characterTokenType && !tokenIsSeparateCharacter)
                    currentToken += characterString;
                else
                {
                    if (currentToken.Length > 0 || currentTokenType == TokenType.Text)
                        tokens.Add(new Token(currentToken, currentTokenType));

                    currentToken = characterString;
                    currentTokenType = characterTokenType;
                }
            }

            if (currentToken.Length > 0 || currentTokenType == TokenType.Text)
                tokens.Add(new Token(currentToken, currentTokenType));

            return tokens;
        }
    }
}
