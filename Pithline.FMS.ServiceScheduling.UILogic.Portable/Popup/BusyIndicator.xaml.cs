using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eqstra.ServiceScheduling
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BusyIndicator : Page
    {
        private Popup _popup;
        public BusyIndicator()
        {
            this.InitializeComponent();
        }
        public void Open()
        {
            CoreWindow currentWindow = Window.Current.CoreWindow;
            if (_popup == null)
            {
                _popup = new Popup();
            }
            _popup.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            _popup.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            this.Tag = _popup;
            this.Height = currentWindow.Bounds.Height;
            this.Width = currentWindow.Bounds.Width;

            _popup.Child = this;
            _popup.IsOpen = true;
        }
        public void Close()
        {
            ((Popup)this.Tag).IsOpen = false;
        }
    }
}
