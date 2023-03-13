using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DateConverter
{
    public DateConverter()
    {
    }
    public string ConversionDate_DDMMYYYY_To_MMDDYYYY(string date)
    {
        if (date.Length != 10)
        {
            return date;
        }
        string[] str = date.Split('/');
        var getdate = str[1] + '/' + str[0] + '/' + str[2];
        return getdate;
    }
    public string ConversionDate_MMDDYYYY_To_DDMMYYYY(string date)
    {
        if (date.Length != 10)
        {
            return date;
        }
        string[] str = date.Split('/');
        var getdate = str[1] + '/' + str[0] + '/' + str[2];
        return getdate;
    }
}
