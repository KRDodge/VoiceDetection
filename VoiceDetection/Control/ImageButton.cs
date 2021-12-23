using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoiceDetection.Control
{
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        [Description("Represents a custom button control that responds to a Click event. Displays an image using a custom Source property if the Source property is bound to an Image in the template.")]
        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton));

        [Description("Represents a custom button control that responds to a Pressed state. Displays an image using a custom Over Source property if the Source property is bound to an Image in the template.")]
        public ImageSource SourceOver
        {
            get { return base.GetValue(SourceOverProperty) as ImageSource; }
            set { base.SetValue(SourceOverProperty, value); }
        }

        public static readonly DependencyProperty SourceOverProperty =
          DependencyProperty.Register("SourceOver", typeof(ImageSource), typeof(ImageButton));
    }
}