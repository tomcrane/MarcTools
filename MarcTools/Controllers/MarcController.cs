using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MarcTools.Models;
using Microsoft.Ajax.Utilities;
using net.sf.saxon;
using net.sf.saxon.trans;
using SobekCM.Bib_Package.MARC;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using Controller = System.Web.Mvc.Controller;

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

        public ContentResult BibFrame(string id, string serialisation = "rdfxml")
        {
            var baseUri = "http://wellcomelibrary.org/bf/" + id + "/";
            var saxonXqy = Server.MapPath("~/lcnetdev/xbin/saxon.xqy");
            var record = GetMarcRecord(id);
            
            var tempMarcXml = Path.GetTempFileName();
            record.Save_MARC_XML(tempMarcXml);
            var bibQuery = new BibFrameQuery();
            var bf = bibQuery.DoBibFrameQuery(saxonXqy, tempMarcXml, baseUri, "rdfxml");
            System.IO.File.Delete(tempMarcXml);

            var cr = new ContentResult {ContentType = "text/plain"};
            // need to give each serialisation its proper mime type

            if (serialisation == "rdfxml")
            {
                cr.ContentEncoding = Encoding.UTF8;
                cr.Content = bf;
                cr.ContentType = "text/xml";
                return cr;
            }

            // load into dotnetrdf as Loc is a bit wrong for other serialisations
            var g = new Graph();
            StringParser.Parse(g, bf);
            IRdfWriter writer = null;
            var sw = new System.IO.StringWriter();
            switch (serialisation)
            {
                case "turtle" :
                    writer = new CompressingTurtleWriter();
                    break;
                default:
                    cr.Content = "invalid serialisation; use one of rdfxml, turtle";
                    cr.ContentType = "text/plain";
                    return cr;

            }
            if (writer is IPrettyPrintingWriter)
            {
                ((IPrettyPrintingWriter)writer).PrettyPrintMode = true;
            }
            writer.Save(g, sw);
            cr.Content = sw.ToString();
            return cr;

            // dirty tidyup
            //if (serialisation == "ntriples" && bf.Contains("?>&lt;"))
            //{
            //    bf = bf.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
            //    bf = bf.Replace("&lt;", "<");
            //    bf = bf.Replace("&gt;", ">");
            //}
            //if (serialisation == "json" && bf.Contains("&#xD;"))
            //{
            //    bf = bf.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
            //    bf = bf.Replace("&#xD;", "");
            //    bf = bf.Replace("&lt;", "<");
            //    bf = bf.Replace("&gt;", ">");
            //}
            ////var cr = new ContentResult
            ////{
            ////    Content = bf,
            ////    ContentType = serialisation.StartsWith("rdfxml") ? "text/xml" : "text/plain",
            ////    ContentEncoding = Encoding.UTF8
            ////};
            ////return cr;
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