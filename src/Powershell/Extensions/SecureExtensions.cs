#region Using Statements
    using System;
    using System.Security;
#endregion



namespace Cake.Powershell
{
    /// <summary>
    /// Responsible for creating secure strings
    /// </summary>
    public static class SecureExtensions
    {
        public static SecureString MakeSecure(this string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var securePassword = new SecureString();

            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();

            return securePassword;
        }
    }
}
