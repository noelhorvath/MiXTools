using Serilog;
using System.Net.Http.Headers;

namespace MiXTools.Shared
{
    internal static class HttpHelper
    {
        private static readonly HttpClient httpClient = new();

        public static async Task DownloadFileAsync(string uri, string path)
        {
            try
            {
                Log.Information("Downloading FileLauncher from GitHub repo...");
                if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri? uriResult))
                    throw new InvalidOperationException("URI is invalid!");

                byte[] fileBytes = await httpClient.GetByteArrayAsync(uriResult);
                File.WriteAllBytes(path, fileBytes);
                Log.Information("File download has finished!");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<IEnumerable<T>> GetDataAsync<T>(string url, string urlParams = "")
        {
            // set URL
            httpClient.BaseAddress = new Uri(url);
            // add an Accept header for JSON format
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (url.Contains(Utils.GITHUB_API_URL))
            {
                var prodValue = new ProductInfoHeaderValue(Utils.APP_NAME, Properties.Settings.Default.AppVersion);
                var commentValue = new ProductInfoHeaderValue($"(+{Utils.GITHUB_REPO_URL})");
                httpClient.DefaultRequestHeaders.UserAgent.Add(prodValue);
                httpClient.DefaultRequestHeaders.UserAgent.Add(commentValue);
            }

            // set parameters for URL and perform a GET
            var response = await httpClient.GetAsync(urlParams);

            if (response != null && response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IEnumerable<T>>();
            else
            {
                string errorMsg = $"StatusCode: {response?.StatusCode}, ReasonPhrase: {response?.ReasonPhrase}";
                throw new HttpRequestException(errorMsg);
            }
        }
    }
}
