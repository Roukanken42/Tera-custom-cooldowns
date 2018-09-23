﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TCC.Data;

namespace TCC.Converters
{
    class BossToLaurelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Npc npc)) return null;
            var laurel = "";

            switch (npc.ZoneId)
            {
                case 620: //Balder's Refuge guardian
                         if (npc.TemplateId == 1000) laurel = "gold";
                    else if (npc.TemplateId == 1001) laurel = "bronze";
                    else if (npc.TemplateId == 1004) laurel = "bronze";
                    else if (npc.TemplateId == 1005) laurel = "bronze";
                    break;
                case 621: //Crack the crustacean guardian
                    if (npc.TemplateId == 3001) laurel = "gold";
                    break;
                case 622: //Dreadreaper guardian
                    if (npc.TemplateId == 1000) laurel = "gold";
                    break;
                case 713: //Ghillieglade
                case 813: //Ghillieglade
                         if (npc.TemplateId == 81301) laurel = "silver";
                    else if (npc.TemplateId == 81398) laurel = "bronze";
                    else if (npc.TemplateId == 81399) laurel = "bronze";

                    break;
                case 628: //Superior guardian
                    if (npc.TemplateId == 1000) laurel = "champion";
                    else if (npc.TemplateId == 3000) laurel = "diamond";
                    else if (npc.TemplateId == 3001) laurel = "diamond";
                    break;
                case 434: //Dreadspire
                    if (npc.TemplateId == 1000 || npc.TemplateId == 2000) laurel = "bronze";
                    else if (npc.TemplateId == 3000 || npc.TemplateId == 4000) laurel = "silver";
                    else if (npc.TemplateId == 5000 || npc.TemplateId == 6000) laurel = "gold";
                    else if (npc.TemplateId == 7000 || npc.TemplateId == 8000 || npc.TemplateId == 9000) laurel = "diamond";
                    else if (npc.TemplateId == 10000) laurel = "champion";
                    break;
                default:
                    if (npc.TemplateId == 1000 || npc.TemplateId == 1001) laurel = "bronze";
                    else if (npc.TemplateId == 2000 || npc.TemplateId == 1002) laurel = "silver";
                    else if (npc.TemplateId == 3000 || npc.TemplateId == 1003) laurel = "gold";
                    break;
            }



            return "../resources/images/Icon_Laurels/" + laurel + "_kr_big.png";


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
