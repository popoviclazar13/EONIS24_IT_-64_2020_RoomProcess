using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace RoomProcess.Helpers
{
    public class AuthRoleAttribute : TypeFilterAttribute
    {
            public AuthRoleAttribute(string claimType, string claimValue) : base(typeof(AuthorizeAction))
            {
                Arguments = new object[] { new Claim(claimType, claimValue) };
            }       
    }
}
