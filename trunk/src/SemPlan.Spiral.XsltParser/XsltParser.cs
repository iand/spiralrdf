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
using SemPlan.Spiral.Utility;

namespace SemPlan.Spiral.XsltParser
{
	/// <summary> 
	/// XsltParser returns an array of statements.	
	/// </summary>
	public class XsltParser: SemPlan.Spiral.Core.Parser
	{
		public event SemPlan.Spiral.Core.StatementHandler NewStatement;
		private XsltTransformer itsXsltTransformer;
		private XsltTransformer itsValidatorXsltTransformer;
		private NTriplesParser itsTriplesParser;
		private Dereferencer itsDereferencer;

		public XsltParser(SemPlan.Spiral.Core.ResourceFactory resourceFactory, SemPlan.Spiral.Core.StatementFactory statementFactory, XsltTransformer xsltTransformer, XsltTransformer validatorXsltTransformer, Dereferencer dereferencer)
		{
			itsTriplesParser = new NTriplesParser(resourceFactory, statementFactory);
			itsXsltTransformer = xsltTransformer;
			itsValidatorXsltTransformer = validatorXsltTransformer;
      itsDereferencer = dereferencer;
		}
	
		/// <summary>
		/// Raise a NewStatement event
		/// </summary>
		public void OnNewStatement(SemPlan.Spiral.Core.Statement s) {
			if (NewStatement != null)
				NewStatement(s);
		}

		/// <summary>
		/// Assign the factory the parser must use to create resources
		/// </summary>
		public void SetResourceFactory(SemPlan.Spiral.Core.ResourceFactory factory) {
			itsTriplesParser.setResourceFactory(factory);
		}
	
		/// <summary>
		/// Assign the factory the parser must use to create statements from resources
		/// </summary>
		public void SetStatementFactory(SemPlan.Spiral.Core.StatementFactory factory) {
			itsTriplesParser.setStatementFactory(factory);
		}

    /// <summary>
    /// Set the Dereferencer to be used to dereference URIs
    /// </summary>
    public void SetDereferencer(Dereferencer dereferencer) {
      itsDereferencer = dereferencer;
    }
    
    public NTriplesParser triplesParser {
      get {return itsTriplesParser;}
    }
		
		/// <summary>
		/// Parse the RDF at the given URI and base URI
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
		/// Parse the RDF using the string paramater as a URI and base URI
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
		/// Parse the RDF using supplied TextReader and base URI
		/// </summary>
		public void Parse(TextReader reader, string baseUri) {
			try {
        XPathDocument rdfContent = new XPathDocument(reader);
        this.Parse(rdfContent, baseUri);
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " + e);
      }
		}
	
		/// <summary>
		/// Parse the RDF using supplied XmlReader and base URI
		/// </summary>
		public void Parse(XmlReader reader, string baseUri){
      try {
        XPathDocument rdfContent = new XPathDocument(reader);
        this.Parse(rdfContent, baseUri);
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " + e);
      }
		}
	
		/// <summary>
		/// Parse the RDF using supplied stream and base URI
		/// </summary>
		public void Parse(Stream stream, string baseUri) {
      try {
        XPathDocument rdfContent = new XPathDocument(stream);
        this.Parse(rdfContent, baseUri);
      }
      catch (Exception e) {
        throw new ParserException("Could not parse content because " +  e);
      }
		}

		/// <summary>
		/// Parse the RDF using supplied XPathDocument and base URI
		/// </summary>
		public void Parse(XPathDocument xPathDoc, string baseUri) {
			MemoryStream stream;
			StreamReader reader;
			stream = itsValidatorXsltTransformer.TransformContent(xPathDoc);
			if (stream.Length > 0) {
				reader = new StreamReader(stream);
				throw new ParserException("The following errors were found in the RDF being parsed: " + reader.ReadToEnd());
			}
			stream = itsXsltTransformer.TransformContentWithBaseUri(xPathDoc, baseUri);
			reader = new StreamReader(stream);
			
			string line = reader.ReadLine();
			while (line != null) 
			{
				SemPlan.Spiral.Core.Statement statement = itsTriplesParser.ParseTriple(line);
				if (statement != null) {
					OnNewStatement(statement);
				}
				line = reader.ReadLine();
			}
		}
		
	}
}
