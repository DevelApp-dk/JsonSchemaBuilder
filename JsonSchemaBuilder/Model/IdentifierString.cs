using DevelApp.JsonSchemaBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevelApp.JsonSchemaBuilder.Model
{
    public sealed class IdentifierString : IEquatable<IdentifierString>
    {
        private string _innerString;

        public IdentifierString()
        {
            _innerString = string.Empty;
        }

        public IdentifierString(string identifierString)
        {
            ValidateString(identifierString);
            _innerString = identifierString;
        }

        /// <summary>
        /// Throw exceptions if format is not proper
        /// </summary>
        /// <param name="identifierString"></param>
        private void ValidateString(string identifierString)
        {
            if (!Regex.IsMatch(identifierString, "^[a-zA-Z][a-zA-Z0-9]*$"))
            {
                throw new IdentifierStringException($"IdentifierString is not in correct format of ascii letter followed by ascii letter or number: {identifierString}");
            }
        }

        /// <summary>
        /// Serialization property that might change as interface so please do not use
        /// </summary>
        public string InnerString
        {
            get
            {
                return _innerString;
            }
            set
            {
                ValidateString(value);
                _innerString = value;
            }
        }

        public IdentifierString Clone()
        {
            return new IdentifierString(_innerString);
        }

        public bool Equals(IdentifierString other)
        {
            return _innerString.Equals(other.ToString());
        }

        public override string ToString()
        {
            return _innerString;
        }

        #region Implicit operators

        public static implicit operator IdentifierString(string rhs)
        {
            return new IdentifierString(rhs);
        }

        public static implicit operator string(IdentifierString rhs)
        {
            return rhs.ToString();
        }

        #endregion
    }

}
