using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoiceDetectionKakao.API;
using VoiceDetectionKakao.Data;

namespace VoiceDetection.View
{
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : UserControl
    {
        private bool isRecording = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void SpeakButtonClick(object sender, EventArgs e)
        {
            if(isRecording == false)
                isRecording = true;
            else
                isRecording = false;

                
        }
    }
}
