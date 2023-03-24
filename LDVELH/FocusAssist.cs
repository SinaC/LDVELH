using System.Windows;
using System.Windows.Controls;

namespace LDVELH
{
    public static class FocusAssist
    {
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
            "IsFocused",
            typeof(bool),
            typeof(FocusAssist),
            new PropertyMetadata(false, null, OnIsFocusedPropertyChanged));

        public static bool GetIsFocused(DependencyObject obj) => (bool)obj.GetValue(IsFocusedProperty);
        public static void SetIsFocused(DependencyObject obj, bool value) => obj.SetValue(IsFocusedProperty, value);

        private static object OnIsFocusedPropertyChanged(DependencyObject obj, object value)
        {
            if (obj is UIElement uie && (bool)value)
            {
                uie.Focus(); // Don't care about false values.
                if (uie is TextBox textBox)
                    textBox.SelectAll();
            }
            return value;
        }
    }
}
