using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace jolcode.MyInterface
{
  public  interface IDialogViewListBox : IView
    {
        bool? ShowDialog();
        bool? DialogResult { get; set; }
        System.Windows.Window Owner { get; set; }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Dispatcher Dispatcher { get; }
        ListBox lsAll { get; set; }
        ListBox lsValid { get; set; }
    }
}
