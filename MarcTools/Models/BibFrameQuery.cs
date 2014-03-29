using System;
using System.IO;
using java.io;
using javax.xml.transform;
using net.sf.saxon;
using net.sf.saxon.dotnet;
using net.sf.saxon.query;
using net.sf.saxon.trans;

namespace MarcTools.Models
{
    public class BibFrameQuery : Query
    {
        public BibFrameQuery()
        {
            // Ensure the extended character sets in charsets.jar are loaded
            GC.KeepAlive(typeof(sun.nio.cs.ext.ExtendedCharsets));
        }

        public string DoBibFrameQuery(string saxonXqyPath, string marcxmlPath, string baseUri, string serialisation)
        {
            const string command = "bibframequery";
            var args = new[]
            {
                saxonXqyPath, 
                "marcxmluri=" + marcxmlPath, 
                "baseuri=" + baseUri,
                "serialization=" + serialisation
            };
            var options = new CommandLineOptions();
            setPermittedOptions(options);
            options.setActualOptions(args);

            config = Configuration.newConfiguration();
            config.setHostLanguage(Configuration.XQUERY);
            var dynamicEnv = new DynamicQueryContext(config);
            parseOptions(options, command, dynamicEnv);
            var staticEnv = config.newStaticQueryContext();
            staticEnv.setSchemaAware(false);

            Source sourceInput = null;
            if (sourceFileName != null)
            {
                sourceInput = processSourceFile(sourceFileName, useURLs);
            }

            var exp = compileQuery(staticEnv, queryFileName, useURLs);
            exp.setAllowDocumentProjection(projection);
            processSource(sourceInput, exp, dynamicEnv);
            var ms = new MemoryStream();
            OutputStream destination = new DotNetOutputStream(ms);
            runQuery(exp, dynamicEnv, destination, outputProperties);
            ms.Position = 0;
            return new StreamReader(ms).ReadToEnd();
        }
    }
}