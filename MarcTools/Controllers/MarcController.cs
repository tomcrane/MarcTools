using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MarcTools.Models;
using SobekCM.Bib_Package.MARC;

namespace MarcTools.Controllers
{
    public class MarcController : Controller
    {

        //private const string Marcxmlroot = @"\\wellcomeit.com\Shares\LIB_WDL_DDS\DDS_LIVE\marcxml";
        private const string Z3950Name = "Wellcome Library";
        private const string Z3950Host = "83.244.194.134";
        private const uint Z3950Port = 210;
        private const string Z3950DbName = "INNOPAC";

        public ActionResult Index()
        {
            return View();
        }

        public ContentResult Marc21(string id)
        {
            var record = GetMarcRecord(id);
            var cr = new ContentResult
            {
                Content = record.ToString(),
                ContentType = "text/plain"
            };
            return cr;
        }

        public ContentResult MarcXml(string id)
        {
            var record = GetMarcRecord(id);
            var cr = new ContentResult
            {
                Content = record.To_MARC_XML(),
                ContentType = "text/xml",
                ContentEncoding = Encoding.UTF8
            };
            return cr;
            
        }

        private MARC_Record GetMarcRecord(String bNumber)
        {
            var normalised = WellcomeLibraryIdentifiers.GetNormalisedBNumber(bNumber, false);
            var identifier = WellcomeLibraryIdentifiers.GetShortBNumber(normalised).ToString(CultureInfo.InvariantCulture);
            string msg;
            var endpoint = new Z3950_Endpoint(Z3950Name, Z3950Host, Z3950Port, Z3950DbName);
            return MARC_Record_Z3950_Retriever.Get_Record_By_Primary_Identifier(identifier, endpoint, out msg);
        }
	}
}