﻿using APLan.Model.CustomObjects;
using APLan.ViewModels;
using Newtonsoft.Json.Schema;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace APLan.Converters
{
    public class CoodrinatesSinglePointConverter : IValueConverter
    { 
        /// <summary>
        /// Convert a point coordinates due to difference of Canvas coordinate system, and translate the point to middle of Canvas.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // remove from the point the GlobalDrawingPoint which represent the first point in the whole drawing to make the drawing near.
            // then convert it to be represented as cartesian coodrinate
            // then transalte it to the middle of the canvas
            if (value is CustomPoint)
            {
                return new Point((((CustomPoint)value).Point.X - DrawViewModel.GlobalDrawingPoint.X) + DrawViewModel.sharedCanvasSize / 2
                          , -(((CustomPoint)value).Point.Y - DrawViewModel.GlobalDrawingPoint.Y) + DrawViewModel.sharedCanvasSize / 2);
            }           
            else
            {
                return new Point((((Point)value).X - DrawViewModel.GlobalDrawingPoint.X) + DrawViewModel.sharedCanvasSize / 2
                         , -(((Point)value).Y - DrawViewModel.GlobalDrawingPoint.Y) + DrawViewModel.sharedCanvasSize / 2);
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
