using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace WpfEndososCandidatos.Helper
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly  DependencyProperty SecurePasswordProperty = DependencyProperty.Register(
            "SecurePassword", typeof(SecureString), typeof(BindablePasswordBox), new PropertyMetadata(default(SecureString)));

        public SecureString SecurePassword
        {
            get
            {
                return (SecureString)GetValue(SecurePasswordProperty);
            }
            set
            {
                SetValue(SecurePasswordProperty, value);
            }
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            SecurePassword = ((PasswordBox)sender).SecurePassword;
        }

        private void BindablePasswordBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Focus();
        }

        

    }
}
