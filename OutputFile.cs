namespace CsvComparator.Models
{
    public class OutputFile
    {
        public string COMPARE { get; set; }
        public string FUNDCODE { get; set; }
        public string ISIN { get; set; }
        public string CUSIP { get; set; }
        public string SEDOL { get; set; }
        public int? BASKETSHARES { get; set; }
        public string CURRENCYCODE { get; set; }
        public string CIL { get; set; }
        public string TRADECOUNTRY { get; set; }

        public int? PLFShares { get; set; }
        public int? PCFShares { get; set; }

        public OutputFile(PrimaryFile pf, string compareResult)
        {
            this.FUNDCODE = pf.FUNDCODE;
            this.ISIN = pf.ISIN;
            this.CUSIP = pf.CUSIP;
            this.SEDOL = pf.SEDOL;
            this.BASKETSHARES = pf.BASKETSHARES;
            this.CURRENCYCODE = pf.CURRENCYCODE;
            this.CIL = pf.CIL;
            this.TRADECOUNTRY = pf.TRADECOUNTRY;
            this.COMPARE = compareResult;
        }

        public OutputFile(SecondaryFile sf, string compareResult)
        {
            this.FUNDCODE = sf.FUNDCODE;
            this.ISIN = sf.ISIN;
            this.CUSIP = sf.CUSIP;
            this.SEDOL = sf.SEDOL;
            this.BASKETSHARES = sf.SHARES;
            this.CURRENCYCODE = sf.CUR;
            this.CIL = sf.CIL;
            this.TRADECOUNTRY = sf.ISO;
            this.COMPARE = compareResult;
        }

        public OutputFile(SecondaryFile sf, int? plfShares, int? pcfShares )
        {
            this.FUNDCODE = sf.FUNDCODE;
            this.ISIN = sf.ISIN;
            this.CUSIP = sf.CUSIP;
            this.SEDOL = sf.SEDOL;
            this.PCFShares = pcfShares;
            this.PLFShares = plfShares;
        }
    }
}
