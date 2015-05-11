using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using bcpp.Filters;
using bcpp.Models;

namespace bcpp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private dbEntities db = new dbEntities();
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Uživatelské jméno nebo heslo je nesprávné.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Main");
        }
        
        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new MyRegistrationModel());
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MyRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.regModel.UserName, model.regModel.Password);


                    if (!Roles.RoleExists("admin"))
                        Roles.CreateRole("admin");
                    if (!Roles.RoleExists("user"))
                        Roles.CreateRole("user");

                    if (model.regModel.UserName.Equals("admin") && !Roles.GetRolesForUser(model.regModel.UserName).Contains("admin"))
                        Roles.AddUserToRole(model.regModel.UserName, "admin");
                    else
                        Roles.AddUserToRole(model.regModel.UserName, "user");
                    
                    WebSecurity.Login(model.regModel.UserName, model.regModel.Password);
                    
                    db.AddToadresa(model.adModel);
                    db.SaveChanges();
                    model.uzModel.uzivatel_id = WebSecurity.GetUserId(model.regModel.UserName);
                    model.uzModel.adresa_id = model.adModel.adresa_id;
                    db.AddTouzivatel(model.uzModel);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Main");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Vaše heslo bylo změněno."
                : message == ManageMessageId.SetPasswordSuccess ? "Vaše heslo bylo nastaveno."
                : message == ManageMessageId.RemoveLoginSuccess ? "Externí přihlášení bylo odstraněno."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Aktuální heslo je nesprávné nebo nové heslo je neplatné.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Akcie");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Uživatelské jméno již existuje. Zadejte prosím jiné uživatelské jméno.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Uživatelské jméno pro tuto e-mailovou adresu již existuje. Zadejte prosím jinou e-mailovou adresu.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Heslo je neplatné. Prosím, zadejte platnou hodnotu hesla.";

                case MembershipCreateStatus.InvalidEmail:
                    return "E-mailová adresa je neplatná. Prosím, zkontrolujte hodnotu a zkuste to znovu.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Heslo je neplatné. Prosím, zkontrolujte hodnotu a zkuste to znovu.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Heslo je neplatné. Prosím, zkontrolujte hodnotu a zkuste to znovu.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Uživatelské jméno je neplatné. Prosím, zkontrolujte hodnotu a zkuste to znovu.";

                case MembershipCreateStatus.ProviderError:
                    return "Zprostředkovatel ověření vrátil chybu. Prosím zkontrolujte zadání a zkuste to znovu. Pokud problém přetrvává, obraťte se na správce systému.";

                case MembershipCreateStatus.UserRejected:
                    return "Požadavek vytvoření uživatele byl zrušen. Prosím zkontrolujte zadání a zkuste to znovu. Pokud problém přetrvává, obraťte se na správce systému.";

                default:
                    return "Došlo k neznámé chybě. Prosím zkontrolujte zadání a zkuste to znovu. Pokud problém přetrvává, obraťte se na správce systému.";
            }
        }
        #endregion
    }
}
