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
    internal enum TokenType
    {
        /// <summary>
        /// Number
        /// </summary>
        Number,

        /// <summary>
        /// Function variable name or reserved word
        /// </summary>
        Identifier,

        /// <summary>
		/// '(' character.
        /// </summary>
        LeftParenthesis,

		/// <summary>
		/// ')' character.
		/// </summary>
		RightParenthesis,

		/// <summary>
		/// ',' character.
		/// </summary>
		Comma,

        /// <summary>
        /// String literal, surrounded/delimited by a " character
        /// </summary>
        Text,

		/// <summary>
		/// Any other token that is not one of the other token types specified in this enum.
		/// Usually a special character such as '*' or '^'.
		/// </summary>U
		Other
    }

    /// <summary>
    /// Represents a character or string that is a unique token type.
    /// </summary>
    internal class Token
    {
        private string value;
        private TokenType type;

        public Token(string token, TokenType type)
        {
            if (token == null)
                throw new ArgumentNullException();

            this.value = token;
            this.type = type;
        }

        public Token()
        {
            this.value = String.Empty;
            this.type = TokenType.Other;
        }

        public Token(char token, TokenType type)
        {
            this.value = token.ToString();
            this.type = type;
        }

        internal string Value
        {
            get
            {
                return this.value;
            }
        }

        internal TokenType Type
		{
			get
			{
				return this.type;
			}
        }

        public override string ToString()
        {
            return this.value;
        }

        public override bool Equals(Object value)
        {
            if (value is string)
                return AreEqual(this, (string)value);
            else
                return false;
        }

        public static bool operator ==(Token token1, string token2)
        {
            return AreEqual(token1, token2);
        }

        public static bool operator !=(Token token1, string token2)
        {
            return !AreEqual(token1, token2);
        }

        private static bool AreEqual(Token token1, string token2)
        {
            if (Object.Equals(token1, null) && Object.Equals(token2, null))
                return true;
            else if (Object.Equals(token1, null) || Object.Equals(token2, null))
                return false;
            else
                return token1.value.Equals(token2, StringComparison.InvariantCultureIgnoreCase);
        }

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
    }
}
