﻿using jolcode.MyInterface;
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
using System.Windows.Shapes;

namespace WpfEndososCandidatos.View.Informes
{
    /// <summary>
    /// Interaction logic for wpfInformes.xaml
    /// </summary>
    public partial class wpfInformes : Window, IDialogView
    {
        public wpfInformes()
        {
            InitializeComponent ();
        }
    }
}