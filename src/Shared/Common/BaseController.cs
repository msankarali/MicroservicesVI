using Microsoft.AspNetCore.Mvc;
using Utilities.Results;

namespace Common
{
    public class BaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(ApiResponse<T> apiResponse)
        {
            return new ObjectResult(apiResponse)
            {
                StatusCode = apiResponse.StatusCode
            };
        }
    }
}