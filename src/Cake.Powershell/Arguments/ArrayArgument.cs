#region Using Statements
using System.Collections.Generic;
using System.Linq;

using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Represents a comma separated array of arguments.
    /// </summary>
    public sealed class ArrayArgument : IProcessArgument
    {
        #region Fields
        private const string ArgumentSeparator = ", ";

        private readonly IEnumerable<IProcessArgument> _arguments;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayArgument"/> class.
        /// </summary>
        /// <param name="arguments">The arguments that will be comma separated.</param>
        public ArrayArgument(IEnumerable<IProcessArgument> arguments)
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
            return string.Join(ArgumentSeparator, strings);
        }
        #endregion
    }
}