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
  
	/// <summary>
	/// Programmer tests for SparqlQuery class
	/// </summary>
  /// <remarks>
  /// $Id: SparqlQueryTest.cs,v 1.2 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class SparqlQueryTest {
    
    
    [Test]
    public void ExecuteEnumeratorPassesSelfToTripleStore() {
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      TripleStoreStore store = new TripleStoreStore();

      IEnumerator results = query.ExecuteEnumerator( store );
      
      Assert.IsTrue( store.WasSolveCalledWith(query), "Query should invoke triple store's solve method");      
    }


    [Test]
    public void ExecuteXmlPassesSelectQueryToTripleStore() {
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      TripleStoreStore store = new TripleStoreStore();

      XmlDocument results = query.ExecuteXml( store );
      
      Assert.IsTrue( store.WasSolveCalledWith(query), "Query should invoke triple store's solve method");      
    }


    [Test]
    public void ExecuteXmlReturnsSparqlResultsFormatXml() {
      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SparqlQuery query = new SparqlQuery("SELECT ?var WHERE { <http://example.com/subj> <http://example.com/property> ?var }");
  
      XmlDocument resultsRaw = query.ExecuteXml( store );
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

    [Test]
    public void ExecuteXmlPassesDescribeQueryToTripleStore() {
      SparqlQuery query = new SparqlQuery("DESCRIBE ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      TripleStoreStore store = new TripleStoreStore();

      XmlDocument results = query.ExecuteXml( store );
      
      Assert.IsTrue( store.WasSolveCalledWith(query), "Query should invoke triple store's Solve method");      
    }

    [Test]
    public void ExecuteTripleStorePassesDescribeQueryToTripleStore() {
      SparqlQuery query = new SparqlQuery("DESCRIBE ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      TripleStoreStore store = new TripleStoreStore();

      TripleStore results = query.ExecuteTripleStore( store );
      
      Assert.IsTrue( store.WasSolveCalledWith(query), "Query should invoke triple store's Solve method");      
    }

    [Test]
    public void ExecuteTripleStoreWithPatternlessQueryDoesNotPassQueryToTripleStore() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject>");
      TripleStoreStore store = new TripleStoreStore();

      TripleStore results = query.ExecuteTripleStore( store );
      
      Assert.AreEqual( false, store.WasSolveCalledWith(query), "Query should not invoke triple store's Solve method");      
    }


    [Test]
    public void ExecuteTripleStoreWithPatternlessQueryGetsDescriptionOfSingleUri() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject>");
      TripleStoreStore store = new TripleStoreStore();

      TripleStore results = query.ExecuteTripleStore( store );

      Assert.IsTrue( store.WasGetDescriptionOfCalledWith(new UriRef("http://example.com/subject"), new CbdBoundingStrategy()), "Query should not invoke triple store's Solve method");      

    }

    [Test]
    public void ExecuteTripleStoreWithPatternlessQueryGetsDescriptionOfMultipleUris() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject> <http://example.com/other>");
      TripleStoreStore store = new TripleStoreStore();

      TripleStore results = query.ExecuteTripleStore( store );

      Assert.IsTrue( store.WasGetDescriptionOfCalledWith(new UriRef("http://example.com/subject"), new CbdBoundingStrategy()), "Query should not invoke triple store's Solve method");      
      Assert.IsTrue( store.WasGetDescriptionOfCalledWith(new UriRef("http://example.com/other"), new CbdBoundingStrategy()), "Query should not invoke triple store's Solve method");      

    }

    [Test]
    public void ExecuteTripleStoreWithPatternlessQueryReturnsDescriptions() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject> <http://example.com/other>");

      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subject"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      store.Add( new Statement( new UriRef("http://example.com/other"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/thing") ) );
      store.Add( new Statement( new UriRef("http://example.com/bogus"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
 
      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<http://example.com/subject> <http://example.com/pred> <http://example.com/obj> .");
      verifier.expect("<http://example.com/other> <http://example.com/pred> <http://example.com/thing> .");

      TripleStore results = query.ExecuteTripleStore( store );
      Assert.IsTrue( verifier.verify(results), "Resulting triple store should contain describing triples" );
    }


    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Sparql.SparqlException))]
    public void ExecuteTripleStoreWithPatternlessQueryButUsingVariablesThrowsException() {
      SparqlQuery query = new SparqlQuery("DESCRIBE ?var");
      TripleStoreStore store = new TripleStoreStore();
      TripleStore results = query.ExecuteTripleStore( store );
    }

    [Test]
    public void ExecuteTripleStoreWithPatternsQueryReturnsDescriptions() {
      SparqlQuery query = new SparqlQuery("DESCRIBE ?var WHERE {<http://example.com/subject> <http://example.com/pred> ?var }");

      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subject"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      store.Add( new Statement( new UriRef("http://example.com/obj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/thing") ) );
 
      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<http://example.com/obj> <http://example.com/pred> <http://example.com/thing> .");

      TripleStore results = query.ExecuteTripleStore( store );
      Assert.IsTrue( verifier.verify(results), "Resulting triple store should contain describing triples" );
    }

    [Test]
    public void ExecuteTripleStoreWithPatternsQueryReturnsDescriptionsForUrisAndVariables() {
      SparqlQuery query = new SparqlQuery("DESCRIBE ?var <http://example.com/other> WHERE {<http://example.com/subject> <http://example.com/pred> ?var }");

      TripleStore store = new MemoryTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subject"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      store.Add( new Statement( new UriRef("http://example.com/obj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/thing") ) );
      store.Add( new Statement( new UriRef("http://example.com/other"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/thing") ) );
 
      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<http://example.com/obj> <http://example.com/pred> <http://example.com/thing> .");
      verifier.expect("<http://example.com/other> <http://example.com/pred> <http://example.com/thing> .");

      TripleStore results = query.ExecuteTripleStore( store );
      Assert.IsTrue( verifier.verify(results), "Resulting triple store should contain describing triples" );
    }


    [Test]
    public void DescribeQueryUsesSpecifiedBoundingStrategy() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject>");
      TripleStoreStore store = new TripleStoreStore();

      BoundingStrategyStub strategy = new BoundingStrategyStub();
      query.BoundingStrategy = strategy;
      
      TripleStore results = query.ExecuteTripleStore( store );

      Assert.IsTrue( store.WasGetDescriptionOfCalledWith(new UriRef("http://example.com/subject"), strategy), "Query should use specified bounding strategy");      
    }

    [Test]
    public void BoundingStrategyIsConciseBoundedDescriptionsByDefault() {
      SparqlQuery query = new SparqlQuery("DESCRIBE <http://example.com/subject>");
      Assert.IsTrue( query.BoundingStrategy is CbdBoundingStrategy);      
    }

  }  

}