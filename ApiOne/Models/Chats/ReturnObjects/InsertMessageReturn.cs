using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats.ReturnObjects
{
    public class InsertMessageReturn
    {
        public string Error { get; set; } = null;
        public string Username { get; set; } = null;

        public InsertMessageReturn()
        {
        }
    }
}
