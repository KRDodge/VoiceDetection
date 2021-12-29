using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using VoiceDetectionKakao.API;
using VoiceDetectionGoogle.API;
using VoiceDetection.Model;
using NAudio.Wave;     
using NAudio.CoreAudioApi;
using System.Windows.Controls;

namespace VoiceDetection.View
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : UserControl
    {
        //private bool isRecording = false;

        public MainPage()
        {
            InitializeComponent();

            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

            InputListView.Items.Clear();

            foreach (var source in sources)
            {
                ListViewItem item = new ListViewItem();
                item.Content = source.ProductName;
                InputListView.Items.Add(item);
            }
        }

        private void SpeakButtonClick(object sender, EventArgs e)
        {
            //SpeachToText();
            if (InputListView.SelectedItems.Count == 0) return;

            VoiceRecorder recorder = VoiceRecorder.GetInstance();
            if (recorder.IsRecording == false) //녹음 시작
            {
                recorder.StartRecordVoice(InputListView.SelectedIndex) ;
            }
            else if(recorder.IsRecording == true)
            {
                recorder.StopRecordVoice();
                SpeachToText();
            }
        }

        private void SpeachToText()
        {
            GoogleAPI api = new GoogleAPI();
            string path = @"C:\Users\media\Documents\test.wav";
            string result = api.GoogleCloudSpeech(path);
            
            SpeakTextBlock.Text = result;

            Console.WriteLine(result);
        }

    }
}
