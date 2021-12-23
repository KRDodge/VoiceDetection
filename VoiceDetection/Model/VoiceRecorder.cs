using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceDetection.Model
{
    public class VoiceRecorder
    {
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        static void Main(string[] args)
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
            Console.WriteLine("recording, press Enter to stop and save ...");
            Console.ReadLine();

            mciSendString("save recsound c:\work\result.wav", "", 0, 0);
            mciSendString("close recsound ", "", 0, 0);
        }
    }
}
