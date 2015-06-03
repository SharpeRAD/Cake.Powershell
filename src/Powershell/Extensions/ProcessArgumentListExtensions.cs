#region Using Statements
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
    }
}
