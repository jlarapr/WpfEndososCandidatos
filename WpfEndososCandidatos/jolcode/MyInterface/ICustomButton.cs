namespace jolcode.MyInterface
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    public interface ICustomButton
    {
       object DataContext { get; set; }
       RelayCommand OnClick { get; }
       event PropertyChangedEventHandler PropertyChanged;

       Brush GetColor {get;set;}

     //   Action ClickAction { get; set; }

    }
}
