using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine.Serialization;
namespace GamePlayModifiersPlus.Utilities
{
    public class PasteRequests
    {
        internal static HttpClient client;
        internal static string url = "https://api.paste.ee/v1/pastes";
        internal static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public static async Task<string> GetHastebin(string content, string desc= "GameplayModifiersPlus Response", string name = "GameplayModifiersPlus Response")
        {
            string result = "";
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("X-Auth-Token", "allN7jLp9kYFY7rYlqi7rsk0LADxmyuWCb7Rn2Ckh");
                
            }
           
            string jsonContent = "{\"expire\":60,\"description\":\"$desc\",\"sections\":[{\"name\":\"$name\",\"syntax\":\"autodetect\",\"contents\":\"$content\"}]}";
            jsonContent = jsonContent.Replace("$desc", desc).Replace("$content", content.Replace("\n", "\\n")).Replace("$name", name);
         //   Plugin.Log(jsonContent);
            var postcontent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
            request.Content = postcontent;
            try
            {
                response = await client.SendAsync(request).ConfigureAwait(false);
                Plugin.Log(response.StatusCode.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    Plugin.Log($"Request Failed: {response.StatusCode} | {response.ReasonPhrase}");
                }
                else
                {
                    string jsonresponse = await response.Content.ReadAsStringAsync();
                    PasteEEResponse responseContent = JsonUtility.FromJson<PasteEEResponse>(jsonresponse);
                    if (responseContent.success)
                        result = responseContent.link;
                }
            }
            catch(Exception ex)
            {
                Plugin.Log("Paste Request Failed");
            }

            return result;
        }



    }
    public class PasteEEResponse
    {
        public string id;
        public string link;
        public bool success;
    }
}

