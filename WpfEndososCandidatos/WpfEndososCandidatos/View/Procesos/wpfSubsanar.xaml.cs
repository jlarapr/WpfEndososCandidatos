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

namespace WpfEndososCandidatos.View.Procesos
{
    /// <summary>
    /// Interaction logic for wpfSubsanar.xaml
    /// </summary>
    public partial class wpfSubsanar : Window, IDialogView
    {
        public wpfSubsanar()
        {
            InitializeComponent();
        }

        private void txtTotalEndososEndososEnLaRadicacion_Copy_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}