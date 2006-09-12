#region Copyright (c) 2004 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2004 Ian Davis and James Carlyle

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to do 
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
------------------------------------------------------------------------------*/
#endregion


using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using SemPlan.Spiral.Core;
using SemPlan.Spiral.XsltParser;
using SemPlan.Spiral.Utility;
using Semaview.Shared.ICalParser;
using System.Text.RegularExpressions;

namespace SemPlan.Spiral.ICalParser
{
	/// <summary> 
	/// ICalParser returns an array of statements.	
	/// </summary>
	public class ICalParser: SemPlan.Spiral.Core.Parser
	{
		public event SemPlan.Spiral.Core.StatementHandler NewStatement;
		private SemPlan.Spiral.XsltParser.XsltParser itsXsltParser;
    private RDFEmitter itsEmitter;
    private Dereferencer itsDereferencer;


		public ICalParser(SemPlan.Spiral.XsltParser.XsltParser xsltParser, Dereferencer dereferencer)
		{
      itsXsltParser = xsltParser;
      itsEmitter = new RDFEmitter();
      itsDereferencer = dereferencer;
		}
	
		public ICalParser(SemPlan.Spiral.XsltParser.XsltParser xsltParser) 
			: this(xsltParser, new SimpleDereferencer()) {}
		
		/// <summary>
		/// Raise a NewStatement event
		/// </summary>
		public void OnNewStatement(SemPlan.Spiral.Core.Statement s) {
			if (NewStatement != null)
				NewStatement(s);
		}

		/// <summary>
		/// Method to capture the inner XsltParser new statement event and raise another to listeners of this parser
		/// </summary>
    public void XsltParserNewStatement(SemPlan.Spiral.Core.Statement s)
    {
     OnNewStatement(s);
    }

     /// <summary>
		/// Assign the factory the parser must use to create resources
		/// </summary>
		public void SetResourceFactory(SemPlan.Spiral.Core.ResourceFactory factory) {
			itsXsltParser.triplesParser.setResourceFactory(factory);
		}
	
     /// <summary>
		/// Assign the factory the parser must use to create statements from resources
		/// </summary>
		public void SetStatementFactory(SemPlan.Spiral.Core.StatementFactory factory) {
			itsXsltParser.triplesParser.setStatementFactory(factory);
		}

    /// <summary>
    /// Set the Dereferencer to be used to dereference URIs
    /// </summary>
    public void SetDereferencer(Dereferencer dereferencer) {
      itsDereferencer = dereferencer;
      itsXsltParser.SetDereferencer(itsDereferencer);
    }
    
		/// <summary>
		/// Parse the iCal at the given URI and base URI
		/// </summary>
		public void Parse(Uri uri, string baseUri) {
      try {
        DereferencerResponse response = itsDereferencer.Dereference(uri);
        if ( response.HasContent ) {
          Stream contentStream = response.Stream;
          this.Parse(contentStream, baseUri);
          contentStream.Close();
        }
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " + e);
      }
		}
		
		/// <summary>
		/// Parse the iCal using the string paramater as a URI and base URI
		/// </summary>
		public void Parse(string uri, string baseUri) {
      try {      
        DereferencerResponse response = itsDereferencer.Dereference(uri);
        if ( response.HasContent ) {
          Stream contentStream = response.Stream;
          this.Parse(contentStream, baseUri);
          contentStream.Close();
        }
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " + e);
      }
      
		}
	
		/// <summary>
		/// Parse the iCal using supplied TextReader and base URI
		/// </summary>
		public void Parse(TextReader reader, string baseUri) {
			try {
        string strippedContent = Regex.Replace(reader.ReadToEnd(), @"\r\n ", "");
        StringReader strippedReader = new StringReader(strippedContent);

        Semaview.Shared.ICalParser.Parser icalParser = new Semaview.Shared.ICalParser.Parser(strippedReader, itsEmitter); 
        icalParser.Parse();
        itsXsltParser.NewStatement += new SemPlan.Spiral.Core.StatementHandler(XsltParserNewStatement);
        itsXsltParser.Parse(new StringReader(itsEmitter.Rdf), baseUri);
        itsXsltParser.NewStatement -= new SemPlan.Spiral.Core.StatementHandler(XsltParserNewStatement);
      }
      catch (Exception e) {
        Console.WriteLine( itsEmitter.Rdf );
        throw new ParserException("Could not parse content because " + e);
      }
		}
	
		/// <summary>
		/// Parse the iCal using supplied XmlReader and base URI
		/// </summary>
		public void Parse(XmlReader reader, string baseUri){
		}
	
		/// <summary>
		/// Parse the iCal using supplied stream and base URI
		/// </summary>
		public void Parse(Stream stream, string baseUri) {
     try {
        StreamReader reader = new StreamReader(stream);
        this.Parse(reader, baseUri);
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " +  e);
      }
		}

	}
}
