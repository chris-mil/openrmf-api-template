using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace openstig_template_api.Models
{
    [Serializable]
    public class Template
    {
        public Template () {
            id= Guid.NewGuid();
        }

        public DateTime created { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public CHECKLIST CHECKLIST { get; set; }
        public string rawChecklist { get; set; }
        
        [BsonId]
        // standard BSonId generated by MongoDb
        public ObjectId InternalId { get; set; }

        public Guid id { get; set; }
        public STIGtype type { get; set; }
        public string typeTitle { get { return Enum.GetName(typeof(STIGtype), type);} }

        [BsonDateTimeOptions]
        // attribute to gain control on datetime serialization
        public DateTime? updatedOn { get; set; }

        public Guid createdBy { get; set; }
        public Guid? updatedBy { get; set; }
    }

    public enum STIGtype{
        
        ASD = 0,
        JAVA = 40,

        /* Development Technologies */
        OracleJRE8 = 101,
        DOTNET = 105,

        /* ANTI VIRUS */
        McAfeeAntiVirus = 201,
        WindowsDefender = 205,

        /* Application Servers */
        ColdFusion = 301,
        BromiumSecurePlatform4 = 305,
        IMBMQv9 = 310,
        IBMWebSphere = 315,

        /* Browsers */
        Chrome = 400,
        IE10 = 405,
        IE11 = 410,
        Firefox = 415,

        /* Databases */

        SQLServer2012Database = 500,
        SQLServer2012Instance = 502,
        SQLServer2014Database = 505,
        SQLServer2014Instance = 507,
        SQLServer2016Database = 510,
        SQLServer2016Instance = 512,
        MongoDB3 = 515,
        Oracle112g = 520,
        Oracle11g = 525,
        Oracle12c = 530,
        PostgreSQL = 535,
        EDBPostgresAdvancedServer = 540, // WTF is this?

        /* Web Servers */
        Apache22Windows = 600,
        Apache22Unix = 605,
        Apache24UnixServer = 610,
        Apache24UnixSite = 615,
        ApacheWindowsServer = 620,
        ApacheWindowsSite = 625,
        IIS7 = 630,
        IIS85 = 635,
        OracleHTTP = 640
    }

}