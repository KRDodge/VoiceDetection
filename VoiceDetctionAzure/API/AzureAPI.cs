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
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

/// <summary>
/// 구글은 Nuget으로 따로 제공
/// </summary>


namespace VoiceDetectionAzure.API
{
    public class AzureAPI
    {

        public static readonly string FetchTokenUri =
        "https://koreacentral.stt.speech.microsoft.com/speech/recognition/conversation/cognitiveservices/v1?language=ko-KR";
        private string resultString = "";


        public string GetVoiceJson(JObject _json)
        {
            string results = string.Empty;
            JToken bodyJToken = _json.SelectToken("DisplayText");
            if (bodyJToken == null)
                return results;

            results = bodyJToken.ToString();

            return results;
        }


        public string GetAzureJson(string _file)
        {
            string result = "";
            string responseText = "";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(FetchTokenUri);
                httpWebRequest.SendChunked = true;
                httpWebRequest.Accept = @"application/json;text/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.ProtocolVersion = HttpVersion.Version11;
                httpWebRequest.ContentType = @"audio/wav; codecs=audio/pcm; samplerate=16000";
                httpWebRequest.Headers["Ocp-Apim-Subscription-Key"] = "02266c5c8210457cb994e8b533be3ffb";
                httpWebRequest.AllowWriteStreamBuffering = false;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    using (var fs = new FileStream(_file, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = null;
                        int bytesRead = 0;
                        using (var requestStream = httpWebRequest.GetRequestStream())
                        {
                            // Read 1024 raw bytes from the input audio file.
                            buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }
                            requestStream.Flush();
                            requestStream.Close();
                        }
                    }
                        
                    //데이터 받기
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        responseText = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            JObject json = new JObject();
            
            json = JObject.Parse(responseText);

            result = GetVoiceJson(json);

            return result;
        }

        async private Task FromFile(string _file)
        {
            var speechConfig = SpeechConfig.FromAuthorizationToken("00a246ed-c6f8-4379-8dc4-ff22ab938539", "koreacentral");
            speechConfig.SpeechRecognitionLanguage = "ko-KR";
            speechConfig.AuthorizationToken = "ffa9fc78368b417c9b98b3c8b863aebb";
            var audioConfig = AudioConfig.FromWavFileInput(_file);
            var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var result = await recognizer.RecognizeOnceAsync();
            Console.WriteLine($"RECOGNIZED: Text={result.Text}");
            this.resultString = result.Text;
        }

        async public Task GetAzureAsync(string _file)
        {
            await FromFile(_file);
        }

        public string GetAzureString()
        {
            return this.resultString;
        }

    }
}

