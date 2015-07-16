namespace jolcode.MyInterface
{
    public interface IDialogView : IView
    {
        bool? ShowDialog();
        bool? DialogResult { get; set; }
        System.Windows.Window Owner { get; set; }
    }
}
