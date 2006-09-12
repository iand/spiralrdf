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

namespace SemPlan.Spiral.Tests.Utility {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for BacktrackingQuerySolver class
	/// </summary>
  /// <remarks>
  /// $Id: BacktrackingQuerySolverTest.cs,v 1.5 2006/02/07 01:11:14 ian Exp $
  ///</remarks>
	[TestFixture]
  public class BacktrackingQuerySolverTest : QuerySolverTest {

    public override TripleStore MakeNewTripleStore() {
      return new MemoryTripleStore();
    }
    
    public override IEnumerator GetSolutions(Query query, TripleStore tripleStore, bool explain) {
      return new BacktrackingQuerySolver( query, tripleStore, explain);
    }

    

		[Test]
        //[Ignore("This is related to the enumerator.reset issue looked at in the two reification tests")]
		public void OrderOfPatternsIsNotSignificantWithMoreThanTwoPatterns() 
		{
			MemoryTripleStore statements = new MemoryTripleStore();

			UriRef gran = new UriRef("http://example.com/gran");
			UriRef mum = new UriRef("http://example.com/mum");
			UriRef uncle = new UriRef("http://example.com/uncle");
			UriRef child = new UriRef("http://example.com/child");

			UriRef parentOf = new UriRef("http://exmaple.com/parentOf");
			UriRef siblingOf = new UriRef("http://example.com/siblingOf");
			
			statements.Add(new Statement(gran, parentOf, mum));
			statements.Add(new Statement(mum, siblingOf, uncle));
			statements.Add(new Statement(mum, parentOf, child));
			
			Query query = new Query();
			query.AddPattern( new Pattern( new Variable("mum"), parentOf, child ) );
			query.AddPattern( new Pattern( new Variable("mum"), siblingOf, uncle) );
			query.AddPattern( new Pattern( gran, parentOf, new Variable("mum")));

			IEnumerator solutions = statements.Solve( query );
			Assert.IsTrue(solutions.MoveNext(), "No solution found");

			query = new Query();
			query.AddPattern( new Pattern( gran, parentOf, new Variable("mum")));
			query.AddPattern( new Pattern( new Variable("mum"), siblingOf, uncle) );
			query.AddPattern( new Pattern( new Variable("mum"), parentOf, child ) );
			
			solutions = statements.Solve( query );
			Assert.IsTrue(solutions.MoveNext(), "No solution found");
		}

        [Test]
        public void StatementAboutReificationCanBeFoundUsingQuery()
        {
            IEnumerator solutions = SolveSolvableReificationQuery();
            bool hasSolutions = solutions.MoveNext();
            Assert.AreEqual(true, hasSolutions);
        }

        [Test]
        public void FindSingleStatementAboutReificationReturnsSingleSolution()
        {
            IEnumerator solutions = SolveSolvableReificationQuery();
            bool hasSolutions = solutions.MoveNext();
            Assert.AreEqual(true, hasSolutions);
            Assert.AreEqual(false, solutions.MoveNext());
        }

        private IEnumerator SolveSolvableReificationQuery()
        {
            MemoryTripleStore store = SetUpReificationStatementsInStore();
            Query query = ConstructReificationTestQuery();
            return store.Solve(query);
        }

        // constants used in below methods 

        // Stuff from RDF schema
        UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
        UriRef rdfStatement = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement");
        UriRef rdfSubject = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#subject");
        UriRef rdfPredicate = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate");
        UriRef rdfObject = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#object");

        // Stuff about my thing.
        // My Thing makes a statement about something else. Describe this using reification.
        UriRef myThing = new UriRef("http://example.org/test#thing");
        UriRef makesStatement = new UriRef("http://example.org/test#makesStatement");

        // Stuff about the statement made by my thing
        UriRef mySubject = new UriRef("http://example.org/test#subject");
        UriRef myPredicate = new UriRef("http://example.org/test#predicate");
        UriRef myObject = new UriRef("http://example.org/test#object");

        private MemoryTripleStore SetUpReificationStatementsInStore()
        {
            // bnode is the statement made by myThing
            BlankNode bnode = new BlankNode();

            MemoryTripleStore store = new MemoryTripleStore();
            store.Add(new Statement(myThing, makesStatement, bnode));
            store.Add(new Statement(bnode, rdfType, rdfStatement));
            store.Add(new Statement(bnode, rdfSubject, mySubject));
            store.Add(new Statement(bnode, rdfPredicate, myPredicate));
            store.Add(new Statement(bnode, rdfObject, myObject));

            //store.Dump();
            //Console.WriteLine("\n\n");
            //store.Write(new RdfXmlWriter(new System.Xml.XmlTextWriter(Console.Out)));
            //Console.WriteLine("\n\n");

            return store;
        }

        private Query ConstructReificationTestQuery()
        {
            Query query = new Query();
            Variable r = new Variable("r");
            //query.AddDistinct(r);
            query.AddPattern(new Pattern(r, rdfSubject, mySubject));
            query.AddPattern(new Pattern(r, rdfPredicate, myPredicate));
            query.AddPattern(new Pattern(r, rdfObject, myObject));
            query.AddPattern(new Pattern(r, rdfType, rdfStatement));
            query.AddPattern(new Pattern(myThing, makesStatement, r));
            return query;
        }

  }  


  


  
}