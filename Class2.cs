using System;
using CsvComparator.Models;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using CsvComparator.Properties;


namespace CsvComparator
{
    class GenerateHtmlContent
    {
        static readonly string primaryFile = ConfigurationManager.AppSettings["PrimaryFilePath"];
        static readonly string secondaryFile = ConfigurationManager.AppSettings["SecondaryFilePath"];
        static readonly string outputFile = ConfigurationManager.AppSettings["OutputFilePath"];

        static void Main(string[] args)
        {
            var pf = FileReader.ReadPrimaryFile(primaryFile);
            var sf = FileReader.ReadSecondaryFile(secondaryFile);
            var diff = Comparator.Compare(pf, sf);

            string html = PrepareData(diff);
            string subject = DateTime.Now.ToString("MM/dd/yyyy") + " ETF Proxy Holding";
            SendEmail.Send(ConfigurationManager.AppSettings["ToAddress"], "", subject, html);

            FileWriter.WriteOutputFile(diff, outputFile);
        }

        private static string PrepareData(System.Collections.Generic.List<OutputFile> diff)
        {

            string html = "<html><head><style type=" + "\"text/css\">" + Resource1.Style.ToString() + "</style></head><body><tr>";

            html += "<th> FUNDCODE </th>";
            html += "<th > ISIN </th>";
            html += "<th> CUSIP </th>";
            html += "<th> SEDOL </th>";
            html += "<th> PLFShares </th>";
            html += "<th> PCFShares </th>";
            html += "<th> PLF PCF Difference </th>";
            html += "<th> ISSUES </th>";
            html += "</tr>";
            foreach (OutputFile item in diff)
            {
                var plfShare = Convert.ToInt64(item.PLFShares ?? 0).ToString("#,##0");
                var pcfShare = Convert.ToInt64(item.PCFShares ?? 0).ToString("#,##0");
                var difference = Convert.ToInt64((item.PLFShares ?? 0) - (item.PCFShares ?? 0)).ToString("#,##0");
                if (item.COMPARE != null && item.COMPARE != "")
                {
                    html +=
                           "<tr class=\"errorBackground\">";
                    if (item.COMPARE.ToUpper().Contains("PRIMARY"))
                    {
                        html = prepareData(html, item, plfShare, pcfShare, difference);
                    }
                    else
                    {
                        html = prepareData(html, item, plfShare, pcfShare, difference);
                    }
                }
                else
                {
                    html += "<tr >";
                    html = prepareData(html, item, plfShare, pcfShare, difference);
                }
            }
            html += "</table>";
            string header = "<h5>ETF Proxy Holding Disparity Audit</h5>";
            html = header + html;

            html += "</body></html>";
            return html;
        }

        private static string prepareData(string html, OutputFile item, string plfShare, string pcfShare, string difference)
        {
            html +=
                "<td >" + item.FUNDCODE + "</td>" +
                "<td >" + item.ISIN + "</td>" +
                "<td >" + item.CUSIP + "</td>" +
                "<td >" + item.SEDOL + "</td>" +
                "<td class=\"highlighter\">" + plfShare + "</td>" +
                "<td class=\"highlighter\">" + pcfShare + "</td>" +
                "<td class=\"highlighter\">" + difference + "</td>" +
                "<td >" + item.COMPARE + "</td>" +
                "</tr>";
            return html;
        }
    }
}
