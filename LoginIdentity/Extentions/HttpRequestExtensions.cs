using Microsoft.AspNetCore.Http;
using System;

namespace Tattys.Site.Extensions
{
	public static class HttpRequestExtensions
	{
		private const string REQUESTED_WITH_HEADER = "X-Requested-With";
		private const string XML_HTTP_REQUEST = "XMLHttpRequest";

		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}

			if (request.Headers != null)
			{
				return request.Headers[REQUESTED_WITH_HEADER] == XML_HTTP_REQUEST;
			}

			return false;
		}
	}
}
