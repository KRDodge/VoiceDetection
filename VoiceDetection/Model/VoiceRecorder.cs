using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using System;
using System.Windows.Threading;
using System.Windows.Forms;

namespace VoiceDetection.Model
{
    public class VoiceRecorder
    {
        private static VoiceRecorder instance;

        NAudio.Wave.WaveIn sourceStream = null;
        NAudio.Wave.DirectSoundOut waveOut = null;
        NAudio.Wave.WaveFileWriter waveWriter = null;

        private bool isRecording = false;
        public bool IsRecording { get { return isRecording; } set { isRecording = value; } }

        public VoiceRecorder()
        {
            
        }

        public static VoiceRecorder GetInstance()
        {
            if (instance == null)
                instance = new VoiceRecorder();
            return instance;
        }

        public void StartRecordVoice(int _sourceNumber)
        {
            sourceStream = new NAudio.Wave.WaveIn();
            sourceStream.DeviceNumber = _sourceNumber;
            sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);

            sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);
            waveWriter = new NAudio.Wave.WaveFileWriter(@"C:\Users\media\Documents\test.wav", sourceStream.WaveFormat);

            sourceStream.StartRecording();

            DispatcherTimer pollingTimer = new DispatcherTimer();
            pollingTimer.Stop();
            pollingTimer.Interval = TimeSpan.FromSeconds(14);
            pollingTimer.Tick += new EventHandler(StopRecordVoice);
            pollingTimer.Start();

            isRecording = true;
        }

        public void StopRecordVoice()
        {

            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
            if(waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }

            isRecording = false;
        }

        public void sourceStream_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (waveWriter == null)
                return;

            waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        public void StopRecordVoice(object sender, EventArgs e)
        {
            StopRecordVoice();
        }

    }
}
