using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace IndraPortal.Helper
{
    public enum FileType
    {
        Excel,
        PDF,
        Word
    }
    public static class ToExportHelper
    {
        public static void ExportTo<T>(this T ds, string ReportName, FileType filetype, Dictionary<string, string> parameters = null) where T : DataSet, new()
        {
            ReportParameterCollection reportParameters = null;
            if (parameters != null)
            {
                reportParameters = new ReportParameterCollection();

                foreach (var parameter in parameters)
                {
                    reportParameters.Add(new ReportParameter(parameter.Key, parameter.Value, true));
                }
            }
            ExportTo(ds, ReportName, filetype, reportParameters);
        }
        public static void ExportTo<T>(this T ds, string ReportName, FileType filetype, ReportParameterCollection parameters = null) where T : DataSet, new()
        {
            string contentType = "application/pdf";
            string header = "attachment; filename=exportado.pdf";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] bytes;

            if (filetype == FileType.Excel)
            {
                reportType = "Excel";
                header = "attachment;filename=exportado.xls";
                contentType = "application/ms-excel";
            }

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", header);

            LocalReport relatorio = new LocalReport();
            relatorio.ReportPath = HttpContext.Current.Server.MapPath(string.Format("~/Report/{0}.rdlc", ReportName));
            relatorio.EnableExternalImages = true;
            relatorio.EnableHyperlinks = true;

            PaperSize reportPgSize = null;
            try
            {
                reportPgSize = relatorio.GetDefaultPageSettings().PaperSize;
            }
            catch (Exception ex) { throw ex; }

            var pgsize = relatorio.GetDefaultPageSettings();
            pgsize.PaperSize.RawKind = (int)PaperKind.A4;
            PaperSize paperSize = reportPgSize ?? pgsize.PaperSize;
            Margins margins = pgsize.Margins;

            margins.Top = 0;
            margins.Bottom = 0;
            margins.Left = 0;
            margins.Right = 0;

            string deviceInfo =
                string.Format(
              CultureInfo.InvariantCulture,
                                    "<DeviceInfo>"
                                    + "<MarginTop>{0}</MarginTop>"
                                    + "<MarginLeft>{1}</MarginLeft>"
                                    + "<MarginRight>{2}</MarginRight>"
                                    + "<MarginBottom>{3}</MarginBottom>"
                                    + "<PageHeight>{4}</PageHeight>"
                                    + "<PageWidth>{5}</PageWidth>"
                                    + "</DeviceInfo>",
              ToInches(margins.Top),
              ToInches(margins.Left),
              ToInches(margins.Right),
              ToInches(margins.Bottom),
              ToInches(pgsize.IsLandscape ? paperSize.Width : paperSize.Height),
              ToInches(pgsize.IsLandscape ? paperSize.Height : paperSize.Width));

            relatorio.DataSources.Clear();
            if (ds != null)
                if (ds.Tables.Count > 0)
                    foreach (DataTable tb in ds.Tables)
                    {
                        ReportDataSource source = new ReportDataSource();
                        source.Name = tb.TableName;
                        source.Value = tb;
                        relatorio.DataSources.Add(source);
                    }
            if (parameters != null)
                foreach (var parameter in parameters)
                {
                    relatorio.SetParameters(parameter);
                }

            //Renderiza o relatório em bytes
            bytes = relatorio.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            HttpContext.Current.Response.BinaryWrite(bytes);

            HttpContext.Current.Response.End();
        }

        private static string ToInches(int hundrethsOfInch)
        {
            double inches = hundrethsOfInch / 100.0;
            return inches.ToString(CultureInfo.InvariantCulture) + "in";
        }

        public static void PrintTo<T>(this T ds, string ReportName, FileType filetype, Dictionary<string, string> parameters = null) where T : DataSet, new()
        {

            string contentType = "application/pdf";
            string header = "attachment; filename=exportado.pdf";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] bytes;

            if (filetype == FileType.Excel)
            {
                reportType = "Excel";
                header = "attachment;filename=exportado.xls";
                contentType = "application/ms-excel";
            }

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", header);


            LocalReport relatorio = new LocalReport();
            relatorio.ReportPath = HttpContext.Current.Server.MapPath(string.Format("~/Report/{0}.rdlc", ReportName));
            relatorio.EnableExternalImages = true;

            var pgsize = relatorio.GetDefaultPageSettings();
            pgsize.PaperSize.RawKind = (int)PaperKind.A4;
            PaperSize paperSize = pgsize.PaperSize;
            Margins margins = pgsize.Margins;
            margins.Top = 0;
            margins.Bottom = 0;
            margins.Left = 0;
            margins.Right = 0;

            string deviceInfo =
                string.Format(
              CultureInfo.InvariantCulture,
                                    "<DeviceInfo>"
                                    + "<MarginTop>{0}</MarginTop>"
                                    + "<MarginLeft>{1}</MarginLeft>"
                                    + "<MarginRight>{2}</MarginRight>"
                                    + "<MarginBottom>{3}</MarginBottom>"
                                    + "<PageHeight>{4}</PageHeight>"
                                    + "<PageWidth>{5}</PageWidth>"
                                    + "</DeviceInfo>",
              ToInches(margins.Top),
              ToInches(margins.Left),
              ToInches(margins.Right),
              ToInches(margins.Bottom),
              ToInches(pgsize.IsLandscape ? paperSize.Width : paperSize.Height),
              ToInches(pgsize.IsLandscape ? paperSize.Height : paperSize.Width));

            relatorio.DataSources.Clear();
            if (ds != null)
                if (ds.Tables.Count > 0)
                    foreach (DataTable tb in ds.Tables)
                    {
                        ReportDataSource source = new ReportDataSource();
                        source.Name = tb.TableName;
                        source.Value = tb;
                        relatorio.DataSources.Add(source);
                    }
            if (parameters != null)
                foreach (var parameter in parameters)
                {
                    relatorio.SetParameters(new ReportParameter(parameter.Key, parameter.Value));
                }






            //Renderiza o relatório em bytes
            bytes = relatorio.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            MemoryStream memoryStream = new MemoryStream(bytes);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string result = SaveMemoryStream(memoryStream);
            if (!string.IsNullOrEmpty(result))
            {
                HttpContext.Current.Response.Redirect(string.Format("~/View/pdf.js/w/?file={0}", result));
            }

        }
        public static void PrintTo<T>(this T ds, string ReportName, FileType filetype, ReportParameterCollection parameters = null) where T : DataSet, new()
        {

            string contentType = "application/pdf";
            string header = "attachment; filename=exportado.pdf";
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] bytes;

            if (filetype == FileType.Excel)
            {
                reportType = "Excel";
                header = "attachment;filename=exportado.xls";
                contentType = "application/ms-excel";

            }
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", header);


            LocalReport relatorio = new LocalReport();
            relatorio.ReportPath = HttpContext.Current.Server.MapPath(string.Format("~/Report/{0}.rdlc", ReportName));
            relatorio.EnableExternalImages = true;

            var pgsize = relatorio.GetDefaultPageSettings();
            pgsize.PaperSize.RawKind = (int)PaperKind.A4;
            PaperSize paperSize = pgsize.PaperSize;
            Margins margins = pgsize.Margins;
            margins.Top = 0;
            margins.Bottom = 0;
            margins.Left = 0;
            margins.Right = 0;

            string deviceInfo =
                string.Format(
              CultureInfo.InvariantCulture,
              "<DeviceInfo><MarginTop>{0}</MarginTop><MarginLeft>{1}</MarginLeft><MarginRight>{2}</MarginRight><MarginBottom>{3}</MarginBottom><PageHeight>{4}</PageHeight><PageWidth>{5}</PageWidth></DeviceInfo>",
              ToInches(margins.Top),
              ToInches(margins.Left),
              ToInches(margins.Right),
              ToInches(margins.Bottom),
              ToInches(pgsize.IsLandscape ? paperSize.Width : paperSize.Height),
              ToInches(pgsize.IsLandscape ? paperSize.Height : paperSize.Width));

            relatorio.DataSources.Clear();
            if (ds != null)
                if (ds.Tables.Count > 0)
                    foreach (DataTable tb in ds.Tables)
                    {
                        ReportDataSource source = new ReportDataSource();
                        source.Name = tb.TableName;
                        source.Value = tb;
                        relatorio.DataSources.Add(source);
                    }
            if (parameters != null)
                foreach (var parameter in parameters)
                {
                    relatorio.SetParameters(parameter);
                }






            //Renderiza o relatório em bytes
            bytes = relatorio.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            MemoryStream memoryStream = new MemoryStream(bytes);
            memoryStream.Seek(0, SeekOrigin.Begin);

            string result = SaveMemoryStream(memoryStream);
            if (!string.IsNullOrEmpty(result))
            {
                HttpContext.Current.Response.Redirect(string.Format("~/View/pdf.js/w/?file={0}", result));
            }

            HttpContext.Current.Response.End();
        }


        private static string SaveMemoryStream(MemoryStream memoryStream)
        {
            string caminho = HttpContext.Current.Server.MapPath("~/uploads/tmp");

            string path = Path.Combine(caminho, HttpContext.Current.Session.SessionID + ".pdf");
            try
            {
                if (!Directory.Exists(caminho))
                    Directory.CreateDirectory(caminho);

                FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write);
                byte[] bytes2 = new byte[memoryStream.Length];
                memoryStream.Read(bytes2, 0, (int)memoryStream.Length);
                file.Write(bytes2, 0, bytes2.Length);
                file.Close();
                memoryStream.Close();
                return String.Format(@"..\..\..\uploads\tmp\{0}", HttpContext.Current.Session.SessionID + ".pdf");
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}