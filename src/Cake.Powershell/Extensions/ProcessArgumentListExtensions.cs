#region Using Statements
using System.Collections.Generic;
using System.Linq;

using Cake.Core.IO;
using Cake.Core.IO.Arguments;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Contains extension methods for <see cref="ProcessArgumentBuilder" />.
    /// </summary>
    public static class ProcessArgumentListExtensions
    {
        #region Methods
        /// <summary>
        /// Appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The text to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder Append(this ProcessArgumentBuilder builder, string name, string text)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new TextArgument(text)));
            }
            return builder;
        }

        /// <summary>
        /// Appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The text to be appended.</param>
        /// <param name="format">The format of the named argument.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder Append(this ProcessArgumentBuilder builder, string name, string text, string format)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new TextArgument(text), format));
            }

            return builder;
        }



        /// <summary>
        /// Quotes and appends the specified text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The text to be quoted and appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuoted(this ProcessArgumentBuilder builder, string name, string text)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(new TextArgument(text))));
            }
            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified argument to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The argument to be quoted and appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuoted(this ProcessArgumentBuilder builder, string name, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(argument)));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified argument to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The argument to be quoted and appended.</param>
        /// <param name="format">The format of the named argument.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuoted(this ProcessArgumentBuilder builder, string name, IProcessArgument argument, string format)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(argument), format));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified text as a string-literal to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The text to be quoted and appended as a string-literal.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendStringLiteral(this ProcessArgumentBuilder builder, string name, string text)
        {
            return AppendStringLiteral(builder, name, new TextArgument(text));
        }

        /// <summary>
        /// Appends the specified argument as a string-literal to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The argument to be quoted and appended as a string-literal.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendStringLiteral(this ProcessArgumentBuilder builder, string name, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new StringLiteralArgument(argument)));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified arguments as an array to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="arguments">The text collection to be appended as an array.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendArray(this ProcessArgumentBuilder builder, string name, IEnumerable<string> arguments)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new ArrayArgument(arguments.Select(argument => new TextArgument(argument)))));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified arguments as an array to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="arguments">The arguments to be appended as an array.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendArray(this ProcessArgumentBuilder builder, string name, IEnumerable<IProcessArgument> arguments)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new ArrayArgument(arguments)));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The secret text to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecret(this ProcessArgumentBuilder builder, string name, string text)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new SecretArgument(new TextArgument(text))));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The secret argument to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecret(this ProcessArgumentBuilder builder, string name, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new SecretArgument(argument)));
            }
            return builder;
        }

        /// <summary>
        /// Appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The secret argument to be appended.</param>
        /// <param name="format">The format of the named argument.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecret(this ProcessArgumentBuilder builder, string name, IProcessArgument argument, string format)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new SecretArgument(argument), format));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The secret text to be quoted and appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuotedSecret(this ProcessArgumentBuilder builder, string name, string text)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(new SecretArgument(new TextArgument(text)))));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The secret argument to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuotedSecret(this ProcessArgumentBuilder builder, string name, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(new SecretArgument(argument))));
            }

            return builder;
        }

        /// <summary>
        /// Quotes and appends the specified secret text to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The secret argument to be appended.</param>
        /// <param name="format">The format of the named argument.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendQuotedSecret(this ProcessArgumentBuilder builder, string name, IProcessArgument argument, string format)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new QuotedArgument(new SecretArgument(argument)), format));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret text as a string-literal to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="text">The secret text to be appended.</param>
        public static ProcessArgumentBuilder AppendStringLiteralSecret(this ProcessArgumentBuilder builder, string name, string text)
        {
            return AppendStringLiteralSecret(builder, name, new TextArgument(text));
        }

        /// <summary>
        /// Appends the specified secret argument as a string-literal to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="argument">The secret argument to be appended.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendStringLiteralSecret(this ProcessArgumentBuilder builder, string name, IProcessArgument argument)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new StringLiteralArgument(new SecretArgument(argument))));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified arguments as an array to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="arguments">The arguments to be appended as an array.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecretArray(this ProcessArgumentBuilder builder, string name, IEnumerable<string> arguments)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new ArrayArgument(arguments.Select(argument => new TextArgument(argument)))));
            }

            return builder;
        }

        /// <summary>
        /// Appends the specified secret arguments as an array to the argument builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The argument name.</param>
        /// <param name="arguments">The secret arguments to be appended as an array.</param>
        /// <returns>The same <see cref="ProcessArgumentBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ProcessArgumentBuilder AppendSecretArray(this ProcessArgumentBuilder builder, string name, IEnumerable<IProcessArgument> arguments)
        {
            if (builder != null)
            {
                builder.Append(new NamedArgument(name, new ArrayArgument(arguments.Select(argument => new SecretArgument(argument)))));
            }

            return builder;
        }
        #endregion
    }
}
