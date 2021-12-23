using VoiceDetection.Control;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace VoiceDetection.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                Set(nameof(CurrentViewModel), ref _currentViewModel, value);
            }
        }

        public MainViewModel()
        {
            Messenger.Default.Register<GotoPage>(this, (action) => ReceiveMessage(action));
            CurrentViewModel = ServiceLocator.Current.GetInstance<MainPageModel>();
        }

        private object ReceiveMessage(GotoPage action)
        {
            switch (action.PageName)
            {
                case PageName.Main:
                    CurrentViewModel = ServiceLocator.Current.GetInstance<MainPageModel>();
                    break;
            }
            return null;
        }
    }
}