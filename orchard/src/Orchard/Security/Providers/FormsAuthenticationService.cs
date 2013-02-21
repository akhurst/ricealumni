using System;
using System.Web;
using System.Web.Security;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Services;

namespace Orchard.Security.Providers {
    public class FormsAuthenticationService : IAuthenticationService {
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUser _signedInUser;
        private bool _isAuthenticated = false;
        private readonly ShellSettings _shellSettings;

        public FormsAuthenticationService(IClock clock, IContentManager contentManager, IHttpContextAccessor httpContextAccessor, ShellSettings shellSettings)
        {
            _clock = clock;
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;
            _shellSettings = shellSettings;

            Logger = NullLogger.Instance;
            
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        public ILogger Logger { get; set; }

        public TimeSpan ExpirationTimeSpan { get; set; }

        public void SignIn(IUser user, bool createPersistentCookie) {
            var now = _clock.UtcNow.ToLocalTime();

            var userData = GetUserData(user);

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.UserName,
                now,
                now.Add(ExpirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie) {
                cookie.Expires = ticket.Expiration;
            }

            //var httpContext = _httpContextAccessor.Current();
            _httpContextAccessor.Current().Response.Cookies.Add(cookie);

            _isAuthenticated = true;
            _signedInUser = user;
        }

        private string SiteUrlPrefix
        {
            get { return _shellSettings.RequestUrlPrefix ?? "/"; }
        }

        public void SignOut() {
            _signedInUser = null;
            _isAuthenticated = false;
            FormsAuthentication.SignOut();
        }

        public void SetAuthenticatedUserForRequest(IUser user) {
            _signedInUser = user;
            _isAuthenticated = true;
        }

        public IUser GetAuthenticatedUser()
        {
            if (_signedInUser != null || _isAuthenticated)
                return _signedInUser;

            var httpContext = _httpContextAccessor.Current();
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity) httpContext.User.Identity;
            int userId;
            if (!ValidateUserData(formsIdentity.Ticket.UserData, out userId))
            {
                Logger.Fatal("User token rejected");
                return null;
            }
            _isAuthenticated = true;
            return _signedInUser = _contentManager.Get(userId).As<IUser>();
        }

        private string GetUserData(IUser user)
        {
            var userData = string.Format("{0}@{1}", Convert.ToString(user.Id), SiteUrlPrefix);
            var httpContext = _httpContextAccessor.Current();
            if (httpContext != null && httpContext.Request.IsAuthenticated && httpContext.User.Identity is FormsIdentity)
            {
                // if we got here, we have already been authenticated by some site

                var formsIdentity = (FormsIdentity) httpContext.User.Identity;
                var userSiteTokens = formsIdentity.Ticket.UserData.Split('&');
                foreach (var userSiteToken in userSiteTokens)
                {
                    var splitSiteToken = userSiteToken.Split('@');

                    // add this token to the cookie if it's valid and it's not for this site (the one for this site has already been added above)
                    if (splitSiteToken.Length == 2 && splitSiteToken[1] != SiteUrlPrefix) 
                    {
                        userData += "&" + userSiteToken;
                    }
                }
            }

            return userData;
        }

        private string GetSiteToken(string userData)
        {
            var userSiteTokens = userData.Split('&');

            foreach (var userSiteToken in userSiteTokens)
            {
                var splitSiteToken = userSiteToken.Split('@');

                if(splitSiteToken.Length == 2)
                {
                    if(splitSiteToken[1]==SiteUrlPrefix)
                        return userSiteToken;
                }
            }

            return null;
        }

        private bool ValidateUserData(string userData, out int userId)
        {
            var siteToken = GetSiteToken(userData);

            if(siteToken == null)
            {
                Logger.Error("User doesn't have a valid cookie for this site");
                userId = -1;
                return false;
            }

            var splitSiteToken = siteToken.Split('@');
            if (splitSiteToken.Length != 2)
            {
                Logger.Error("User cookie invalid");
                userId = -1;
                return false;
            }

            var userIdString = splitSiteToken[0];
            if (!int.TryParse(userIdString, out userId))
            {
                Logger.Fatal("User id not a parsable integer");
                return false;
            }

            if(splitSiteToken[1]==SiteUrlPrefix)
            {
                return true;
            }
            else
            {
                Logger.Error("User not authenticated against this site");
                return false;
            }
        }
    }
}
