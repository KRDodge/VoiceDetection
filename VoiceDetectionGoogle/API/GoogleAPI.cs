using System;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

/// <summary>
/// 구글은 Nuget으로 따로 제공
/// </summary>


namespace VoiceDetectionGoogle.API
{
    public class GoogleAPI
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
            
        }

        public string GoogleCloudSpeech(string _file)
        {
            string result = "";

            if (File.Exists(_file) == false)
                return result;


            var credential = GoogleCredential.GetApplicationDefault();
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets("airy-strength-335906");
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            string FilePath = _file;
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            byte[] fileData = new byte[fs.Length];
            fs.Read(fileData, 0, fileData.Length);
            fs.Close();

            string CRLF = "\r\n";
            string postData = "--" + boundary + CRLF + "Content-Disposition: form-data; name=\"image\"; filename=\"";
            postData += Path.GetFileName(FilePath) + "\"" + CRLF + "Content-Type: image/jpeg" + CRLF + CRLF;
            string footer = CRLF + "--" + boundary + "--" + CRLF;

            Stream DataStream = new MemoryStream();
            DataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
            DataStream.Write(fileData, 0, fileData.Length);
            DataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
            DataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            DataStream.Position = 0;
            byte[] formData = new byte[DataStream.Length];
            DataStream.Read(formData, 0, formData.Length);
            DataStream.Close();

            RecognitionAudio audio4 = RecognitionAudio.FromBytes(formData);

            //MemoryStream ms = new MemoryStream(binary);

            //using (Stream stream = ms) // Any regular .NET stream
            //{
            //    RecognitionAudio audio5 = RecognitionAudio.FromStream(stream);
            //    result = audio5.ToString();
            //}

            SpeechClient client = SpeechClient.Create();
            RecognitionConfig config = new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                SampleRateHertz = 16000,
                LanguageCode = LanguageCodes.Korean.SouthKorea,
            };
            RecognizeResponse response = client.Recognize(config, audio4);
            Console.WriteLine(response);


            return result;
        }
    }
}

