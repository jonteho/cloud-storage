using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CloudPhotoApp.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            if (!Request.IsAuthenticated)
            {
                var signInRequest = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("", "",
                    false);
                return Redirect(signInRequest.RequestUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            // Ta bort kakan och sessionen
            if (FederatedAuthentication.SessionAuthenticationModule != null)
            {
                FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Delete();
                FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(true);
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Index", "Home");
        }
	}
}