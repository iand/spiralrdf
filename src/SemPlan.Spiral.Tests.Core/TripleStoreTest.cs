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
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  using System.IO;
  using System.Xml;
  
	/// <summary>
	/// Programmer tests for all TripleStore classes
	/// </summary>
  /// <remarks>
  /// $Id: TripleStoreTest.cs,v 1.5 2006/01/20 10:37:44 ian Exp $
  ///</remarks>
	[TestFixture]
  public abstract class TripleStoreTest : QuerySolverTest {


    private void AddDummyTriple(TripleStore store) {
      store.Add(new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj")));
    }

    private TripleStore MakeTripleStoreWithOneDummyStatement() {
      TripleStore store = MakeNewTripleStore();
      AddDummyTriple( store );
      return store;
    }

  
    [Test]
    public void NewInstanceIsEmpty() {
      TripleStore store = MakeNewTripleStore();
      Assert.IsTrue( store.IsEmpty() );
      store.Clear();
    }


    [Test]
    public void IsEmptyIsNotTrueIfStatementAdded() {
      TripleStore store = MakeTripleStoreWithOneDummyStatement();
      Assert.IsFalse( store.IsEmpty() );
      store.Clear();
   }

    [Test]
    public void IsEmptyAfterClear() {
      TripleStore store = MakeTripleStoreWithOneDummyStatement();
      store.Clear();
      Assert.IsTrue( store.IsEmpty() );
   }


    [Test]
    public void NodeCountZeroOnCreate() {
      TripleStore store = MakeNewTripleStore();
      Assert.AreEqual(0, store.NodeCount);
      store.Clear();
    }

    [Test]
    public void AddIncrementsNodeCountOnceForSubjectAndOnceForObject() {
      TripleStore store = MakeTripleStoreWithOneDummyStatement();
      Assert.AreEqual(3, store.NodeCount);
      store.Clear();
   }

    [Test]
    public void ClearMakesNodeCountZero() {
      TripleStore store = MakeTripleStoreWithOneDummyStatement();
      store.Clear();
      Assert.AreEqual(0, store.NodeCount);
    }


    [Test]
    public void StatementCountZeroOnCreate() {
      TripleStore store = MakeNewTripleStore();
      Assert.AreEqual(0, store.StatementCount);
      store.Clear();
    }

    [Test]
    public void AddStatementIncrementsStatementCount() {
      TripleStore store = MakeTripleStoreWithOneDummyStatement();
      Assert.AreEqual(1, store.StatementCount);
      store.Clear();
    }

    [Test]
    public void AddResourceStatementIncrementsStatementCount() {
      TripleStore store = MakeNewTripleStore();
      
      Resource subjectResource = store.GetResourceDenotedBy( new UriRef("http://example.com/subj") );
      Resource predicateResource = store.GetResourceDenotedBy( new UriRef("http://example.com/pred") );
      Resource objectResource = store.GetResourceDenotedBy( new UriRef("http://example.com/obj") );
      
      store.Add( new ResourceStatement( subjectResource, predicateResource, objectResource) );
      Assert.AreEqual(1, store.StatementCount);
      store.Clear();
    }

    [Test]
    public void ResourceCountZeroOnCreate() {
      TripleStore store = MakeNewTripleStore();
      Assert.AreEqual(0, store.ResourceCount);
      store.Clear();
    }

    [Test]
    public void ClearMakesResourceCountZero() {
      TripleStore store = MakeNewTripleStore();
      AddDummyTriple( store );
      store.Clear();
      Assert.AreEqual(0, store.ResourceCount);
    }

    [Test]
    public void AddDoesNotAllowDuplicates() {
      TripleStore store = MakeNewTripleStore();
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj")  ) );
      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<http://example.com/subj> <http://example.com/pred> <http://example.com/obj> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }

    [Test]
    public void GetResourceDenotedByAlwaysReturnsSameResourceForSameGraphMember() {
      TripleStore store = MakeNewTripleStore();
      Assert.AreEqual( store.GetResourceDenotedBy(new UriRef("http://example.com/uri")), store.GetResourceDenotedBy(new UriRef("http://example.com/uri")), "UriRef" );
      Assert.AreEqual( store.GetResourceDenotedBy(new PlainLiteral("squash") ), store.GetResourceDenotedBy( new PlainLiteral("squash")) , "Plain literal no language");
      Assert.AreEqual( store.GetResourceDenotedBy(new PlainLiteral("squash", "de") ), store.GetResourceDenotedBy(new PlainLiteral("squash", "de")), "Plain literal with language" );
      Assert.AreEqual( store.GetResourceDenotedBy(new TypedLiteral("squash", "http://example.com/type")), store.GetResourceDenotedBy(new TypedLiteral("squash", "http://example.com/type")), "Typed literal" );

      BlankNode blank = new BlankNode();
      Assert.AreEqual( store.GetResourceDenotedBy(blank), store.GetResourceDenotedBy( blank ), "Blank node" );
      store.Clear();
    }

    [Test]
    public void GetResourceDenotedReturnsSameResourceEvenAfterAllStatementsReferringToResourceHaveBeenRemoved() {
      TripleStore store = MakeNewTripleStore();
      
      BlankNode theSubjectNode = new BlankNode();
      UriRef thePredicateNode = new UriRef("http://example.com/pred");
      BlankNode theObjectNode = new BlankNode();
      
      store.Add(new Statement( theSubjectNode, thePredicateNode, theObjectNode ) );
    
      Resource theSubjectResourceBefore = store.GetResourceDenotedBy( theSubjectNode );
      Resource thePredicateResourceBefore = store.GetResourceDenotedBy( thePredicateNode );
      Resource theObjectResourceBefore = store.GetResourceDenotedBy( theObjectNode );

      store.Remove( new ResourceStatement( theSubjectResourceBefore, thePredicateResourceBefore, theObjectResourceBefore) );

      Assert.AreEqual( theSubjectResourceBefore, store.GetResourceDenotedBy( theSubjectNode ), "Subject resource should be the same");
      Assert.AreEqual( thePredicateResourceBefore, store.GetResourceDenotedBy( thePredicateNode ), "Predicate resource should be the same");
      Assert.AreEqual( theObjectResourceBefore, store.GetResourceDenotedBy( theObjectNode ), "Object resource should be the same");
      store.Clear();
    }

    [Test]
    public void GetResourceDenotedByDoesNotChangeIsEmptyFlag() {
      TripleStore store = MakeNewTripleStore();
      store.GetResourceDenotedBy(new UriRef("http://example.com/uri"));
      Assert.IsTrue( store.IsEmpty());
      store.Clear();
    }



    [Test]
    public void writeCallsWriterStartOutputOnce() {
      RdfWriterCounter writer = new RdfWriterCounter();
      TripleStore store = MakeNewTripleStore();
      
      store.Write(writer);
      
      Assert.AreEqual( 1, writer.StartOutputCalled );
      store.Clear();
    }

    [Test]
    public void writeCallsWriterEndOutputOnce() {
      RdfWriterCounter writer = new RdfWriterCounter();
      TripleStore store = MakeNewTripleStore();
      
      store.Write(writer);
      
      Assert.AreEqual( 1, writer.EndOutputCalled );
      store.Clear();
    }

    [Test]
    public void AddStatementDistinguishesDifferentBlankNodeInstancesForSubjects() {
      TripleStore store = MakeNewTripleStore();

      BlankNode node1 = new BlankNode();
      BlankNode node2 = new BlankNode();
      
      store.Add( new Statement( node1, new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );
      store.Add( new Statement( node1, new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2")) );
      store.Add( new Statement( node2, new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("_:node1 <http://example.com/pred> <http://example.com/obj1> .");
      verifier.expect("_:node1 <http://example.com/pred> <http://example.com/obj2> .");
      verifier.expect("_:node2 <http://example.com/pred> <http://example.com/obj2> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }

    [Test]
    public void AddStatementDistinguishesDifferentBlankNodeInstancesForObjects() {
      TripleStore store = MakeNewTripleStore();

      store.Add( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/pred"), new BlankNode()) );
      store.Add( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/pred"), new BlankNode()) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<http://example.com/subj1> <http://example.com/pred> _:blank1 .");
      verifier.expect("<http://example.com/subj1> <http://example.com/pred> _:blank2 .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }


    [Test]
    public void AddDistinguishesDifferentBlankNodeInstances() {
      Node theSubject = new NodeStub("http://example.com/subj");
      Arc thePredicate = new UriRef("http://example.com/pred");
      Node theFirstObject = new UriRef("http://example.com/obj1");
      Node theSecondObject = new UriRef("http://example.com/obj2");

      TripleStore store = MakeNewTripleStore();
      store.Add( new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );
      store.Add( new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("_:node1 <http://example.com/pred> <http://example.com/obj1> .");
      verifier.expect("_:node2 <http://example.com/pred> <http://example.com/obj2> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }
  
    [Test]
    public void GetStatementEnumeratorIsEmptyForNewTripleStore() {
      TripleStore store = MakeNewTripleStore();
      IEnumerator statementEnum = store.GetStatementEnumerator();
      Assert.IsFalse( statementEnum.MoveNext() , "Enumerator has no first statement");
      store.Clear();
    }

    [Test]
    public void GetStatementEnumeratorReturnsResourceStatementInstances() {
      TripleStore store = MakeNewTripleStore();
      store.Add( new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );

      IEnumerator statementEnum = store.GetStatementEnumerator();

      Assert.IsTrue( statementEnum.MoveNext() , "Enumerator has first statement");
      Assert.IsTrue( statementEnum.Current is ResourceStatement , "Enumerator returns a ResourceStatement");
      store.Clear();
    }

    [Test]
    public void GetStatementEnumerator() {
      Node theSubject = new NodeStub("http://example.com/subj");
      Arc thePredicate = new UriRef("http://example.com/pred");
      Node theFirstObject = new UriRef("http://example.com/obj1");
      Node theSecondObject = new UriRef("http://example.com/obj2");

      Statement statement1 = new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) ;
      Statement statement2 = new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2"));
      TripleStore store = MakeNewTripleStore();
      store.Add( statement1 );
      store.Add( statement2 );

      ResourceStatement resStatement1 = new ResourceStatement( store.GetResourceDenotedBy( statement1.GetSubject() ), store.GetResourceDenotedBy( statement1.GetPredicate() ), store.GetResourceDenotedBy( statement1.GetObject() ) );
      ResourceStatement resStatement2 = new ResourceStatement( store.GetResourceDenotedBy( statement2.GetSubject() ), store.GetResourceDenotedBy( statement2.GetPredicate() ), store.GetResourceDenotedBy( statement2.GetObject() ) );
      IEnumerator statementEnum = store.GetStatementEnumerator();
      
      Assert.IsTrue( statementEnum.MoveNext() , "Enumerator has first statement");
      
      ResourceStatement firstStatement = (ResourceStatement)statementEnum.Current;

      Assert.IsTrue( statementEnum.MoveNext() , "Enumerator has second statement");

      ResourceStatement secondStatement = (ResourceStatement)statementEnum.Current;

      Assert.IsFalse( statementEnum.MoveNext() );
      Assert.IsTrue( (firstStatement.Equals( resStatement1 ) && secondStatement.Equals( resStatement2)) ||(firstStatement.Equals( resStatement2) && secondStatement.Equals( resStatement1 )) , "Statements have correct components");
      store.Clear();
    }

    //~ [Test]
    //~ public void AddMakesNodesAvailable() {
      //~ TripleStore store = MakeNewTripleStore();
      //~ store.Add(new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj")) );
      //~ Assert.AreEqual(3, store.NodeCount);

      //~ IList nodes = store.nodes;
      //~ IEnumerator nodeEnum = nodes.GetEnumerator();
      
      //~ Assert.IsTrue( nodeEnum.MoveNext(), "Has first node");
      //~ Node node1 = (Node)nodeEnum.Current;
      
      //~ Assert.IsTrue( nodeEnum.MoveNext(), "Has second node");
      //~ Node node2 = (Node)nodeEnum.Current;

      //~ Assert.IsTrue( nodeEnum.MoveNext(), "Has third node");
      //~ Node node3 = (Node)nodeEnum.Current;

      //~ Assert.IsFalse( nodeEnum.MoveNext(), "Has no more nodes");


      //~ Assert.IsTrue( 
      
        //~ ( node1.Equals( new UriRef("http://example.com/subj") ) && node2.Equals( new UriRef("http://example.com/pred")) && node3.Equals( new UriRef("http://example.com/obj"))  ) 
        //~ || ( node1.Equals( new UriRef("http://example.com/subj") ) && node2.Equals( new UriRef("http://example.com/obj")) && node3.Equals( new UriRef("http://example.com/pred"))  ) 
        //~ || ( node1.Equals( new UriRef("http://example.com/obj") ) && node2.Equals( new UriRef("http://example.com/pred")) && node3.Equals( new UriRef("http://example.com/subj"))  ) 
        //~ ||( node1.Equals( new UriRef("http://example.com/obj") ) && node2.Equals( new UriRef("http://example.com/subj")) && node3.Equals( new UriRef("http://example.com/pred"))  ) 
        //~ || ( node1.Equals( new UriRef("http://example.com/pred") ) && node2.Equals( new UriRef("http://example.com/obj")) && node3.Equals( new UriRef("http://example.com/subj"))  ) 
        //~ || ( node1.Equals( new UriRef("http://example.com/pred") ) && node2.Equals( new UriRef("http://example.com/subj")) && node3.Equals( new UriRef("http://example.com/obj"))  ) 
        //~ );
   //~ }

    //~ [Test]
    //~ public void NodeCountAlwaysEqualsCountOfNodesProperty() {
      //~ TripleStore store = MakeNewTripleStore();
      //~ Assert.AreEqual(store.NodeCount, store.nodes.Count, "Empty store");

      //~ store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      //~ Assert.AreEqual(store.NodeCount, store.nodes.Count, "One triple");

      //~ store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      //~ store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      //~ store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      //~ Assert.AreEqual(store.NodeCount, store.nodes.Count, "4 triples");
      
    //~ }

    [Test]
    public void NodeCountAlwaysEqualsResourceCountForUnsmushedTriples() {
      TripleStore store = MakeNewTripleStore();
      Assert.AreEqual(store.NodeCount, store.ResourceCount);

      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      Assert.AreEqual(store.NodeCount, store.ResourceCount);

      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      Assert.AreEqual(store.NodeCount, store.ResourceCount);
      store.Clear();
    }

    [Test]
    public void cloneReturnsObjectOfSameType() {
      TripleStore store = MakeNewTripleStore();
      TripleStore clone = (TripleStore)store.Clone();
      Assert.AreEqual( store.GetType(), clone.GetType() );
      clone.Clear();
      store.Clear();
    }

    [Test]
    public void cloneReturnsTripleStoreContainingSameStatements() {
      TripleStore store = MakeNewTripleStore();

      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred2"), new PlainLiteral("foo", "jp") ) );
      store.Add(new Statement( new UriRef("http://example.com/the-thing"), new UriRef("http://example.com/pred3"), new TypedLiteral("1", "http://example.com/types/numeric") ) );
      store.Add(new Statement( new UriRef("http://example.com/other-thing"), new UriRef("http://example.com/pred4"), new UriRef("http://example.com/the-thing")) );

      TripleStore clone = (TripleStore)store.Clone();

      Assert.AreEqual(store.NodeCount, clone.NodeCount);
      Assert.AreEqual(store.ResourceCount, clone.ResourceCount);


      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("_:a <http://example.com/pred> _:b .");
      verifier.expect("_:c <http://example.com/pred2> \"foo\"@jp .");
      verifier.expect("<http://example.com/the-thing> <http://example.com/pred3> \"1\"^^<http://example.com/types/numeric> .");
      verifier.expect("<http://example.com/other-thing> <http://example.com/pred4> <http://example.com/the-thing> .");

      Assert.IsTrue( verifier.verify(clone) );

      clone.Clear();
      store.Clear();
    }
      
    [Test]
    public void CloneReturnsIndependentTripleStore() {
      TripleStore store = MakeNewTripleStore();
      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );

      TripleStore clone = (TripleStore)store.Clone();
      int cloneNodeCountBefore = clone.NodeCount;
      int cloneResourceCountBefore = clone.ResourceCount;

      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
      Assert.AreEqual(cloneNodeCountBefore, clone.NodeCount, "Clone node count is unchanged");
      Assert.AreEqual(cloneResourceCountBefore, clone.ResourceCount ,"Clone resource count is unchanged");

      store.Clear();
      Assert.AreEqual(cloneNodeCountBefore, clone.NodeCount, "Clone node count is unchanged after Clearing original");
      Assert.AreEqual(cloneResourceCountBefore, clone.ResourceCount ,"Clone resource count is unchanged after Clearing original");

    }


    [Test]
    public void ReadAndWriteRdf() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      TripleStore store = MakeNewTripleStore();

      SemPlan.Spiral.Core.ParserFactory parserFactory = new SemPlan.Spiral.XsltParser.XsltParserFactory();
      SemPlan.Spiral.Core.Parser parser = parserFactory.MakeParser( new ResourceFactory() , new StatementFactory());

      parser.NewStatement += store.GetStatementHandler();
      parser.Parse(  reader, "http://example.org/node" );
      parser.NewStatement -= store.GetStatementHandler();
   
      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
   
      store.Write( writer );
     
      Assert.AreEqual("<http://example.org/node> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://example.org/type> .\r\n", output.ToString());   
      store.Clear();
    }

    [Test]
    public void RemoveNonExistentTripleDoesNotDecreaseNodeCountOrResourceCount() {
      TripleStore store = MakeNewTripleStore();
      store.Add(new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new BlankNode() ) );
    
      int NodeCountBefore = store.NodeCount;
      int ResourceCountBefore = store.ResourceCount;

      store.Remove( new ResourceStatement( new Resource(), new Resource(), new Resource() ) );

      Assert.IsTrue( NodeCountBefore <= store.NodeCount, "Node count should not decrease" );
      Assert.IsTrue( ResourceCountBefore <=  store.ResourceCount, "Resource count should not decrease" );
      store.Clear();
   }

    [Test]
    public void RemoveExistingTriple() {
      TripleStore store = MakeNewTripleStore();
      
      BlankNode theSubjectNode = new BlankNode();
      UriRef thePredicateNode = new UriRef("http://example.com/pred");
      BlankNode theObjectNode = new BlankNode();
      
      store.Add(new Statement( theSubjectNode, thePredicateNode, theObjectNode ) );
    
      Resource theSubjectResource = store.GetResourceDenotedBy( theSubjectNode );
      Resource thePredicateResource = store.GetResourceDenotedBy( thePredicateNode );
      Resource theObjectResource = store.GetResourceDenotedBy( theObjectNode );

      //store.verbose = true;
      store.Remove( new ResourceStatement( theSubjectResource, thePredicateResource, theObjectResource ) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
     
      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
   }



    [Test]
    public void AddTripleStoreWithNoCommonNodes() {
      UriRef node1 = new  UriRef("any:node1");
      UriRef node2 = new  UriRef("any:node2");
      UriRef node3 = new  UriRef("any:node3");
      UriRef node4 = new  UriRef("any:node4");
      UriRef node5 = new  UriRef("any:node5");
      UriRef node6 = new  UriRef("any:node6");

      UriRef predicate1 = new UriRef("any:predicate1");
      UriRef predicate2 = new UriRef("any:predicate2");
      UriRef predicate3 = new UriRef("any:predicate3");
  
      Statement statement1 = new Statement(node1, predicate1, node2);
      Statement statement2 = new Statement(node3, predicate2, node4);
      Statement statement3 = new Statement(node5, predicate3, node6);
    
      TripleStore firstStore = MakeNewTripleStore();
      firstStore.Add(statement1);

      TripleStore secondStore = MakeNewTripleStore();
      secondStore.Add(statement2);
      secondStore.Add(statement3);

      firstStore.Add( secondStore );
      TripleStoreVerifier verifierFirst = new TripleStoreVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierFirst.expect("<any:node5> <any:predicate3> <any:node6> .");

      Assert.IsTrue( verifierFirst.verify(firstStore),"first triple store includes triples from second" );
      
      TripleStoreVerifier verifierSecond = new TripleStoreVerifier();
      verifierSecond.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierSecond.expect("<any:node5> <any:predicate3> <any:node6> .");

      Assert.IsTrue( verifierSecond.verify(secondStore), "second triple store is unchanged" );
      
      firstStore.Clear();
      secondStore.Clear();
    }

    [Test]
    public void AddTripleStoreWithCommonNodes() {
      UriRef node1 = new  UriRef("any:node1");
      UriRef node2 = new  UriRef("any:node2");
      UriRef node3 = new  UriRef("any:node3");
      UriRef node4 = new  UriRef("any:node4");
      UriRef node5 = new  UriRef("any:node5");
      UriRef node6 = new  UriRef("any:node6");

      UriRef predicate1 = new UriRef("any:predicate1");
      UriRef predicate2 = new UriRef("any:predicate2");
      UriRef predicate3 = new UriRef("any:predicate3");
  
      Statement statement1 = new Statement(node1, predicate1, node2);
      Statement statement2 = new Statement(node3, predicate2, node4);
      Statement statement3 = new Statement(node5, predicate3, node6);
      Statement statement4 = new Statement(node1, predicate2, node6);
    
      TripleStore firstStore = MakeNewTripleStore();
      firstStore.Add(statement1);

      TripleStore secondStore = MakeNewTripleStore();
      secondStore.Add(statement2);
      secondStore.Add(statement3);
      secondStore.Add(statement4);


      firstStore.Add( secondStore );
      TripleStoreVerifier verifierFirst = new TripleStoreVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierFirst.expect("<any:node5> <any:predicate3> <any:node6> .");
      verifierFirst.expect("<any:node1> <any:predicate2> <any:node6> .");
      Assert.IsTrue( verifierFirst.verify(firstStore),"first knowledge base includes triples from second" );
      
      TripleStoreVerifier verifierSecond = new TripleStoreVerifier();
      verifierSecond.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierSecond.expect("<any:node5> <any:predicate3> <any:node6> .");
      verifierSecond.expect("<any:node1> <any:predicate2> <any:node6> .");

      Assert.IsTrue( verifierSecond.verify(secondStore), "second knowledge base is unchanged" );
      firstStore.Clear();
      secondStore.Clear();
    }


    [Test]
    public void AddTripleStoreWithIdenticalNodesDenotingDifferentResources() {
      UriRef node1 = new  UriRef("any:node1");
      UriRef node2 = new  UriRef("any:node2");
      UriRef node3 = new  UriRef("any:node3");

      UriRef predicate1 = new UriRef("any:predicate1");
      UriRef predicate2 = new UriRef("any:predicate2");
  
      Statement statement1 = new Statement(node1, predicate1, node2);
      Statement statement2 = new Statement(node1, predicate2, node3);
    
      TripleStore firstStore = MakeNewTripleStore();
      firstStore.Add(statement1);

      Resource node1ResourceBefore = firstStore.GetResourceDenotedBy( node1 );

      TripleStore secondStore = MakeNewTripleStore();
      firstStore.Add(statement2); 

      firstStore.Add( secondStore );

      Resource node1ResourceAfter = firstStore.GetResourceDenotedBy( node1 );

      Assert.AreEqual( node1ResourceBefore, node1ResourceAfter);

      TripleStoreVerifier verifierFirst = new TripleStoreVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node1> <any:predicate2> <any:node3> .");
      Assert.IsTrue( verifierFirst.verify(firstStore),"first knowledge base includes triples from second" );

      firstStore.Clear();
      secondStore.Clear();
    }
    


    [Test]
    public void ResourcesPropertyContainsResourceInstances() {
      TripleStore store = MakeNewTripleStore();
      store.Add( new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );

      IList resources = store.Resources;

      Assert.AreEqual( 3, resources.Count , "Should be 3 resources");
      Assert.IsTrue( resources[0] is Resource , "First resource is an instance of Resource class");
      Assert.IsTrue( resources[1] is Resource , "First resource is an instance of Resource class");
      Assert.IsTrue( resources[2] is Resource , "First resource is an instance of Resource class");
      store.Clear();
    }

    //~ [Test]
    //~ public void nodesPropertyContainsNodeInstances() {
      //~ TripleStore store = MakeNewTripleStore();
      //~ store.Add( new Statement( new BlankNode(), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );

      //~ IList nodes = store.nodes;

      //~ Assert.AreEqual( 3, nodes.Count , "Should be 3 nodes");
      //~ Assert.IsTrue( nodes[0] is Node , "First node is an instance of Node class");
      //~ Assert.IsTrue( nodes[1] is Node , "First node is an instance of Node class");
      //~ Assert.IsTrue( nodes[2] is Node , "First node is an instance of Node class");
    //~ }

    [Test]
    public void AddDenotationMakesResourceAccesibleViaDenotee() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
     
      Resource originalResource = store.GetResourceDenotedBy(new UriRef("http://example.com/subj"));

      store.AddDenotation( new UriRef("foo:bar"), originalResource);
      
      Assert.AreEqual( originalResource, store.GetResourceDenotedBy(new UriRef("foo:bar")) );
      store.Clear();
    }

    [Test]
    public void EvaluateRule() {
      TripleStore store = MakeNewTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      store.Add( new Statement(new UriRef("http://example.com/subj1"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );
      store.Add( new Statement(new UriRef("http://example.com/subj2"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2")) );
      Rule rule = new Rule();
      rule.AddAntecedent( new Pattern( new Variable("var1"), new UriRef("http://example.com/pred"), new Variable("var2")) );
      rule.AddConsequent( new Pattern( new Variable("var1"), new UriRef("http://example.com/newPred"), new Variable("var2") ) );

      ruleProcessor.Process( rule, store );
      
      Assert.IsFalse( store.IsEmpty() , "Destination is non-empty");

      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/newPred"), new UriRef("http://example.com/obj1") ) ), "Destination contains first match consequent");
      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/newPred"), new UriRef("http://example.com/obj2") ) ), "Destination contains second match consequent");
      
      store.Clear();
    }    
  
    [Test]
    public void AddAllowsSingleQuotesInLiteralValues() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new PlainLiteral("d'arby")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> \"d'arby\" .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }

    [Test]
    public void AddAllowsSingleQuotesInLiteralLanguages() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new PlainLiteral("darby", "f'oo")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> \"darby\"@f'oo .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }

    [Test]
    public void AddAllowsSingleQuotesInLanguageLiteralValues() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new PlainLiteral("d'arby", "foo")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> \"d'arby\"@foo .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }
    
    [Test]
    public void AddAllowsSingleQuotesInTypedLiteralValues() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new TypedLiteral("d'arby", "ex:foo")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> \"d'arby\"^^<ex:foo> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }
    [Test]
    public void AddAllowsSingleQuotesInTypedLiteralDatatypes() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new TypedLiteral("darby", "ex:fo'o")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> \"darby\"^^<ex:fo'o> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }

    [Test]
    public void AddAllowsSingleQuotesInUriRefs() {
      TripleStore store = MakeNewTripleStore();
      
      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new UriRef("ex:fo'o")) );

      TripleStoreVerifier verifier = new TripleStoreVerifier();
      verifier.expect("<ex:subj> <ex:pred> <ex:fo'o> .");

      Assert.IsTrue( verifier.verify(store) );
      store.Clear();
    }
    
    
    
    [Test]
    public void Contains() {
      TripleStore store = MakeNewTripleStore();
      

      store.Add( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new UriRef("ex:obj"))  );

      Assert.IsTrue( store.Contains( new Statement( new UriRef("ex:subj"), new UriRef("ex:pred"), new UriRef("ex:obj")) ) );
      store.Clear();
    }
/* 

TODO: test first ts has 2 nodes denoting separate resources, second ts has same two nodes denoting single resource. And vice versa.

Need test for    void evaluate(Rule rule);, especially test circular rules

*/    

  }
  
}
