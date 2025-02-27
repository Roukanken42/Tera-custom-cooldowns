﻿using Nostrum.WPF.Controls;
using Nostrum.WPF.Factories;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using TCC.ViewModels.ClassManagers;

namespace TCC.UI.Controls.Classes
{
    public partial class BrawlerLayout
    {
        private BrawlerLayoutVM? _dc;
        private readonly DoubleAnimation _an;

        public BrawlerLayout()
        {
            _an = AnimationFactory.CreateDoubleAnimation(150, to: 400);
            InitializeComponent();
        }

        private void BrawlerLayout_OnLoaded(object sender, RoutedEventArgs e)
        {
            _dc = (BrawlerLayoutVM)DataContext;
            _dc.StaminaTracker.PropertyChanged += ST_PropertyChanged;
        }

        private void ST_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(_dc.StaminaTracker.Factor)) return;
            if (_dc == null) return;
            _an.To = _dc.StaminaTracker.Factor * (359.99 - 2 * MainReArc.StartAngle) + MainReArc.StartAngle;
            MainReArc.BeginAnimation(Arc.EndAngleProperty,_an);
        }
    }
}
