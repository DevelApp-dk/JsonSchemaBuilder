using DevelApp.JsonSchemaBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DevelApp.JsonSchemaBuilder.Model
{
    /// <summary>
    /// Represents a namespace where each dot represents a sub namespace
    /// </summary>
    public sealed class NamespaceString : IEquatable<NamespaceString>
    {
        private string _innerString;

        public NamespaceString()
        {
            _innerString = string.Empty;
        }

        public NamespaceString(string identifierString)
        {
            _innerString = ValidateAndCorrectString(identifierString);
        }

        /// <summary>
        /// Throw exceptions if format is not proper
        /// </summary>
        /// <param name="identifierString"></param>
        private string ValidateAndCorrectString(string identifierString)
        {
            string correctedString = identifierString.Replace('\\', '.').Replace(" ","");

            if (!Regex.IsMatch(correctedString, "^[a-zA-Z][a-zA-Z0-9\\.]*$"))
            {
                throw new NamespaceStringException($"NamespaceString is not in correct format of ascii letter followed by ascii letter, dot or number: {identifierString}");
            }
            return correctedString;
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
                _innerString = ValidateAndCorrectString(value);
            }
        }

        public NamespaceString Clone()
        {
            return new NamespaceString(_innerString);
        }

        public bool Equals(NamespaceString other)
        {
            return _innerString.Equals(other.ToString());
        }

        public override string ToString()
        {
            return _innerString;
        }

        /// <summary>
        /// Translates every dot in the namespace to a sub directory using Path.Combine
        /// fx DevelApp.Modules.Default translates into DevelApp\Modules\Default on a windows machine
        /// </summary>
        public string ToFilePath
        {
            get
            {
                string[] filePathParts = _innerString.Split('.');
                string filePath = string.Empty;
                for (int counter = 0; counter <= filePathParts.Length - 1; counter += 1)
                {
                    filePath = Path.Combine(filePath, filePathParts[0]);
                }
                return filePath;
            }
        }

        #region Implicit operators

        public static implicit operator NamespaceString(string rhs)
        {
            return new NamespaceString(rhs);
        }

        public static implicit operator string(NamespaceString rhs)
        {
            return rhs.ToString();
        }

        #endregion
    }

}
