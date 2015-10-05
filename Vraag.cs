using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChallenge
{
    public abstract class Vraag
    {
        protected string _vraag;
        protected string juistAntwoord;
        protected string[] vraagDelen;
        

        public Vraag(string vraag)
        {
            vraagDelen = vraag.Split(',');
            this._vraag = vraagDelen[0];
        }

        public bool CheckJuist(string antwoord)
        {
            if (antwoord.ToLower().Trim() == juistAntwoord.ToLower().Trim())
            {
                return true;
            }

            return false;
        }

        public string _Vraag
        {
            get { return _vraag; }
            set { _vraag = value; }
        }

        public string JuistAntwoord
        {
            get { return juistAntwoord; }
            set { juistAntwoord = value; }
        }

        public string[] VraagDelen
        {
            get { return vraagDelen; }
            set { vraagDelen = value; }
        }
        

        
    }
}
