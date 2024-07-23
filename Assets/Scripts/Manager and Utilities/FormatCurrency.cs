using System.Linq;

public static class FormatCurrency
{
    public static string Format(float value)
    {
        var x1 = 3;
        var newstr = "";
        var count = 0;

        var p = value.ToString().Split('.');
        var cF = p[0];
        char[] ch = new char[cF.Length];
        for (int i = 0; i < cF.Length; i++)
        {
            count++;
            if (count % x1 == 1 && count != 1)
            {
                newstr = newstr + ',' + cF[i];
            }
            else
            {
                newstr = newstr + cF[i];
            }
        }

        return newstr;
    }

    public static string KFormat(float value)
    {
        var newstr = "";
        if (value < 1000)
        {
            newstr = value.ToString();
        }
        else if (value >= 1000 && value < 1000000)
        {
            var newvalue = (value / 1000).ToString("N2");

            if (newvalue.Contains('.'))
            {
                var p = newvalue.Split('.');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "K";
            }
            else
            {
                var p = newvalue.Split(',');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "K";
            }
        }
        else if (value >= 1000000 && value < 1000000000)
        {
            var newvalue = (value / 1000000).ToString("N2");
            if (newvalue.Contains('.'))
            {
                var p = newvalue.Split('.');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "M";
            }
            else
            {
                var p = newvalue.Split(',');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "M";
            }
        }
        else
        {
            var newvalue = (value / 1000000000).ToString("N2");
            if (newvalue.Contains('.'))
            {
                var p = newvalue.Split('.');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "B";
            }
            else
            {
                var p = newvalue.Split(',');
                var cF = p[0];
                newstr = Format(float.Parse(cF)) + "," + p[1] + "B";
            }
        }

        return newstr;
    }
}
