using ACadSharp.Entities;
using APLan.ViewModels;
using ERDM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace APLan.Model.Converters
{
    public class GetGeoCoordinatesConverter : IValueConverter
    {
        public static ERDM.ERDMmodel erdmmodel;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value is string)
            {
                List<ERDM.Tier_0.GeoCoordinates> geoCoordinatesList = erdmmodel.Tier0.GeoCoordinates;
                foreach(ERDM.Tier_0.GeoCoordinates geoCoordinates in geoCoordinatesList)
                {
                    if(geoCoordinates.id == (string)value)
                    {
                        System.Windows.Point requirePoint = new System.Windows.Point((double)geoCoordinates.xCoordinate, (double)geoCoordinates.yCoordinate);

                        if(DrawViewModel.GlobalDrawingPoint.X == 0)
                        {
                            DrawViewModel.GlobalDrawingPoint.X = requirePoint.X;
                            DrawViewModel.GlobalDrawingPoint.Y = requirePoint.Y;
                        }
                        return new System.Windows.Point((requirePoint.X - DrawViewModel.GlobalDrawingPoint.X) + DrawViewModel.sharedCanvasSize / 2
                         , -(requirePoint.Y - DrawViewModel.GlobalDrawingPoint.Y) + DrawViewModel.sharedCanvasSize / 2);
                    }
                }
            }            

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
