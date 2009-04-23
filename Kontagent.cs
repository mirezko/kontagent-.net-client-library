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
    /// Wrapper class for Kontagent API calls
    /// </summary>
    public class Kontagent
    {
        internal const string InviteSentMessage = "ins";
        internal const string InviteResponseMessage = "inr";
        internal const string PageRequestMessage = "pgr";
        internal const string AppAddedMessage = "apa";
        internal const string AppRemovedMessage = "apr";
        internal const string CommClickMessage = "ucc";
        internal const string FeedPostMessage = "fdp";
        internal const string UserInfoMessage = "cpu";

        public const string CommClick_Feed = "fdp";
        public const string CommClick_AppUserNotif = "ap";

        private string ApiKey { get; set; }
        private string Secret { get; set; }

        public Kontagent(string apiKey, string secret)
        {
            ApiKey = apiKey;
            Secret = secret;
        }

        internal void InviteSent(string sender, string[] recipients, string trackingId, string templateId)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"s", sender},
                {"u", trackingId},
                {"r", Util.MakeCommaSaperatedString<string>(recipients)}
            };
            if (!string.IsNullOrEmpty(templateId))
            {
                parameterDictionary.Add("t", templateId.ToString());
            }

            RequestHelper.SendRequest(InviteSentMessage, ApiKey, Secret, parameterDictionary);
        }

        public void InviteSent(string[] recipients)
        {
            InviteSent(
                HttpParams.FBUserId,
                recipients,
                HttpParams.SentTrackingId,
                HttpParams.TemplateId);
        }

        internal void InviteResponse(string recipient, bool hasAdded, string trackingId, string templateId)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"i", hasAdded ? "1" : "0"},
                {"u", trackingId},
                {"tu", "invite"}
            };
            if (!string.IsNullOrEmpty(recipient))
            {
                parameterDictionary.Add("r", recipient.ToString());
            }
            if (!string.IsNullOrEmpty(templateId))
            {
                parameterDictionary.Add("t", templateId);
            }

            RequestHelper.SendRequest(InviteResponseMessage, ApiKey, Secret, parameterDictionary);
        }

        public void InviteResponse()
        {
            string tid = HttpParams.RespTrackingId;
            if (!string.IsNullOrEmpty(tid))
            {
                InviteResponse(
                    HttpParams.FBUserId,
                    HttpParams.FBIsAdded,
                    tid,
                    HttpParams.TemplateId);
            }
        }

        internal void PageRequest(string user, string pageUrl, string ipAddr)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"u", pageUrl}
            };
            if (!string.IsNullOrEmpty(user))
            {
                parameterDictionary.Add("s", user.ToString());
            }
            if (!string.IsNullOrEmpty(ipAddr))
            {
                parameterDictionary.Add("ip", ipAddr.ToString());
            }

            RequestHelper.SendRequest(PageRequestMessage, ApiKey, Secret, parameterDictionary);
        }

        public void PageRequest(string pageUrl)
        {
            PageRequest(
                HttpParams.FBUserId,
                pageUrl,
                HttpParams.FBUserRemoteAddr);
        }

        internal void AppAdded(string user, string trackingId, string shortTrackingId)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"s", user}
            };
            if (!string.IsNullOrEmpty(trackingId))
            {
                parameterDictionary.Add("u", trackingId);
            }
            if (!string.IsNullOrEmpty(shortTrackingId))
            {
                parameterDictionary.Add("su", shortTrackingId);
            }

            RequestHelper.SendRequest(AppAddedMessage, ApiKey, Secret, parameterDictionary);
        }

        public void AppAdded()
        {
            if (HttpParams.FBIsInstalled)
            {
                AppAdded(
                    HttpParams.FBUserId,
                    HttpParams.RespTrackingId,
                    null);
            }
        }

        internal void AppRemoved(string user)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"s", user}
            };

            RequestHelper.SendRequest(AppRemovedMessage, ApiKey, Secret, parameterDictionary);
        }

        public void AppRemoved()
        {
            AppRemoved(HttpParams.FBUserId);
        }

        internal void CommClick(string user, string type, string subType1, string subType2, bool hasAdded, string shortTrackingId)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"tu", type},
                {"i", hasAdded ? "1" : "0"}
            };
            if (!string.IsNullOrEmpty(user))
            {
                parameterDictionary.Add("s", user.ToString());
            }
            if (!string.IsNullOrEmpty(subType1))
            {
                parameterDictionary.Add("st1", subType1);
            }
            if (!string.IsNullOrEmpty(subType2))
            {
                parameterDictionary.Add("st2", subType2);
            }
            if (!string.IsNullOrEmpty(shortTrackingId))
            {
                parameterDictionary.Add("su", shortTrackingId);
            }

            RequestHelper.SendRequest(CommClickMessage, ApiKey, Secret, parameterDictionary);
        }

        public void CommClick()
        {
            string type = HttpParams.KRefType;
            if (!string.IsNullOrEmpty(type))
            {
                CommClick(
                    HttpParams.FBUserId,
                    type,
                    HttpParams.KRefSubType1,
                    HttpParams.KRefSubType2,
                    HttpParams.FBIsInstalled,
                    null);
            }
        }

        internal void FeedPost(string user, string templateId, int postType, string subType1, string subType2)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"s", user},
                {"tu", "post"},
                {"pt", postType.ToString()}
            };
            if (!string.IsNullOrEmpty(templateId))
            {
                parameterDictionary.Add("t", templateId);
            }
            if (!string.IsNullOrEmpty(subType1))
            {
                parameterDictionary.Add("st1", subType1);
            }
            if (!string.IsNullOrEmpty(subType2))
            {
                parameterDictionary.Add("st2", subType2);
            }
            RequestHelper.SendRequest(FeedPostMessage, ApiKey, Secret, parameterDictionary);
        }

        public void FeedPost(int? templateId, string subType1, string subType2)
        {
            FeedPost(
                HttpParams.FBUserId,
                templateId == null ? null : templateId.ToString(),
                1, // all stories now
                subType1,
                subType2);
        }

        public void UserInfo(string user, int? birthYear, string gender, string city, string country,
            string state, string zip, int? friends)
        {
            if (string.IsNullOrEmpty(ApiKey))
                return;

            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>()
            {
                {"s", user}
            };
            if (birthYear != null)
            {
                parameterDictionary.Add("b", birthYear.ToString());
            }
            if (!string.IsNullOrEmpty(gender))
            {
                parameterDictionary.Add("g", gender.Substring(0, 1));
            }
            if (!string.IsNullOrEmpty(city))
            {
                parameterDictionary.Add("ly", city);
            }
            if (!string.IsNullOrEmpty(country))
            {
                parameterDictionary.Add("lc", country);
            }
            if (!string.IsNullOrEmpty(state))
            {
                parameterDictionary.Add("ls", state);
            }
            if (!string.IsNullOrEmpty(zip))
            {
                parameterDictionary.Add("lp", zip);
            }
            if (friends != null)
            {
                parameterDictionary.Add("f", friends.ToString());
            }
            RequestHelper.SendRequest(UserInfoMessage, ApiKey, Secret, parameterDictionary);
        }
    }
}
