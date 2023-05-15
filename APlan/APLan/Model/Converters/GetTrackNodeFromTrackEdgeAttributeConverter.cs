using ACadSharp.Entities;
using APLan.ViewModels;
using ERDM;
using ERDM.Tier_0;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace APLan.Model.Converters
{
    public class GetTrackNodeFromTrackEdgeAttributeConverter : IValueConverter
    {
        public static ERDM.ERDMmodel erdmmodel;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else if(value is string)
            {
                List<ERDM.Tier_1.TrackNode> trackNodeList = erdmmodel.Tier1.TrackNode;
                foreach (ERDM.Tier_1.TrackNode trackNode in trackNodeList)
                {
                    if(trackNode.id == (string)value)
                    {
                        GetGeoCoordinatesConverter newConv = new GetGeoCoordinatesConverter();
                        System.Windows.Point requiredPoint = (System.Windows.Point)newConv.Convert(trackNode.isLocatedAtGeoCoordinates, null, null, null);
                        
                        
                        if (DrawViewModel.GlobalDrawingPoint.X == 0)
                        {
                            DrawViewModel.GlobalDrawingPoint.X = requiredPoint.X;
                            DrawViewModel.GlobalDrawingPoint.Y = requiredPoint.Y;
                        }

                        return requiredPoint;
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
