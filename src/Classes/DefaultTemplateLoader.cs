using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using openrmf_templates_api.Models;
using openrmf_templates_api.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace openrmf_templates_api.Classes
{
    public static class DefaultTemplateLoader
    {
        /// <summary>
        /// At startup, reads the directory included below and then one by one, loads the template CKL files
        /// and puts them into the database via a Template record that is a SYSTEM template.
        /// </summary>
        /// <returns>
        ///  No return.
        /// </returns>
        public static bool LoadTemplates() {
            // load the templates
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/templatefiles/";
            // make sure there are files in here
            if (!Directory.Exists(path)) return false;
            // get the list of CKL files in here
            string[] filenames = Directory.GetFiles(path,"*.ckl");
            string rawChecklist = "";
            Template t;
            Settings s = new Settings();
            s.ConnectionString = Environment.GetEnvironmentVariable("MONGODBCONNECTION");
            s.Database = Environment.GetEnvironmentVariable("MONGODB");
            TemplateRepository _templateRepo = new TemplateRepository(s);
            // remove old ones first
            if (filenames.Length > 0) _templateRepo.RemoveSystemTemplates().Wait();
            // cycle through the files
            try {
                foreach (string file in filenames) {
                    // read in the file
                    rawChecklist = File.ReadAllText(file);
                    rawChecklist = rawChecklist.Replace("\t","").Replace(">\n<","><");
                    t = MakeTemplateSystemRecord(rawChecklist);
                    // save them to the database
                    _templateRepo.AddTemplate(t).Wait();
                }
            }
            catch (Exception ex) {
                // log the error with the ex.Message
            }

            // return success
            return true;
        }

        /// <summary>
        /// Take the raw checklist and generate a System Template record to save into
        /// the database. This is called from the above routine when this API initially
        /// starts up.
        /// </summary>
        /// <param name="rawChecklist">The long XML string of the checklist</param>
        /// <returns>
        ///  A template record with a GUID that is all 1's, type of SYSTEM template, and the 
        ///  rest of the Template data filled in to save as a database record.
        /// </returns>
        private static Template MakeTemplateSystemRecord(string rawChecklist) {
            Template newArtifact = new Template();
            newArtifact.created = DateTime.Now;
            // create the GUID of all 1's for the SYSTEM GUID
            newArtifact.createdBy = Guid.Parse("11111111-1111-1111-1111-111111111111");
            newArtifact.updatedOn = DateTime.Now;
            newArtifact.rawChecklist = rawChecklist;
            // make these SYSTEM types versus the default USER type
            newArtifact.templateType = "SYSTEM";
            newArtifact.description = "SYSTEM Template loaded by default by OpenRMF for SCAP Scans and Creation Wizard.";

            // parse the checklist and get the data needed
            rawChecklist = rawChecklist.Replace("\n","").Replace("\t","");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rawChecklist);

            XmlNodeList assetList = xmlDoc.GetElementsByTagName("ASSET");
            // get the title and release which is a list of children of child nodes buried deeper :face-palm-emoji:
            XmlNodeList stiginfoList = xmlDoc.GetElementsByTagName("STIG_INFO");
            foreach (XmlElement child in stiginfoList.Item(0).ChildNodes) {
                if (child.FirstChild.InnerText == "releaseinfo")
                    newArtifact.stigRelease = child.LastChild.InnerText;
                else if (child.FirstChild.InnerText == "title") {
                    newArtifact.stigType = child.LastChild.InnerText;
                    newArtifact.stigType = newArtifact.stigType.Replace("STIG", "Security Technical Implementation Guide").Replace("MS Windows","Windows")
                        .Replace("Microsoft Windows","Windows").Replace("Dot Net","DotNet");
                }
            }

            if (newArtifact != null && !string.IsNullOrEmpty(newArtifact.stigRelease)) {
                newArtifact.stigRelease = newArtifact.stigRelease.Replace("Release: ", "R"); // i.e. R11, R2 for the release number
                newArtifact.stigRelease = newArtifact.stigRelease.Replace("Benchmark Date:","dated");
            }
            return newArtifact;
        }
    }
}