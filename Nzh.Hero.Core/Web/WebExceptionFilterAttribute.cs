﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.Common.NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Hero.Core.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class WebExceptionFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            LogNHelper.Exception(ex);
            var result = new { statusCode = 300, msg = "服务内部异常,请联系管理员" };
            context.Result = new ContentResult
            {
                Content = result.ToJson(),
                StatusCode = StatusCodes.Status200OK,
                ContentType = "text/html;charset=utf-8"
            };
            context.ExceptionHandled = true;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
    public class WebExceptionFilter : IExceptionFilter, IAsyncExceptionFilter, IFilterMetadata
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                LogNHelper.Exception(context.Exception);
                var result = new { statusCode = 300, msg = "服务内部异常,请联系管理员" };
                context.Result = new ContentResult
                {
                    Content = result.ToJson(),
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "text/html;charset=utf-8"
                };
            }
            context.ExceptionHandled = true; 
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
