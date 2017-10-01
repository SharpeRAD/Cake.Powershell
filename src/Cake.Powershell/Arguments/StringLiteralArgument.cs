#region Using Statements
using System.Globalization;

using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Represents a string-literal argument. Specifically,
    /// '$' chars will not be evaulated as variables, nor will '()' pairs as expressions.
    /// </summary>
    public sealed class StringLiteralArgument : IProcessArgument
    {
        #region Fields
        private readonly IProcessArgument _argument;
        #endregion





        #region Constructors
        /// <summary>
        /// Initializes an instance of the <see cref="StringLiteralArgument"/> class.
        /// </summary>
        /// <param name="argument">An <see cref="IProcessArgument"/> to wrap as a string-literal.</param>
        public StringLiteralArgument(IProcessArgument argument)
        {
            _argument = argument;
        }
        #endregion





        #region Methods
        /// <summary>
        /// Renders the argument as a string-literal <see cref="string"/>.
        /// </summary>
        /// <returns>A string-literal represenation of the argument.</returns>
        public string Render()
        {
            return MakePowershellLiteralString(_argument.Render());
        }

        /// <summary>
        /// Renders the argument as a string-literal <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return MakePowershellLiteralString(_argument.RenderSafe());
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return RenderSafe();
        }



        private static string MakePowershellLiteralString(string text)
        {
            return string.Format(CultureInfo.InvariantCulture, "'{0}'", text.Replace("'", "''"));
        }
        #endregion
    }
}