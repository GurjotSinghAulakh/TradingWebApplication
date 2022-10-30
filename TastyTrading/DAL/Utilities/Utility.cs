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
         * Method for generating a base64 string for orderID 
         */
         
        /* Source : https://www.codegrepper.com/profile/ege-bilecen */

        /* It creates a new GUID, converts it to a base64 string, and then removes the '=' and '+' characters from the string */ 

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