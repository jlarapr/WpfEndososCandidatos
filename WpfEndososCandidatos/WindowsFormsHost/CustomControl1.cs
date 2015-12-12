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

namespace WindowsFormsHost
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WindowsFormsHost"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WindowsFormsHost;assembly=WindowsFormsHost"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CustomControl1 : Control
    {
        public static readonly DependencyProperty FilePathProperty
            = DependencyProperty.Register("FilePath", typeof(string), typeof(WpfAcrobatCtrl), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, new PropertyChangedCallback(WpfAcrobatCtrl.FilePathChanged)));


        private string _filePath = string.Empty;
        private CustomControl1 _customAcrobatCtrl;


        private static void FilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WpfAcrobatCtrl)d).FilePathChanged((string)e.OldValue, (string)e.NewValue);
            CommandManager.InvalidateRequerySuggested();
        }

        public WpfAcrobatCtrl()
        {
            InitializeComponent();
            _customAcrobatCtrl = new CustomAcrobatCtrl();
            wpfWindowsFormsHostCtrl.Child = _customAcrobatCtrl;

        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                this.SetValue(FilePathProperty, value);
            }
        }


        private void FilePathChanged(string oldFilePath, string newFilePath)
        {
            _filePath = newFilePath;
            _customAcrobatCtrl.LoadFile(_filePath);
        }



        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }
    }
}
