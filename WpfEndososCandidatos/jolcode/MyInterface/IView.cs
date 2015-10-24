namespace jolcode.MyInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

   public interface IView
    {
        object DataContext { get; set; }
        void Close();
        event CancelEventHandler Closing;
        event MouseEventHandler MouseMove;
        event MouseButtonEventHandler MouseLeftButtonDown;
        event RoutedEventHandler Loaded;
        event EventHandler Activated;
    }
}
