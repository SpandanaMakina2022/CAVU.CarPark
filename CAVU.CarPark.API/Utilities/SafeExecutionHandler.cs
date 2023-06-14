using System.Net;
using CAVU.CarPark.Abstraction.Service;
using Microsoft.AspNetCore.Mvc;

namespace CAVU.CarPark.API.Utilities
{
    public static class SafeExecutionHandler
    {
        public static async Task<ActionResult<ReturnType>> ExecuteAsync<ReturnType>(
            this ControllerBase controller,
            ILogger logger,
            IBookingService bookingService,
            Func<IBookingService, Task<ReturnType>> runnable)
        {
            try
            {
                return controller.Ok(await runnable(bookingService));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return controller.StatusCode((int)HttpStatusCode.BadRequest, new { correlationId = controller.HttpContext.TraceIdentifier, created = DateTime.UtcNow, Error = ex.Message });
            }
        }
    }
}
