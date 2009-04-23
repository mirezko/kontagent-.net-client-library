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
using System.Web;
using System.Net;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;

namespace KontagentAPI
{
    /// <summary>
    /// Helper class for signing & sending Http Requests to Kontagent server asynchronously
    /// </summary>
    internal class RequestHelper
    {
        internal static void SendRequest(string messageType, string apiKey, string secret, IDictionary<string, string> parameterDictionary)
        {
            string requestUrl = ConfigurationManager.AppSettings["KontagentTestMode"] == "1" ? 
                Resources.KontagentTestURL : Resources.KontagentURL;

            requestUrl = requestUrl.Replace("{messagetype}", messageType);
            requestUrl = requestUrl.Replace("{apikey}", apiKey);

            string parameters = CreateHTTPParameterList(parameterDictionary, secret);

            DoRequest(requestUrl, parameters);
        }

        internal static string CreateHTTPParameterList(IDictionary<string, string> parameterList, string secret)
        {
            var queryBuilder = new StringBuilder();

            parameterList.Add("ts", DateTime.UtcNow.Ticks.ToString("x", CultureInfo.InvariantCulture));
            parameterList.Add("an_sig", GenerateSignature(parameterList, secret));

            // Build the query
            foreach (var kvp in parameterList)
            {
                queryBuilder.Append(kvp.Key);
                queryBuilder.Append("=");
                queryBuilder.Append(HttpUtility.UrlEncode(kvp.Value));
                queryBuilder.Append("&");
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);

            return queryBuilder.ToString();
        }

        internal static string GenerateSignature(IDictionary<string, string> parameters, string secret)
        {
            var signatureBuilder = new StringBuilder();

            // Sort the keys of the method call in alphabetical order
            var keyList = ParameterDictionaryToList(parameters);
            keyList.Sort();

            // Append all the parameters to the signature input paramaters
            foreach (string key in keyList)
                signatureBuilder.Append(String.Format(CultureInfo.InvariantCulture, "{0}={1}", key, parameters[key]));

            // Append the secret to the signature builder
            signatureBuilder.Append(secret);

            var md5 = MD5.Create();
            // Compute the MD5 hash of the signature builder
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureBuilder.ToString().Trim()));

            // Reinitialize the signature builder to store the actual signature
            signatureBuilder = new StringBuilder();

            // Append the hash to the signature
            foreach (var hashByte in hash)
                signatureBuilder.Append(hashByte.ToString("x2", CultureInfo.InvariantCulture));

            return signatureBuilder.ToString();
        }

        internal static List<string> ParameterDictionaryToList(IEnumerable<KeyValuePair<string, string>> parameterDictionary)
        {
            var parameters = new List<string>();

            foreach (var kvp in parameterDictionary)
            {
                parameters.Add(String.Format(CultureInfo.InvariantCulture, "{0}", kvp.Key));
            }
            return parameters;
        }

        internal static void DoRequest(string requestUrl, string paremeters)
        {
            UriBuilder uri = new UriBuilder(requestUrl);
            if (!string.IsNullOrEmpty(paremeters))
            {
                uri.Query += paremeters;
            }

            var webRequest = WebRequest.Create(uri.Uri);
            webRequest.Method = "GET";
            webRequest.BeginGetResponse(null, null);

            // don't need to process the response
            //HttpWebResponse resp = webRequest.GetResponse() as HttpWebResponse;
        }
    }
}
