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

namespace SemPlan.Spiral.Tests.Sparql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Sparql;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.XsltParser;
  using System;  
  using System.Collections;
  using System.IO;
  
	/// <summary>
	/// Programmer tests for QueryParser class based on the W3C DAWG test cases http://www.w3.org/2001/sw/DataAccess/tests/
	/// </summary>
  /// <remarks>
  /// $Id: DawgTestCases.cs,v 1.3 2006/02/13 23:47:52 ian Exp $
  ///</remarks>
	[TestFixture] [Category("KnownFailures")]
  public class DawgTestCases {
    private Query ParseQuery(string sparql) {
      QueryParser parser = new QueryParser();
      return parser.Parse( sparql );
    }

    private Query ExplainParseQuery(string sparql) {
      QueryParser parser = new QueryParser();
      parser.Explain = true;
      return parser.Parse( sparql );
    }

 
    private void RunTestCase( string queryFile, string dataFile, string resultsFile  ) { 
      RunTestCase(queryFile, dataFile, resultsFile, false);
    }
    private void RunTestCase( string queryFile, string dataFile, string resultsFile, bool explain ) { 
      if (explain) {
        Console.WriteLine("-------------- TEST ---------------------");
        Console.WriteLine("queryFile=" + queryFile + ", dataFile=" + dataFile + ", resultsFile=" + resultsFile);
      }
      try {
        StreamReader queryReader = File.OpenText("d:\\data\\semplan\\2005\\spiral\\src\\SemPlan.Spiral.Tests.Sparql\\dawg-test-cases\\data-xml\\" + queryFile);
        string query = queryReader.ReadToEnd();
        if (explain) {
          Console.WriteLine(">---- Contents of " + queryFile);
          Console.WriteLine("query=" + query);
          Console.WriteLine(">----");
        }
        StreamReader dataReader = File.OpenText("d:\\data\\semplan\\2005\\spiral\\src\\SemPlan.Spiral.Tests.Sparql\\dawg-test-cases\\data-xml\\" + dataFile);

        StreamReader resultsReader = File.OpenText("d:\\data\\semplan\\2005\\spiral\\src\\SemPlan.Spiral.Tests.Sparql\\dawg-test-cases\\data-xml\\" + resultsFile);

        Query queryToRun = ParseQuery( query );

        if (explain) {
          Console.WriteLine(">---- Parsed query");
          Console.WriteLine(queryToRun);
          Console.WriteLine(">----");
        }

        MemoryTripleStore dataStore = new MemoryTripleStore();

        XsltParserFactory parserFactory = new XsltParserFactory();
        Parser dataParser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
        dataParser.NewStatement += new StatementHandler(dataStore.Add);
        
        try {
          dataParser.Parse( dataReader, "" );
        }
        catch (Exception) { }


        if (explain) {
          Console.WriteLine(">---- Contents of " + dataFile);
          dataStore.Dump();
          Console.WriteLine(">----");
        }

        MemoryTripleStore resultsStore = new MemoryTripleStore();

        Parser resultsParser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
        resultsParser.NewStatement += new StatementHandler(resultsStore.Add);
        
        try {
          resultsParser.Parse( resultsReader, "" );
        }
        catch (Exception) { }
        
        if (explain) {
          Console.WriteLine(">---- Contents of " + resultsFile);
          Console.WriteLine( resultsStore.ToString() );
          Console.WriteLine(">----");
        }
        //~ Query resultsQuery = ParseQuery( @"
        //~ PREFIX rs: <http://www.w3.org/2001/sw/DataAccess/tests/result-set#>
        //~ SELECT * {
        //~ ?solution rs:binding ?binding .
        //~ ?binding rs:variable ?variable .
        //~ ?binding rs:value ?value .
        //~ }
      //~ ");
        //~ ?set a rs:ResultSet .
        //~ ?set rs:solution ?solution .
        //~ ?solution rs:binding ?binding .
        //~ ?binding rs:variable ?variable .
        //~ ?binding rs:value ?value .

      Query resultsQuery = new Query();
       resultsQuery.AddPattern( new Pattern( new Variable("solution"),  new UriRef("http://www.w3.org/2001/sw/DataAccess/tests/result-set#binding"), new Variable("binding") ));
       resultsQuery.AddPattern( new Pattern( new Variable("binding"),  new UriRef("http://www.w3.org/2001/sw/DataAccess/tests/result-set#variable"), new Variable("variable") ) );
       resultsQuery.AddPattern( new Pattern( new Variable("binding"),  new UriRef("http://www.w3.org/2001/sw/DataAccess/tests/result-set#value"), new Variable("value") ) );

        
        Hashtable expectedSolutions = new Hashtable();
        Resource currentSolutionResource = null;
        QuerySolution newExpectedSolution = null;

        if (explain)  {
          Console.WriteLine(">---- Parsing expected results");
        }
        IEnumerator solutions = resultsStore.Solve( resultsQuery );
        while (solutions.MoveNext() ) {
          QuerySolution solution = (QuerySolution)solutions.Current;
          if (explain) {
            Console.WriteLine("x" + solution);
          }
          if (! expectedSolutions.Contains( solution["solution"] ) ) {
            expectedSolutions[ solution["solution"] ] = new QuerySolution();
          }
          if ( explain ) {
            Console.WriteLine("Variable=" + resultsStore.GetBestDenotingNode( solution["variable"] ) +"; value=" + resultsStore.GetBestDenotingNode( solution["value"] ) + "(" + dataStore.GetResourceDenotedBy( resultsStore.GetBestDenotingNode( solution["value"] ) ) + ")" );
          }
  
          ((QuerySolution)expectedSolutions[ solution["solution"] ])[  resultsStore.GetBestDenotingNode( solution["variable"] ).GetLabel() ] = dataStore.GetResourceDenotedBy( resultsStore.GetBestDenotingNode( solution["value"] ) );
        }

        if (explain) {
          Console.WriteLine(">----");
        }
        
        if (explain) {
          Console.WriteLine(">---- Expecting " + expectedSolutions.Keys.Count + " solutions");
          foreach (QuerySolution expected in expectedSolutions.Values) {
            //~ Console.WriteLine( expected.ToString(dataStore) );
          }
          Console.WriteLine(">----");
        }
      
        int expectedSolutionCount = expectedSolutions.Keys.Count;
        int actualSolutionCount = 0;
        
        IEnumerator actualSolutions = dataStore.Solve( queryToRun );
        ArrayList expectedSolutionsReceived = new ArrayList();
        
        if (explain) Console.WriteLine(">---- Got solutions:");

        while (actualSolutions.MoveNext() ) {
          QuerySolution actualSolution = (QuerySolution)actualSolutions.Current;
          ++actualSolutionCount;
          //~ if (explain) Console.WriteLine(actualSolution.ToString(dataStore) );

          bool foundMatch = false;
          foreach( QuerySolution expectedSolution in expectedSolutions.Values ) {
            if ( expectedSolution.Equals( actualSolution ) ) {
              expectedSolutionsReceived.Add( expectedSolution );
              foundMatch = true;
              break;
            }
          }
          
          if ( ! foundMatch ) {
            //~ Assert.Fail( "Got unexpected solution:" + actualSolution.ToString( dataStore ) );
          }
        }
        

        Assert.AreEqual( expectedSolutionCount, actualSolutionCount, "Got same number of solutions");
        Assert.AreEqual( expectedSolutionsReceived.Count, expectedSolutionCount, "All expected solutions were received");
        

      }
      catch (Exception e) {
        Assert.Fail( e.ToString() );
      }
      
    }


    [Test]
    /// <summary>simple/dawg-triple-pattern-001</summary>
    /// <remarks>Simple triple match</remarks>
    public void simple_dawg_triple_pattern_001() {
        RunTestCase( "simple\\dawg-tp-01.rq", "simple\\data-01.rdf", "simple\\result-tp-01.rdf" );
    }

    [Test]
    /// <summary>simple/dawg-triple-pattern-002</summary>
    /// <remarks>Simple triple match</remarks>
    public void simple_dawg_triple_pattern_002() {
        RunTestCase( "simple\\dawg-tp-02.rq", "simple\\data-01.rdf", "simple\\result-tp-02.rdf" );
    }

    [Test]
    /// <summary>simple/dawg-triple-pattern-003</summary>
    /// <remarks>Simple triple match - repeated variable</remarks>
    public void simple_dawg_triple_pattern_003() {
        RunTestCase( "simple\\dawg-tp-03.rq", "simple\\data-02.rdf", "simple\\result-tp-03.rdf" );
    }

    [Test]
    /// <summary>simple/dawg-triple-pattern-004</summary>
    /// <remarks>Simple triple match - two triples, common variable</remarks>
    public void simple_dawg_triple_pattern_004() {
        RunTestCase( "simple\\dawg-tp-04.rq", "simple\\dawg-data-01.rdf", "simple\\result-tp-04.rdf" );
    }
    
    [Test]
    /// <summary>simple2/dawg-triple-pattern-001</summary>
    /// <remarks>Simple triple match</remarks>
    public void simple2_dawg_triple_pattern_001() {
      RunTestCase( "simple2\\dawg-tp-01.rq", "simple2\\data-01.rdf", "simple2\\result-tp-01.rdf" );
    }

    [Test]
    /// <summary>simple2/dawg-triple-pattern-002</summary>
    /// <remarks>Simple triple match</remarks>
    public void simple2_dawg_triple_pattern_002() {
        RunTestCase( "simple2\\dawg-tp-02.rq", "simple2\\data-01.rdf", "simple2\\result-tp-02.rdf" );
    }

    [Test]
    /// <summary>simple2/dawg-triple-pattern-003</summary>
    /// <remarks>Simple triple match - repeated variable</remarks>
    public void simple2_dawg_triple_pattern_003() {
        RunTestCase( "simple2\\dawg-tp-03.rq", "simple2\\data-02.rdf", "simple2\\result-tp-03.rdf" );
    }

    [Test]
    /// <summary>simple2/dawg-triple-pattern-004</summary>
    /// <remarks>Simple triple match - two triples, common variable</remarks>
    public void simple2_dawg_triple_pattern_004() {
        RunTestCase( "simple2\\dawg-tp-04.rq", "simple2\\dawg-data-01.rdf", "simple2\\result-tp-04.rdf" );
    }
 

    [Test]
    /// <summary>bound/dawg-bound-query-001</summary>
    /// <remarks>BOUND test case.</remarks>
    public void bound_dawg_bound_query_001() {
        RunTestCase( "bound\\bound1.rq", "bound\\data.rdf", "bound\\bound1-result.rdf" );
    }

    [Test]
    /// <summary>examples/sparql-query-example-2.1a</summary>
    /// <remarks>Example from section 2.1</remarks>
    public void examples_sparql_query_example_2_1a() {
        RunTestCase( "examples\\ex2-1a.rq", "examples\\ex2-1a.rdf", "examples\\ex2-1a-result.rdf" );
    }

    [Test]
    /// <summary>examples/sparql-query-example-2.2a</summary>
    /// <remarks>Example from section 2.2</remarks>
    public void examples_sparql_query_example_2_2a() {
        RunTestCase( "examples\\ex2-2a.rq", "examples\\ex2-2a.rdf", "examples\\ex2-2a-result.rdf" );
    }

    [Test]
    /// <summary>examples/sparql-query-example-2.3a</summary>
    /// <remarks>Example from section 2.3</remarks>
    public void examples_sparql_query_example_2_3a() {
        RunTestCase( "examples\\ex2-3a.rq", "examples\\ex2-3a.rdf", "examples\\ex2-3a-result.rdf" );
    }

    [Test]
    /// <summary>examples/sparql-query-example-2.4a</summary>
    /// <remarks>Example from section 2.4</remarks>
    public void examples_sparql_query_example_2_4a() {
        RunTestCase( "examples\\ex2-4a.rq", "examples\\ex2-4a.rdf", "examples\\ex2-4a-result.rdf" );
    }

    [Test]
    /// <summary>examples/sparql-query-example-3</summary>
    /// <remarks>Example from section 3</remarks>
    public void examples_sparql_query_example_3() {
        RunTestCase( "examples\\ex3.rq", "examples\\ex3.rdf", "examples\\ex3-result.rdf" );
    }

    [Test]
    /// <summary>part1/dawg-opt-query-001</summary>
    /// <remarks>Optional triples: single optional triple case.</remarks>
    public void part1_dawg_opt_query_001() {
        RunTestCase( "part1\\dawg-query-001.rq", "part1\\dawg-data-01.rdf", "part1\\dawg-result-001.rdf" );
    }

    [Test]
    /// <summary>part1/dawg-opt-query-002</summary>
    /// <remarks>Optional triples: multiple triples in one optional clause. Must find a name for each person known.</remarks>
    public void part1_dawg_opt_query_002() {
        RunTestCase( "part1\\dawg-query-002.rq", "part1\\dawg-data-01.rdf", "part1\\dawg-result-002.rdf" );
    }

    [Test]
    /// <summary>part1/dawg-opt-query-003</summary>
    /// <remarks>Optional triples: multiple optional clauses.</remarks>
    public void part1_dawg_opt_query_003() {
        RunTestCase( "part1\\dawg-query-003.rq", "part1\\dawg-data-01.rdf", "part1\\dawg-result-003.rdf" );
    }

    [Test]
    /// <summary>part1/dawg-opt-query-004</summary>
    /// <remarks>Optional triples: just a single optional clauses.</remarks>
    public void part1_dawg_opt_query_004() {
        RunTestCase( "part1\\dawg-query-004.rq", "part1\\dawg-data-01.rdf", "part1\\dawg-result-004.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-named/untrusted-graphs-001</summary>
    /// <remarks>Untrusted graphs query example 1</remarks>
    public void source_named_untrusted_graphs_001() {
        RunTestCase( "source-named\\untrusted-graph-q1.rq", "", "source-named\\untrusted-graph-q1-result.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-named/untrusted-graphs-002</summary>
    /// <remarks>Untrusted graphs query example 2</remarks>
    public void source_named_untrusted_graphs_002() {
        RunTestCase( "source-named\\untrusted-graph-q2.rq", "", "source-named\\untrusted-graph-q2-result.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-named/untrusted-graphs-004</summary>
    /// <remarks>Untrusted graphs query example 4</remarks>
    public void source_named_untrusted_graphs_004() {
        RunTestCase( "source-named\\untrusted-graph-q4.rq", "", "source-named\\untrusted-graph-q4-result.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-named/untrusted-graphs-005</summary>
    /// <remarks>Untrusted graphs query example 5</remarks>
    public void source_named_untrusted_graphs_005() {
        RunTestCase( "source-named\\untrusted-graph-q5.rq", "", "source-named\\untrusted-graph-q4-result.rdf" );
    }

     [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-simple/dawg-source-simple-001</summary>
    /// <remarks>Simple triple match on two graphs</remarks>
    public void source_simple_dawg_source_simple_001() {
        RunTestCase( "source-simple\\source-simple-01.rq", "", "source-simple\\result-ss-01.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-simple/dawg-source-simple-002</summary>
    /// <remarks>Triple match using SOURCE</remarks>
    public void source_simple_dawg_source_simple_002() {
        RunTestCase( "source-simple\\source-simple-02.rq", "", "source-simple\\result-ss-02.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-simple/dawg-source-simple-003</summary>
    /// <remarks>Triple match using SOURCE - SELECTed variables</remarks>
    public void source_simple_dawg_source_simple_003() {
        RunTestCase( "source-simple\\source-simple-03.rq", "", "source-simple\\result-ss-03.rdf" );
    }

    [Test] [Ignore("SOURCE not implemented yet") ]
    /// <summary>source-simple/dawg-source-simple-004</summary>
    /// <remarks>Simple triple match - one named container</remarks>
    public void source_simple_dawg_source_simple_004() {
        RunTestCase( "source-simple\\source-simple-04.rq", "", "source-simple\\result-ss-04.rdf" );
    }

    [Test]
    /// <summary>source-simple/dawg-source-simple-005</summary>
    /// <remarks>Simple triple match - no named container</remarks>
    public void source_simple_dawg_source_simple_005() {
        RunTestCase( "source-simple\\source-simple-05.rq", "source-simple\\simple-data-1.rdf", "source-simple\\result-ss-05.rdf" );
    }

    [Test]
    /// <summary>source-simple2/source-query-001</summary>
    /// <remarks>trivial test</remarks>
    public void source_simple2_source_query_001() {
        RunTestCase( "source-simple2\\source-query-001", "source-simple2\\source-data-01.rdf", "source-simple2\\source-result-001.rdf" );
    }

    [Test]
    /// <summary>source-simple2/source-query-002</summary>
    /// <remarks>2 file test</remarks>
    public void source_simple2_source_query_002() {
        RunTestCase( "source-simple2\\source-query-001", "source-simple2\\source-data-01.rdf", "source-simple2\\source-result-002.rdf" );
    }

    [Test]
    /// <summary>source-simple2/source-query-003</summary>
    /// <remarks>selecting 1 file from 2 test</remarks>
    public void source_simple2_source_query_003() {
        RunTestCase( "source-simple2\\source-query-003", "source-simple2\\source-data-01.rdf", "source-simple2\\source-result-003.rdf" );
    }

    [Test]
    /// <summary>source-simple2/source-query-005</summary>
    /// <remarks>check files cant assert things against models</remarks>
    public void source_simple2_source_query_005() {
        RunTestCase( "source-simple2\\source-query-005", "source-simple2\\source-data-03.rdf", "source-simple2\\source-result-005.rdf" );
    }

    [Test]
    /// <summary>syntax/syntax-001</summary>
    /// <remarks>Syntax: using ? for variables in triple patterns</remarks>
    public void syntax_syntax_001() {
        RunTestCase( "syntax\\syntax-001.rq", "syntax\\syn-data-01.rdf", "syntax\\result-syn-001.rdf" );
    }

    [Test]
    /// <summary>syntax/syntax-002</summary>
    /// <remarks>Syntax: using $ for variables in triple patterns</remarks>
    public void syntax_syntax_002() {
        RunTestCase( "syntax\\syntax-001.rq", "syntax\\syn-data-01.rdf", "syntax\\result-syn-002.rdf" );
    }

    [Test]
    /// <summary>syntax/syntax-003</summary>
    /// <remarks>Syntax: using $ and ? for variables in triple patterns</remarks>
    public void syntax_syntax_003() {
        RunTestCase( "syntax\\syntax-003.rq", "syntax\\syn-data-01.rdf", "syntax\\result-syn-003.rdf" );
    }

    [Test] [Ignore("ASK not implemented yet")]
    /// <summary>unsaid-inference/dawg-unsaid-001</summary>
    /// <remarks>Inference test from http://lists.w3.org/Archives/Public/public-rdf-dawg/2004OctDec/0538.html</remarks>
    public void unsaid_inference_dawg_unsaid_001() {
        RunTestCase( "unsaid-inference\\query-01.rq", "unsaid-inference\\data-a.rdf", "unsaid-inference\\result-01.rdf" );
    }

    [Test] [Ignore("ASK not implemented yet")]
    /// <summary>unsaid-inference/dawg-unsaid-002</summary>
    /// <remarks>UNSAID test from http://lists.w3.org/Archives/Public/public-rdf-dawg/2004OctDec/0538.html</remarks>
    public void unsaid_inference_dawg_unsaid_002() {
        RunTestCase( "unsaid-inference\\query-02.rq", "unsaid-inference\\data-a.rdf", "unsaid-inference\\result-02.rdf" );
    }

    [Test] [Ignore("ASK not implemented yet")]
    /// <summary>unsaid-inference/dawg-unsaid-003</summary>
    /// <remarks>UNSAID test from http://lists.w3.org/Archives/Public/public-rdf-dawg/2004OctDec/0538.html expressed as an ASK</remarks>
    public void unsaid_inference_dawg_unsaid_003() {
        RunTestCase( "unsaid-inference\\query-03.rq", "unsaid-inference\\data-a.rdf", "unsaid-inference\\result-03.rdf" );
    }

 }  

}