using Microsoft.AspNetCore.Mvc;
using PKiK.Server.Services;
using PKiK.Shared;

namespace PKiK.Server.API {
    public static class Result {
        public static IActionResult Pass(IActionResult result, string controller, string method) {
            if(result.GetType() == typeof(ObjectResult)) {
                return PassObjectResult((ObjectResult)result, controller, method);
            }
            if (result.GetType() == typeof(StatusCodeResult)) {
                return PassStatusCodeResult((StatusCodeResult)result, controller, method);
            }
            return result;
        }

        private static IActionResult PassObjectResult(ObjectResult result, string controller, string method) {
            if (!ResponseValidator.IsSuccess(result.StatusCode ?? 200)) {
                string message = controller + ", " + method;
                if (result.StatusCode.HasValue) {
                    message += ": " + result.StatusCode.Value.ToString();
                }
                if(result.Value != null) {
                    message += ": " + result.Value.ToString();
                }
                Log.Event(message);
            }
            return result;
        }

        private static IActionResult PassStatusCodeResult(StatusCodeResult result, string controller, string method) {
            if(!ResponseValidator.IsSuccess((int)result.StatusCode)) {
                string message = controller + ", " + method + ": " + result.StatusCode.ToString();
                Log.Event(message);
            }
            return result;
        }
    }
}
