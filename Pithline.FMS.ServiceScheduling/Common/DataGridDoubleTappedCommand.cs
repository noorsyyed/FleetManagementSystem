using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Pithline.FMS.ServiceScheduling.Common
{
    public class DataGridDoubleTappedCommand
    {


        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}   

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DataGridDoubleTappedCommand), new PropertyMetadata(null, CommandPropertyChanged));

        private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SfDataGrid).DoubleTapped += DataGridDoubleTappedCommand_DoubleTapped;
        }

        private static void DataGridDoubleTappedCommand_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            var grid = sender as SfDataGrid;

            ICommand command = GetCommand(grid);

            if (grid.SelectedItem != null)
            {
                command.Execute(grid.SelectedItem);
            }
        }

        public static ICommand GetCommand(DependencyObject attached)
        {
            return (ICommand)attached.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject attached, ICommand value)
        {
            attached.SetValue(CommandProperty, value);
        }
    }
}
