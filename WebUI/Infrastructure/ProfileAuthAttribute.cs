namespace WebUI.Infrastructure
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Security.Principal;
    using System.Web.Mvc.Filters;
    using System.Web.Routing;
    using Ninject;
    using Domain;

    public class ProfileAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        [Inject]
        private IRepository repository { get; set; }

        public ProfileAuthAttribute()
        {
            repository = DependencyResolver.Current.GetService<IRepository>();
        }

        public void OnAuthentication(AuthenticationContext context)
        {
            IIdentity ident = context.Principal.Identity;
            if (ident.IsAuthenticated)
            {
                if (repository.Profiles.FirstOrDefault(p => p.ProfileId.ToString() == ident.Name.Split('|')[1]) == null)
                {
                    context.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Profile"},
                    {"action",  "Create"},
                    {"returnUrl", context.HttpContext.Request.RawUrl}
                });
            }
            //else
            //    FormsAuthentication.SignOut();
        }


    }
}