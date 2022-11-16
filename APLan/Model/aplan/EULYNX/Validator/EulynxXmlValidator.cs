using System;
using System.Collections;
using System.Collections.Generic;

namespace aplan.eulynx.validator
{
    internal class EulynxXmlValidator
    {
        //instance
        private static EulynxXmlValidator eulynxXmlValidator;
        private ValidatorSchema validatorSchema;
        //private ValidatorSchematron validatorSchematron;
        public string inputUri { get; set; }

        //singleton constructor
        private EulynxXmlValidator() { }

        //singleton method
        public static EulynxXmlValidator getInstance()
        {
            if (eulynxXmlValidator == null)
            {
                eulynxXmlValidator = new EulynxXmlValidator();
            }
            return eulynxXmlValidator;
        }

        public string validate(string _inputUri, string version , List<string> NameSpaces)
        {
            string report=null;
            // Schema 
            validatorSchema = new ValidatorSchema(_inputUri, version,  NameSpaces);
            validatorSchema.validate();
            report =validatorSchema.makeReport();

            // Schematron
            //validatorSchematron = new ValidatorSchematron(inputUri);
            //validatorSchematron.validate();
            //validatorSchematron.makeReport();
            return report;
        }

        //public void reportConsole()
        //{
        //    Console.WriteLine(validatorSchema.Status | validatorSchematron.Status);
        //}

        public ArrayList XSDvalidationVersionCheck(ArrayList nameSapces)
        {
            ArrayList Version_NameSpaces = new ArrayList();
            string version = "";

            List<string> version1_0 = new List<string>();

            version1_0.Add("http://dataprep.eulynx.eu/schema/DB/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/Generic/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/NR/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/ProRail/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/RFI/1.0");
            version1_0.Add("http://www.railsystemmodel.org/schemas/Common/1.2");
            version1_0.Add("http://www.railsystemmodel.org/schemas/NetEntity/1.2");
            version1_0.Add("http://www.railsystemmodel.org/schemas/Signalling/1.2");
            version1_0.Add("http://www.railsystemmodel.org/schemas/Track/1.2");
            version1_0.Add("http://dataprep.eulynx.eu/schema/Signalling/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/SNCF/1.0");
            version1_0.Add("http://dataprep.eulynx.eu/schema/TRV/1.0");


            List<string> version1_1 = new List<string>();

            version1_1.Add("http://dataprep.eulynx.eu/schema/DB/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/Generic/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/NR/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/ProRail/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/RFI/1.1");
            version1_1.Add("http://www.railsystemmodel.org/schemas/Common/202206");
            version1_1.Add("http://www.railsystemmodel.org/schemas/NetEntity/202206");
            version1_1.Add("http://www.railsystemmodel.org/schemas/Signalling/202206");
            version1_1.Add("http://www.railsystemmodel.org/schemas/Track/202206");
            version1_1.Add("http://dataprep.eulynx.eu/schema/Signalling/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/SNCF/1.1");
            version1_1.Add("http://dataprep.eulynx.eu/schema/TRV/1.1");
 

            foreach (string nSpace in nameSapces)
            {
                version = "1.0";
                if (!version1_0.Contains(nSpace))
                {
                    version = "";
                    break;
                }
            }
            if (!version.Equals(""))
            {
                Version_NameSpaces.Add(version);
                Version_NameSpaces.Add(version1_0);
                return Version_NameSpaces;
            }
            else
            {
                foreach (string nSpace in nameSapces)
                {
                    version = "1.1";
                    //Version_NameSpaces.RemoveAt(0);
                    //Version_NameSpaces.Insert(0, version);
                    if (!version1_1.Contains(nSpace))
                    {
                        version = "";
                        break;
                    }
                }
            }
            if (!version.Equals(""))
            {
                Version_NameSpaces.Add(version);
                Version_NameSpaces.Add(version1_1);
                return Version_NameSpaces;
            }
            return Version_NameSpaces;
        }
    }
}
