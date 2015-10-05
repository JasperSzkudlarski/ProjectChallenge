using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChallenge
{
    //auteur: Thomas Ven;
    //datum: 28/04/2015
    class Hashing
    {
        // zet de input string in een byte array die daarna
        // door het SHA1 hashing algoritme wordt gehaald. 
        // de output hiervan wordt weer omgezet naar een string voor op te kunnen slat in een txt bestand.
        // omdat Bitconverter standaard iedere byte omzet naar zijn hexadecimale waardes gescheiden door een "-"
        // moet deze nog worden weggehaalt door Replace("-", String.Empty), zodat je de echte hash krijgt.

        public static string HashIt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            using (var sha1 = new SHA1Managed())
            {
                byte[] inputData = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha1.ComputeHash(inputData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}
