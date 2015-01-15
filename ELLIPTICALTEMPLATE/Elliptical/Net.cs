using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Elliptical.Net
{

    #region "HttpResult"

    public class HttpResult
    {
        public string Html { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
    }

    #endregion

    #region "Basic Credentials"

    public class BasicCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Error { get; set; }
    }

    #endregion

    #region "Http"

    public static class Http
    {
        #region "public"

        public static string WebGet(string address)
        {
            try
            {
                var result = "";

                using (var client = new WebClient())
                {
                    using (var reader = new StreamReader(client.OpenRead(address)))
                    {
                        var s = reader.ReadToEnd();
                        result = s;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static HttpResult WebGet(string address, string baseUrl)
        {
            var httpResult = new HttpResult();
            try
            {
                var result = "";

                using (var client = new WebClient())
                {
                    using (var reader = new StreamReader(client.OpenRead(address)))
                    {
                        var s = reader.ReadToEnd();
                        result = s;
                    }
                }
                httpResult.Html = result;
                httpResult.StatusCode = HttpStatusCode.OK;
                httpResult.Error = "";
                return httpResult;
            }
            catch (Exception e)
            {
                httpResult.Html = "";
                if (e.GetType().Name == "WebException")
                {
                    var we = (WebException) e;
                    var response = (HttpWebResponse) we.Response;
                    httpResult.StatusCode = response.StatusCode;
                    httpResult.Error = we.Message;
                }
                else
                {
                    httpResult.StatusCode = HttpStatusCode.InternalServerError;
                    httpResult.Error = e.Message;
                }

                return httpResult;
            }
        }

        public static HttpResult WebGet(string address, string username, string password)
        {
            var httpResult = new HttpResult();
            try
            {
                var result = "";

                using (var client = new WebClient())
                {
                    client.Headers.Add("Authorization",
                        "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
                    using (var reader = new StreamReader(client.OpenRead(address)))
                    {
                        var s = reader.ReadToEnd();
                        result = s;
                    }
                }
                httpResult.Html = result;
                httpResult.StatusCode = HttpStatusCode.OK;
                httpResult.Error = "";
                return httpResult;
            }
            catch (Exception e)
            {
                httpResult.Html = "";
                if (e.GetType().Name == "WebException")
                {
                    var we = (WebException) e;
                    var response = (HttpWebResponse) we.Response;
                    httpResult.StatusCode = response.StatusCode;
                    httpResult.Error = we.Message;
                }
                else
                {
                    httpResult.StatusCode = HttpStatusCode.InternalServerError;
                    httpResult.Error = e.Message;
                }

                return httpResult;
            }
        }

        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (!isValidDomain)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string ConvertPathToUrl(string path, string domain)
        {
            var url = "";
            if (path.IndexOf("http://") == -1)
            {
                url = "http://" + domain + path;
            }
            else
            {
                url = path;
            }
            return path;
        }

        public static string ConvertPathToUrl(string path, string domain, string port)
        {
            var url = "";
            if (path.IndexOf("http://") == -1)
            {
                url = "http://" + domain + ":" + port + path;
            }
            else
            {
                url = path;
            }
            return path;
        }

        public static string ConvertPathToUrl(string path, string domain, string host, string port)
        {
            var url = "";
            if (path.IndexOf("http://") == -1)
            {
                url = "http://" + host + "." + domain + ":" + port + path;
            }
            else
            {
                url = path;
            }
            return path;
        }

        public static BasicCredentials DecodeBasicAuthorization(string auth)
        {
            var basicCredentials = new BasicCredentials();
            try
            {
                var decoded = Encoding.ASCII.GetString(Convert.FromBase64String(auth));
                var authArray = decoded.Split(':');
                var user = authArray[0];
                var pass = authArray[1];
                basicCredentials.Username = user;
                basicCredentials.Password = pass;
                basicCredentials.Error = false;
                return basicCredentials;
            }
            catch (Exception)
            {
                basicCredentials.Error = true;
                return basicCredentials;
            }
        }

        #endregion

        #region "private"

        private static bool isValidDomain = true;

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            var idn = new IdnMapping();

            var domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                isValidDomain = false;
            }
            return match.Groups[1].Value + domainName;
        }

        #endregion
    }

    #endregion

    #region "Smtp"

    public static class Smtp
    {
        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        /// <returns></returns>
        public static bool SendMail(string to, string from, string subject, string body, bool isHTML)
        {
            try
            {
                var mailServer = ConfigurationManager.AppSettings["MailServer"];
                var objMail = new MailMessage();
                var fromMail = new MailAddress(from);
                objMail.To.Add(to);
                objMail.From = fromMail;
                objMail.Priority = MailPriority.High;
                objMail.Subject = subject;
                objMail.Body = body;
                objMail.IsBodyHtml = isHTML;

                var client = new SmtpClient(mailServer);
                client.Send(objMail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        /// <param name="host"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static bool SendMail(string to, string from, string subject, string body, bool isHTML, string host,
            NetworkCredential credentials)
        {
            try
            {
                var objMail = new MailMessage();
                var fromMail = new MailAddress(from);
                objMail.To.Add(to);
                objMail.From = fromMail;
                objMail.Priority = MailPriority.High;
                objMail.Subject = subject;
                objMail.Body = body;
                objMail.IsBodyHtml = isHTML;

                var client = new SmtpClient(host);
                client.Credentials = credentials;
                client.Send(objMail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        /// <returns></returns>
        public static bool SendMail(List<string> to, string from, string subject, string body, bool isHTML)
        {
            try
            {
                var mailServer = ConfigurationManager.AppSettings["MailServer"];
                var objMail = new MailMessage();
                var fromMail = new MailAddress(from);
                foreach (var item in to)
                {
                    objMail.To.Add(item);
                }
                objMail.From = fromMail;
                objMail.Priority = MailPriority.High;
                objMail.Subject = subject;
                objMail.Body = body;
                objMail.IsBodyHtml = isHTML;

                var client = new SmtpClient(mailServer);
                client.Send(objMail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        /// <param name="host"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static bool SendMail(List<string> to, string from, string subject, string body, bool isHTML, string host,
            NetworkCredential credentials)
        {
            try
            {
                var objMail = new MailMessage();
                var fromMail = new MailAddress(from);
                foreach (var item in to)
                {
                    objMail.To.Add(item);
                }
                objMail.From = fromMail;
                objMail.Priority = MailPriority.High;
                objMail.Subject = subject;
                objMail.Body = body;
                objMail.IsBodyHtml = isHTML;

                var client = new SmtpClient(host);
                client.Credentials = credentials;
                client.Send(objMail);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    #endregion
}

#region "Ftp"

namespace Elliptical.Net.Ftp
{
    public class Client
    {
        public bool Binary = true;
        public bool EnableSsl = false;
        public bool Hash = false;
        public bool Passive = true;
        private string uri;
        private readonly int bufferSize = 1024;
        private readonly string password;
        private readonly string userName;

        public Client(string uri, string userName, string password)
        {
            this.uri = uri;
            this.userName = userName;
            this.password = password;
        }

        public string AppendFile(string source, string destination)
        {
            var request = createRequest(combine(uri, destination), WebRequestMethods.Ftp.AppendFile);

            using (var stream = request.GetRequestStream())
            {
                using (var fileStream = File.Open(source, FileMode.Open))
                {
                    int num;

                    var buffer = new byte[bufferSize];

                    while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (Hash)
                            Console.Write("#");

                        stream.Write(buffer, 0, num);
                    }
                }
            }

            return getStatusDescription(request);
        }

        public string ChangeWorkingDirectory(string path)
        {
            uri = combine(uri, path);

            return PrintWorkingDirectory();
        }

        public string DeleteFile(string fileName)
        {
            var request = createRequest(combine(uri, fileName), WebRequestMethods.Ftp.DeleteFile);

            return getStatusDescription(request);
        }

        public string DownloadFile(string source, string dest)
        {
            var request = createRequest(combine(uri, source), WebRequestMethods.Ftp.DownloadFile);

            var buffer = new byte[bufferSize];

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var fs = new FileStream(dest, FileMode.OpenOrCreate))
                    {
                        var readCount = stream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            if (Hash)
                                Console.Write("#");

                            fs.Write(buffer, 0, readCount);
                            readCount = stream.Read(buffer, 0, bufferSize);
                        }
                    }
                }

                return response.StatusDescription;
            }
        }

        public Stream DownloadFileStream(string source)
        {
            var request = createRequest(combine(uri, source), WebRequestMethods.Ftp.DownloadFile);
            var response = (FtpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();

            return stream;
        }

        public DateTime GetDateTimestamp(string fileName)
        {
            var request = createRequest(combine(uri, fileName), WebRequestMethods.Ftp.GetDateTimestamp);

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                return response.LastModified;
            }
        }

        public long GetFileSize(string fileName)
        {
            var request = createRequest(combine(uri, fileName), WebRequestMethods.Ftp.GetFileSize);

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                return response.ContentLength;
            }
        }

        public string[] ListDirectory()
        {
            var list = new List<string>();

            var request = createRequest(WebRequestMethods.Ftp.ListDirectory);

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public string[] ListDirectoryDetails()
        {
            var list = new List<string>();

            var request = createRequest(WebRequestMethods.Ftp.ListDirectoryDetails);

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public string MakeDirectory(string directoryName)
        {
            var request = createRequest(combine(uri, directoryName), WebRequestMethods.Ftp.MakeDirectory);

            return getStatusDescription(request);
        }

        public string PrintWorkingDirectory()
        {
            var request = createRequest(WebRequestMethods.Ftp.PrintWorkingDirectory);

            return getStatusDescription(request);
        }

        public string RemoveDirectory(string directoryName)
        {
            var request = createRequest(combine(uri, directoryName), WebRequestMethods.Ftp.RemoveDirectory);

            return getStatusDescription(request);
        }

        public string Rename(string currentName, string newName)
        {
            var request = createRequest(combine(uri, currentName), WebRequestMethods.Ftp.Rename);

            request.RenameTo = newName;

            return getStatusDescription(request);
        }

        public string UploadFile(string source, string destination)
        {
            var request = createRequest(combine(uri, destination), WebRequestMethods.Ftp.UploadFile);

            using (var stream = request.GetRequestStream())
            {
                using (var fileStream = File.Open(source, FileMode.Open))
                {
                    int num;

                    var buffer = new byte[bufferSize];

                    while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (Hash)
                            Console.Write("#");

                        stream.Write(buffer, 0, num);
                    }
                }
            }

            return getStatusDescription(request);
        }

        public string UploadFileWithUniqueName(string source)
        {
            var request = createRequest(WebRequestMethods.Ftp.UploadFileWithUniqueName);

            using (var stream = request.GetRequestStream())
            {
                using (var fileStream = File.Open(source, FileMode.Open))
                {
                    int num;

                    var buffer = new byte[bufferSize];

                    while ((num = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (Hash)
                            Console.Write("#");

                        stream.Write(buffer, 0, num);
                    }
                }
            }

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                return Path.GetFileName(response.ResponseUri.ToString());
            }
        }

        private FtpWebRequest createRequest(string method)
        {
            return createRequest(uri, method);
        }

        #region "private"

        private FtpWebRequest createRequest(string uri, string method)
        {
            var r = (FtpWebRequest) WebRequest.Create(uri);

            r.Credentials = new NetworkCredential(userName, password);
            r.Method = method;
            r.UseBinary = Binary;
            r.EnableSsl = EnableSsl;
            r.UsePassive = Passive;

            return r;
        }

        private string getStatusDescription(FtpWebRequest request)
        {
            using (var response = (FtpWebResponse) request.GetResponse())
            {
                return response.StatusDescription;
            }
        }

        private string combine(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace("\\", "/");
        }

        #endregion
    }
}

#endregion