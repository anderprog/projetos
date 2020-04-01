using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace IndraPortal.Helper
{
    public class LogFile
    {
        static String strFile = HttpContext.Current.Server.MapPath("~/log/IndraPortal.log");

        public static void log(Exception e, String mensagem, bool optionalIgnoreErros = true)
        {
            // nao mostrar ThreadAbortException no log
            if ((e != null) && (e is System.Threading.ThreadAbortException)) { return; }

            FileStream file = null;
            try
            {
                if (!Directory.Exists(new FileInfo(strFile).DirectoryName))
                    Directory.CreateDirectory(new FileInfo(strFile).DirectoryName);


                if (File.Exists(strFile))
                {
                    file = File.Open(strFile, FileMode.Append);
                }
                else
                {
                    file = File.Open(strFile, FileMode.OpenOrCreate);
                }
                using (StreamWriter sw = new StreamWriter(file))
                {
                    if (e is DbEntityValidationException)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ((DbEntityValidationException)e).EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);

                        sw.WriteLine(String.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1} - {2}", DateTime.Now, mensagem, new Exception(e.Message + " - " + exceptionMessage)));
                    }
                    else
                        sw.WriteLine(String.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1} - {2}", DateTime.Now, mensagem, e));
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {// caso ocorra um erro durante o salvamento do log
                if (optionalIgnoreErros)
                {// se a opcao ignore erros esteja true(default) nao ira gerar nenhuma exception
                    // ignora o erro
                }
                else
                {// caso a opcao ignore erros esteja false gera exception
                    // REMOVIDO POIS SE OCORRER ERRO DURANTE A GRAVACAO DE ERROS TAMBEM NAO DEVE MOSTRAR ERRO
                    // throw ex;
                }
            }
            finally
            {
                if (file != null) file.Close();
            }
        }

        //public static void logDebug(Exception e, String mensagem)
        //{
        //    if (Properties.Settings.Default.LogDebug)
        //    {
        //        log(e, mensagem);
        //    }
        //}

        //public static void logSql(Exception e, String mensagem)
        //{
        //    if (Properties.Settings.Default.LogSQL)
        //    {
        //        log(e, mensagem);
        //    }
        //}

        public static string serialize(object dados)
        {
            string serializado = new JavaScriptSerializer().Serialize(dados);
            return serializado;
        }

        public static object deserialize(Type type, string serializado)
        {
            object dados = new JavaScriptSerializer().Deserialize(serializado, type);
            return dados;
        }
    }
}