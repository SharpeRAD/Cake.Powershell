#region Using Statements
using System;
using System.Globalization;

using Cake.Core.IO;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Represents a named argument and its value.
    /// </summary>
    public sealed class NamedArgument : IProcessArgument
    {
        #region Fields (4)
        private readonly string _Name;
        private readonly IProcessArgument _Value;

        private string _Format;
        private const string DefaultFormat = "-{0} {1}";
        #endregion





        #region Constructor (2)
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        public NamedArgument(string name, IProcessArgument value)
            : this(name, value, NamedArgument.DefaultFormat)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The argument value.</param>
        /// <param name="format">The format of argument.</param>
        public NamedArgument(string name, IProcessArgument value, string format)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            _Name = name;
            _Value = value;
            _Format = format;
        }
        #endregion





        #region Properties (1)
        /// <summary>
        /// Gets or sets the format of the argument
        /// </summary>
        /// <value>The argument format.</value>
        public string Format
        {
            get
            {
                return _Format;
            }

            set
            {
                _Format = value;
            }
        }
        #endregion





        #region Methods (3)
        /// <summary>
        /// Render the arguments as a <see cref="System.String" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            return string.Format(CultureInfo.InvariantCulture, _Format, _Name, _Value.Render());
        }

        /// <summary>
        /// Renders the argument as a <see cref="System.String"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return string.Format(CultureInfo.InvariantCulture, _Format, _Name, _Value.RenderSafe());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return RenderSafe();
        }
        #endregion
    }
}
