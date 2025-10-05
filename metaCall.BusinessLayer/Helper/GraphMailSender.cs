using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class GraphMailSender
    {
        public static async Task<string> GetAccessTokenAsync(string tenantId, string clientId, string clientSecret)
        {
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var body = new Dictionary<string, string>
        {
            {"client_id", clientId},
            {"scope", "https://graph.microsoft.com/.default"},
            {"client_secret", clientSecret},
            {"grant_type", "client_credentials"}
        };

            using (var client = new HttpClient())
            using (var content = new FormUrlEncodedContent(body))
            {
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(json);
                return result["access_token"].ToString();
            }
        }

        public static async Task SendMailWithAttachmentAsync(
            string accessToken,
            string fromUser,
            string toUser,
            string subject,
            string bodyText,
            string attachmentPath,
            string filename)
        {
            string apiUrl = $"https://graph.microsoft.com/v1.0/users/{fromUser}/sendMail";

            byte[] fileBytes = File.ReadAllBytes(attachmentPath);
            string base64Content = Convert.ToBase64String(fileBytes);

            var mailPayload = new
            {
                message = new
                {
                    subject = subject,
                    body = new
                    {
                        contentType = "HTML",
                        content = bodyText
                    },
                    toRecipients = new[] {
                    new { emailAddress = new { address = toUser } }
                },
                    attachments = new[] {
                        new Dictionary<string, object>
                        {
                            {"@odata.type", "#microsoft.graph.fileAttachment"},
                            {"name", filename},
                            {"contentType", "application/octet-stream"},
                            {"contentBytes", base64Content}
                        }
                    }
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(mailPayload);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json"))
                {
                    var response = await client.PostAsync(apiUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        string err = await response.Content.ReadAsStringAsync();
                        throw new Exception($"SendMail failed: {response.StatusCode} - {err}");
                    }
                }
            }
        }
    }
}
