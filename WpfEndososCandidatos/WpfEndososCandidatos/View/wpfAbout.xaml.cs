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

namespace WpfEndososCandidatos.View
{
    /// <summary>
    /// Interaction logic for wpfAbout.xaml
    /// </summary>
    public partial class wpfAbout : Window, IDialogView
    {
        public wpfAbout()
        {
            InitializeComponent();
        }
    }
}
