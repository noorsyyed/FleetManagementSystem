using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection
{
    public sealed partial class DetailsDialog : ContentDialog
    {
        public DetailsDialog()
        {
            this.InitializeComponent();
           
            Window.Current.SizeChanged += Current_SizeChanged;
            var b = Window.Current.Bounds;
            this.scrlViewTask.MaxHeight = b.Height - 100;
            this.scrlViewCust.MaxHeight = b.Height - 100;
        }
        void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var b = Window.Current.Bounds;
            this.scrlViewTask.MaxHeight = b.Height - 100;
            this.scrlViewCust.MaxHeight = b.Height - 100;
        }
    }
}