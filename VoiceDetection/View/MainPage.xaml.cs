using System;
using System.Collections.Generic;
using System.Windows.Controls;
using VoiceDetectionGoogle.API;
using VoiceDetectionEtri.API;
using VoiceDetection.Model;
using System.Windows.Threading;
using VoiceDetection.Model;


namespace VoiceDetection.View
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public partial class MainPage : UserControl
    {
        DispatcherTimer pollingTimer = null;
        private APIEnum.APICompanies api = APIEnum.APICompanies.None;

        public MainPage()
        {
            InitializeComponent();

            pollingTimer = new DispatcherTimer();
            pollingTimer.Interval = TimeSpan.FromSeconds(14);
            pollingTimer.Tick += new EventHandler(SpeakButtonClick);

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

        private void EtriButtonClick(object sender, EventArgs e)
        {
            api = APIEnum.APICompanies.Etri;
            APINameTextBlock.Text = "Etri";
        }

        private void GoogleButtonClick(object sender, EventArgs e)
        {
            api = APIEnum.APICompanies.Etri;
            APINameTextBlock.Text = "Google";
        }

        private void SpeakButtonClick(object sender, EventArgs e)
        {
            //SpeachToText();
            if (InputListView.SelectedItems.Count == 0) return;

            VoiceRecorder recorder = VoiceRecorder.GetInstance();
            if (recorder.IsRecording == false) //녹음 시작
            {
                RecordingTextBlock.Text = "Recording";
                recorder.StartRecordVoice(InputListView.SelectedIndex) ;
                pollingTimer.Stop();
                pollingTimer.Start();
            }
            else if(recorder.IsRecording == true)
            {
                RecordingTextBlock.Text = "";
                recorder.StopRecordVoice();
                if (api == APIEnum.APICompanies.None)
                {
                    return;
                }
                else if(api == APIEnum.APICompanies.Google)
                {
                    SpeachToTextGoogle();

                }
                else if(api == APIEnum.APICompanies.Etri)
                {
                    SpeachToTextEtri();

                }
            }
        }

        private void SpeachToTextGoogle()
        {
            GoogleAPI api = new GoogleAPI();
            string path = @"C:\Users\media\Documents\test.wav";
            string result = api.GetVoiceJsonRest(path);

            SpeakTextBlock.Text = result;

            Console.WriteLine(result);
        }

        private void SpeachToTextEtri()
        {
            EtriAPI api = new EtriAPI();
            string path = @"C:\Users\media\Documents\test.wav";
            string result = api.GetVoiceJsonRest(path);

            SpeakTextBlock.Text = result;

            Console.WriteLine(result);
        }

    }
}
