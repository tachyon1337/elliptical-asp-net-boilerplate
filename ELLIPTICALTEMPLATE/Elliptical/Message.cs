using System.Net;

namespace Elliptical.Mvc
{
    public class Message<T>
    {
        public Message()
        {
            Status = true;
            StatusCode = HttpStatusCode.OK;
            ReasonPhrase = "";
            Content = "";
        }

        public Message(bool status, HttpStatusCode statusCode)
        {
            Status = status;
            StatusCode = statusCode;
            ReasonPhrase = "";
            Content = "";
        }

        public Message(bool status, HttpStatusCode statusCode, string reasonPhrase)
        {
            Status = status;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Content = "";
        }

        public Message(bool status, HttpStatusCode statusCode, string reasonPhrase, T data)
        {
            Status = status;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Content = "";
            Data = data;
        }

        public bool Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Content { get; set; }
        public T Data { get; set; }
    }

    public class HtmlMessage
    {
        public string Body { get; set; }
    }

    public static class HtmlStringBody
    {
        public static string Get(string contentBody, string hrefBase, string headerTitle, string headerCssClass)
        {
            var cssHref = hrefBase + "/Content/css/app.css";
            return getBody(contentBody, hrefBase, cssHref, headerTitle, headerCssClass);
        }

        public static string Get(string contentBody, string hrefBase, string headerTitle, string headerCssClass,
            string cssHref)
        {
            return getBody(contentBody, hrefBase, cssHref, headerTitle, headerCssClass);
        }

        private static string getBody(string contentBody, string hrefBase, string cssHref, string headerTitle,
            string headerCssClass)
        {
            var html = "<!DOCTYPE html>";
            html += "<html>";
            html += "<head lang=\"en\">";
            html += "<base href=\"" + hrefBase + "\">";
            html += "<meta charset=\"UTF-8\">";
            html +=
                "<meta name=\"viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0\" />";
            html += "<meta name=\"apple-mobile-web-app-capable\" content=\"yes\" />";
            html += "<link href=\"" + cssHref + "\" rel=\"stylesheet\">";
            html += "</head>";
            html += "<body class=\"email-body\">";
            html += "<h2 class=\"" + headerCssClass + "\">" + headerTitle + "</h2>";
            html += contentBody;
            html += "</body>";
            html += "</html>";

            return html;
        }
    }
}