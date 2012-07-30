/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
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
            const char TextDelimiter = '\"';

            var whitespaceCharacters = new[] { ' ', '\t' };
            var numericCharacters = new[] { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var identifierCharacters = new[] { '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
			bool isNumericNegative = false;
            bool parsingText = false;

            var tokens = new List<Token>();
            var currentTokenType = TokenType.Other;
            var currentToken = String.Empty;
			var expressionEnumerator = new PeekableEnumerator<char>(expression);

            while (expressionEnumerator.MoveNext())
            {
				var character = expressionEnumerator.Current;

				// if the character is a '-' and the subsequent character is a numeric character then this is a negative number. 
				// otherwise it is some other character TokenType.Other -- probably a subtraction operator.
				isNumericNegative = character == NumericNegative && expressionEnumerator.CanPeek && numericCharacters.Contains(expressionEnumerator.Peek);

                if (character == TextDelimiter || parsingText)
                {
                    if (character == TextDelimiter && !parsingText)         // started parsing
                    {
                        AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                        currentToken = String.Empty;    // do not include quote characters around the string
                        currentTokenType = TokenType.Text;
                        parsingText = true;
                    }
                    else if (character == TextDelimiter && parsingText)     // finished parsing
                        parsingText = false;
                    else
                        currentToken += character;
                }
                else if (whitespaceCharacters.Contains(character))
                {
                    AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                    currentToken = String.Empty;
                    currentTokenType = TokenType.Other;
                }
                else if (identifierCharacters.Contains(character))
                {
                    if (currentTokenType == TokenType.Identifier)
                        currentToken += character;
                    else
                    {
                        AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                        currentToken = character.ToString();
                        currentTokenType = TokenType.Identifier;
                    }
                }
                else if (numericCharacters.Contains(character) || isNumericNegative)
                {
                    if (currentTokenType == TokenType.Number)
                        currentToken += character;
                    else
                    {
                        AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                        currentToken = character.ToString();
                        currentTokenType = TokenType.Number;
                    }
                }
                else if (character == LeftParenthesis)
				{
                    AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                    AddToken(tokens, TokenType.LeftParenthesis, character.ToString());
                	currentToken = String.Empty;
				}
                else if (character == RightParenthesis)
				{
                    AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                    AddToken(tokens, TokenType.RightParenthesis, character.ToString());
                	currentToken = String.Empty;
				}
                else if (character == Comma)
				{
                    AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                    AddToken(tokens, TokenType.Comma, character.ToString());
                	currentToken = String.Empty;
                }
				else
				{
                    if (currentTokenType == TokenType.Other)
                        currentToken += character;
                    else
                    {
                        AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);
                        currentToken = character.ToString();
                        currentTokenType = TokenType.Other;
                    }
				}
            }

            AddTokenIfItsNotEmpty(tokens, currentTokenType, currentToken);

            return tokens;
        }

        private static void AddTokenIfItsNotEmpty(List<Token> tokens, TokenType tokenType, string token)
        {
            // add the previous token to the list
            if (token.Length > 0)
                AddToken(tokens, tokenType, token);
        }

        private static void AddToken(List<Token> tokens, TokenType tokenType, string token)
        {
            tokens.Add(new Token(token, tokenType));
        }
    }
}
