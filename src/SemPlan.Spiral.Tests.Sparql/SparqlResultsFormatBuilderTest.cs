#region Copyright (c) 2006 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006 Ian Davis and James Carlyle

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

namespace SemPlan.Spiral.Tests.Sparql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Sparql;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  using System.Xml;
  using System.Xml.XPath;
  
	/// <summary>
	/// Programmer tests for SparqlResultsFormatBuilder class
	/// </summary>
  /// <remarks>
  /// $Id: SparqlResultsFormatBuilderTest.cs,v 1.1 2006/01/26 14:42:00 ian Exp $
  ///</remarks>
	[TestFixture]
  public class SparqlResultsFormatBuilderTest {
    
    
    [Test]
    public void BuildReturnsSparqlResultsFormatXml() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
  
      SparqlResultsFormatBuilder builder = new SparqlResultsFormatBuilder();
  
      XmlDocument resultsRaw = builder.Build( query.ExecuteEnumerator(store), query );
      XmlDocument resultsCanonical = new XmlDocument();
      resultsCanonical.LoadXml( resultsRaw.InnerXml );
  
      XmlDocument expected = new XmlDocument();
      expected.LoadXml( @"<sparql xmlns=""http://www.w3.org/2005/sparql-results#"">
  <head>
    <variable name=""var""/>
  </head>
  <results ordered=""false"" distinct=""false"">
    <result>
      <binding name=""var""><uri>http://example.com/obj</uri></binding>
    </result>
  </results>
</sparql>");

      


      Assert.AreEqual( expected.InnerXml, resultsCanonical.InnerXml, "Result of query should equal expected XML" );    

    }

    private void AssertXPath( string xPath, SparqlQuery query, TripleStore store) {
      SparqlResultsFormatBuilder builder = new SparqlResultsFormatBuilder();
  
      XmlDocument results = builder.Build( query.ExecuteEnumerator(store), query );
      
      XPathNavigator nav = results.CreateNavigator();
      XPathExpression expr = nav.Compile(xPath);
 
      XmlNamespaceManager nsmgr = new XmlNamespaceManager(nav.NameTable);
      nsmgr.AddNamespace("s", "http://www.w3.org/2005/sparql-results#");
      expr.SetContext(nsmgr);
      
      
      Assert.IsTrue( nav.Select(expr).MoveNext(), "Result of query should match XPath " + xPath + " result was " + results.InnerXml );    
    }


    [Test]
    public void BuildIncludesUriBindings() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='var']/s:uri[ .= 'http://example.com/obj']", query, store);
    }


    [Test]
    public void BuildIncludesPlainLiteralBindings() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("scooby") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='var']/s:literal[ .= 'scooby']", query, store);
    }

    [Test]
    public void BuildIncludesLanguageLiteralBindings() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("scooby", "fr") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='var']/s:literal[@xml:lang='fr' and .= 'scooby']", query, store);
    }

    [Test]
    public void BuildIncludesTypedLiteralBindings() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new TypedLiteral("true", "http://www.w3.org/2001/XMLSchema#boolean") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='var']/s:literal[@datatype='http://www.w3.org/2001/XMLSchema#boolean' and .= 'true']", query, store);
    }

    [Test]
    public void BuildIncludesBlankNodeBindings() {
      TripleStore store = new MemoryTripleStore();
      BlankNode node = new BlankNode();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), node) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='var']/s:bnode[ .= '" + node.GetLabel() + "']", query, store);
    }

    [Test]
    public void BuildIncludesUnboundVariables() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/object")) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var ?what WHERE { <http://example.com/subj> <http://example.com/property> ?var OPTIONAL { ?var <http://example.com/other> ?what } }");
      AssertXPath("/s:sparql/s:results/s:result/s:binding[@name='what']/s:unbound", query, store);
    }


    [Test]
    public void BuildReadsDistinct() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/object")) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var ?what WHERE { <http://example.com/subj> <http://example.com/property> ?var OPTIONAL { ?var <http://example.com/other> ?what } }");
      query.IsDistinct = true;
      AssertXPath("/s:sparql/s:results[@distinct='true']", query, store);
    }

    [Test]
    public void BuildReadsOrderBy() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/object")) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var ?what WHERE { <http://example.com/subj> <http://example.com/property> ?var } ORDER BY ?var");
      AssertXPath("/s:sparql/s:results[@ordered='true']", query, store);
    }


  }  

}