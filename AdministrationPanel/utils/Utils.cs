using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public static string GuidFromString(string str)
        {
            string result = "";
            using (MD5 md = MD5.Create())
            {
                byte[] hash = md.ComputeHash(Encoding.UTF8.GetBytes(str));
                Guid res = new Guid(hash);
                result = res.ToString();
            }
            return result;
        }

        public static void saveWindowState(string window, double x, double y)
        {
            Registry.CurrentUser.CreateSubKey("AdminTools");
            RegistryKey adminTools = Registry.CurrentUser.OpenSubKey("AdminTools", true);
            adminTools.CreateSubKey("WindowStates");
            RegistryKey windowStates = adminTools.OpenSubKey("WindowStates", true);
            string hashedWindow = GuidFromString(window);
            windowStates.CreateSubKey(hashedWindow);
            RegistryKey currentWindow = windowStates.OpenSubKey(hashedWindow,true);
            currentWindow.SetValue("x", x);
            currentWindow.SetValue("y", y);
            currentWindow.Close();
            windowStates.Close();
            adminTools.Close();


        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void saveConnection(string login, string password)
        {
            Registry.CurrentUser.CreateSubKey("AdminTools");
            var adminTools = Registry.CurrentUser.OpenSubKey("AdminTools", true);
            adminTools.CreateSubKey("Cridentials");
            var cridentials = adminTools.OpenSubKey("Cridentials", true);
            cridentials.SetValue(Base64Encode("login"), Base64Encode(login));
            cridentials.SetValue(Base64Encode("password"), Base64Encode(password));
            cridentials.Close();
            adminTools.Close();

        }

        public static void deleteConnection()
        {

            var adminTools = Registry.CurrentUser.OpenSubKey("AdminTools", true);
            if (adminTools == null) return;
            var cridentials = adminTools.OpenSubKey("Cridentials", true);
            if (cridentials == null) return;
            cridentials.DeleteValue(Base64Encode("login"));
            cridentials.DeleteValue(Base64Encode("password"));
            cridentials.Close();
            adminTools.Close();
        }

        public static Dictionary<string, string> loadConnection()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var adminTools = Registry.CurrentUser.OpenSubKey("AdminTools", true);
            if (adminTools == null) return null;
            var cridentials = adminTools.OpenSubKey("Cridentials", true);
            if (cridentials == null) return null;
            string login, password;
            try
            {
                login = Base64Decode(cridentials.GetValue(Base64Encode("login")).ToString());
                password = Base64Decode(cridentials.GetValue(Base64Encode("password")).ToString());
            }
            catch
            {
                return null;
            }
            result.Add("login", login);
            result.Add("password", password);
            cridentials.Close();
            adminTools.Close();
            return result;
        }

        public static void loadWindowState(Window win)
        {
            var adminTools = Registry.CurrentUser.OpenSubKey("AdminTools", true);
            if (adminTools == null) return;
            var windowStates = adminTools.OpenSubKey("WindowStates", true);
            if (windowStates == null) return;
            string hashedWindow = GuidFromString(win.Title);
            var currentWindow = windowStates.OpenSubKey(hashedWindow,true);
            if(currentWindow == null) return;
            double x, y;
            double.TryParse(currentWindow.GetValue("x").ToString(), out x);
            double.TryParse(currentWindow.GetValue("y").ToString(), out y);
            if (x == null || y == null) return;
            win.Left = x;
            win.Top = y;
            currentWindow.Close();
            windowStates.Close();
            adminTools.Close();
        }

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

        public static async void createEntry<T>(T entry, string path)
        {
            using (var client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(entry);
                var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7253/api/{path}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokens.access);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.SendAsync(request);

            }
        }

        public static async void deleteEntry(string id, string path)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7253/api/{path}/{id}");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokens.access);
                await client.SendAsync(request);
            }
        }
    }
}
