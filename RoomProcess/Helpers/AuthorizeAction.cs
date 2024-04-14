using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace RoomProcess.Helpers
{
    public class AuthorizeAction : IAuthorizationFilter
    {
        readonly Claim _claim;
        private readonly IAuthHelper _auth;

        public AuthorizeAction(Claim claim, IAuthHelper auth)
        {
            _claim = claim;
            _auth = auth;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                if (_auth.ValidateCurrentToken(token))
                {
                    var claimValue = _auth.GetClaim(token, _claim.Type);
                    if (_claim.Type == "Role")
                    {
                        var roleArray = _claim.Value.Split(',');
                        foreach (var role in roleArray)
                        {
                            if (role == claimValue)
                                return;
                        }
                    }
                    else
                    {
                        var KorisnikID = context.HttpContext.Request.RouteValues["id"];
                        if (KorisnikID.Equals(claimValue))
                            return;
                    }
                }
            }
            context.Result = new JsonResult(new { message = "Unauthorized." }) { StatusCode = StatusCodes.Status403Forbidden };
            return;
        }
    }
}
