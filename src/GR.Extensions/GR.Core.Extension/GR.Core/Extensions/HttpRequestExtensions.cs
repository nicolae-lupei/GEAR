using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GR.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        /// <summary>
        /// Check if is ajax request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
		public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;

            return false;
        }

        /// <summary>
        /// Get app base url
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static string GetAppBaseUrl(this IHttpContextAccessor accessor)
        {
            var request = accessor?.HttpContext?.Request;
            return $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
        }

        /// <summary>
        /// Get app url
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetAppBaseUrl(this HttpContext context)
        {
            var request = context?.Request;
            return $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
        }


        /// <summary>
        /// Delete cookies
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static void DeleteCookies(this HttpContext ctx)
        {
            foreach (var cookie in ctx.Request.Cookies.Keys)
            {
                ctx.Response.Cookies.Delete(cookie);
            }
        }

        /// <summary>
        /// Is local ip address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsLocalIpAddress(this IPAddress ipAddress)
        {
            try
            {
                // get local IP addresses
                var localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                // is localhost
                if (IPAddress.IsLoopback(ipAddress)) return true;
                // is local address
                if (localIPs.Contains(ipAddress)) return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }

        /// <summary>
        /// Create a link action
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controller"></param>
        /// <param name="values"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string ActionLink(this IUrlHelper urlHelper, string actionName, string controller, object values, HttpContext context = null)
        {
            var httpContext = context ?? urlHelper.ActionContext.HttpContext;
            var host = httpContext.Request.Host.ToUriComponent();

            return urlHelper.Action(new UrlActionContext
            {
                Action = actionName,
                Controller = controller,
                Protocol = httpContext.Request.Scheme,
                Host = host,
                Values = values
            });
        }
    }
}