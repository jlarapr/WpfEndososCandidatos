using System.ComponentModel;
using System.Windows.Threading;

namespace jolcode.MyInterface
{
    public interface IMainView : IView
    {
        void Show();
        [EditorBrowsable(EditorBrowsableState.Advanced)]
         Dispatcher Dispatcher { get; }
    }
}
