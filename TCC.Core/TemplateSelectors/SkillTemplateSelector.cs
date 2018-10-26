﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TCC.Data;

namespace TCC.TemplateSelectors
{
    public class SkillTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RoundTemplate { get; set; }
        public DataTemplate SquareTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return Settings.SkillShape == ControlShape.Round ? RoundTemplate : SquareTemplate;
        }

    }
}
