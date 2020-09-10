using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DB
{
   public class DBOffline
    {
        public static string ArquivoDbase
        {
            get
            {
                return $"{Utilz.raizAppData()}db.dbase";
            }
        }
        public static string ArquivoLog
        {
            get
            {
                return $"{Utilz.raizAppData()}db.log";
            }
        }

        /// <summary>
        /// Gravação de log
        /// </summary>
        /// <param name="Mensagem"></param>
        public void Log(string Mensagem)
        {
            try
            {
                var pasta = Utilz.getPasta(ArquivoLog);
                if (!Directory.Exists(pasta))
                {
                    Directory.CreateDirectory(pasta);
                }


                StreamWriter arquivo = null;

                if (!File.Exists(ArquivoLog))
                {
                    arquivo = new StreamWriter(new FileStream(ArquivoLog, FileMode.CreateNew, FileAccess.ReadWrite));
                }
                else
                {
                    arquivo = new StreamWriter(ArquivoLog, true);
                }

                string Data = DateTime.Now.ToShortDateString();
                string Hora = DateTime.Now.ToShortTimeString();
                arquivo.WriteLine(
                    "\n=====================================================================\n" +
                    Data + "|" + Hora + "|---->" + this.ToString() + "\n" + Mensagem +
                    "\n=====================================================================\n"

                    );
                arquivo.Close();
            }
            catch (Exception)
            {

            }
        }

        private Tabela _tabela { get; set; }
        public Tabela tabela
        {
            get
            {
                if(_tabela ==null)
                {
                    _tabela = new Tabela();
                    _tabela.Carregar(ArquivoDbase);
                }
                return _tabela;
            }
        }

        private static DBOffline _db { get; set; }
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static DBOffline GetDBOffline()
        {
           if(_db == null)
            {
                _db = new DBOffline();
                
            }
            return _db;
        }
        public void Salvar()
        {
            tabela.Gravar(ArquivoDbase);
        }
        public void Create(Linha l)
        {
            tabela.lastId++;
            l.id = tabela.lastId;
            tabela.linhas.Add(l);
            Log("Linha Adicionada");
        }
        public bool Update(Linha l)
        {
            var s = tabela.get(l.id);
            if(s!=null)
            {
                tabela.get(l.id).celulas = l.celulas;
                Log($"Linha {l.id} Atualizada");
                return true;
            }
            else
            {
                Log($"Linha {l.id} Não encontrada");
            }
            return false;

        }
        public List<Linha> Read(string Chave, string Valor, bool Exato = false)
        {
            return tabela.filtrar(Chave, Valor, Exato);
        }
        public void Delete(Linha l)
        {
            tabela.linhas.Remove(l);
        }
        private DBOffline()
        {

        }
    }


    [Serializable]
    public class Linha
    {
        public override string ToString()
        {
            return " Células: " + this.celulas.Count;
        }
        public int id { get; set; } = 0;
        /// <summary>
        /// MonoState
        /// </summary>
        /// <param name="Coluna"></param>
        /// <returns></returns>
        public Celula get(string Coluna)
        {
            var s = celulas.Find(x => x.Coluna.ToUpper() == Coluna.ToUpper());
            if (s != null)
            {
                return s;
            }
            else
            {
                return new Celula();
            }
        }
        public void set(string Coluna, string valor)
        {
            var Retorno = celulas.Find(x => x.Coluna.ToUpper() == Coluna.ToUpper());
            if (Retorno != null)
            {
                Retorno.Valor = valor;
            }
        }
        public List<Celula> celulas { get; set; } = new List<Celula>();
        public Linha(List<Celula> celulas)
        {
            this.celulas = celulas;
        }
    }

    [Serializable]
    public class Tabela
    {
        public override string ToString()
        {
            return "[" + Nome + "]" + "/L:" + linhas.Count();
        }
        public Linha get(int id)
        {
            return this.linhas.Find(x => x.id == id);
        }
        public int lastId { get; set; } = 0;
        public string Nome { get; set; } = "";
        public Tabela Carregar(string Arquivo)
        {

            if (File.Exists(Arquivo))
            {
                try
                {
                    using (ZipFile zip = ZipFile.Read(Arquivo))
                    {
                        ZipEntry e = zip["tabela.dbdlm"];
                        if (zip.Entries.Count > 0)
                        {

                            XmlSerializer x = new XmlSerializer(typeof(Tabela));
                            Tabela ts = (Tabela)x.Deserialize(zip.Entries.ToArray()[0].OpenReader());
                            this.linhas = ts.linhas;
                            this.Nome = ts.Nome;
                            return ts;

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro descompactando arquivo \n" + Arquivo + "\n" + ex.Message + "\n" + ex.StackTrace);
                }
            }
            return new Tabela();
        }
        public bool Gravar(string Arquivo)
        {
            try
            {

                string arquivo_tmp = Application.StartupPath + @"\tabela.dbdlm";

                if (File.Exists(arquivo_tmp))
                {
                    File.Delete(arquivo_tmp);
                }

                XmlSerializer x = new XmlSerializer(typeof(Tabela));
                TextWriter writer = new StreamWriter(arquivo_tmp);
                x.Serialize(writer, this);
                writer.Close();

                if (File.Exists(Arquivo))
                {
                    File.Delete(Arquivo);
                }
                using (ZipFile zip = new ZipFile())
                {

                    zip.AddFile(arquivo_tmp, "");

                    zip.Save(Arquivo);
                }
                File.Delete(arquivo_tmp);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro Tentando criar o arquivo {Arquivo}\n" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }

        }
        public List<Linha> linhas { get; set; } = new List<Linha>();
        public List<Linha> filtrar(string Chave, string Valor, bool exato = false)
        {
            List<Linha> Retorno = new List<Linha>();
            if (exato)
            {
                return linhas.FindAll(x => x.celulas.FindAll(y => y.Coluna == Chave && y.Valor == Valor).Count > 0);
            }
            else
            {
                return linhas.FindAll(x => x.celulas.FindAll(y => y.Coluna.ToLower().Replace(" ", "") == Chave.ToLower().Replace(" ", "") && y.Valor.ToLower().Replace(" ", "").Contains(Valor.ToLower().Replace(" ", ""))).Count > 0);
            }
        }
        public Tabela()
        {

        }
    }


    [Serializable]
    public class Celula
    {
        public override string ToString()
        {
            return "[" + Coluna + "]=" + Valor;
        }
        public string Coluna { get; set; } = "";
        public string Valor { get; set; } = "";
        public Celula()
        {

        }
    }
}
