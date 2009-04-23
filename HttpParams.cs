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

namespace KontagentAPI
{
    /// <summary>
    /// Helper class for Http Parameters
    /// </summary>
    internal class HttpParams
    {
        public static string AppendQueryParam(string url, string paramNameValue)
        {
            if (url.Contains("?"))
            {
                url += "&" + paramNameValue;
            }
            else
            {
                url += "?" + paramNameValue;
            }
            return url;
        }

        public static string FBUserRemoteAddr
        {
            get
            {
                return HttpContext.Current.Request["HTTP_X_FB_USER_REMOTE_ADDR"];
            }
        }

        public static string FBUserId
        {
            get
            {
                string fbUserId = string.Empty;
                fbUserId = HttpContext.Current.Items["fb_sig_user"] as string;
                if (string.IsNullOrEmpty(fbUserId))
                {
                    fbUserId = HttpContext.Current.Request["fb_sig_page_id"];
                    if (string.IsNullOrEmpty(fbUserId))
                    {
                        fbUserId = HttpContext.Current.Request["fb_sig_user"];
                        if (string.IsNullOrEmpty(fbUserId))
                        {
                            if (FbInProfileTab)
                            {
                                fbUserId = HttpContext.Current.Request["fb_sig_profile_user"];
                            }
                            else
                            {
                                fbUserId = HttpContext.Current.Request["fb_sig_canvas_user"];
                            }
                        }
                    }
                }
                return fbUserId;
            }
        }

        public static string FBFriends
        {
            get
            {
                string friends = HttpContext.Current.Request["fb_sig_friends"];
                return friends == null ? string.Empty : friends;
            }
        }

        public static bool FbInProfileTab
        {
            get
            {
                return HttpContext.Current.Request["fb_sig_in_profile_tab"] == "1";
            }
        }

        public static bool FBIsAdded
        {
            get
            {
                return HttpContext.Current.Request["fb_sig_added"] == "1";
            }
        }

        public static bool FBIsInstalled
        {
            get
            {
                return HttpContext.Current.Request["installed"] == "1";
            }
        }

        public static string RespTrackingId
        {
            get
            {
                return HttpContext.Current.Request["resp_tid"];
            }
        }

        public static string SentTrackingId
        {
            get
            {
                return HttpContext.Current.Request["sent_tid"];
            }
        }

        public static string TemplateId
        {
            get
            {
                return HttpContext.Current.Request["tlid"];
            }
        }

        public static string KRefType
        {
            get
            {
                return HttpContext.Current.Request["kref"];
            }
        }

        public static string KRefSubType1
        {
            get
            {
                return HttpContext.Current.Request["krefst1"];
            }
        }

        public static string KRefSubType2
        {
            get
            {
                return HttpContext.Current.Request["krefst2"];
            }
        }
    }
}
