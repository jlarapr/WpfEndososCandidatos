namespace WpfEndososCandidatos.ViewModel
{
    using jolcode;
    using jolcode.Base;
    using jolcode.MyInterface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WpfEndososCandidatos.View;
    class vmMantDB : ViewModelBase<IDialogView>
    {
        private string _dfPathtoPictures_txt;
        private RelayCommand _InitWindow;

        public vmMantDB()
            : base(new wpfMantDB())
        { }


        public RelayCommand InitWindow
        {
            get
            {
                if (_InitWindow == null)
                {
                    _InitWindow = new RelayCommand(param => OnInitWindow());
                }
                return _InitWindow;
            }
        }

        private object OnInitWindow()
        {
            throw new NotImplementedException();
        }

        public string dfPathtoPictures_txt
        {
            get
            {
                return _dfPathtoPictures_txt;
            }
            set
            {
                if (_dfPathtoPictures_txt != value)
                {
                    _dfPathtoPictures_txt = value;
                    this.RaisePropertychanged("dfPathtoPictures_txt");
                }
            }
        }


        public void OnShow()
        {
            this.View.ShowDialog();
        }

    }//end
}//end
