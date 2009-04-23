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
    /// Represents an object for a Http Request. To create it, call
    /// KontagentRequestInstance.Init in your Global.asax Application_BeginRequest.
    /// The init must be done before any other Kontagent functions can be called.
    /// </summary>
    public class KontagentRequestInstance
    {
        /// <summary>
        /// The current Http Request Kontagent Instance
        /// </summary>
        public static Kontagent Instance
        {
            get
            {
                return HttpContext.Current.Items["KontagentRequestInstance"] as Kontagent;
            }
        }

        /// <summary>
        /// Call this Init in Global.asax Application_Begin Request.
        /// The init must be done before any other Kontagent functions can be called.
        /// </summary>
        /// <param name="apiKey">Pass in your Kontagent apiKey</param>
        /// <param name="secret">Pass in your Kontagent secretKey</param>
        public static void Init(string apiKey, string secret)
        {
            Kontagent instance = new Kontagent(apiKey, secret);
            HttpContext.Current.Items["KontagentRequestInstance"] = instance;
        }

        /// <summary>
        /// Setup the url's for Invite Tracking. This function must be called to setup
        /// the Invite's fb:request-form action Url and also the Url's associated with
        /// Invite accept - fb:req-choice. 
        /// </summary>
        /// <param name="url">The original Url</param>
        /// <param name="templateId">Optional templateId</param>
        /// <param name="forAccept">Set this to true for fb:req-choice Urls, else false</param>
        /// <returns>The modified Url</returns>
        public static string MungeInviteUrl(string url, int? templateId, bool forAccept)
        {
            string id = HttpContext.Current.Items["KontagentInviteGuid"] as string;
            if (string.IsNullOrEmpty(id))
            {
                id = Util.GuidToString(Guid.NewGuid());
                HttpContext.Current.Items["KontagentInviteGuid"] = id;
            }

            if (forAccept)
            {
                url = HttpParams.AppendQueryParam(url, "resp_tid=" + id);
            }
            else
            {
                url = HttpParams.AppendQueryParam(url, "sent_tid=" + id);
            }

            if (templateId != null)
            {
                url = HttpParams.AppendQueryParam(url, "tlid=" + templateId);
            }

            return url;
        }

        /// <summary>
        /// Setup the url's for other forms of asynch communication, like feed forms.
        /// </summary>
        /// <param name="url">The original Url</param>
        /// <param name="commType">Can be Kontagent.CommClick_Feed only for now</param>
        /// <param name="subType1">Optional</param>
        /// <param name="subType2">Optinal</param>
        /// <returns>The modified Url</returns>
        public static string MungeCommUrl(string url, string commType, string subType1, string subType2)
        {
            url = HttpParams.AppendQueryParam(url, "kref=" + commType);
            if (!string.IsNullOrEmpty(subType1))
            {
                url = HttpParams.AppendQueryParam(url, "krefst1=" + subType1);
            }
            if (!string.IsNullOrEmpty(subType2))
            {
                url = HttpParams.AppendQueryParam(url, "krefst2=" + subType2);
            }
            return url;
        }

        /// <summary>
        /// Call this function on each of your page load to enable Kontagent tracking. Ideally have all your
        /// pages use a Master Page, and put this in Master Page Page_Load event.
        /// </summary>
        /// <param name="pageUrl">The current page Url</param>
        public static void OnPageLoad(string pageUrl)
        {
            Kontagent instance = Instance;
            instance.PageRequest(pageUrl);
            instance.InviteResponse();
            instance.AppAdded();
            instance.CommClick();
        }

        /// <summary>
        /// Call this from the remove callback of your app
        /// </summary>
        public static void AppRemoved()
        {
            Kontagent instance = Instance;
            instance.AppRemoved();
        }

        /// <summary>
        /// Call this after invites are sent.
        /// </summary>
        /// <param name="recipients">Array of invited recipients (fb userIds)</param>
        public static void InviteSent(string[] recipients)
        {
            Kontagent instance = Instance;
            instance.InviteSent(recipients);
        }

        /// <summary>
        /// Call this function to record/update user's demographic information. 
        /// Can be called when user installs the app. 
        /// </summary>
        /// <param name="birthYear">birthyear</param>
        /// <param name="gender">gender</param>
        /// <param name="city">city</param>
        /// <param name="country">country</param>
        /// <param name="state">state</param>
        /// <param name="zip">zip</param>
        public static void RecordUserInfo(int? birthYear, string gender, string city, string country,
            string state, string zip)
        {
            Kontagent instance = Instance;
            instance.UserInfo(HttpParams.FBUserId, birthYear, gender, city, country, state, zip, HttpParams.FBFriends.Split(',').Length);
        }
    }
}
