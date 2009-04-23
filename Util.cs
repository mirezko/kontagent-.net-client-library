//===================================================================================
// Kontagent API .Net Client Library
// Written by 101 Apps (vikas@101apps.com)
//
// This code is distributed under The Code Project Open License (COPL). This license 
// be found here: http://www.codeproject.com/info/cpol10.aspx. 
// No claim of suitability, guarantee, or any warranty whatsoever is provided. 
// The software is provided "as-is".
//===================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KontagentAPI
{
    /// <summary>
    /// Some String utility functions
    /// </summary>
    internal class Util
    {
        internal static string GuidToString(Guid guid)
        {
            return guid.ToString().Replace("-", "");
        }

        internal static string MakeCommaSaperatedString<type>(type[] list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (type o in list)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(o.ToString());
            }

            return sb.ToString();
        }

        internal static string AppendCommaSeperatedString<type>(string str, type[] list)
        {
            if (str.Length > 0 && !str.EndsWith(","))
            {
                str += ",";
            }
            str += MakeCommaSaperatedString(list);
            return str;
        }
    }
}
