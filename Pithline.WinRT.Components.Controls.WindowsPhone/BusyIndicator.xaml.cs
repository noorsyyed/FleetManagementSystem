using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.WinRT.Components.Controls.WindowsPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusyIndicator : ContentDialog, INotifyPropertyChanged
    {
       // private Popup _popup;
        public BusyIndicator()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
        public void Open(string message)
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            //if (_popup == null)
            //{
            //    _popup = new Popup();
            //}

            //_popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            //_popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            this.Message = message;
           // this.Tag = _popup;
            this.Height = currentWindow.Bounds.Height;
            this.Width = currentWindow.Bounds.Width;
            //_popup.Child = this;
            //_popup.IsOpen = true;
            //_popup.VerticalOffset = 0;
            this.ShowAsync();

        }
        public void Close()
        {
           // ((Popup)this.Tag).IsOpen = false;
            this.Hide();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
