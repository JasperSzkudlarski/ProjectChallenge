using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChallenge
{
    // Auteur:  Thomas Ven
    // Datum:   13/04/2015
    class EmptyFieldException : ApplicationException
    {
        public EmptyFieldException(string Message)
            : base(Message)
        { }
    }
}
