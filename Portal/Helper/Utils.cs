using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace IndraPortal.Helper
{
    public static class Utils
    {
        public static bool ChecaMaioridade(DateTime data_sistema, DateTime data_nasc, int idadeMaioridadeCobertura)
        {
            if ((data_sistema.Year - data_nasc.Year) == idadeMaioridadeCobertura) // Checa se o cliente tem 18 anos
            {
                // caso o calculo de ano de certo, necessario verificar mes e dia tambem, para certificar que sao 18 anos completados
                if (data_sistema.Month < data_nasc.Month) // Menor
                {
                    return false;
                }
                else if (data_sistema.Month == data_nasc.Month)
                {
                    if (data_sistema.Day > data_nasc.Day)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                if ((data_sistema.Year - data_nasc.Year) < idadeMaioridadeCobertura)
                    return false;
                else
                    return true;
            }
        }


        public static string DateToToken(this DateTime date)
        {
            return string.Format("{0}{1}{2}{3}", date.Millisecond.ToString().PadLeft(4, '0'), date.Day.ToString().PadLeft(2, '0'), date.Month.ToString().PadLeft(2, '0'), date.Year.ToString().PadLeft(4, '0'));
        }
        public static DateTime TokenToDate(this string token)
        {
            token = token.Remove(0, 4);
            return Convert.ToDateTime(string.Format("{0}/{1}/{2}", token.Substring(0, 2), token.Substring(2, 2), token.Substring(4, 4)));
        }

        public static string Left(this String varString, int tamanho)
        {
            try
            {
                return (varString.Length < tamanho) ? varString : varString.Substring(0, tamanho);
            }
            catch (Exception)
            {
                return String.Empty;
            }

        }

        public static DataTable GetNullToStringEmpty(DataTable dtSource)
        {
            // Create a target table with same structure as source and fields as strings
            // We can change the column datatype as long as there is no data loaded
            DataTable dtTarget = dtSource.Clone();
            foreach (DataColumn col in dtTarget.Columns)
                col.DataType = typeof(string);

            // Start importing the source into target by ItemArray copying which 
            // is found to be reasonably fast for nulk operations. VS 2015 is reporting
            // 500-525 milliseconds for loading 100,000 records x 10 columns 
            // after null conversion in every cell which may be usable in many
            // circumstances.
            // Machine config: i5 2nd Gen, 8 GB RAM, Windows 7 64bit, VS 2015 Update 1
            int colCountInTarget = dtTarget.Columns.Count;
            foreach (DataRow sourceRow in dtSource.Rows)
            {
                // Get a new row loaded with data from source row
                DataRow targetRow = dtTarget.NewRow();
                targetRow.ItemArray = sourceRow.ItemArray;

                // Update DBNull.Values to empty string in the new (target) row
                // We can safely assign empty string since the target table columns
                // are all of string type
                for (int ctr = 0; ctr < colCountInTarget; ctr++)
                    if (targetRow[ctr] == DBNull.Value)
                        targetRow[ctr] = String.Empty;

                // Now add the null filled row to target datatable
                dtTarget.Rows.Add(targetRow);
            }

            // Return the target datatable
            return dtTarget;
        }

        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            value = Regex.Replace(value, @"\W+", " ");
            return Regex.Replace(value, @"\s+", " ").Trim();
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static Expression<Func<T, object>> GetLambdaExpressionForSortProperty<T>(string propertyname) where T : class
        {
            Expression member = null;
            var propertyParts = propertyname.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var param = Expression.Parameter(typeof(T), "arg");
            Expression previous = param;

            foreach (string s in propertyParts)
            {
                member = Expression.Property(previous, s);
                previous = member;
            }

            if (member.Type.IsValueType)
            {
                member = Expression.Convert(member, typeof(object));
            }

            return Expression.Lambda<Func<T, object>>(member, param);
        }

        public static Object isNull(Object databaseValue)
        {
            if ((DBNull.Value).Equals(databaseValue))
            {
                return null;
            }
            else
            {
                return databaseValue;
            }
        }

        public static DateTime? isNullDateTime(Object databaseValue)
        {
            if ((DBNull.Value).Equals(databaseValue))
            {
                return null;
            }
            else
            {
                try
                {
                    DateTime retorno;
                    if (DateTime.TryParse("" + databaseValue, out retorno))
                    {
                        return retorno;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Int32 isNullInt32(Object databaseValue)
        {
            Int32 retorno = 0;
            if ((DBNull.Value).Equals(databaseValue) || databaseValue == null || string.IsNullOrWhiteSpace(databaseValue.ToString()))
            {
                return 0;
            }
            else
            {
                if (Int32.TryParse(databaseValue.ToString(), out retorno))
                {
                    return retorno;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static Int64 isNullInt64(Object databaseValue)
        {
            Int64 retorno = 0;
            if ((DBNull.Value).Equals(databaseValue) || databaseValue == null || string.IsNullOrWhiteSpace(databaseValue.ToString()))
            {
                return 0;
            }
            else
            {
                if (Int64.TryParse(databaseValue.ToString(), out retorno))
                {
                    return retorno;
                }
                else
                {
                    return 0;
                }
            }
        }

        internal static Double isNullDouble(Object databaseValue)
        {
            Double retorno = 0.0;
            if (DBNull.Value.Equals(databaseValue) || databaseValue == null || string.IsNullOrWhiteSpace(databaseValue.ToString()))
            {
                return 0.0;
            }
            else
            {
                if (Double.TryParse(databaseValue.ToString(), out retorno))
                {
                    return retorno;
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public static string formatDate(DateTime? date)
        {
            return date == null ? "" : date.Value.ToString("dd/MM/yyyy");
        }

        public static DateTime? stringToDate(string str)
        {
            return isNullDateTime(str);
        }

        public static object zeroToNull(long value)
        {
            if (value == 0)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public static int DateDiffDay(DateTime? lValue, DateTime? rValue)
        {
            if (lValue == null) return 0;
            if (rValue == null) return 0;
            {
                TimeSpan diff = ((DateTime)lValue) - ((DateTime)rValue);
                return Math.Abs(diff.Days);
            }
        }

        public static int DateDiffMonth(DateTime? lValue, DateTime? rValue)
        {
            if (lValue == null) return 0;
            if (rValue == null) return 0;
            {
                return Math.Abs((((DateTime)lValue).Month - ((DateTime)rValue).Month) + 12 * (((DateTime)lValue).Year - ((DateTime)rValue).Year));
            }
        }

        public static DateTime? DateAddMonth(DateTime? data, int meses)
        {
            if (data == null) return null;
            {
                return ((DateTime)data).AddMonths(meses);
            }
        }

        public static int ExtrairNumeros(string s)
        {
            var result = Regex.Replace(s, @"[^\d]", String.Empty);

            if (result != string.Empty && result.Any(char.IsDigit))
                return Convert.ToInt32(result);
            else
                return 0;
        }
        public static long RemoveTracosPontos(string s)
        {
            var result = Regex.Replace(s, @"[^\d]", String.Empty);

            if (result != string.Empty && result.Any(char.IsDigit))
                return Convert.ToInt64(result);
            else
                return 0;
        }

        public static string SoNumeros(string s)
        {
            try
            {
                string result = Regex.Replace("" + s, @"[^\d]", "");
                return result;
            }
            catch
            {
                return "";
            }
        }



        public static double moneyToDouble(object money)
        {
            Double retorno = 0.0;
            if ((DBNull.Value).Equals(money) || money == null || string.IsNullOrWhiteSpace(money.ToString()))
            {
                return 0.0;
            }
            else
            {
                if (Double.TryParse(money.ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture, out retorno))
                {
                    return retorno;
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public static double PercentToDouble(object percent)
        {
            Double retorno = 0.0;
            if ((DBNull.Value).Equals(percent) || percent == null || string.IsNullOrWhiteSpace(percent.ToString()))
            {
                return 0.0;
            }
            else
            {
                if (Double.TryParse(percent.ToString().Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, ""), out retorno))
                {
                    return retorno;
                }
                else
                {
                    return 0.0;
                }
            }
        }

        public static string formatMoney(double money)
        {
            return string.Format("{0:#,##0.00}", money);
        }

        public static string formatMoneyReais(double money)
        {
            #region Primeiro Jeito de Fazer
            //CultureInfo das = new CultureInfo("pt-Br");
            //return Convert.ToDouble(money).ToString("C");
            #endregion

            #region Segundo Jeito de Fazer
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "R$";
            return string.Format(numberFormatInfo, "{0:C}", money);
            #endregion
        }

        public static string formatMoneyReais(string money)
        {
            #region Segundo Jeito de Fazer
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "R$";
            return string.Format(numberFormatInfo, "{0:C}", money);
            #endregion
        }

        public static bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");

            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)

                if (valor[i] != valor[0])
                {
                    igual = false;
                }
            if (igual || valor == "123456789")
            {
                return false;
            }

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)

                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)

                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                    return false;

            return true;
        }

        public static bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = vrCNPJ.Replace(".", "");

            CNPJ = CNPJ.Replace("/", "");

            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;

            int nrDig;

            string ftmt;

            bool[] CNPJOK;

            ftmt = "6543298765432";

            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;

            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;

            CNPJOK = new bool[2];
            CNPJOK[0] = false;
            CNPJOK[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)

                        soma[0] += (digitos[nrDig] *
                            int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)

                        soma[1] += (digitos[nrDig] *
                            int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (

                        resultado[nrDig] == 1))

                        CNPJOK[nrDig] = (
                            digitos[12 + nrDig] == 0);
                    else
                        CNPJOK[nrDig] = (
                            digitos[12 + nrDig] == (
                            11 - resultado[nrDig]));
                }
                return (CNPJOK[0] && CNPJOK[1]);
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ToDataTable<T>(this T obj) where T : class
        {
            DataTable dt = new DataTable(obj.GetType().Name);
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
                dt.Columns.Add(pi.Name);
            DataRow row = dt.NewRow();

            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                var value = pi.GetValue(obj, null);
                row[pi.Name] = value == null ? DBNull.Value : value;
            }
            dt.Rows.Add(row);
            dt.AcceptChanges();
            return dt;
        }

        public static DataTable ToDataTable<T>(this List<T> objs) where T : class
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in objs)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static bool isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static object Wscol(DataTable tb, string nomecoluna)
        {
            try
            {
                return string.Format("{0}", tb.Rows[0][nomecoluna]);
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static string formatCPF(string cpf)
        {
            string num = ((new System.Text.RegularExpressions.Regex("[^0-9]")).Replace("" + cpf, ""));
            if (num.Length < 11)
            {
                num = num.PadLeft(11, '0');
            }
            string ret = num.Substring(0, 3) + "." + num.Substring(3, 3) + "." + num.Substring(6, 3) + "-" + num.Substring(9, 2);
            return ret;
        }

        public static string formatCNPJ(string cnpj)
        {
            string num = ((new System.Text.RegularExpressions.Regex("[^0-9]")).Replace("" + cnpj, ""));
            if (num.Length < 14)
            {
                num = num.PadLeft(14, '0');
            }
            //00.000.000/0000-00
            //01 234 567 8901 23
            string ret = num.Substring(0, 2) + "." + num.Substring(2, 3) + "." + num.Substring(5, 3) + "/" + num.Substring(8, 4) + "-" + num.Substring(12, 2);
            return HttpUtility.HtmlEncode(ret);
        }

        /// <summary>
        /// retorna o valor pelo nome do campo da primeira linha do dataset 
        /// se o campo existir
        /// </summary>
        /// <param name="dtName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string getDataSetField(DataTable dtName, string fieldName)
        {
            if (dtName != null && dtName.Rows != null && dtName.Rows.Count > 0)
            {
                if (dtName.Columns.Contains(fieldName))
                {
                    return dtName.Rows[0][fieldName].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// retorna o valor pelo nome do campo da linha informada 
        /// se o campo existir
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string getDataRowField(DataRow row, string fieldName)
        {
            if (row != null)
            {
                if (row.Table.Columns.Contains(fieldName))
                {
                    return row[fieldName].ToString().Trim();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dropDownList">nome do dropDownList que deseja popular</param>
        /// <param name="qtdAnos">Digite a quantidade de anos que deseja popular à partir do atual</param>
        public static void PopulaAnosDropDownList(DropDownList dropDownList, int qtdAnos)
        {
            dropDownList.Items.Clear();

            try
            {
                List<String> ultimosAnos = new List<String>();
                int anoCorrente = DateTime.Now.Year;
                for (int i = anoCorrente - qtdAnos; i <= anoCorrente; i++)
                {
                    ultimosAnos.Add(Convert.ToString(i));
                }

                dropDownList.DataSource = (from c in ultimosAnos
                                           select new ListItem()
                                           {
                                               Text = c,
                                               Value = c
                                           });
                dropDownList.DataBind();
            }

            catch (Exception ex)
            {
                LogFile.log(ex, "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True or False</returns>
        public static bool IsValidMesAno(String input)
        {
            try
            {
                String pattern = @"^(((0[123456789]|10|11|12)([/])(([1][9][0-9][0-9])|([2][0-9][0-9][0-9]))))";
                if (Regex.IsMatch(input, pattern))
                {
                    DateTime output;
                    DateTime.TryParse("01/" + input, out output);
                    if (output.Year > 1900)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch { return false; }


        }
        public static bool IsValidDiaMesAno(String input)
        {
            DateTime dataOut = new DateTime();

            try
            {
                DateTime.TryParse(input, out dataOut);

                return true;
            }
            catch { return false; }
        }


        public static DateTime? pegaUltimoDiaDoMes(DateTime? data)
        {
            DateTime? retorno = null;
            try
            {
                if (data != null)
                {
                    var finallyDay = DateTime.DaysInMonth(((DateTime)data).Year, ((DateTime)data).Month);
                    retorno = new DateTime(((DateTime)data).Year, ((DateTime)data).Month, finallyDay);
                }
            }
            catch (Exception ex)
            {
                LogFile.log(ex, "erro ao calcular ultimo dia do mes");
            }
            return retorno;
        }

        public static string formatTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return "";

            long retorno = isNullInt64(Regex.Replace(telefone, @"[^\d]", ""));

            return string.Format("{0:(00) 0000-0000}", retorno);
        }

        public static string formatCelular(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return "";

            long retorno = isNullInt64(Regex.Replace(telefone, @"[^\d]", ""));

            return string.Format("{0:(00) 00000-0000}", retorno);
        }
    }
}