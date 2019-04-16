using CsvComparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvComparator
{
    public static class Comparator
    {
        public static List<dynamic> Compare(List<PrimaryFile> primaryFile, List<SecondaryFile> secondaryFile)
        {
            var pfOuterQuery = from pf in primaryFile
                               join sf in secondaryFile
                               on new { pf.FUNDCODE, pf.ISIN } equals new { sf.FUNDCODE, sf.ISIN }
                               into g
                               from sf in g.DefaultIfEmpty()
                               select new { pf, sf };

            var sfOuterQuery = from sf in secondaryFile
                               join pf in primaryFile
                               on new { sf.FUNDCODE, sf.ISIN } equals new { pf.FUNDCODE, pf.ISIN }
                               into g
                               from pf in g.DefaultIfEmpty()
                               select new { sf, pf };

#if DEBUG
            Console.WriteLine("\n-------------------Comparing Primary File with Secondary File-------------------\n");
            foreach (var v in pfOuterQuery)
            {
                Console.WriteLine(v.pf + " - " + v.sf);
            }

            Console.WriteLine("\n-------------------Comparing Secondary File with Primary File-------------------\n");
            foreach (var v in sfOuterQuery)
            {
                Console.WriteLine(v.sf + " - " + v.pf);
            }
#endif
            var onlyPFRecords = pfOuterQuery.Where(r => r.sf == null).Select(r => r.pf).ToList();
            var onlySFRecords = sfOuterQuery.Where(r => r.pf == null).Select(r => r.sf).ToList();
            var matchedRecords = pfOuterQuery.Where(r => r.pf != null && r.sf != null);
            var dataNotEqualRecords = matchedRecords.Where(r => r.pf.BASKETSHARES != r.sf.SHARES ||
                                                        r.pf.CURRENCYCODE != r.sf.CUR ||
                                                        r.pf.CIL != r.sf.CIL ||
                                                        r.pf.TRADECOUNTRY != r.sf.ISO).Select(r => new { r.pf, r.sf }).ToList();



            List<dynamic> list = new List<dynamic>();


            foreach (var pf in onlyPFRecords)
            {
                list.Add(new OutputFile(pf, "only in primary file"));
            }


            foreach (var sf in onlySFRecords)
            {
                list.Add(new OutputFile(sf, "only in secondary file"));
            }


            foreach (var record in dataNotEqualRecords)
            {
                var opf = new OutputFile(record.pf, "diff in primary file");
                var osf = new OutputFile(record.sf, "diff in secondary file");
                if (opf.BASKETSHARES == osf.BASKETSHARES)
                {
                    opf.BASKETSHARES = null;
                    osf.BASKETSHARES = null;
                }
                if (opf.CURRENCYCODE == osf.CURRENCYCODE)
                {
                    opf.CURRENCYCODE = null;
                    osf.CURRENCYCODE = null;
                }
                if (opf.CIL == osf.CIL)
                {
                    opf.CIL = null;
                    osf.CIL = null;
                }
                if (opf.TRADECOUNTRY == osf.TRADECOUNTRY)
                {
                    opf.TRADECOUNTRY = null;
                    osf.TRADECOUNTRY = null;
                }
                list.Add(opf);
                list.Add(osf);
            }
            return CompareAndWriteOutput(list);
        }

        private static List<dynamic> CompareAndWriteOutput(List<dynamic> list)
        {
            List<dynamic> output = new List<dynamic>();

            var uniqueFunds = list.Select(g => g.FUNDCODE).Distinct();

            foreach (string fund in uniqueFunds)
            {
                var uniqueISIN = list.Where(g => g.FUNDCODE == fund).Select(t => t.ISIN).Distinct();

                foreach (var item in uniqueISIN)
                {
                    var matchedRecord = list.Where(g => g.ISIN == item && g.FUNDCODE == fund).ToArray();

                    var secondaryFile = new SecondaryFile
                    {
                        FUNDCODE = matchedRecord[0].FUNDCODE,
                        ISIN = matchedRecord[0].ISIN,
                        CUSIP = matchedRecord[0].CUSIP,
                        SEDOL = matchedRecord[0].SEDOL,
                    };

                    output.Add(new OutputFile(secondaryFile, matchedRecord[0].BASKETSHARES, matchedRecord[1].BASKETSHARES));
                }
            }
            return output;
        }
    }
}
