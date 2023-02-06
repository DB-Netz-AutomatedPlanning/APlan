﻿using RCA_Model.Tier_0;
using RCA_Model.Tier_1;
using RCA_Model.Tier_2;
using RCA_Model.Tier_3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERDM_Implementation
{
    public class ERDMhelperFunctions
    {
        public static TrackEdgeSection CreateOrFindTrackEdgeSection(double x1, double y1, double z1, double offset1, double x2, double y2, double z2, double offset2,string? segmentID,string? trackEdgeID,double length,MapData? mapData, RCA_Model.Tier_0.Version verison)
        {
        
            TrackEdge trackEdge = mapData?.consistsOfTier1Objects?.Find(x => x is TrackEdge && x.name.Equals(trackEdgeID)) as TrackEdge;

            var startTrackEdge = CreatOrFindTrackEdgePoint(x1, y1, z1,offset1,trackEdge ,mapData, verison);
            var endTrackEdge = CreatOrFindTrackEdgePoint(x2, y2, z2, offset1,trackEdge, mapData, verison);

            // necessary composition for creation of a segment.
            TrackEdgeSection TES = new();

            // assign ids.
            TES.id = Guid.NewGuid().ToString();
            TES.name = segmentID;
            TES.length = length;

            TES.hasStartTrackEdgePoint = startTrackEdge.id;
            TES.hasEndTrackEdgePoint = endTrackEdge.id;
            TES.version = verison.id;
            TES.isPartOfTrackEdge = trackEdge?.id;

            mapData?.consistsOfTier2Objects?.Add(TES);

            return TES;
        }
        /// <summary>
        /// find of create geoCoordinate based on X,Y and Z coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="mapData"></param>
        /// <returns></returns>
        public static GeoCoordinates CreatOrFindGeoCoordinates(double x, double y, double z, MapData mapData)
        {
            //search for existing geoCoordinate
            if (mapData.consistsOfTier0Objects!=null)
            {
                foreach (var item in mapData.consistsOfTier0Objects)
                {
                    if (item is GeoCoordinates)
                    {
                        var geoCoordinate = item as GeoCoordinates;
                        if (geoCoordinate?.xCoordinate == x && geoCoordinate?.yCoordinate == y && geoCoordinate?.zCoordinate == z)
                        {
                            return geoCoordinate;
                        }
                    }
                }
            }

            //create a new one.
            var newGeoCoordinate = CreateNewGeoCoordinates(x, y, z, mapData);
            return newGeoCoordinate;
        }
        /// <summary>
        /// find of create TrackEdgePoint based on X,Y and Z coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="mapData"></param>
        /// <returns></returns>
        public static TrackEdgePoint CreatOrFindTrackEdgePoint(double x, double y, double z,double offset,TrackEdge trackEdge, MapData mapData, RCA_Model.Tier_0.Version version)
        {
            //search for existing geoCoordinate
            var geoCoordiante = CreatOrFindGeoCoordinates(x,y,z,mapData);

            var trackEdgePoint = mapData?.consistsOfTier2Objects?.Find(x => x is TrackEdgePoint && (x as TrackEdgePoint).isLocatedAtGeoCoordinates.Equals(geoCoordiante.id)) as TrackEdgePoint;

            //if found
            if (trackEdgePoint != null)
                return trackEdgePoint;

            //create a new one.
            return CreateNewTrackEdgePoint(x, y, z, offset, trackEdge, mapData,version);
        }
        /// <summary>
        /// create trackEdge using start and end nodes
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <param name="mapData"></param>
        /// <param name="version"></param>
        public static void CreateNewTrackEdge(string? name,double length, TrackNode? startNode, TrackNode? endNode, MapData mapData, RCA_Model.Tier_0.Version version)
        {
            //Create new one if not found.
            var trackEdge = new TrackEdge()
            {
                id = Guid.NewGuid().ToString(),
                name = name,
                version = version.id,
                length = length,
                //gauge = new List<TrackEdgeGauge?>() { TrackEdgeGauge._750 }, //introduced for validation.
                hasStartTrackNode = startNode?.id,
                hasEndTrackNode = endNode?.id
            };
            // add to Tier0
            mapData.consistsOfTier1Objects?.Add(trackEdge);
        }

        /// <summary>
        /// create new geoCoordinates based on X,Y and Z values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="mapData"></param>
        /// <returns></returns>
        private static GeoCoordinates CreateNewGeoCoordinates(double x, double y, double z, MapData mapData)
        {
            var newGeoCoordinate = new GeoCoordinates()
            {
                id = Guid.NewGuid().ToString(),
                //alphanumerical name.
                name = Convert.ToInt32(x).ToString() + "A" + Convert.ToInt32(y).ToString() + "A" + Convert.ToInt32(z).ToString(),
                xCoordinate = x,
                yCoordinate = y,
                zCoordinate = z,
            };
            // add to Tier0
            mapData.consistsOfTier0Objects?.Add(newGeoCoordinate);
            return newGeoCoordinate;
        }

        /// <summary>
        /// create new TrackEdgePoint based on a geoCoordinate
        /// </summary>
        /// <param name="geoCoordinate"></param>
        /// <param name="offset"></param>
        /// <param name="mapdata"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static TrackEdgePoint CreateNewTrackEdgePoint(double x, double y, double z, double offset,TrackEdge trackEdge, MapData mapData,RCA_Model.Tier_0.Version version)
        {
            //find geoCoordinate with the same x,y and z or create one.
            var geoCoordiante = CreatOrFindGeoCoordinates(x, y, z, mapData);

            TrackEdgePoint trackEdgePoint = new TrackEdgePoint()
            {
                id = Guid.NewGuid().ToString(),
                isLocatedAtGeoCoordinates = geoCoordiante.id,
                offset = offset,
                name = geoCoordiante.name,
                version = version.id,
                isPositionedOnTrackEdge = trackEdge.id
            };
            mapData.consistsOfTier2Objects?.Add(trackEdgePoint);
            return trackEdgePoint;
        }

        /// <summary>
        /// create new TrakNode based on geoCoordinate
        /// </summary>
        /// <param name="geoCoordiante"></param>
        /// <param name="name"></param>
        /// <param name="mapData"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static TrackNode CreateNewTrackNode(GeoCoordinates geoCoordiante, string? name, MapData mapData, RCA_Model.Tier_0.Version version)
        {
            TrackNode node = new TrackNode()
            {
                id = Guid.NewGuid().ToString(),
                name = name,
                isLocatedAtGeoCoordinates = geoCoordiante.id,
                version = version.id,
                nodeType = TrackNodeType.EndOfTrack
            };
            mapData?.consistsOfTier1Objects?.Add(node);
            return node;
        }

        public static List<string> ExtractTrackEdgeSectionsForGradients(double startKM, double endKM,MapData mapData)
        {
            var trackEdgeSections = new List<string>();
            var result = mapData.consistsOfTier2Objects.FindAll(x => x is TrackEdgePoint && (((TrackEdgePoint)x).offset >= startKM && ((TrackEdgePoint)x).offset <= endKM));

            foreach (var item in mapData.consistsOfTier2Objects)
            {
                if (item is TrackEdgeSection)
                {
                    var trackEdgeSection = (TrackEdgeSection)item;

                    var sections = result.FindAll(x => x.id.Equals(trackEdgeSection.hasStartTrackEdgePoint) || x.id.Equals(trackEdgeSection.hasEndTrackEdgePoint));
                    if (sections.Count!=0)
                    {
                        trackEdgeSections.Add(trackEdgeSection.id);
                    }
                }

            }
            return trackEdgeSections;
        }

        /// <summary>
        /// Try to parse a double value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double parseDouble(string? value)
        {
            double.TryParse(value, out var x);
            return x;
        }

        /// <summary>
        /// create SHA512 value from string input
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string CreateSHA512(string strData)
        {
            var message = Encoding.UTF8.GetBytes(strData);
            using (var alg = SHA512.Create())
            {
                string hex = "";
                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                // Truncate the string to 128 characters
                hex = hex.Substring(0, 128);
                return hex;
            }
        }

        /// <summary>
        /// extract length of track edge from the xls file of horizontal segments
        /// </summary>
        /// <param name="edgeID"></param>
        /// <param name="segmentsXlsItems"></param>
        /// <returns></returns>
        public static double ExtractEdgeLengthFromSegmentsFile(string? edgeID,ArrayList segmentsXlsItems)
        {
            double length=0.1;

            foreach (Dictionary<string, string> dict in segmentsXlsItems)
            {
                dict.TryGetValue("Track_Edge", out string? edge);
                if (edge!=null && edge.Equals(edgeID))
                {
                    dict.TryGetValue("Length [km]", out string? segmentLength);
                    var len = parseDouble(segmentLength);
                    length += len*1000; //file unit is km
                }
            }

            return length;
        }
        /// <summary>
        /// replace non alphanumerical charachters with AaA
        /// </summary>
        /// <returns></returns>
        public static string ReplaceNonAlphaNumericalChar(string input) {

            string output="";
            
            foreach(char a in input.ToArray())
            {
                if (!char.IsLetterOrDigit(a))
                {
                    output += "AaA";
                }
                else
                {
                    output += a;
                }
            }
            return output;
        }
    }
}