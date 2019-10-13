﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TCC.Windows.Widgets
{
    public class TccPopup : Popup
    {
        public double MouseLeaveTolerance
        {
            get => (double)GetValue(MouseLeaveToleranceProperty);
            set => SetValue(MouseLeaveToleranceProperty, value);
        }

        public static readonly DependencyProperty MouseLeaveToleranceProperty = DependencyProperty.Register("MouseLeaveTolerance",
            typeof(double),
            typeof(TccPopup),
            new PropertyMetadata(0D));

        public TccPopup()
        {
            WindowManager.ForegroundManager.VisibilityChanged += OnVisiblityChanged;
            FocusManager.ForegroundChanged += OnForegroundChanged;
        }


        protected override void OnOpened(EventArgs e)
        {
            FocusManager.PauseTopmost = true;
            base.OnOpened(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            FocusManager.PauseTopmost = false;
        }

        private void OnForegroundChanged()
        {
            if (FocusManager.IsForeground) return;
            Dispatcher?.Invoke(() => IsOpen = false);
        }

        private void OnVisiblityChanged()
        {
            if (WindowManager.ForegroundManager.Visible) return;
            Dispatcher?.Invoke(() => IsOpen = false);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (Child == null) return;
            var content = (FrameworkElement) Child;
            var pos = e.MouseDevice.GetPosition(content);
            if ((pos.X > MouseLeaveTolerance && pos.X < content.ActualWidth - MouseLeaveTolerance)
                && (pos.Y > MouseLeaveTolerance && pos.Y < content.ActualHeight - MouseLeaveTolerance)) return;
            IsOpen = false;
        }
    }
}