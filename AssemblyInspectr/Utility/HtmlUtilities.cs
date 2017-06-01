#region Copyright (c) 2017 G. Gagnaux, https://github.com/ggagnaux/AssemblyInspectr
/*
AssemblyInspectr - A Winforms based application to display .NET assembly metadata

Copyright (c) 2017 G. Gagnaux, https://github.com/ggagnaux/AssemblyInspectr

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
using System;
using System.Text;

namespace AssemblyInspectr.Utility
{
    public static class HtmlUtilities
    {
        public static string GenerateTableRow(string itemName, string itemValue, string _class = null)
        {
            var col1 = string.IsNullOrEmpty(itemName) ? "&nbsp;" : itemName;
            var col2 = string.IsNullOrEmpty(itemValue) ? "&nbsp;" : itemValue;

            var sb = new StringBuilder();
            sb.AppendLine(HtmlRowStart(_class));
            sb.AppendLine(HtmlCellStart());
            sb.AppendLine(col1);
            sb.AppendLine(HtmlCellEnd());
            sb.AppendLine(HtmlCellStart());
            sb.AppendLine(col2);
            sb.AppendLine(HtmlCellEnd());
            sb.AppendLine(HtmlRowEnd());
            return sb.ToString();
        }

        public static string GenerateTableRowWithPadding(string itemName, string itemValue, string _class = null)
        {
            var col1 = string.IsNullOrEmpty(itemName) ? "&nbsp;" : itemName;
            var col2 = string.IsNullOrEmpty(itemValue) ? "&nbsp;" : itemValue;

            var padding = "&nbsp;&nbsp;&nbsp;";

            var sb = new StringBuilder();
            sb.AppendLine(HtmlRowStart(_class));
            sb.AppendLine(HtmlCellStart());
            sb.AppendLine(padding + col1);
            sb.AppendLine(HtmlCellEnd());
            sb.AppendLine(HtmlCellStart());
            sb.AppendLine(padding + col2);
            sb.AppendLine(HtmlCellEnd());
            sb.AppendLine(HtmlRowEnd());
            return sb.ToString();
        }


        public static string HtmlStart() => $"<html lang=\"en\">";
        public static string HtmlEnd() => $"</html>";
        public static string HtmlHeadStart() => HtmlTag("head");
        public static string HtmlHeadEnd() => $"</head>";
        public static string HtmlBodyStart(string _class = null) => HtmlTag("body", _class);
        public static string HtmlBodyEnd() => $"</body>";
        public static string HtmlTableStart(string _class = null) => HtmlTag("table", _class);
        public static string HtmlTableEnd(string _class = null) => $"</table>";
        public static string HtmlRowStart(string _class = null) => HtmlTag("tr", _class);
        public static string HtmlRowEnd() => $"</tr>";
        public static string HtmlCellStart(string _class = null) => HtmlTag("td", _class);
        public static string HtmlCellEnd() => $"</td>";

        public static string HtmlTag(string tagName, string _class = null)
        {
            if (String.IsNullOrEmpty(tagName))
                throw new ArgumentNullException();

            var classDesignator = string.Empty;
            if (!string.IsNullOrEmpty(_class))
            {
                classDesignator = $" class=\"{_class}\"";
            }
            return $"<{tagName}{classDesignator}>";
        }
    }
}
