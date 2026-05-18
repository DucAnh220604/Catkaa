using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Catkaa.MicroPms.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? "";

        protected int? CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdClaim, out int userId))
                    return userId;
                return null;
            }
        }
    }
}
