using System;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using VoiceDetectionKakao.Data;


namespace VoiceDetectionKakao.API
{
    public class KakaoAPI
    {
        public async Task<string> GetVoiceJson(string _filePath)
        {
            string results = string.Empty;

            UnicodeEncoding uniencoding = new UnicodeEncoding();
            byte[] binary;
            if (File.Exists(_filePath))
            {
                binary = uniencoding.GetBytes(_filePath);
            }
            else
                return results;
            
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://kakaoi-newtone-openapi.kakao.com/v1/recognize"))
                {
                    request.Headers.TryAddWithoutValidation("Transfer-Encoding", "chunked");
                    request.Headers.TryAddWithoutValidation("Authorization", "KakaoAK {9644d7ac9a0a2674f0b3a40b9e7bda78}");

                    request.Content = new StringContent(File.ReadAllBytes(binary));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    var response = await httpClient.SendAsync(request);
                    
                    results = string.Empty;
                    if(response.IsSuccessStatusCode)
                        results = await response.Content.ReadAsStringAsync();

                    GetVoiceData(results);
                }
            }
            return results;
        }

        public void GetVoiceData(string message) //안쓰는 함수 일단 냅둠
        {
            if (message == null)
                return;

            VoiceKaKaoData data = new VoiceKaKaoData();
            data.VoiceString = message;
        }
    }
}

