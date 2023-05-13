using AdministrationPanel.Models;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AdministrationPanel.utils
{


    public static class Utils
    {
        public struct Tokens 
        {
            public string? access;
            public string? refresh;
        }

        public static Tokens tokens;

        public static async Task<List<T>> requestTable<T>( T table,string path)
        {
            using (var client = new HttpClient())
            {
                List<T> result = new List<T>();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7253/api/"+path);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokens.access);
                //request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue($"Authorization: Bearer {tokens.access}"));
                await client.SendAsync(request).ContinueWith(async (resp) => 
                {
                    string json = await resp.Result.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<T>>(json);
                });
                    return result;
            }
        }

        public static async void updateTable<T>(T   updatingPiece, string path,string id)
        {
            using (var client = new HttpClient())
            {
                string jsoned = JsonConvert.SerializeObject(updatingPiece);
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7253/api/{path}/{id}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokens.access);
                request.Content = new StringContent(jsoned, Encoding.UTF8, "application/json");
                await client.SendAsync(request);
            }
            
        }
    }
}
