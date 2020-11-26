using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Reboost.Shared;
using System.Threading.Tasks;

namespace Reboost.WebApi
{
    public static class GlobalExceptionHandleExtension
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {                    
                    context.Response.ContentType = "application/json";
                    var errorMessage = "";
                    
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is AppException)
                        {
                            var ex = contextFeature.Error as AppException;
                            context.Response.StatusCode = (int)ex.Code;
                            errorMessage = ex.Message;
                        }
                        else
                        {
                            context.Response.StatusCode = 500;
                            errorMessage = contextFeature.Error.Message;
                        }
                        
                       await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = errorMessage }));
                    }
                });
            });
        }
    }
    //public class ErrorHandlingMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public ErrorHandlingMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        try
    //        {
    //            await _next(context);
    //        }
    //        catch (System.Exception ex)
    //        {
    //            await HandleExceptionAsync(context, ex);
    //        }
    //    }

    //    private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    //    {
    //        var errorRes = new ResponseBase<ResponseDataBase<object>>();
    //        errorRes.Error.Code = 400;
    //        errorRes.Error.MessageRaw = "Invalid argument";
    //        errorRes.Error.AddErrorItem(exception);
            
    //        context.Response.ContentType = "application/json";
    //        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorRes));
    //    }
    //}
}
