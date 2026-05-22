using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Exceptions;

namespace Employee_Management_System.Presentations.Middleware;
public class InternalExceptionLoggingMiddleware
{
    private readonly RequestDelegate   _next;

    private readonly ILogger<InternalExceptionLoggingMiddleware> _logger;

    public InternalExceptionLoggingMiddleware(RequestDelegate next, ILogger<InternalExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // 次のミドルウェアまたはControllerへ処理を渡す
            await _next(context);
        }
        catch (InternalException ex)
        {
            // エラーログを出力する
            _logger.LogError(ex, "InternalExceptionが発生しました");

            // レスポンスが未送信の場合のみ処理
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                // システム停止中画面へ遷移
                context.Response.Redirect("/System/Maintenance");
            }
        }
    }
}