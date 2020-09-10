using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace DB
{
    internal class Utilz
    {
        public static string raizAppData()
        {
            return criarPasta(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\", "DBOffline");
        }
        public static string getPasta(string Arquivo)
        {
            try
            {
                return System.IO.Path.GetDirectoryName(Arquivo);
            }
            catch (Exception)
            {
            }
            return "";
        }
        public static string criarPasta(string Raiz, string Pasta)
        {
            if (!Raiz.EndsWith(@"\")) { Raiz = Raiz + @"\"; }
            string NovaPasta = (Raiz + @"\" + Pasta).Replace(@"\\", @"\");
            if (NovaPasta.StartsWith(@"\")) { NovaPasta = @"\" + NovaPasta; }

            if (!Directory.Exists(NovaPasta))
            {
                try
                {
                    Directory.CreateDirectory(NovaPasta);
                    return NovaPasta + @"\";
                }
                catch (Exception)
                {
                    return "";
                }

            }
            return NovaPasta + @"\";
        }
    }
}
