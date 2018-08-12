#region Using Statements
using System;
using System.Globalization;

using Cake.Core.IO;
using Cake.Core.IO.Arguments;

#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Represents a named argument and its value.
    /// </summary>
    public sealed class KeyValueArgument : IProcessArgument
    {
        #region Fields
        private readonly string _Key;
        private readonly IProcessArgument _Value;

        private string _Format;
        private const string DefaultFormat = "\"{0}\" = \"{1}\";";
        #endregion





        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueArgument"/> class.
        /// </summary>
        /// <param name="key">The key of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public KeyValueArgument(string key, TextArgument value)
            : this(key, value, KeyValueArgument.DefaultFormat)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueArgument"/> class.
        /// </summary>
        /// <param name="key">The key of the argument.</param>
        /// <param name="value">The argument value.</param>
        /// <param name="format">The format of argument.</param>
        public KeyValueArgument(string key, TextArgument value, string format)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            _Key = key;
            _Value = value;
            _Format = format;
        }
        #endregion





        #region Properties
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





        #region Methods
        /// <summary>
        /// Render the arguments as a <see cref="System.String" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            return string.Format(CultureInfo.InvariantCulture, _Format, _Key, _Value.Render());
        }

        /// <summary>
        /// Renders the argument as a <see cref="System.String"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return string.Format(CultureInfo.InvariantCulture, _Format, _Key, _Value.RenderSafe());
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
