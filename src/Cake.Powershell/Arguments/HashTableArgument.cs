#region Using Statements
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Represents a hashtable argument.
    /// </summary>
    public sealed class HashTableArgument : IProcessArgument
    {
        #region Fields

        private const string OpenHashTable = "@{ ";
        private const string CloseHashTable = " }"; 

        private readonly IEnumerable<IProcessArgument> _arguments;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HashTableArgument"/> class.
        /// </summary>
        /// <param name="arguments">The arguments that will be comma separated.</param>
        public HashTableArgument(IEnumerable<KeyValueArgument> arguments)
        {
            _arguments = arguments;
        }
        #endregion





        #region Methods
        /// <summary>
        /// Render the arguments as a <see cref="T:System.String" />.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>
        /// A comma separated string representation of the arguments.
        /// </returns>
        public string Render()
        {
            return ConcatenateStrings(_arguments.Select(argument => argument.Render()));
        }

        /// <summary>
        /// Renders the argument as a <see cref="T:System.String" />.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>
        /// A comma separated safe string representation of the argument.
        /// </returns>
        public string RenderSafe()
        {
            return ConcatenateStrings(_arguments.Select(argument => argument.RenderSafe()));
        }



        private static string ConcatenateStrings(IEnumerable<string> strings)
        {
            var stringBuilder = new StringBuilder(OpenHashTable);

            foreach (var s in strings)
            {
                stringBuilder.AppendFormat("{0} ", s);
            }

            stringBuilder.Append(CloseHashTable);

            return stringBuilder.ToString();
        }
        #endregion
    }
}