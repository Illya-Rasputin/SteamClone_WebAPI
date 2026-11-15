using Microsoft.AspNetCore.Mvc;
using BLL.Services;

namespace API.Extensions
{
    public static class ControllerBaseExtension
    {
        public static IActionResult GetResult(this ControllerBase controller, ServiceResponse response)
        {
            return response.IsSuccess
                ? controller.Ok(response)
                : controller.BadRequest(response);
        }
    }
}
