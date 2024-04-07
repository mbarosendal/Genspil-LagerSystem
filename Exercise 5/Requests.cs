using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektGenspil
{
    internal class Requests
    {
        public string NameOfCustomer;
        public string Contact;
        public string AdditionalDetails;

        public Requests() 
        {
            this.NameOfCustomer = string.Empty;
            this.Contact = string.Empty;
            this.AdditionalDetails = string.Empty;
        }

    }
}
