 
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace APLan.Converters
{
    public class PathToPathFigureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Geometry g = (Geometry)value;
            PathSegment pathSegment = g.GetFlattenedPathGeometry().Figures[0].Segments[0];
            if(pathSegment is LineSegment)
            {
                LineSegment l = (LineSegment)pathSegment;
                return l.Point.X;
            }
            return null;
        }
            
            
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
             


            throw new NotImplementedException();
        }
    }
}
