
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Grid;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace Pithline.FMS.BusinessLogic.Behaviors
{
    public class HighlightFormFieldOnErrors : Behavior<FrameworkElement>
    {
        public ReadOnlyCollection<string> PropertyErrors
        {
            get { return (ReadOnlyCollection<string>)GetValue(PropertyErrorsProperty); }
            set { SetValue(PropertyErrorsProperty, value); }
        }

        public string HighlightTextBoxtStyleName
        {
            get { return (string)GetValue(HighlightTextBoxtStyleNameProperty); }
            set { SetValue(HighlightTextBoxtStyleNameProperty, value); }
        }

        public string HighlightComboBoxStyleName
        {
            get { return (string)GetValue(HighlightComboBoxtStyleNameProperty); }
            set { SetValue(HighlightComboBoxtStyleNameProperty, value); }
        }

        public string HighlightSfDataGridStyleName
        {
            get { return (string)GetValue(HighlightSfDataGridtStyleNameProperty); }
            set { SetValue(HighlightSfDataGridtStyleNameProperty, value); }
        }

        public static DependencyProperty PropertyErrorsProperty =
            DependencyProperty.RegisterAttached("PropertyErrors", typeof(ReadOnlyCollection<string>), typeof(HighlightFormFieldOnErrors), new PropertyMetadata(BindableValidator.EmptyErrorsCollection, OnPropertyErrorsChanged));

        public static DependencyProperty HighlightTextBoxtStyleNameProperty =
            DependencyProperty.RegisterAttached("HighlightTextBoxtStyle", typeof(string), typeof(HighlightFormFieldOnErrors), new PropertyMetadata("HighlightTextBoxtStyle"));

        public static DependencyProperty HighlightComboBoxtStyleNameProperty =
    DependencyProperty.RegisterAttached("HighlightComboBoxtStyle", typeof(string), typeof(HighlightFormFieldOnErrors), new PropertyMetadata("HighlightComboBoxtStyle"));

        public static DependencyProperty HighlightSfDataGridtStyleNameProperty =
DependencyProperty.RegisterAttached("HighlightSfDataGridtStyle", typeof(string), typeof(HighlightFormFieldOnErrors), new PropertyMetadata("HighlightSfDataGridtStyle"));

        private static void OnPropertyErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (args == null || args.NewValue == null)
            {
                return;
            }

            var control = ((Behavior<FrameworkElement>)d).AssociatedObject;

            var propertyErrors = (ReadOnlyCollection<string>)args.NewValue;
            Style style = null;
            if (control.Visibility == Visibility.Visible)
            {
                if (control.GetType().Equals(typeof(TextBox)))
                {
                    style = (propertyErrors.Any()) ? (Style)Application.Current.Resources[((HighlightFormFieldOnErrors)d).HighlightTextBoxtStyleName] : null;

                }

                if (control.GetType().Equals(typeof(ComboBox)))
                {
                    style = (propertyErrors.Any()) ? (Style)Application.Current.Resources[((HighlightFormFieldOnErrors)d).HighlightComboBoxStyleName] : null;
                }

                if (control.GetType().Equals(typeof(SfDataGrid)))
                {
                    style = (propertyErrors.Any()) ? (Style)Application.Current.Resources[((HighlightFormFieldOnErrors)d).HighlightSfDataGridStyleName] : null;
                }

                control.Style = style;
            }
        }

        protected override void OnAttached()
        {

        }

        protected override void OnDetached()
        {

        }
    }
}
