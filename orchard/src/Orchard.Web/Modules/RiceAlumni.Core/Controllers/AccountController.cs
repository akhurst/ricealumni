using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.MultiTenancy.Services;
using Orchard.Security;

namespace RiceAlumni.Core.Controllers
{
    public class AccountController : Controller
    {
        private ITenantService tenantService;

        public AccountController(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        [AlwaysAccessible]
        public ActionResult SiteAccessDenied()
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            var siteRelativePath = HttpRuntime.AppDomainAppVirtualPath;
            var returnUrlWithoutAppPath = returnUrl.Substring(returnUrl.IndexOf(siteRelativePath) + siteRelativePath.Length).Trim('/');
            var firstSiteToken = !returnUrlWithoutAppPath.Contains("/") ? returnUrlWithoutAppPath : returnUrlWithoutAppPath.Substring(0, returnUrlWithoutAppPath.IndexOf('/'));
            var tenants = tenantService.GetTenants();

            string tenantUrl = null;

            foreach (var tenant in tenants)
            {
                if (tenant.RequestUrlPrefix!= null && (tenant.RequestUrlPrefix.ToLowerInvariant() == firstSiteToken.ToLowerInvariant()))
                    tenantUrl = firstSiteToken;
            }

            if (tenantUrl != null)
            {
                return Redirect(string.Format(
                    "~/{0}/Users/Account/AccessDenied?ReturnUrl={1}",
                    tenantUrl,
                    returnUrl
                    ));
            }
            else
            {
                return Redirect("~/Users/Account/AccessDenied?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl));
            }
        }

    }
}