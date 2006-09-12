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

namespace SemPlan.Spiral.Tests.Core {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using SemPlan.Spiral.Utility; 
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for QuerySolver class
	/// </summary>
  /// <remarks>
  /// $Id: QuerySolverTest.cs,v 1.12 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
	[TestFixture]
  public abstract class QuerySolverTest {
    
    public abstract TripleStore MakeNewTripleStore();
    public IEnumerator GetSolutions(Query query, TripleStore tripleStore) {
      return GetSolutions(query, tripleStore, false);
    }
    public abstract IEnumerator GetSolutions(Query query, TripleStore tripleStore, bool explain);

    [Test]
    public void SingleStatementSingleVariable() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj")), solution1["var"] );    
      
      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }

    [Test]
    public void SingleStatementTwoVariables() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("var1"), new UriRef("http://example.com/property"), new Variable("var2") ) );
      Query query = builder.GetQuery();
  
      IEnumerator solutions = GetSolutions(query, statements);
      
      Assert.AreEqual( true, solutions.MoveNext() );      
      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }
  

  
    [Test]
    public void MultipleStatementsSingleVariableSingleSolution() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/other"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/other"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj")), solution1["var"] );    
      

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }
  

    [Test]
    public void MultipleStatementsSingleVariableTwoSolutions() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj1") ) );
      statements.Add( new Statement( new UriRef("http://example.com/other"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj2") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();
  
      QuerySolution expectedSolution1 = new QuerySolution();
      expectedSolution1["var"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/obj1"));
  
      QuerySolution expectedSolution2 = new QuerySolution();
      expectedSolution2["var"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/obj2"));


      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution2 = (QuerySolution)solutions.Current;

      Assert.AreEqual( false, solutions.MoveNext() );      

      Assert.IsTrue( expectedSolution1.Equals(solution1) && expectedSolution2.Equals(solution2) 
                            ||  expectedSolution1.Equals(solution2) && expectedSolution2.Equals(solution1) 
                            );    

      statements.Clear();
    }

    [Test]
    public void MultipleStatementsTwoPatternsSingleSolution() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/child1"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      statements.Add( new Statement( new UriRef("http://example.com/child1"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/dad") ) );
      statements.Add( new Statement( new UriRef("http://example.com/child2"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("child") , new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum")) );
      builder.AddPattern( new Pattern( new Variable("child"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/dad")) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("child") );
      
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/child1") ), solution1["child"] );    
      

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }

    [Test]
    public void MultipleStatementsTwoChainedPatternsSingleSolution() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/child1"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      statements.Add( new Statement( new UriRef("http://example.com/mum"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("child") , new UriRef("http://example.com/childOf"), new Variable("parent")) );
      builder.AddPattern( new Pattern( new Variable("parent"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran")) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("child") );
      query.AddVariable( new Variable("parent") );
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/child1") ), solution1["child"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/mum") ), solution1["parent"] );    
      

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }


    [Test]
    public void MultipleStatementsTwoChainedPatternsOneSolutionDifferentTripleOrder() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/mum"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran") ) );
      statements.Add( new Statement( new UriRef("http://example.com/child1"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("child") , new UriRef("http://example.com/childOf"), new Variable("parent")) );
      builder.AddPattern( new Pattern( new Variable("parent"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran")) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("child") );
      query.AddVariable( new Variable("parent") );
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/child1")), solution1["child"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/mum")), solution1["parent"] );    
      

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }

    [Test]
    public void MultipleStatementsTwoChainedPatternsTwoSolutions() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/child2"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      statements.Add( new Statement( new UriRef("http://example.com/mum"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran") ) );
      statements.Add( new Statement( new UriRef("http://example.com/child1"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/mum") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("child") , new UriRef("http://example.com/childOf"), new Variable("parent")) );
      builder.AddPattern( new Pattern( new Variable("parent"), new UriRef("http://example.com/childOf"), new UriRef("http://example.com/gran")) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("child") );
      query.AddVariable( new Variable("parent") );

      QuerySolution expectedSolution1 = new QuerySolution();
      expectedSolution1["child"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/child2") );
      expectedSolution1["parent"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/mum") );

      QuerySolution expectedSolution2 = new QuerySolution();
      expectedSolution2["child"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/child1") );
      expectedSolution2["parent"] = statements.GetResourceDenotedBy(new UriRef("http://example.com/mum") );

      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution1 = (QuerySolution)solutions.Current;

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;

      Assert.AreEqual( false, solutions.MoveNext() );      

      Assert.IsTrue( expectedSolution1.Equals(solution1) && expectedSolution2.Equals(solution2) 
                            ||  expectedSolution1.Equals(solution2) && expectedSolution2.Equals(solution1) 
                            );    

      statements.Clear();
    }

    [Test]
    public void MatchingPlainLiterals() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/fred"), new UriRef("http://example.com/name"), new PlainLiteral("Fred") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/name"), new PlainLiteral("Jim") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/knows"), new UriRef("http://example.com/fred")) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("person") , new UriRef("http://example.com/knows"), new Variable("friend")) );
      builder.AddPattern( new Pattern( new Variable("person"), new UriRef("http://example.com/name"), new PlainLiteral("Jim") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("friend") );
      query.AddVariable( new Variable("person") );

      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/fred") ), solution2["friend"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/jim") ), solution2["person"] );    

      Assert.AreEqual( false, solutions.MoveNext() );      
    }

    [Test]
    public void MatchingLiterals() {
      TripleStore statements = MakeNewTripleStore();

      
      statements.Add( new Statement( new UriRef("http://example.com/fred"), new UriRef("http://example.com/name"), new TypedLiteral("Fred", "http://example.com/integer") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/name"), new PlainLiteral("Jim", "en") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/knows"), new UriRef("http://example.com/fred")) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("person") , new UriRef("http://example.com/knows"), new Variable("friend")) );
      builder.AddPattern( new Pattern( new Variable("person"), new UriRef("http://example.com/name"), new PlainLiteral("Jim", "en") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("friend") );
      query.AddVariable( new Variable("person") );

      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext(), "Has first solution" );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/fred") ), solution2["friend"], "?friend is fred" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/jim") ), solution2["person"], "?person is jim" );    

      Assert.AreEqual( false, solutions.MoveNext(), "Has no further solutions" );      
      statements.Clear();
    }

    [Test]
    public void SelectingLiterals() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/fred"), new UriRef("http://example.com/name"), new PlainLiteral("Fred") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/name"), new PlainLiteral("Jim") ) );
      statements.Add( new Statement( new UriRef("http://example.com/jim"), new UriRef("http://example.com/knows"), new UriRef("http://example.com/fred")) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("person") , new UriRef("http://example.com/knows"), new Variable("friend")) );
      builder.AddPattern( new Pattern( new Variable("person"), new UriRef("http://example.com/name"), new PlainLiteral("Jim") ) );
      builder.AddPattern( new Pattern( new Variable("friend"), new UriRef("http://example.com/name"),  new Variable("friendName") ) );
      Query query = builder.GetQuery();
  
      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new PlainLiteral("Fred") ), solution1["friendName"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/jim") ), solution1["person"] );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }


    [Test]
    public void OrderOfPatternsIsNotSignificant() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement(new UriRef("http://example.com/subj1"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new UriRef("http://example.com/subj2")) );
      statements.Add( new Statement(new UriRef("http://example.com/subj2"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new UriRef("http://example.com/subj3")) );
      
      SimpleQueryBuilder builder1 = new SimpleQueryBuilder();
      builder1.AddPattern( new Pattern( new Variable("vvv"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new Variable("xxx") ) );
      builder1.AddPattern( new Pattern( new Variable("uuu"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new Variable("vvv") ) );
      Query query1 = builder1.GetQuery();

      IEnumerator solutions1 = GetSolutions(query1, statements);
      
      Assert.AreEqual( true, solutions1.MoveNext(),  "First pattern ordering gives solution" );      
      QuerySolution solution1 = (QuerySolution)solutions1.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj1") ), solution1["uuu"], "First pattern ordering has solution for variable uuu" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj3") ), solution1["xxx"], "First pattern ordering has solution for variable xxx" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj2") ), solution1["vvv"], "First pattern ordering has solution for variable vvv" );    

      Assert.AreEqual( false, solutions1.MoveNext(), "First pattern has no further solution" );      

      // Same query but order of patterns reversed
      SimpleQueryBuilder builder2 = new SimpleQueryBuilder();
      builder2.AddPattern( new Pattern( new Variable("uuu"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new Variable("vvv") ) );
      builder2.AddPattern( new Pattern( new Variable("vvv"), new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf"), new Variable("xxx") ) );
      Query query2 = builder2.GetQuery();

      IEnumerator solutions2 = GetSolutions(query2, statements);

      Assert.AreEqual( true, solutions2.MoveNext(), "Reversed pattern ordering gives solution" );      
      QuerySolution solution2 = (QuerySolution)solutions2.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj1") ), solution2["uuu"], "Reversed pattern ordering has solution for variable uuu" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj3") ), solution2["xxx"], "Reversed pattern ordering has solution for variable xxx" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj2") ), solution2["vvv"], "Reversed pattern ordering has solution for variable vvv" );    

      Assert.AreEqual( false, solutions2.MoveNext() , "Reversed pattern has no further solution" );      

      statements.Clear();
    }


    [Test]
    public void SolverUsesSingleDistinctVariableForSinglePattern() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj3"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("obj") );
      query.IsDistinct = true;
      
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj")), solution1["obj"] );    

      Assert.AreEqual( false, solutions.MoveNext(), "Should be only a single solution" );      

      statements.Clear();
    }

    [Test]
    public void SolverUsesSingleDistinctVariableForMultiplePatterns() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/obj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj2") ) );
      statements.Add( new Statement( new UriRef("http://example.com/obj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj3") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      builder.AddPattern( new Pattern( new Variable("obj"),  new UriRef("http://example.com/property"), new Variable("obj2") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("obj") );
      query.IsDistinct = true;
      
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj")), solution1["obj"] );    

      Assert.AreEqual( false, solutions.MoveNext(), "Should be only a single solution" );      

      statements.Clear();
    }

    [Test]
    public void NoSolutions() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( false, solutions.MoveNext() );    

      statements.Clear();
    }

    [Test]
    public void NoPatterns() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      Query query = builder.GetQuery();
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( false, solutions.MoveNext() );    

      statements.Clear();
    }

    [Test] 
    public void OptionalPatternMatchesIfPossible() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/other"), new UriRef("http://example.com/obj2") ) );

      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/other"), new Variable("obj2") ) );
      Query query = builder.GetQuery();
          
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj") ), solution1["subj"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj") ), solution1["obj"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj2") ), solution1["obj2"] );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    
    }
    
    [Test]
    public void OptionalPatternDoesntFailQueryIfNoMatch() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );

      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/other"), new Variable("obj2") ) );
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj") ), solution1["subj"] );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj") ), solution1["obj"] );    
      Assert.AreEqual( false, solution1.IsBound("obj2") );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    
    }

    [Test]
    public void OnlySelectedVariablesAreAvailableInSolution() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj3"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("obj") );
  
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( false, solution1.IsBound("subj") );
      statements.Clear();
    }


    [Test]
    public void QueriesInvolvingUnknownResourcesGiveNoResults() {
      TripleStore statements = MakeNewTripleStore();

      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern(new Pattern(new Variable("sub"), new UriRef("ex:foo"), new Variable("obj")));
      builder.AddPattern(new Pattern(new Variable("obj"), new UriRef("ex:foo2"), new Variable("obj2")));
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements);
      Assert.IsNotNull( solutions );
      Assert.IsFalse( solutions.MoveNext() );
    }


    [Test]
    public void SolverPopulatesNodesInQuerySolution() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements);
      solutions.MoveNext();
      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( new UriRef("http://example.com/obj"), solution1.GetNode("var") );    
    }


    [Test]
    public void AllPatternsInOptionalGroupCanMatch() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property2"), new UriRef("http://example.com/obj2") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property3"), new UriRef("http://example.com/obj3") ) );

      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property2"), new Variable("obj2") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property3"), new Variable("obj3") ) );
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements );
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( new UriRef("http://example.com/obj"), solution1.GetNode("obj") );    
      Assert.AreEqual( new UriRef("http://example.com/obj2"), solution1.GetNode("obj2") );    
      Assert.AreEqual( new UriRef("http://example.com/obj3"), solution1.GetNode("obj3") );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    
    }

    [Test]
    public void AllOrNoPatternsInOptionalGroupMustMatch() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property2"), new UriRef("http://example.com/obj2") ) );

      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property2"), new Variable("obj2") ) );
      builder.AddOptional( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property3"), new Variable("obj3") ) );
      Query query = builder.GetQuery();

      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( new UriRef("http://example.com/obj"), solution1.GetNode("obj") );    
      Assert.AreEqual( false, solution1.IsBound("obj2") );    
      Assert.AreEqual( false, solution1.IsBound("obj3") );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    
    }


    [Test] 
    public void AlternatePatternMatchesIfPossible() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/other"), new UriRef("http://example.com/obj2") ) );

      Query query = new Query();
      QueryGroupOr alternates = new QueryGroupOr();
      query.QueryGroup = alternates;
      
      QueryGroupAnd and1 = new QueryGroupAnd();
      alternates.Add( and1 );
      
      QueryGroupPatterns patterns1 = new QueryGroupPatterns();
      patterns1.Add( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      and1.Add( patterns1 );
      
      QueryGroupAnd and2 = new QueryGroupAnd();
      alternates.Add( and2 );

      QueryGroupPatterns patterns2 = new QueryGroupPatterns();
      patterns2.Add( new Pattern( new Variable("subj"),  new UriRef("http://example.com/other"), new Variable("obj2") ) );
      and2.Add( patterns2 );
          
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext(), "Should have first solution" );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj") ), solution1["subj"], "First solution should have binding for subj" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj") ), solution1["obj"], "First solution should have binding for obj" );    
      Assert.AreEqual( false, solution1.IsBound("obj2"), "First solution should not have binding for obj2" );    

      Assert.AreEqual( true, solutions.MoveNext(), "Should have second solution" );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj") ), solution2["subj"], "Second solution should have binding for subj" );    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj2") ), solution2["obj2"], "Second solution should have binding for obj2" );    
      Assert.AreEqual( false, solution2.IsBound("obj"), "Second solution should not have binding for obj" );    

      Assert.AreEqual( false, solutions.MoveNext(), "Should not have third solution" );      
      statements.Clear();
    
    }
    
    [Test]
    public void AlternatePatternDoesntFailQueryIfNoMatch() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );


      Query query = new Query();
      QueryGroupOr alternates = new QueryGroupOr();
      query.QueryGroup = alternates;
      
      QueryGroupAnd and1 = new QueryGroupAnd();
      alternates.Add( and1 );
      
      QueryGroupPatterns patterns1 = new QueryGroupPatterns();
      patterns1.Add( new Pattern( new Variable("subj"),  new UriRef("http://example.com/property"), new Variable("obj") ) );
      and1.Add( patterns1 );
      
      QueryGroupAnd and2 = new QueryGroupAnd();
      alternates.Add( and2 );

      QueryGroupPatterns patterns2 = new QueryGroupPatterns();
      patterns2.Add( new Pattern( new Variable("subj"),  new UriRef("http://example.com/other"), new Variable("obj2") ) );
      and2.Add( patterns2 );

      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/subj") ), solution1["subj"], "Should have binding for subj");    
      Assert.AreEqual( statements.GetResourceDenotedBy(new UriRef("http://example.com/obj") ), solution1["obj"], "Should have binding for obj" );    
      Assert.AreEqual( false, solution1.IsBound("obj2"), "Should not have binding for obj2" );    

      Assert.AreEqual( false, solutions.MoveNext(), "Should not have second solution" );      
      statements.Clear();
    }

 
    [Test]
    public void SingleConstraint() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new UriRef("http://example.com/obj") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("foo") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      builder.AddConstraint( new Constraint( new IsLiteral( new VariableExpression( new Variable("var") ) ) ) ); 
      Query query = builder.GetQuery();
      
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( statements.GetResourceDenotedBy(new PlainLiteral("foo")), solution1["var"] );    
      
      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }

      /**
     <remarks>
      Based on this example from the spec
 
      PREFIX foaf: <http://xmlns.com/foaf/0.1/>
      PREFIX vcard: <http://www.w3.org/2001/vcard-rdf/3.0#>
      SELECT ?foafName ?mbox ?gname ?fname
      WHERE
        {  ?x foaf:name ?foafName .
           OPTIONAL { ?x foaf:mbox ?mbox } .
           OPTIONAL {  ?x vcard:N ?vc .
                       ?vc vcard:Given ?gname .
                       OPTIONAL { ?vc vcard:Family ?fname }
                    }
        } 
    </remarks>
*/    
    [Test]
    public void NestedOptionals() {
      TripleStore statements = MakeNewTripleStore();
      BlankNode nodeA = new BlankNode();
      BlankNode nodeB = new BlankNode();
      BlankNode nodeC  = new BlankNode();
      BlankNode nodeD  = new BlankNode();
      BlankNode nodeE  = new BlankNode();
      BlankNode nodeF  = new BlankNode();
      
      statements.Add( new Statement( nodeA, new UriRef("http://xmlns.com/foaf/0.1/name"), new PlainLiteral("Alice") ) );
      statements.Add( new Statement( nodeA, new UriRef("http://xmlns.com/foaf/0.1/mbox"), new UriRef("mailto:alice@work.example") ) );
      statements.Add( new Statement( nodeA, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#N"), nodeB) );
      statements.Add( new Statement( nodeB, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Family"), new PlainLiteral("Hacker") ) );
      statements.Add( new Statement( nodeB, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Given"), new PlainLiteral("Alice") ) );
      
      statements.Add( new Statement( nodeC, new UriRef("http://xmlns.com/foaf/0.1/name"), new PlainLiteral("Bob") ) );
      statements.Add( new Statement( nodeC, new UriRef("http://xmlns.com/foaf/0.1/mbox"), new UriRef("mailto:bob@work.example") ) );
      statements.Add( new Statement( nodeC, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#N"), nodeD) );
      statements.Add( new Statement( nodeD, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Family"), new PlainLiteral("Hacker") ) );
      
      statements.Add( new Statement( nodeE, new UriRef("http://xmlns.com/foaf/0.1/name"), new PlainLiteral("Ella") ) );
      statements.Add( new Statement( nodeE, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#N"), nodeF) );
      statements.Add( new Statement( nodeF, new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Given"), new PlainLiteral("Eleanor") ) );
      
      
      Query query = new Query();
      query.OrderBy = new VariableExpression( new Variable("foafName" ) );


      QueryGroupAnd groupAnd = new QueryGroupAnd();
      QueryGroupPatterns groupRequired = new QueryGroupPatterns();
      groupRequired.Add( new Pattern( new Variable("x"), new UriRef("http://xmlns.com/foaf/0.1/name"), new Variable("foafName") ) );

      QueryGroupPatterns groupOptional = new QueryGroupPatterns();
      groupOptional.Add( new Pattern( new Variable("x"), new UriRef("http://xmlns.com/foaf/0.1/mbox"), new Variable("mbox") ) );

      groupAnd.Add( groupRequired );
      groupAnd.Add( new QueryGroupOptional( groupOptional ) );

      QueryGroupPatterns groupOptional2 = new QueryGroupPatterns();
      groupOptional2.Add( new Pattern( new Variable("x"), new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#N"), new Variable("vc") ) );
      groupOptional2.Add( new Pattern( new Variable("vc"), new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Given"), new Variable("gname") ) );
      
      QueryGroupPatterns groupOptional3 = new QueryGroupPatterns();
      groupOptional3.Add( new Pattern( new Variable("vc"), new UriRef("http://www.w3.org/2001/vcard-rdf/3.0#Family"), new Variable("fname") ) );

      QueryGroupAnd groupAnd3 = new QueryGroupAnd();
      groupAnd3.Add( groupOptional2 );
      groupAnd3.Add( new QueryGroupOptional( groupOptional3 ) );

      QueryGroupAnd groupAndRoot = new QueryGroupAnd();
      groupAndRoot.Add( groupAnd );
      groupAndRoot.Add( new QueryGroupOptional( groupAnd3 ) );
      

      query.QueryGroup = groupAndRoot;

      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual(  new PlainLiteral("Alice"), solution1.GetNode("foafName") );    
      Assert.AreEqual(  new UriRef("mailto:alice@work.example"), solution1.GetNode("mbox") );    
      Assert.AreEqual(  new PlainLiteral("Alice"), solution1.GetNode("gname") );    
      Assert.AreEqual(  new PlainLiteral("Hacker"), solution1.GetNode("fname") );    
      
      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual(  new PlainLiteral("Bob"), solution2.GetNode("foafName") );    
      Assert.AreEqual(  new UriRef("mailto:bob@work.example"), solution2.GetNode("mbox") );    
      Assert.AreEqual(  false, solution2.IsBound("gname") );    
      Assert.AreEqual(  false, solution2.IsBound("fname") );    

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution3 = (QuerySolution)solutions.Current;
      Assert.AreEqual(  new PlainLiteral("Ella"), solution3.GetNode("foafName") );    
      Assert.AreEqual(  false, solution3.IsBound("mbox") );    
      Assert.AreEqual(  new PlainLiteral("Eleanor"), solution3.GetNode("gname") );    
      Assert.AreEqual(  false, solution3.IsBound("fname") );    

      Assert.AreEqual( false, solutions.MoveNext() );      
      statements.Clear();
    }


    [Test]
    public void ConstraintAppliesToOptionalBlock() {
      TripleStore statements = MakeNewTripleStore();
      BlankNode nodeA = new BlankNode();
      BlankNode nodeB = new BlankNode();

      statements.Add( new Statement( nodeA, new UriRef("http://xmlns.com/foaf/0.1/givenName"), new PlainLiteral("Alice") ) );
      statements.Add( new Statement( nodeB, new UriRef("http://xmlns.com/foaf/0.1/givenName"), new PlainLiteral("Bob") ) );
      statements.Add( new Statement( nodeB, new UriRef("http://purl.org/dc/elements/1.1/date"), new PlainLiteral("2005-04-04T04:04:04Z") ) );
      
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      Query query = new Query();
      
      QueryGroupAnd groupAnd = new QueryGroupAnd();
      
      QueryGroupPatterns groupRequired = new QueryGroupPatterns();
      QueryGroupPatterns groupOptional = new QueryGroupPatterns();
      QueryGroupConstraints groupConstraints = new QueryGroupConstraints();
      
      query.AddVariable( new Variable("name") );
      groupRequired.Add( new Pattern( new Variable("x"), new UriRef("http://xmlns.com/foaf/0.1/givenName"), new Variable("name") ) );
      groupOptional.Add( new Pattern( new Variable("x"),  new UriRef("http://purl.org/dc/elements/1.1/date"), new Variable("date") ) );
      groupConstraints.Add( new Constraint( new Bound( new Variable("date") ) ) ); 

      groupAnd.Add( groupRequired );
      groupAnd.Add( groupOptional );
      groupAnd.Add( groupConstraints );
      
      query.QueryGroup = groupAnd;
      
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual(  new PlainLiteral("Bob"), solution1.GetNode("name") );    

      Assert.AreEqual( false, solutions.MoveNext(), "Should be no further solutions" );    
    }

    [Test]
    public void OptionalBlockMatchesAllPossible() {
      TripleStore statements = MakeNewTripleStore();
      BlankNode nodeA = new BlankNode();
      BlankNode nodeB = new BlankNode();

      statements.Add( new Statement( nodeA, new UriRef("http://xmlns.com/foaf/0.1/givenName"), new PlainLiteral("Alice") ) );
      statements.Add( new Statement( nodeB, new UriRef("http://xmlns.com/foaf/0.1/givenName"), new PlainLiteral("Bob") ) );
      statements.Add( new Statement( nodeB, new UriRef("http://purl.org/dc/elements/1.1/date"), new PlainLiteral("2005-04-04T04:04:04Z") ) );
      
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new Variable("x"), new UriRef("http://xmlns.com/foaf/0.1/givenName"), new Variable("name") ) );
      builder.AddOptional( new Pattern( new Variable("x"),  new UriRef("http://purl.org/dc/elements/1.1/date"), new Variable("date") ) );
      Query query = builder.GetQuery();
      query.AddVariable( new Variable("name") );
     
      IEnumerator solutions = GetSolutions(query, statements);

      Assert.AreEqual( true, solutions.MoveNext() );    

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( true, solutions.MoveNext() , "Should have second solution");    
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( false, solutions.MoveNext(), "Should be no further solutions" );    
      
      Assert.IsTrue( 
        ( solution1.GetNode("name").Equals( new PlainLiteral("Bob") )  && solution2.GetNode("name").Equals( new PlainLiteral("Alice") ) )
        ||
        ( solution1.GetNode("name").Equals( new PlainLiteral("Alice") )  && solution2.GetNode("name").Equals( new PlainLiteral("Bob") ) )
      );

    }

    [Test]
    public void OrderBySimpleVariableExpressionAscending() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("c") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("a") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("b") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();
      query.OrderBy = new VariableExpression( new Variable("var" ) );
      query.OrderDirection = Query.SortOrder.Ascending;
      
      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "a", solution1.GetNode("var").GetLabel() );      

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "b", solution2.GetNode("var").GetLabel() );      

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution3 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "c", solution3.GetNode("var").GetLabel() );      

      statements.Clear();
    }

    [Test]
    public void OrderBySimpleVariableExpressionDescending() {
      TripleStore statements = MakeNewTripleStore();
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("c") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("a") ) );
      statements.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new PlainLiteral("b") ) );
      
      SimpleQueryBuilder builder = new SimpleQueryBuilder();
      builder.AddPattern( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/property"), new Variable("var") ) );
      Query query = builder.GetQuery();
      query.OrderBy = new VariableExpression( new Variable("var" ) );
      query.OrderDirection = Query.SortOrder.Descending;
      
      IEnumerator solutions = GetSolutions(query, statements);
      Assert.AreEqual( true, solutions.MoveNext() );      

      QuerySolution solution1 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "c", solution1.GetNode("var").GetLabel() );      

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution2 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "b", solution2.GetNode("var").GetLabel() );      

      Assert.AreEqual( true, solutions.MoveNext() );      
      QuerySolution solution3 = (QuerySolution)solutions.Current;
      Assert.AreEqual( "a", solution3.GetNode("var").GetLabel() );      

      statements.Clear();
    }

  }  

  


  
}