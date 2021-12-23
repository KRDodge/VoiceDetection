using System;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using VoiceDetectionKakao.Data;

namespace VoiceDetectionKakao.API
{
    public class VoiceDetectionKakao
    {
        public async Task GetFaceJson(string _filePath)
        {

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://kakaoi-newtone-openapi.kakao.com/v1/recognize"))
                {
                    request.Headers.TryAddWithoutValidation("Transfer-Encoding", "chunked");
                    request.Headers.TryAddWithoutValidation("Authorization", "KakaoAK {9644d7ac9a0a2674f0b3a40b9e7bda78}");

                    request.Content = new StringContent(File.ReadAllText(_filePath));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    var response = await httpClient.SendAsync(request);
                    
                    string results = string.Empty;
                    if(response.IsSuccessStatusCode)
                        results = await response.Content.ReadAsStringAsync();

                    GetVoiceData(results);
                }
            }
        }

        public void GetVoiceData(string message)
        {
            if (message == null)
                return;

            VoiceKaKaoData data = new VoiceKaKaoData();
            data.VoiceString = message;
        }
    }
}

