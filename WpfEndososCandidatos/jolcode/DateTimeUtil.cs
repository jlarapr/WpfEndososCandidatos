using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jolcode
{
    public enum DateInterval
    {
        Year,
        Month,
        Weekday,
        Day,
        Hour,
        Minute,
        Second
    }
  public  class DateTimeUtil
    {

        public static long DateDiff(DateInterval interval, DateTime? date1, DateTime? date2)
        {
            TimeSpan? ts = null;

            if (date1 != null && date2 != null)
                ts = date2 - date1;
            else
                return 999999; 

            
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Value.Year - date1.Value.Year;
                case DateInterval.Month:
                    return (date2.Value.Month - date1.Value.Month) + (12 * (date2.Value.Year - date1.Value.Year));
                case DateInterval.Weekday:
                    return Fix(ts.Value.TotalDays) / 7;
                case DateInterval.Day:
                    return Fix(ts.Value.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.Value.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.Value.TotalMinutes);
                default:
                    return Fix(ts.Value.TotalSeconds);
            }
        }
        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        public static DateTime? MyValidarFecha(string param)
        {
            DateTime tempdate;
            //Xceed.Wpf.Toolkit.MaskedTextBox myMaskedTextBoxValue = new Xceed.Wpf.Toolkit.MaskedTextBox();
            //myMaskedTextBoxValue.Value = "00/00/0000";
            //myMaskedTextBoxValue.ValueDataType = typeof(DateTime);
            //myMaskedTextBoxValue.Text = param;

            param = param.Replace("/","");

            if (DateTime.TryParseExact(param,"MMddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out tempdate))
                return tempdate;//.ToString("MM/dd/yyyy");
            else
                return null;

        }

        public static DateTime? MyValidarFechaMMddyy(string param)
        {
            DateTime tempdate;
            //Xceed.Wpf.Toolkit.MaskedTextBox myMaskedTextBoxValue = new Xceed.Wpf.Toolkit.MaskedTextBox();
            //myMaskedTextBoxValue.Value = "00/00/0000";
            //myMaskedTextBoxValue.ValueDataType = typeof(DateTime);
            //myMaskedTextBoxValue.Text = param;

            param = param.Replace("/", "");

            if (DateTime.TryParseExact(param, "MMddyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate))
                return tempdate;//.ToString("MM/dd/yyyy");
            else
                return null;

        }

        public static string MyValidarFecha2(string param)
        {
            DateTime tempdate;
            //Xceed.Wpf.Toolkit.MaskedTextBox myMaskedTextBoxValue = new Xceed.Wpf.Toolkit.MaskedTextBox();
            //myMaskedTextBoxValue.Value = "00/00/0000";
            //myMaskedTextBoxValue.ValueDataType = typeof(DateTime);
            //myMaskedTextBoxValue.Text = param;

          param = param.Replace("/","");

            if (DateTime.TryParseExact(param,"MMddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out tempdate))
                return tempdate.ToString("MM/dd/yyyy");
            else if(DateTime.TryParseExact(param,"MMdyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,out tempdate))
                return tempdate.ToString("MM/dd/yyyy");
            else if (DateTime.TryParseExact(param, "Mdyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate))
                return tempdate.ToString("MM/dd/yyyy");
            else if (DateTime.TryParseExact(param, "Mddyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate))
                return tempdate.ToString("MM/dd/yyyy");
            else if (DateTime.TryParseExact(param, "Mdyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate))
                return tempdate.ToString("MM/dd/yyyy");
            else
                return null;

        }

    }
}
