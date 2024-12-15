using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioBtInoa.Model
{
    public class EmailModel
    {
        public string destinatario { get; set; }
        public string smtpServer { get; set; }
        public int porta { get; set; }
        public string usuario { get; set; }
        public string assunto { get; set; }
        public string corpo { get; set; }
    }
}
