namespace jolcode.Base
{
    using jolcode.MyInterface;
    using System.ComponentModel;
    using System.Windows.Input;

    public class ViewModelBase<ViewType> : INotifyPropertyChanged
        where ViewType : IView
    {
       
         public event PropertyChangedEventHandler PropertyChanged;
        private readonly ViewType view;
        public ViewType View
        {
            get
            {
                return this.view;
            }
        }
        public ViewModelBase(ViewType view)
        {
            this.view = view;
            this.view.DataContext = this;
            //this.view.Loaded += view_Loaded;
         
        }

        

        void view_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            KeyEventArgs e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab);
            e1.RoutedEvent = Keyboard.KeyDownEvent;
            InputManager.Current.ProcessInput(e1);
        }
        public void RaisePropertychanged(string proertyName)
        {
            if (this.PropertyChanged !=null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(proertyName));
            }
        }
    }//end
}
