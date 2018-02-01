using System.Collections.Generic;
using ExpertPdf.HtmlToPdf;

namespace FrameworkLibrary
{
    public class PDFHelper
    {
        private PdfConverter pdfConverter = new PdfConverter();

        public PDFHelper(string headerText, string footerText = "")
        {
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.ShowHeader = true;
            pdfConverter.PdfDocumentOptions.ShowFooter = true;            
            pdfConverter.PdfDocumentOptions.LeftMargin = 40;
            pdfConverter.PdfDocumentOptions.RightMargin = 40;
            pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
            pdfConverter.PdfDocumentOptions.TopMargin = 20;
            //pdfConverter.PdfDocumentOptions.BottomMargin = 20;
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            pdfConverter.ScriptsEnabled = true;
            pdfConverter.ScriptsEnabledInImage = true;
            pdfConverter.PdfBookmarkOptions.HtmlElementSelectors = new string[] { "H1", "H2" };

            pdfConverter.PdfHeaderOptions.HeaderText = headerText;            
            //pdfConverter.PdfHeaderOptions.HeaderTextColor = Color.Blue;
            //pdfConverter.PdfHeaderOptions.HeaderDescriptionText = string.Empty;
            //pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;

            pdfConverter.PdfFooterOptions.FooterText = footerText;
            //pdfConverter.PdfFooterOptions.FooterTextColor = Color.Blue;
            pdfConverter.PdfFooterOptions.DrawFooterLine = true;
            pdfConverter.PdfFooterOptions.PageNumberText = "Page";
            pdfConverter.PdfFooterOptions.ShowPageNumber = true;

            pdfConverter.AvoidImageBreak = true;
            pdfConverter.AvoidTextBreak = true;

            pdfConverter.LicenseKey = "";            
        }

        /// <summary>
        /// Shortcut to generate PDF documents
        /// </summary>
        /// <param name="pathToPdfBodyTemplate">The path to PDF body template.</param>
        /// <param name="pathToPdfFooterTemplate">The path to PDF footer template.</param>
        /// <param name="objectToPassInToTemplate">The object to pass in to template.</param>
        /// <param name="keyValuePairTemplateVariables">The key value pair template variables.</param>
        /// <param name="downloadFileName">Name of the download file.</param>
        public static void QuickPDFGenerator(string pathToPdfBodyTemplate, string downloadFileName, string pathToPdfFooterTemplate = "", object objectToPassInToTemplate = null, Dictionary<string, string> keyValuePairTemplateVariables = default(Dictionary<string, string>))
        {
            string bodyHtml = ParserHelper.ParseData(LoaderHelper.RenderControl(pathToPdfBodyTemplate, objectToPassInToTemplate), keyValuePairTemplateVariables);

            string footerHtml = "";

            if (pathToPdfFooterTemplate == null)
                footerHtml = ParserHelper.ParseData(LoaderHelper.RenderControl(pathToPdfFooterTemplate, objectToPassInToTemplate), keyValuePairTemplateVariables);

            PDFHelper pdfHelper = new PDFHelper("Header Text");

            if (footerHtml != "")
            {
                pdfHelper.Converter.PdfFooterOptions.ShowPageNumber = false;
                pdfHelper.Converter.PdfFooterOptions.HtmlToPdfArea = new HtmlToPdfArea(footerHtml, URIHelper.BaseUrl);
            }

            pdfHelper.ConvertHTMLToPDF(bodyHtml, StringHelper.CreateSlug(downloadFileName));
        }

        public PdfConverter Converter
        {
            get
            {
                return pdfConverter;
            }
        }

        public void ConvertUrlToPdf(string url, string downloadName)
        {
            byte[] downloadBytes = pdfConverter.GetPdfFromUrlBytes(url);
            DownloadPDF(downloadBytes, downloadName);
        }

        public void ConvertHTMLToPDF(string html, string downloadName)
        {
            byte[] downloadBytes = pdfConverter.GetPdfBytesFromHtmlString(html);
            DownloadPDF(downloadBytes, downloadName);
        }

        private void DownloadPDF(byte[] downloadBytes, string downloadName)
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition",
                "attachment; filename=" + StringHelper.CreateSlug(downloadName) + ".pdf; size=" + downloadBytes.Length.ToString());
            response.Flush();
            response.BinaryWrite(downloadBytes);
            response.Flush();
            response.End();
        }
    }
}