using System;
using System.Net;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Buffers.Text;
using BaseLibS.Util;

/// <summary>
/// 구글은 Nuget으로 따로 제공
/// </summary>


namespace VoiceDetectionEtri.API
{
    public class EtriAPI
    {
        public string GetVoiceJson(JObject _json)
        {
            string results = string.Empty;
            JToken bodyJToken = _json.SelectToken("results");
            if (bodyJToken == null)
                return results;


            foreach (JToken bodyMembers in bodyJToken)
            {
                JToken mainToken = bodyMembers.SelectToken("alternatives");
                foreach (JToken mainMembers in mainToken)
                {
                    results = mainMembers.SelectToken("transcript").ToString();
                }
            }

            return results;
        }

        public string GetVoiceJsonRest(string _filePath)
        {
            string url = "http://aiopen.etri.re.kr:8000/WiseASR/Recognition"; // HOST 및 URL
            string rest_api_key = "93f0493d-cd96-4f8e-b6d7-3beced78a336"; 
            String languageCode = "KOREAN";     // 언어 코드
            String audioFilePath = "AUDIO_FILE_PATH";  // 녹음된 음성 파일 경로
            String audioContents = "";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url); // 해당 URL로 네트웍을 만든다
            request.ContentType = "application/json; charset=UTF-8";
            request.Method = "POST";

            //FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            //byte[] bytes = new byte[fs.Length];
            //bytes = File.ReadAllBytes(_filePath);
            //fs.Close();

            FileStream audioFile = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            byte[]  bytes = File.ReadAllBytes(_filePath);

            audioContents = Convert.ToBase64String(bytes);

            JObject jobject = new JObject();
            jobject.Add("access_key", rest_api_key);
            JObject jobject2 = new JObject();
            jobject2.Add("language_code", languageCode);
            jobject2.Add("audio", audioContents);
            jobject.Add("argument", jobject2);

            using (Stream dataStream = request.GetRequestStream())
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(jobject.ToString());

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

            }


            //using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    JObject jobject = new JObject();
            //    jobject.Add("access_key", rest_api_key);
            //    JObject jobject2 = new JObject();
            //    jobject2.Add("language_code", languageCode);
            //    jobject2.Add("audio", audioContents);
            //    jobject.Add("argument", jobject2);

            //    string json = "{\"access_key\" : " + rest_api_key + " , " +  "\"argument\" :" ;
            //    json +="{ \"language_code\" : " + languageCode + " , "
            //        + "\"audio\" : " + audioContents + "}}";


            //    streamWriter.Write("argument", jobject);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //using (Stream reqStream = request.GetRequestStream()) // 네트웍을 열어서 데이터를 보낸다.
            //{
            //    reqStream.Write(bytes, 0, bytes.Length); // 데이터 쓰기
            //    reqStream.Close();
            //}

            string responseText = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) // 보낸데이터를 기반으로 받는다
            {
                HttpStatusCode status = response.StatusCode; 
                Console.WriteLine(status);

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
    }
}

