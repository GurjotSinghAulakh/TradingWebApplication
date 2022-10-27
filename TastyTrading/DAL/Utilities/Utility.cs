using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TastyTrading.DAL.Utilities
{
    [ExcludeFromCodeCoverage]
    public static class Utility
    {
        /**
         * Method for generating random hex number for customer and ticket reference
         */
        /* Source : https://www.codegrepper.com/profile/ege-bilecen */

        public static string CreateUniqueString()
        {
            Guid g = Guid.NewGuid();

            string uniqueString = Convert.ToBase64String(g.ToByteArray());
            uniqueString = uniqueString.Replace("=", "");
            uniqueString = uniqueString.Replace("+", "");

            return uniqueString;
        }
      
    }
}