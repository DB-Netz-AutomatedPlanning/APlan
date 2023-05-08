using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace aplan.eulynx.validator
{
    internal class ValidatorSchema
    {
        private string inputUri; // eulynx XML File
        private string[] xsd_FilePath;
        private List<string> targetNamespace;

        private static List<string> xmlSchemaReport;
        //private int status = 0; // success state -> 0: no error and warning, 1: error/s and warning/s
        public int Status
        { get { return (xmlSchemaReport.Count) == 0 ? 0 : 1; } }

        public ValidatorSchema(string inputUri,string version , List<string> NameSpaces)
        {
            this.inputUri = inputUri;
            targetNamespace = NameSpaces;
            attachXSDwithNameSpace(version);
            xmlSchemaReport = new List<string>();
        }
        public void attachXSDwithNameSpace(string version)
        {
            switch (version)
            {
                case "1.0":
                    xsd_FilePath = new string[]
                    {
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/DB.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/Generic.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/NR.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/ProRail.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/RFI.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/RsmCommon.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/RsmNetEntity.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/RsmSignalling.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/RsmTrack.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/Signalling.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/SNCF.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.0_Schema/TRV.xsd"
                    };
                    break;
                case "1.1":
                    xsd_FilePath = new string[]
                    {
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/DB.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/Generic.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/NR.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/ProRail.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/RFI.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/RsmCommon.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/RsmNetEntity.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/RsmSignalling.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/RsmTrack.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/Signalling.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/SNCF.xsd",
                         System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Validate/Validator/EULYNX_DP_V1.1_Schema/TRV.xsd"
                    };
                    break;
                default:
                    // code block
                    break;
            }

        }
        public void validate()
        {
            // Set the XMLReaderSettings
            // - Schema type (XSD)
            // - Generate XmlSchemaSet and add them to settings
            // - Add event handler (catch the error if validation is not match)
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas.Add(generateSchemaSet(targetNamespace, xsd_FilePath));
            settings.ValidationEventHandler += new ValidationEventHandler(validationCallback);

            // Create the XmlReader object
            XmlReader xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(@inputUri)), settings);

            // Parse the file
            while (xmlReader.Read()) { }

            // Shows that validation is completed
            // Console.WriteLine("Schema Validation completed");

            // Close xmlReader object
            xmlReader.Close();   
        }

        public string makeReport()
        {
            string report = null;
            // Set a variable to the Documents path.
            //string docPath = Environment.CurrentDirectory;

            // Write the string array to a new file named "logs.txt".
            //DirectoryInfo di = Directory.CreateDirectory(@"..\..\..\report");
            if (xmlSchemaReport.Count == 0)
            {
                return "Validation is Successful";
            }
            else
            {
                foreach (string line in xmlSchemaReport)
                {
                    report += line;
                }
            }
            return report;

        }

        private XmlSchemaSet generateSchemaSet(List<string> tNs, string[] schemaPaths)
        {
            // Create the XmlSchemaSet class
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            // Add the schemas to the collection
            for (int i = 0; i < tNs.Count; i++)
            {
                schemaSet.Add(tNs[i], schemaPaths[i]);
            }

            return schemaSet;
        }

        private static void validationCallback(object sender, ValidationEventArgs args)
        {
            // Console output
            string msg = String.Format(
                    args.Severity +
                    ": Line: {0}, Position {1}: \"{2}\" \n",
                    args.Exception.LineNumber,
                    args.Exception.LinePosition,
                    args.Exception.Message);
            // Console.WriteLine(msg);

            // Report File
            xmlSchemaReport.Add(msg);
        }
    }
}
