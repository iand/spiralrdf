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
  using SemPlan.Spiral.Expressions;
  using SemPlan.Spiral.Sparql;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for QueryParser class
	/// </summary>
  /// <remarks>
  /// $Id: QueryParserTest.cs,v 1.11 2006/02/10 22:10:12 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QueryParserTest {
    private Query ParseQuery(string sparql) {
      QueryParser parser = new QueryParser();
      return parser.Parse( sparql );
    }

    private Query ExplainParseQuery(string sparql) {
      QueryParser parser = new QueryParser();
      parser.Explain = true;
      return parser.Parse( sparql );
    }

    
    [Test]
    public void simpleSelectOneVariable() {
      Query query = ParseQuery("SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      
      IList variables = query.Variables;
      Assert.AreEqual( 1, variables.Count );      
      Assert.AreEqual( "var", ((Variable)variables[0]).Name );      
    }

    [Test]
    public void simpleSelectTwoVariables() {
      Query query = ParseQuery("SELECT ?var1 , ?var2 WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");      
      IList variables = query.Variables;
      
      Assert.AreEqual( 2, variables.Count );      
      
      Assert.AreEqual( "var1", ((Variable)variables[0]).Name );      
      Assert.AreEqual( "var2", ((Variable)variables[1]).Name );      
    }

    [Test]
    public void simpleSelectOnePattern() {
      SparqlQuery expected = new SparqlQuery();

      expected.AddVariable( new Variable("var") );
      expected.PatternGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      
      AssertParse( expected, "SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
    }

    [Test]
    public void simpleSelectOnePatternWithTerminatingDot() {
      SparqlQuery expected = new SparqlQuery();

      expected.AddVariable( new Variable("var") );
      expected.PatternGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      AssertParse( expected, "SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var .}" );
    }

    [Test]
    public void simpleSelectTwoPatterns() {
      Query query = ParseQuery("SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var . <http://example.com/subject2> <http://example.com/predicate> ?var .}");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 2, patterns.Count );      
      Assert.AreEqual( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new UriRef("http://example.com/subject2"), new UriRef("http://example.com/predicate"), new Variable("var")).ToString() , patterns[1].ToString() );      
    }


    [Test]
    public void simpleSelectThreeVariables() {
      Query query = ParseQuery("SELECT ?s , ?p , ?v WHERE { ?s ?p ?v }");
      IList variables = query.Variables;
      
      Assert.AreEqual( 3, variables.Count );      
      
      Assert.AreEqual( "s", ((Variable)variables[0]).Name );      
      Assert.AreEqual( "p", ((Variable)variables[1]).Name );      
      Assert.AreEqual( "v", ((Variable)variables[2]).Name );      

      IList patterns = query.PatternGroup.Patterns;
      Assert.AreEqual( 1, patterns.Count );      
     Assert.AreEqual( new Pattern( new Variable("s"), new Variable("p"), new Variable("v")).ToString() , patterns[0].ToString() );      
     
    }

    [Test]
    public void simpleSelectThreePatterns() {
      Query query = ParseQuery("SELECT ?var WHERE { <http://example.com/subject> <http://example.com/predicate> ?var . <http://example.com/subject2> <http://example.com/predicate> ?var . <http://example.com/subject2> <http://example.com/predicate> ?var .}");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 3, patterns.Count );      
      Assert.AreEqual( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new UriRef("http://example.com/subject2"), new UriRef("http://example.com/predicate"), new Variable("var")).ToString() , patterns[1].ToString() );      
      Assert.AreEqual( new Pattern( new UriRef("http://example.com/subject2"), new UriRef("http://example.com/predicate"), new Variable("var")).ToString() , patterns[2].ToString() );      
    }


    [Test]
    public void simpleSelectOneVariableWildcard() {
      Query query = ParseQuery("SELECT * WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");
      Assert.AreEqual( true, query.SelectAll );            
    }

    [Test]
    public void simpleSelectThreeVariablesWildcard() {
      Query query = ParseQuery("SELECT * WHERE { ?s ?p ?v }");
      Assert.AreEqual( true, query.SelectAll );            
    }

    [Test]
    public void simpleSelectDuplicatedVariableWildcard() {
      Query query = ParseQuery("SELECT * WHERE { ?s ?s ?s }");
      Assert.AreEqual( true, query.SelectAll );            
    }

    [Test]
    public void simpleSelectOnePatternPlainLiteralValueDoubleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> \"foo\" }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("foo")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void simpleSelectOnePatternPlainLiteralValueSingleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'foo' }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("foo")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void simpleSelectOnePatternLanguageLiteralValueDoubleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> \"foo\"@en }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual("en", ((PlainLiteral)((Pattern)patterns[0]).GetObject()).GetLanguage() ) ;
    }

    [Test]
    public void simpleSelectOnePatternLanguageLiteralValueSingleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> \'foo\'@en }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual("en", ((PlainLiteral)((Pattern)patterns[0]).GetObject()).GetLanguage() ) ;
    }

    [Test]
    public void simpleSelectOnePatternTypedLiteralValueDoubleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> \"foo\"^^<http://example.com/datatype> }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("foo", "http://example.com/datatype")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void simpleSelectOnePatternTypedLiteralValueSingleQuoted() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'foo'^^<http://example.com/datatype> }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("foo", "http://example.com/datatype")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void simpleSelectWithPrefix() {
      Query query = ParseQuery("PREFIX eg: <http://example.com/> SELECT ?var WHERE { ?var eg:predicate 'foo'^^<http://example.com/datatype> }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("foo", "http://example.com/datatype")).ToString() , patterns[0].ToString() );      
    }


    [Test]
    public void simpleSelectWithMultiplePrefixes() {
      Query query = ParseQuery("PREFIX eg: <http://example.com/> PREFIX ex: <http://example.org/> SELECT ?var WHERE { ?var eg:predicate ex:subject }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new UriRef("http://example.org/subject")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void whereIsOptional() {
      Query query = ParseQuery("PREFIX eg: <http://example.com/> SELECT ?var { ?var eg:predicate 'foo'^^<http://example.com/datatype> }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("foo", "http://example.com/datatype")).ToString() , patterns[0].ToString() );      
    }


     [Test]
    public void numericInteger() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 1 }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("1", "http://www.w3.org/2001/XMLSchema#integer")).ToString() , patterns[0].ToString() );      
    }

     [Test]
    public void numericDecimal() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 1.3 }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("1.3", "http://www.w3.org/2001/XMLSchema#decimal")).ToString() , patterns[0].ToString() );      
    }

     [Test]
    public void numericDouble() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 1.3e5 }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("1.3e5", "http://www.w3.org/2001/XMLSchema#double")).ToString() , patterns[0].ToString() );      
    }

     [Test]
    public void booleanTrue() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> true }");      
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("true", "http://www.w3.org/2001/XMLSchema#boolean")).ToString() , patterns[0].ToString() );      
    }

     [Test]
    public void booleanFalse() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> false }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new TypedLiteral("false", "http://www.w3.org/2001/XMLSchema#boolean")).ToString() , patterns[0].ToString() );      
    }

     [Test]
    public void simpleSelectTwoPatternsWithValueDelimiter() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'x' ; 'y' }");      
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 2, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("x")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("y")).ToString() , patterns[1].ToString() );      
      
    }

     [Test]
    public void simpleSelectThreePatternsWithValueDelimiter() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'x' ; 'y' ; <http://example.com/obj> }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 3, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("x")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("y")).ToString() , patterns[1].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new UriRef("http://example.com/obj") ).ToString() , patterns[2].ToString() );      
      
    }

     [Test]
    public void simpleSelectTwoPatternsWithPredicateDelimiter() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'x' , <http://example.com/predicate2> 'y' }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 2, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("x")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate2"), new PlainLiteral("y")).ToString() , patterns[1].ToString() );      
      
    }

     [Test]
    public void simpleSelectPatternsWithPredicateAndValueDelimiters() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/predicate> 'x' ; 'y' , <http://example.com/predicate2> 'z' }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 3, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("x")).ToString() , patterns[0].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate"), new PlainLiteral("y")).ToString() , patterns[1].ToString() );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://example.com/predicate2"), new PlainLiteral("z")).ToString() , patterns[2].ToString() );      
    }

 
    [Test]
    public void simpleSelectAShorthand() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var a <http://example.com/obj> . }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      
      Assert.AreEqual( new Pattern( new Variable("var"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://example.com/obj")).ToString() , patterns[0].ToString() );      
    }

    [Test]
    public void simpleSelectBlankNode() {
      Query query = ParseQuery("SELECT ?var WHERE { ?var <http://example.com/pred> _:a . }");
      IList patterns = query.PatternGroup.Patterns;
      
      Assert.AreEqual( 1, patterns.Count );      

      Assert.IsTrue( ((Pattern)patterns[0]).GetObject() is BlankNode );
    }


    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Sparql.SparqlException))]
    public void PrefixRequiresTextAfterToBeUri() {
      Query query = ParseQuery("PREFIX eg: foo:bar SELECT ?var WHERE { ?var eg:predicate 'foo'^^<http://example.com/datatype> }");
    }

    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Sparql.SparqlException))]
    public void PrefixRequiresUri() {
      Query query = ParseQuery("PREFIX eg:");
    }


    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Sparql.SparqlException))]
    public void PrefixKeywordExpectsPrefixes() {
      Query query = ParseQuery("PREFIX foo SELECT * WHERE { ?x ?y ?z}");
    }
 
    [Test]
    public void DollarSignsForVariables() {
      Query query = ParseQuery("SELECT $var WHERE { <http://example.com/subject> <http://example.com/predicate> $var }");
      IList variables = query.Variables;
      Assert.AreEqual( 1, variables.Count );      
      Assert.AreEqual( "var", ((Variable)variables[0]).Name );      
    }

 
    //~ [Test]
    //~ [ExpectedException(typeof(SemPlan.Spiral.Sparql.SparqlException))]
    //~ public void MissingVariablesInSelect() {
      //~ Query query = ParseQuery("SELECT WHERE { ?var <http://example.com/pred> _:a . }";

      //~ QueryParser parser = new QueryParser();
      //~ Query query = parser.parse( sparql );

    //~ }

    private void AssertIsParseable(string queryText) {
      try {
        Query query = ParseQuery(queryText);
        Assert.IsNotNull( query );
      }
      catch(Exception e) {
        Assert.Fail( queryText + " was not parseable because " + e.ToString() );
      }
    }

    private void AssertIsNotParseable(string queryText) {
      try {
        Query query = ParseQuery(queryText);
        Assert.Fail( "Invalid query should fail parse: " + query );
      }
      catch(Exception) {  }
    }



    private void AssertParse(Query expectedQuery, string queryText) {
      AssertParse( expectedQuery, queryText, false);
    }
    private void AssertParse(Query expectedQuery, string queryText, bool explain) {
      QueryParser parser = new QueryParser();
      parser.Explain = explain;
      
      Query query = null;
      try {
        query = parser.Parse( queryText );
      }
      catch(Exception e) {
        Assert.Fail( queryText + " was not parseable because " + e.ToString() );
      }
      Assert.IsNotNull( query , "Parsed query should not be null" );
      Assert.AreEqual( expectedQuery, query, "Parsed query should equal expected query");
    }


    /// <summary>syntax-basic-01.rq</summary>
    [Test]
    public void syntax_basic_01_rq() {
      SparqlQuery expected  = new SparqlQuery();
      expected.SelectAll = true;
      
      AssertParse(expected, @"SELECT *
WHERE { }");
    }


    /// <summary>syntax-basic-02.rq</summary>
    [Test]
    public void syntax_basic_02_rq() {
      SparqlQuery expected  = new SparqlQuery();
      expected.SelectAll = true;

      AssertParse(expected, @"SELECT * {}");
    }

    /// <summary>syntax-basic-03.rq</summary>
    [Test]
    public void syntax_basic_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new Variable("x"), new Variable("y"), new Variable("z") ) );

      AssertParse(expected, @"# No trailing dot
PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { ?x ?y ?z }");
    }

    /// <summary>syntax-basic-04.rq</summary>
    [Test]
    public void syntax_basic_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new Variable("x"), new Variable("y"), new Variable("z") ) );

      AssertParse(expected, @"# With trailing dot
SELECT *
WHERE { ?x ?y ?z . }");
    }

    /// <summary>syntax-basic-05.rq</summary>
    [Test]
    public void syntax_basic_05_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new Variable("x"), new Variable("y"), new Variable("z") ) );
      expected.PatternGroup.AddPattern( new Pattern( new Variable("a"), new Variable("b"), new Variable("c") ) );

      AssertParse(expected, @"# Two triples : no trailing dot
SELECT *
WHERE { ?x ?y ?z . ?a ?b ?c }");
    }

    /// <summary>syntax-basic-06.rq</summary>
    [Test]
    public void syntax_basic_06_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new Variable("x"), new Variable("y"), new Variable("z") ) );
      expected.PatternGroup.AddPattern( new Pattern( new Variable("a"), new Variable("b"), new Variable("c") ) );

      AssertParse(expected, @"# Two triples : with trailing dot
SELECT *
WHERE { ?x ?y ?z . ?a ?b ?c . }");
    }

    /// <summary>syntax-bnodes-01.rq</summary>
    [Test]
    public void syntax_bnodes_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new BlankNode(), new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q") ) );
      AssertParse(expected, @"PREFIX : <http://example.org/ns#> SELECT * WHERE { [:p :q ] }");
    }

    /// <summary>syntax-bnodes-02.rq</summary>
    [Test]
    public void syntax_bnodes_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new BlankNode(), new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q") ) );
      AssertParse(expected, @"PREFIX : <http://example.org/ns#> SELECT * WHERE { [] :p :q }");
    }

    /// <summary>syntax-bnodes-03.rq</summary>
    [Test]
    public void syntax_bnodes_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new UriRef("http://example.org/ns#x"), new BlankNode(), new UriRef("http://example.org/ns#q") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#> SELECT * WHERE { :x [] :q }");
    }

    /// <summary>syntax-bnodes-04.rq</summary>
    [Test]
    public void syntax_bnodes_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern( new Pattern( new UriRef("http://example.org/ns#x"), new BlankNode(), new UriRef("http://example.org/ns#q") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#> SELECT * WHERE { :x _:a :q }");
    }

    /// <summary>syntax-bnodes-05.rq</summary>
    [Test]
    public void syntax_bnodes_05_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      BlankNode blank1 = new BlankNode();
      BlankNode blank2 = new BlankNode();
      expected.PatternGroup.AddPattern( new Pattern(blank1, new Variable("x"), new Variable("y") ) );
      expected.PatternGroup.AddPattern( new Pattern(blank1, new UriRef("http://example.org/ns#p"), blank2) );
      expected.PatternGroup.AddPattern( new Pattern(blank2, new Variable("pa"), new Variable("b") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#>
SELECT * WHERE { [ ?x ?y ] :p [ ?pa ?b ] }");
    }

    /// <summary>syntax-bnodes-06.rq</summary>
    [Test]
    public void syntax_bnodes_06_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new BlankNode(), new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { [ :p :q ; ] }");
    }

    /// <summary>syntax-bnodes-07.rq</summary>
    [Test]
    public void syntax_bnodes_07_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      BlankNode blank1 = new BlankNode();
      expected.PatternGroup.AddPattern(new Pattern(  blank1, new UriRef("http://example.org/ns#p1"), new UriRef("http://example.org/ns#q1") ) );
      expected.PatternGroup.AddPattern(new Pattern(  blank1, new UriRef("http://example.org/ns#p2"), new UriRef("http://example.org/ns#q2") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { _:a :p1 :q1 .
        _:a :p2 :q2 .
      }");
    }

    /// <summary>syntax-bnodes-08.rq</summary>
    [Test]
    public void syntax_bnodes_08_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      BlankNode blank1 = new BlankNode();
      BlankNode blank2 = new BlankNode();
      expected.PatternGroup.AddPattern( new Pattern(blank1, new Variable("x"), new Variable("y") ) );
      expected.PatternGroup.AddPattern( new Pattern(blank1, new UriRef("http://example.org/ns#p"), blank2) );
      expected.PatternGroup.AddPattern( new Pattern(blank2, new Variable("pa"), new Variable("b") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#>
SELECT * WHERE { [ ?x ?y ] :p [ ?pa ?b ] }");
    }

    /// <summary>syntax-bnodes-09.rq</summary>
    [Test]
    public void syntax_bnodes_09_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new BlankNode(), new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q") ) );

      AssertParse(expected, @"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { [ :p :q ; ] }");
    }

    /// <summary>syntax-expr-01.rq</summary>
    [Test]
    public void syntax_expr_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new Variable("s"), new Variable("p"), new Variable("o") ) );
      expected.AddConstraint( new Constraint( new VariableExpression( new Variable("o") ) ) );

      AssertParse(expected, @"SELECT * WHERE { ?s ?p ?o . FILTER (?o) }");
    }

    /// <summary>syntax-expr-02.rq</summary>
    [Test]
    public void syntax_expr_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new Variable("s"), new Variable("p"), new Variable("o") ) );
      expected.AddConstraint( new Constraint(  new VariableExpression( new Variable("o") ) ) );

      AssertParse(expected, @"SELECT *
WHERE { ?s ?p ?o . FILTER (?o) }");
    }

    /// <summary>syntax-expr-03.rq</summary>
    [Test]
    public void syntax_expr_03_rq() {
      AssertIsParseable(@"SELECT *
WHERE { ?s ?p ?o . FILTER REGEX(?o, ""foo"") }");
    }

    /// <summary>syntax-expr-04.rq</summary>
    [Test]
    public void syntax_expr_04_rq() {
      AssertIsParseable(@"SELECT *
WHERE { ?s ?p ?o . FILTER REGEX(?o, ""foo"", ""i"") }");
    }

    /// <summary>syntax-expr-05.rq</summary>
    [Test]
    public void syntax_expr_05_rq() {
      AssertIsParseable(@"PREFIX xsd:   <http://www.w3.org/2001/XMLSchema#>
SELECT *
WHERE { ?s ?p ?o . FILTER (xsd:integer(?o)) }");
    }

    /// <summary>syntax-expr-06.rq</summary>
    [Test]
    public void syntax_expr_06_rq() {
      AssertIsParseable(@"PREFIX :      <http://example.org/ns#> 
PREFIX xsd:   <http://www.w3.org/2001/XMLSchema#>
SELECT *
WHERE { ?s ?p ?o . FILTER (:myFunc(?s,?o)) }");
    }

    /// <summary>syntax-forms-01.rq</summary>
    [Test]
    public void syntax_forms_01_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/ns#>
SELECT * WHERE { ( [ ?x ?y ] ) :p ( [ ?pa ?b ] 57 ) }");
    }

    /// <summary>syntax-forms-02.rq</summary>
    [Test]
    public void syntax_forms_02_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/ns#>
SELECT * WHERE { ( [] [] ) }");
    }

    /// <summary>syntax-keywords-01.rq</summary>
    [Test]
    public void syntax_keywords_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new Variable("x"), new UriRef("http://example.org/ns#foo"), new Variable("z") ) );
      expected.AddConstraint( new Constraint(  new VariableExpression( new Variable("z") ) ) );

      AssertParse(expected, @"# use keyword FILTER as a namespace prefix
PREFIX FILTER: <http://example.org/ns#> 
SELECT *
WHERE { ?x FILTER:foo ?z FILTER (?z) }");
    }

    /// <summary>syntax-keywords-02.rq</summary>
    [Test]
    public void syntax_keywords_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new Variable("x"), new UriRef("http://example.org/ns#FILTER"), new Variable("z") ) );
      expected.AddConstraint( new Constraint(  new VariableExpression( new Variable("z") ) ) );

      AssertParse(expected, @"# use keyword FILTER as a local name
PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { ?x :FILTER ?z FILTER (?z) }");
    }

    /// <summary>syntax-keywords-03.rq</summary>
    [Test]
    public void syntax_keywords_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern(  new Variable("x"), new UriRef("http://example.org/ns#foo"), new Variable("z") ) );

      AssertParse(expected, @"# use keyword UNION as a namespace prefix
PREFIX UNION: <http://example.org/ns#> 
SELECT *
WHERE { ?x UNION:foo ?z }");
    }

    /// <summary>syntax-limit-offset-07.rq</summary>
    [Test]
    public void syntax_limit_offset_07_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Ascending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      expected.ResultLimit = 5;
      
      AssertParse(expected, @"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY ?o
LIMIT 5");
    }

    /// <summary>syntax-limit-offset-08.rq</summary>
    [Test]
    public void syntax_limit_offset_08_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Ascending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      expected.ResultLimit = 5;
      expected.ResultOffset = 3;
      
      AssertParse(expected, @"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY ?o
LIMIT 5
OFFSET 3");
    }

    /// <summary>syntax-limit-offset-09.rq</summary>
    [Test]
    public void syntax_limit_offset_09_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Ascending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      expected.ResultOffset = 3;
      
      AssertParse(expected, @"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY ?o
OFFSET 3");
    }

    /// <summary>syntax-lists-01.rq</summary>
    [Test]
    public void syntax_lists_01_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/ns#> 
SELECT * WHERE { ( ?x ) :p ?z  }");
    }

    /// <summary>syntax-lists-02.rq</summary>
    [Test]
    public void syntax_lists_02_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/ns#> 
SELECT * WHERE { ?x :p ( ?z ) }");
    }

    /// <summary>syntax-lists-03.rq</summary>
    [Test]
    public void syntax_lists_03_rq() {
      AssertIsParseable(@"SELECT * WHERE { ( ?z ) }");
    }
    
    /// <summary>syntax-lists-04.rq</summary>
    [Test]
    public void syntax_lists_04_rq() {
      AssertIsParseable(@"SELECT * WHERE { ( ( ?z ) ) }");
    }

    /// <summary>syntax-lists-05.rq</summary>
    [Test]
    public void syntax_lists_05_rq() {
      AssertIsParseable(@"SELECT * WHERE { ( ( ) ) }");
    }

    /// <summary>syntax-lit-01.rq</summary>
    [Test]
    public void syntax_lit_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p ""x"" }");
    }

    /// <summary>syntax-lit-02.rq</summary>
    [Test]
    public void syntax_lit_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p 'x' }");
    }

    /// <summary>syntax-lit-03.rq</summary>
    [Test]
    public void syntax_lit_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x\"y'z") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p ""x\""y'z"" }");
    }

    /// <summary>syntax-lit-04.rq</summary>
    [Test]
    public void syntax_lit_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x\"y'z") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p 'x""y\'z' }");
    }

    /// <summary>syntax-lit-05.rq</summary>
    [Test]
    public void syntax_lit_05_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x\"") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p ""x\"""" }");
    }

    /// <summary>syntax-lit-06.rq</summary>
    [Test]
    public void syntax_lit_06_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral("x'") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p 'x\'' }");
    }

    /// <summary>syntax-lit-07.rq</summary>
    [Test]
    public void syntax_lit_07_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new TypedLiteral("123", "http://www.w3.org/2001/XMLSchema#integer") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p 123 }");
    }

    /// <summary>syntax-lit-08.rq</summary>
    [Test]
    public void syntax_lit_08_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new TypedLiteral("123.", "http://www.w3.org/2001/XMLSchema#decimal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p 123. . }");
    }

    /// <summary>syntax-lit-09.rq</summary>
    [Test]
    public void syntax_lit_09_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long
""""
Literal
") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long
""""
Literal
"""""" }");
    }

    /// <summary>syntax-lit-10.rq</summary>
    [Test]
    public void syntax_lit_10_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long
'' """"""
Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long
'' """"""
Literal''' }");
    }

    /// <summary>syntax-lit-11.rq</summary>
    [Test]
    public void syntax_lit_11_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long""""""Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long""""\""Literal"""""" }");
    }

    /// <summary>syntax-lit-12.rq</summary>
    [Test]
    public void syntax_lit_12_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long'''Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long''\'Literal''' }");
    }

    /// <summary>syntax-lit-13.rq</summary>
    [Test]
    public void syntax_lit_13_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long""""""Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long\""""""Literal"""""" }");
    }

    /// <summary>syntax-lit-14.rq</summary>
    [Test]
    public void syntax_lit_14_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long'''Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long\'''Literal''' }");
    }

    /// <summary>syntax-lit-15.rq</summary>
    [Test]
    public void syntax_lit_15_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long '' Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long '' Literal''' }");
    }

    /// <summary>syntax-lit-16.rq</summary>
    [Test]
    public void syntax_lit_16_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long ' Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long ' Literal''' }");
    }

    /// <summary>syntax-lit-17.rq</summary>
    [Test]
    public void syntax_lit_17_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long''\Literal with '\ single quotes ") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p '''Long''\\Literal with '\\ single quotes ''' }");
    }

    /// <summary>syntax-lit-18.rq</summary>
    [Test]
    public void syntax_lit_18_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long """" Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long """" Literal"""""" }");
    }

    /// <summary>syntax-lit-19.rq</summary>
    [Test]
    public void syntax_lit_19_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long "" Literal") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long "" Literal"""""" }");
    }

    /// <summary>syntax-lit-20.rq</summary>
    [Test]
    public void syntax_lit_20_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.Base = "http://example.org/";
      expected.PatternGroup.AddPattern(new Pattern(  new UriRef("http://example.org/#x"), new UriRef("http://example.org/#p"), new PlainLiteral(@"Long""""\Literal with ""\ single quotes") ) );

      AssertParse(expected, @"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT * WHERE { :x :p """"""Long""""\\Literal with ""\\ single quotes"""""" }");
    }

    /// <summary>syntax-order-01.rq</summary>
    [Test]
    public void syntax_order_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Ascending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      
      AssertParse(expected,@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY ?o");
    }
    
    /// <summary>syntax-order-02.rq</summary>
    [Test]
    public void syntax_order_02_rq() {
      AssertIsParseable(@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY (?o+5)");
    }
    
    /// <summary>syntax-order-03.rq</summary>
    [Test]
    public void syntax_order_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Ascending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      
      AssertParse(expected,@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY ASC(?o)");
    }
    
    /// <summary>syntax-order-04.rq</summary>
    [Test]
    public void syntax_order_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );
      expected.OrderDirection = Query.SortOrder.Descending;
      expected.OrderBy = new VariableExpression( new Variable("o") );
      
      AssertParse(expected,@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY DESC(?o)");
    }
    
    /// <summary>syntax-order-05.rq</summary>
    [Test]
    public void syntax_order_05_rq() {
      AssertIsParseable(@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY DESC(:func(?s, ?o))");
    }
    
    /// <summary>syntax-order-06.rq</summary>
    [Test]
    public void syntax_order_06_rq() {
      AssertIsParseable(@"PREFIX :      <http://example.org/ns#> 
SELECT *
{ ?s ?p ?o }
ORDER BY 
  DESC(?o+57) :func2(?o) ASC(?s)");
    }
    
    /// <summary>syntax-pat-01.rq</summary>
    [Test]
    public void syntax_pat_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      
      AssertParse(expected, @"PREFIX : <http:/example.org/ns#> 
SELECT *
{ }");
    }
    
    /// <summary>syntax-pat-02.rq</summary>
    [Test]
    public void syntax_pat_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#x"), new Variable("y"), new Variable("z")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#x"), new UriRef("http://example.org/ns#y"), new UriRef("http://example.org/ns#z")  ) );

      AssertParse(expected, @"# No DOT after optional
PREFIX : <http://example.org/ns#> 
SELECT *
{ ?a :b :c OPTIONAL{:x :y :z} :x ?y ?z }");
    }
    
    /// <summary>syntax-pat-03.rq</summary>
    [Test]
    public void syntax_pat_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();

      group1.AddPattern(new Pattern( new Variable("a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      group1.AddPattern(new Pattern( new UriRef("http://example.org/ns#x1"), new UriRef("http://example.org/ns#y1"), new UriRef("http://example.org/ns#z1")  ) );
      group2.AddPattern(new Pattern( new UriRef("http://example.org/ns#x"), new UriRef("http://example.org/ns#y"), new UriRef("http://example.org/ns#z")  ) );
      group3.AddPattern(new Pattern( new UriRef("http://example.org/ns#x2"), new UriRef("http://example.org/ns#y2"), new UriRef("http://example.org/ns#z2")  ) );
      
      group1.OptionalGroup = group2;
      group1.AddAlternateGroup( group3 );
            
      expected.PatternGroup= group1;

      AssertParse(expected, @"# No DOT between non-triples patterns
PREFIX : <http://example.org/ns#> 
SELECT *
{ ?a :b :c 
  OPTIONAL{:x :y :z} 
  { :x1 :y1 :z1 } UNION { :x2 :y2 :z2 }
}");
    }
    
    /// <summary>syntax-pat-04.rq</summary>
    [Test]
    public void syntax_pat_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();

      group1.AddPattern(new Pattern( new Variable("a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      group1.AddPattern(new Pattern( new UriRef("http://example.org/ns#x1"), new UriRef("http://example.org/ns#y1"), new UriRef("http://example.org/ns#z1")  ) );
      group2.AddPattern(new Pattern( new UriRef("http://example.org/ns#x"), new UriRef("http://example.org/ns#y"), new UriRef("http://example.org/ns#z")  ) );
      group3.AddPattern(new Pattern( new UriRef("http://example.org/ns#x2"), new UriRef("http://example.org/ns#y2"), new UriRef("http://example.org/ns#z2")  ) );
      
      group1.OptionalGroup = group2;
      group1.AddAlternateGroup( group3 );
            
      expected.PatternGroup= group1;

      AssertParse(expected, @"# No DOT between non-triples patterns
PREFIX : <http://example.org/ns#> 
SELECT *
{
  OPTIONAL{:x :y :z} 
  ?a :b :c 
  { :x1 :y1 :z1 } UNION { :x2 :y2 :z2 }
}");
    }
    
    /// <summary>syntax-qname-01.rq</summary>
    [Test]
    public void syntax_qname_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("x"), new UriRef("http://example.org/ns#p"), new Variable("z")  ) );
      AssertParse(expected,@"PREFIX : <http://example.org/ns#> 
SELECT *
{ ?x :p ?z }");
    }
    
    /// <summary>syntax-qname-02.rq</summary>
    [Test]
    public void syntax_qname_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#x"), new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#z")  ) );
      AssertParse(expected,@"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { :x :p :z . }");
    }
    
    /// <summary>syntax-qname-03.rq</summary>
    [Test]
    public void syntax_qname_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new BlankNode(), new UriRef("http://example.org/ns#p.rdf"), new UriRef("http://example.org/ns#z.z")  ) );
      AssertParse(expected,@"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { :_1 :p.rdf :z.z . }");
    }
    
    /// <summary>syntax-qname-04.rq</summary>
    [Test]
    public void syntax_qname_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#"), new UriRef("http://example.org/ns2#"), new UriRef("http://example.org/ns#a")  ) );
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#"), new UriRef("http://example.org/ns#"), new UriRef("http://example.org/ns#")  ) );
      AssertParse(expected,@"PREFIX : <http://example.org/ns#> 
PREFIX a: <http://example.org/ns2#> 
SELECT *
WHERE { : a: :a . : : : . }");
    }
    
    /// <summary>syntax-qname-05.rq</summary>
    [Test]
    public void syntax_qname_05_rq() {
      AssertIsParseable(@"PREFIX :  <> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-06.rq</summary>
    [Test]
    public void syntax_qname_06_rq() {
      AssertIsParseable(@"PREFIX :  <#> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-07.rq</summary>
    [Test]
    public void syntax_qname_07_rq() {
      AssertIsParseable(@"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-08.rq</summary>
    [Test]
    public void syntax_qname_08_rq() {
      AssertIsParseable(@"BASE   <http://example.org/>
PREFIX :  <#> 
PREFIX x.y:  <x#> 
SELECT *
WHERE { :a.b  x.y:  : . }");
    }
    
    /// <summary>syntax-qname-09.rq</summary>
    [Test]
    public void syntax_qname_09_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/ns#> 
SELECT *
WHERE { :_1 :p.rdf :z.z . }");
    }
    
    /// <summary>syntax-qname-10.rq</summary>
    [Test]
    public void syntax_qname_10_rq() {
      AssertIsParseable(@"PREFIX :  <http://example.org/ns#> 
PREFIX a: <http://example.org/ns2#> 
SELECT *
WHERE { : a: :a . : : : . }");
    }
    
    /// <summary>syntax-qname-11.rq</summary>
    [Test]
    public void syntax_qname_11_rq() {
      AssertIsParseable(@"PREFIX :  <> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-12.rq</summary>
    [Test]
    public void syntax_qname_12_rq() {
      AssertIsParseable(@"PREFIX :  <#> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-13.rq</summary>
    [Test]
    public void syntax_qname_13_rq() {
      AssertIsParseable(@"BASE   <http://example.org/>
PREFIX :  <#> 
SELECT *
WHERE { : : : . }");
    }
    
    /// <summary>syntax-qname-14.rq</summary>
    [Test]
    public void syntax_qname_14_rq() {
      AssertIsParseable(@"BASE   <http://example.org/>
PREFIX :  <#> 
PREFIX x.y:  <x#> 
SELECT *
WHERE { :a.b  x.y:  : . }");
    }
    
    /// <summary>syntax-struct-01.rq</summary>
    [Test]
    public void syntax_struct_01_rq() {
      AssertIsParseable(@"# Operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { } }");
    }
    
    /// <summary>syntax-struct-02.rq</summary>
    [Test]
    public void syntax_struct_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-03.rq</summary>
    [Test]
    public void syntax_struct_03_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Triple, no DOT, operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-04.rq</summary>
    [Test]
    public void syntax_struct_04_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Triple, DOT, operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r . OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-05.rq</summary>
    [Test]
    public void syntax_struct_05_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Triple, DOT, operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r . OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-06.rq</summary>
    [Test]
    public void syntax_struct_06_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected,@"# Triple, DOT, operator, DOT
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r . OPTIONAL { :a :b :c } . }");
    }
    
    /// <summary>syntax-struct-07.rq</summary>
    [Test]
    public void syntax_struct_07_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Operator, no DOT
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-08.rq</summary>
    [Test]
    public void syntax_struct_08_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Operator, DOT
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { :a :b :c } . }");
    }
    
    /// <summary>syntax-struct-09.rq</summary>
    [Test]
    public void syntax_struct_09_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("x"), new Variable("y"), new Variable("z")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Operator, triple
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { :a :b :c } ?x ?y ?z }");
    }
    
    /// <summary>syntax-struct-10.rq</summary>
    [Test]
    public void syntax_struct_10_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new Variable("x"), new Variable("y"), new Variable("z")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Operator, DOT triple
PREFIX :  <http://example.org/ns#> 
SELECT *
{ OPTIONAL { :a :b :c } . ?x ?y ?z }");
    }
    
    /// <summary>syntax-struct-11.rq</summary>
    [Test]
    public void syntax_struct_11_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Triple, semi, operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r ; OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-struct-12.rq</summary>
    [Test]
    public void syntax_struct_12_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#p"), new UriRef("http://example.org/ns#q"), new UriRef("http://example.org/ns#r")  ) );
      expected.PatternGroup.OptionalGroup.AddPattern(new Pattern( new UriRef("http://example.org/ns#a"), new UriRef("http://example.org/ns#b"), new UriRef("http://example.org/ns#c")  ) );
      
      AssertParse(expected, @"# Triple, semi, DOT, operator
PREFIX :  <http://example.org/ns#> 
SELECT *
{ :p :q :r ; . OPTIONAL { :a :b :c } }");
    }
    
    /// <summary>syntax-union-01.rq</summary>
    [Test]
    public void syntax_union_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;

      PatternGroup group1 = new PatternGroup();
      group1.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );

      PatternGroup group2 = new PatternGroup();
      group2.AddPattern(new Pattern( new Variable("a"), new Variable("b"), new Variable("c")  ) );

      group1.AddAlternateGroup( group2 );
      expected.PatternGroup = group1;

      AssertParse(expected, @"PREFIX : <http://example.org/ns#>
SELECT *
{
  { ?s ?p ?o } UNION { ?a ?b ?c } 
}");
    }
    
    /// <summary>syntax-union-02.rq</summary>
    [Test]
    public void syntax_union_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.SelectAll = true;

      PatternGroup group1 = new PatternGroup();
      group1.AddPattern(new Pattern( new Variable("s"), new Variable("p"), new Variable("o")  ) );

      PatternGroup group2 = new PatternGroup();
      group2.AddPattern(new Pattern( new Variable("a"), new Variable("b"), new Variable("c")  ) );

      PatternGroup group3 = new PatternGroup();
      group3.AddPattern(new Pattern( new Variable("r"), new Variable("s"), new Variable("t")  ) );

      group1.AddAlternateGroup( group2 );
      group1.AddAlternateGroup( group3 );
      expected.PatternGroup = group1;

      AssertParse(expected, @"PREFIX : <http://example.org/ns#>
SELECT *
{
  { ?s ?p ?o } UNION { ?a ?b ?c } UNION { ?r ?s ?t }
}");
    }




    [Test]
    public void syntax_general_01_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b><c> }");
    }
    
    [Test]
    public void syntax_general_02_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>_:x }");
    }

    [Test]
    public void syntax_general_03_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>_:x FILTER(_:x < 3) }");
    }

    [Test]
    public void syntax_general_04_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>1 }");
    }

    [Test]
    public void syntax_general_05_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>+11 }");
    }

    [Test]
    public void syntax_general_06_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>-1 }");
    }

    [Test]
    public void syntax_general_07_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>1.0 }");
    }

    [Test]
    public void syntax_general_08_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>+1.0 }");
    }

    [Test]
    public void syntax_general_09_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>-1.0 }");
    }

    [Test]
    public void syntax_general_10_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>1.0e0 }");
    }

    [Test]
    public void syntax_general_11_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>+1.0e+1 }");
    }

    [Test]
    public void syntax_general_12_rq() {
      AssertIsParseable(@"SELECT * WHERE { <a><b>-1.0e-1 }");
    }

    [Test]
    public void syntax_lists_01_2_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { () :p 1 }");
    }

    [Test]
    public void syntax_lists_02_2_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { ( ) :p 1 }");
    }

    [Test]
    public void syntax_lists_03_2_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { ( 
) :p 1 }");
    }

    [Test]
    public void syntax_lists_04_2_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { ( 1 2
) :p 1 }");
    }

    [Test]
    public void syntax_lists_05_2_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { ( 1 2
) }");
    }


    [Test]
    public void syntax_bnode_01_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { [] :p [] }");
    }

    [Test]
    public void syntax_bnode_02_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
# Tab
SELECT * WHERE { [ ] :p [
	] }");
    }

    [Test]
    public void syntax_bnode_03_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT * WHERE { [ :p :q 
 ] }");
    }

    [Test]
    public void syntax_function_01_rq() {
      AssertIsParseable(@"PREFIX q: <http://example.org/>
SELECT * WHERE { FILTER (q:name()) }");
    }

    [Test]
    public void syntax_function_02_rq() {
      AssertIsParseable(@"PREFIX q: <http://example.org/>
SELECT * WHERE { FILTER (q:name( )) }");
    }

    [Test]
    public void syntax_function_03_rq() {
      AssertIsParseable(@"PREFIX q: <http://example.org/>
SELECT * WHERE { FILTER (q:name(
)) }");
    }

    [Test]
    public void syntax_function_04_rq() {
      AssertIsParseable(@"PREFIX q: <http://example.org/>
SELECT * WHERE { FILTER (q:name(1
)) . FILTER (q:name(1,2)) . FILTER (q:name(1
,2))}");
    }


    [Test]
    public void syntax_form_select_01_rq() {
      AssertIsParseable(@"SELECT * WHERE { }");
    }

    [Test]
    public void syntax_form_select_02_rq() {
      AssertIsParseable(@"SELECT * { }");
    }

    [Test]
    public void syntax_form_ask_02_rq() {
      AssertIsParseable(@"ASK {}");
    }

    [Test]
    public void syntax_form_construct_01_rq() {
      AssertIsParseable(@"CONSTRUCT { ?s <p1> <o> . ?s <p2> ?o } WHERE {?s ?p ?o}");
    }

    [Test]
    public void syntax_form_construct_02_rq() {
      AssertIsParseable(@"CONSTRUCT { ?s <p1> <o> . ?s <p2> ?o .} WHERE {?s ?p ?o}");
    }

    [Test]
    public void syntax_form_construct_03_rq() {
      AssertIsParseable(@"PREFIX  rdf:    <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
CONSTRUCT { [] rdf:subject ?s ;
               rdf:predicate ?p ;
               rdf:object ?o }
WHERE {?s ?p ?o}");
    }

    [Test]
    public void syntax_form_construct_04_rq() {
      AssertIsParseable(@"PREFIX  rdf:    <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
CONSTRUCT { [] rdf:subject ?s ;
               rdf:predicate ?p ;
               rdf:object ?o . }
WHERE {?s ?p ?o}");
    }

    [Test]
    public void syntax_form_construct_06_rq() {
      AssertIsParseable(@"CONSTRUCT {} WHERE {}");
    }

    [Test]
    public void syntax_form_describe_01_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.ResultForm = SparqlQuery.ResultFormType.Describe;
      expected.AddDescribeTerm( new UriRef("u") );

      AssertParse(expected, @"DESCRIBE <u>");
    }

    [Test]
    public void syntax_form_describe_02_rq() {
      SparqlQuery expected = new SparqlQuery();
      expected.ResultForm = SparqlQuery.ResultFormType.Describe;
      expected.AddDescribeTerm( new UriRef("u") );
      expected.AddDescribeTerm( new Variable("u") );
      expected.AddVariable( new Variable("u") );
      expected.PatternGroup.AddPattern(new Pattern( new UriRef("http://localhost/x"), new UriRef("http://localhost/q"), new Variable("u")  ) );

      AssertParse(expected, @"DESCRIBE <u> ?u WHERE { <x> <q> ?u . }");
    }


    [Test]
    public void syntax_dataset_01_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT ?x
FROM <http://example.org/graph>
WHERE {}");
    }

    [Test]
    public void syntax_dataset_02_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT ?x
FROM NAMED <http://example.org/graph1>
WHERE {}");
    }

    [Test]
    public void syntax_dataset_03_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT ?x
FROM NAMED :graph1
FROM NAMED :graph2
WHERE {}");
    }

    [Test]
    public void syntax_dataset_04_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT ?x
FROM :g1
FROM :g2
FROM NAMED :graph1
FROM NAMED :graph2
WHERE {}");
    }



    [Test]
    public void syntax_graph_01_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  GRAPH ?g { } 
}");
    }

    [Test]
    public void syntax_graph_02_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  GRAPH [] { } 
}");
    }

    [Test]
    public void syntax_graph_03_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  GRAPH :a { } 
}");
    }

    [Test]
    public void syntax_graph_04_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  GRAPH ?g { :x :b ?a } 
}");
    }

    [Test]
    public void syntax_graph_05_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  :x :p :z
  GRAPH ?g { :x :b ?a } 
}");
    }

    [Test]
    public void syntax_graph_06_rq() {
      AssertIsParseable(@"PREFIX : <http://example.org/>
SELECT *
WHERE
{
  :x :p :z
  GRAPH ?g { :x :b ?a . GRAPH [] { :x :p ?x } }
}");
    }

    [Test]
    public void syntax_esc_01_rq() {
      AssertIsParseable(@"SELECT *
WHERE { <x> <p> ""\t"" }");
    }

    [Test]
    public void syntax_esc_02_rq() {
      AssertIsParseable(@"SELECT *
WHERE { <x> <p> ""x\t"" }");
    }

    [Test]
    public void syntax_esc_03_rq() {
      AssertIsParseable(@"SELECT *
WHERE { <x> <p> ""\tx"" }");
    }

    [Test] [Category("KnownFailures")]
    public void syntax_esc_04_rq() {
      AssertIsParseable(@"PREFIX : <http://example/> 
SELECT *
WHERE { <\u0078> :\u0070 ?xx\u0078 }");
    }


    [Test]
    public void syntax_bad_01_rq() {
      AssertIsNotParseable(@"# More a test that bad syntax tests work!
PREFIX ex:   <http://example/ns#>
SELECT *
");
    }
    [Test]
    public void syntax_bad_02_rq() {
      AssertIsNotParseable(@"# Missing DOT between triples
PREFIX :   <http://example/ns#>
SELECT *
{ :s1 :p1 :o1 :s2 :p2 :o2 . }
");
    }
    [Test]
    public void syntax_bad_03_rq() {
      AssertIsNotParseable(@"# DOT, no triples
SELECT * WHERE
{ . }");
    }
    [Test]
    public void syntax_bad_04_rq() {
      AssertIsNotParseable(@"# DOT, no triples
SELECT * WHERE
{ . FILTER(?x) }
");
    }
    [Test]
    public void syntax_bad_05_rq() {
      AssertIsNotParseable(@"# Broken ;
SELECT * WHERE
{ ; FILTER(?x) }");
    }
    [Test]
    public void syntax_bad_06_rq() {
      AssertIsNotParseable(@"# Broken ;
PREFIX :   <http://example/ns#>
SELECT * WHERE
{ :s ; :p :o }");
    }
    [Test]
    public void syntax_bad_07_rq() {
      AssertIsNotParseable(@"# Broken ;
PREFIX :   <http://example/ns#>
SELECT * WHERE
{ :s :p ; }
");
    }
    [Test]
    public void syntax_bad_08_rq() {
      AssertIsNotParseable(@"# Broken ;
PREFIX :   <http://example/ns#>
SELECT * WHERE
{ :s :p ; FILTER(?x) }
");
    }
    [Test]
    public void syntax_bad_09_rq() {
      AssertIsNotParseable(@"# Broken ;
PREFIX :   <http://example/ns#>
SELECT * WHERE
{ :s :p :o . ;  }");
    }
    
    [Test]
    public void syntax_bad_10_rq() {
      AssertIsNotParseable(@"CONSTRUCT");
    }

 
     [Test]
    public void DescribeVariablesAreAddedToVariablesCollection() {
      Query query = ParseQuery("DESCRIBE ?var1 ,?var2 WHERE { <http://example.com/subject> <http://example.com/predicate> ?var }");      
      IList variables = query.Variables;
      
      Assert.AreEqual( 2, variables.Count );      
      
      Assert.AreEqual( "var1", ((Variable)variables[0]).Name );      
      Assert.AreEqual( "var2", ((Variable)variables[1]).Name );      
    }

 
 }  

}