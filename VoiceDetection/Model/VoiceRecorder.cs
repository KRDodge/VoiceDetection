using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Forms;

namespace VoiceDetection.Model
{
    public class VoiceRecorder
    {
        //[DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        private bool m_isRecording = false;

        public bool IsRecording { get { return m_isRecording; }}

        public void StartRecordVoice()
        {
            mciSendString("open new Type waveaudio Alias recsound", null, 0, 0);
            mciSendString("record recsound", "", 0, 0);

            DispatcherTimer pollingTimer = new DispatcherTimer();
            pollingTimer.Stop();
            pollingTimer.Interval = TimeSpan.FromSeconds(10);
            pollingTimer.Tick += new EventHandler(StopRecordVoice);
            pollingTimer.Start();

            m_isRecording = true;
        }

        public void StopRecordVoice()
        {
            mciSendString("save recsound C:\\Users\\media\test.wav", null, 0, 0);
            mciSendString("close Sound ", null, 0, 0);
            m_isRecording = false;
        }

        public void StopRecordVoice(object sender, EventArgs e)
        {
            StopRecordVoice();
        }

    }
}
