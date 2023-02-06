using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace APLan.HelperClasses
{
    public class RulesValidator
    {
        private string euxmlPath;
        private XmlDocument doc;
        public RulesValidator(string euxmlPath)
        {
            this.euxmlPath = euxmlPath;
            this.doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(euxmlPath);
        }
        /// <summary>
        /// Run all rules to be tested according to the German book.
        /// </summary>
        /// <returns></returns>
        public string runRulesTesting()
        {
            string report = "";

            ArrayList mainSignalAssetId = getMainSignalAssetId();
            List<XmlNode> mainSignalAsset = getmainSignalAsset(mainSignalAssetId);
            List<XmlNode> mainRsmSignal = getmainRsmSignal(mainSignalAsset);
            List<XmlNode> mainSignalFrame = getmainSignalFrame(mainSignalAsset);
            List<XmlNode> mainRsmSignalLocation = getmainRsmSignalLocation(mainRsmSignal);
            ArrayList mainSignalFrame_ids = getmainSignalFrame_ids(mainSignalFrame);

            report += Rule_4_8(mainSignalFrame, mainSignalAsset, mainRsmSignal)+"\n";

            report += Rule_4_3(mainRsmSignalLocation, mainRsmSignal) + "\n";

            return report;
        }
        /// <summary>
        /// 4.8  Abstände von Gleismitte
        /// </summary>
        /// <param name="mainSignalFrame"></param>
        /// <param name="mainSignalAsset"></param>
        /// <param name="mainRsmSignal"></param>
        /// <returns></returns>
        private string Rule_4_8(List<XmlNode> mainSignalFrame, List<XmlNode> mainSignalAsset, List<XmlNode> mainRsmSignal)
        {
            string report="Rule 4.8 :" + "\n";

            foreach (XmlNode node in mainSignalFrame)
            {
                XmlNode currSignalAsset = null;
                XmlNode currRsmSignal = null;
                XmlNode horizontalOffsetOfReferencePoint = null;

                horizontalOffsetOfReferencePoint = node.SelectSingleNode("./*[local-name()='hasPosition' and @*[local-name() = 'type']='HorizontalOffsetOfReferencePoint']");

                foreach (XmlNode node2 in mainSignalAsset)
                {
                    var reference = node2.SelectSingleNode("./*[local-name()='hasSignalFrame']").Attributes["ref"].Value;
                    var id = node.SelectSingleNode("./*[local-name()='id']").InnerText;
                    if (reference.Equals(id))
                    {
                        currSignalAsset = node2;
                        break;
                    }
                }

                foreach (XmlNode node3 in mainRsmSignal)
                {
                    var reference = currSignalAsset.SelectSingleNode("./*[local-name()='refersToRsmSignal']").Attributes["ref"].Value;
                    var id = node3.SelectSingleNode("./*[local-name()='id']").InnerText;
                    if (reference.Equals(id))
                    {
                        currRsmSignal = node3;
                    }
                }

                var distance = horizontalOffsetOfReferencePoint.SelectSingleNode("./*[local-name()='value']/*[local-name()='value']").InnerText;


                // -4.8  Abstände von Gleismitte
                // -Maximum distance of mainsignal from the center of the tracks(4.15m)
                if (Convert.ToDouble(distance) >= 4.15)
                {
                    report  +=  "The distance between the center of the signal (with rsmSignal id:" + "\n"
                                + currRsmSignal.SelectSingleNode("./*[local-name()='id']").InnerText +
                                ") screen (main frame) and the center of the track" + "\n" +
                                distance + "m' should not exceed 4.15m"
                                + "\n"
                                + "\n";
                }

            }
            if (report.Equals("Rule 4.8 :" + "\n"))
            {
                return "Rule 4.8 is satisfied";
            }
            return report;
        }
        /// <summary>
        /// 4.3 Regelanordnung
        /// </summary>
        /// <param name="mainRsmSignalLocation"></param>
        /// <param name="mainRsmSignal"></param>
        /// <returns></returns>
        private string Rule_4_3(List<XmlNode> mainRsmSignalLocation, List<XmlNode> mainRsmSignal)
        {
            string report = "Rule 4.3 :" + "\n";
            foreach (XmlNode currSignalLocation in mainRsmSignalLocation)
            {
                XmlNode currRsmSignal = null;

                foreach (XmlNode node3 in mainRsmSignal)
                {
                    var reference = node3.SelectSingleNode("./*[local-name()='locations']").Attributes["ref"].Value;
                    var id = currSignalLocation.SelectSingleNode("./*[local-name()='id']").InnerText;
                    if (reference.Equals(id))
                    {
                        currRsmSignal = node3;
                    }
                }
                // 4.3 Regelanordnung
                // Report if the signal is located to the right or centre but not applied in 'normal' direction
                var x = currSignalLocation.SelectSingleNode("./*[local-name()='associatedNetElements']/*[local-name()='isLocatedToSide' and text()='centre' or (text()='right') ]/../*[local-name()='appliesInDirection' and text()!='normal']/..");
                if (x != null)
                {
                    report +=   "The arrangement of the signal (with rsmSignal id:" + "\n" +
                                currRsmSignal.SelectSingleNode("./*[local-name()='id']").InnerText
                                + "\n" + "in the direction of '" + x.SelectSingleNode("./*[local-name()='appliesInDirection']").InnerText
                                + "\n" + "' cannot be located to the side '"
                                + "\n" + x.SelectSingleNode("./*[local-name()='isLocatedToSide']").InnerText
                                + "\n";
                }

                //-4.3 Regelanordnung
                //-Report if the signal is located to the left or centre but not applied in 'reverse' direction
                //This is somehow contradicting the previous one !!
            }
            if (report.Equals("Rule 4.3 :" + "\n"))
            {
                return "Rule 4.3 is satisfied";
            }
            return report;
        }


        //helper functions.
        private  ArrayList getMainSignalAssetId()
        {
            ArrayList mainSignalAssetId = new ArrayList();
            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='ownsSignalType']");
            foreach (XmlNode xn in xnList)
            {
                if (xn.HasChildNodes)
                {
                    string reference;
                    foreach (XmlNode item in xn.ChildNodes)
                    {
                        if (item.LocalName == "isOfSignalTypeType" && item.InnerText == "main")
                        {
                            mainSignalAssetId.Add(xn.SelectSingleNode("./*[local-name()='appliesToSignal']").Attributes["ref"].Value);
                        }
                    }
                }
            }
            return mainSignalAssetId;
        }

        private List<XmlNode> getmainSignalAsset(ArrayList mainSignalAssetId)
        {
            List<XmlNode> myNodes = new List<XmlNode>();
            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='ownsTrackAsset']/*[local-name()='id']");
            foreach (XmlNode xn in xnList)
            {

                if (mainSignalAssetId.Contains(xn.InnerText))
                {
                    myNodes.Add(xn.ParentNode);
                }
            }
            return myNodes;
        }

        private List<XmlNode> getmainRsmSignal(List<XmlNode> mainSignalAsset)
        {
            List<XmlNode> myNodes = new List<XmlNode>();
            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='ownsSignal']");
            foreach (XmlNode node in mainSignalAsset)
            {
                var refrence = node.SelectSingleNode("./*[local-name()='refersToRsmSignal']").Attributes["ref"].Value;
                foreach (XmlNode xn in xnList)
                {
                    var refrence2 = xn.SelectSingleNode("./*[local-name()='id']").InnerText;
                    if (refrence2 == refrence)
                    {
                        myNodes.Add(xn);
                    }
                }
            }
            return myNodes;
        }

        private List<XmlNode> getmainSignalFrame(List<XmlNode> mainSignalAsset)
        {
            List<XmlNode> myNodes = new List<XmlNode>();
            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='ownsSignalFrame']");
            foreach (XmlNode node in mainSignalAsset)
            {
                var refrence = node.SelectSingleNode("./*[local-name()='hasSignalFrame']").Attributes["ref"].Value;
                foreach (XmlNode xn in xnList)
                {
                    var refrence2 = xn.SelectSingleNode("./*[local-name()='id']").InnerText;
                    var isOfSignalFrameType = xn.SelectSingleNode("./*[local-name()='isOfSignalFrameType']").InnerText;
                    if (refrence2 == refrence && isOfSignalFrameType == "main")
                    {
                        myNodes.Add(xn);
                    }
                }
            }
            return myNodes;
        }

        private List<XmlNode> getmainRsmSignalLocation(List<XmlNode> mainRsmSignal)
        {
            List<XmlNode> myNodes = new List<XmlNode>();
            XmlNodeList xnList = doc.SelectNodes("//*[local-name()='usesLocation']");
            foreach (XmlNode node in mainRsmSignal)
            {
                var refrence = node.SelectSingleNode("./*[local-name()='locations']").Attributes["ref"].Value;
                foreach (XmlNode xn in xnList)
                {
                    var refrence2 = xn.SelectSingleNode("./*[local-name()='id']").InnerText;
                    if (refrence2 == refrence)
                    {
                        myNodes.Add(xn);
                    }
                }
            }
            return myNodes;
        }

        private ArrayList getmainSignalFrame_ids(List<XmlNode> mainSignalFrame)
        {
            ArrayList myIDs = new ArrayList();
            foreach (XmlNode node in mainSignalFrame)
            {
                myIDs.Add(node.SelectSingleNode("./*[local-name()='id']").InnerText);
            }
            return myIDs;
        }

    }
}
