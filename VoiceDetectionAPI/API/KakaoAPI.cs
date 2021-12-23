using System;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using VoiceDetectionKakao.Data;
/// <summary>
/// 카카오는 기업정보(사업자번호)를 필수로 필요함
/// </summary>


namespace VoiceDetectionKakao.API
{
    public class KakaoAPI
    {
        public async Task<string> GetVoiceJson(string _filePath)
        {
            string results = string.Empty;

            UnicodeEncoding uniencoding = new UnicodeEncoding();
            FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            byte[] binary = new byte[fs.Length];
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

                    request.Content = new StreamContent(fs);
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

        public string GetVoiceJsonRest(string _filePath)
        {
            string url = "https://kakaoi-newtone-openapi.kakao.com/v1/recognize"; // HOST 및 URL
            string rest_api_key = "22f264a793755a77e222af849f6ab5f2"; // 내 어플리케이션 => 어플선택 => 기본정보의 앱 키 > REST Key 값 부여            

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); // 해당 URL로 네트웍을 만든다
            request.Headers.Add("Authorization", rest_api_key); // 헤더에 옵션값을 추가한다.
            request.ContentType = "application/octet-stream";// 콘텐츠타입을 명시한다
            request.Method = "POST"; // get 으로 보낼지 post로 보낼지 명시한다.

            FileStream file = new FileStream(_filePath, FileMode.Open); // 보낼 파일을 오픈한다.
            byte[] byteDataParams = new byte[file.Length]; // 보낼파일의 크기만큼 바이트배열을 만든다.

            file.Read(byteDataParams, 0, byteDataParams.Length); // 파일을 읽어서 바이트배열에 데이터를 넣는다.

            request.ContentLength = byteDataParams.Length; // 네트웍으로 보낼 데이터 길이를 명시한다.

            using (Stream reqStream = request.GetRequestStream()) // 네트웍을 열어서 데이터를 보낸다.
            {
                reqStream.Write(byteDataParams, 0, byteDataParams.Length); // 데이터 쓰기
            }

            string responseText = string.Empty;
            using (WebResponse response = request.GetResponse()) // 보낸데이터를 기반으로 받는다
            {
                Stream stream = response.GetResponseStream(); // 받은 데이터를 스트림으로 쓴다
                using (StreamReader sr = new StreamReader(stream)) // 스트림을 읽기 위해 리더를 오픈한다.
                {
                    responseText = sr.ReadToEnd(); // 스트림의 내용을 읽어서 문자열로 반환해준다.
                }
            }
            return responseText;
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

