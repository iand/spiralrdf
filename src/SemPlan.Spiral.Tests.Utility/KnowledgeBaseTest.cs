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

namespace SemPlan.Spiral.Tests.Utility {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.XsltParser;
  using System;  
  using System.Collections;
  using System.IO;
  using System.Xml;
  
	/// <summary>
	/// Programmer tests for KnowledgeBase class
	/// </summary>
  /// <remarks>
  /// $Id: KnowledgeBaseTest.cs,v 1.3 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class KnowledgeBaseTest {

    [Test]
    public void CountZeroOnCreate() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      Assert.AreEqual(0, knowledge.Count);
    }
    
    [Test]
    public void AddIncrementsCountOnce() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(new StatementStub());
      Assert.AreEqual(1, knowledge.Count);
   }

    [Test]
    public void ClearMakesCountZero() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(new StatementStub());
      knowledge.Clear();
      Assert.AreEqual(0, knowledge.Count);
    }


    [Test]
    public void GetDescriptionOf() {
      NodeStub theSubject = new NodeStub("theSubject");
      UriRef thePredicate = new UriRef("thePredicate");
      NodeStub theObject = new NodeStub("theObject");
  
      Statement statement = new Statement(theSubject, thePredicate, theObject);
    
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(statement);
      ResourceDescription desc = knowledge.GetDescriptionOf(theSubject);
      
      Assert.IsNotNull( desc );
      bool hasProperty =  desc.Contains( thePredicate, theObject );
      
      Assert.IsTrue(  hasProperty , "has property");
    } 


    [Test]
    public void GetDescriptionOfUnknownNodeReturnsEmptyDescription() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      NodeStub theSubject = new NodeStub("theSubject");

      ResourceDescription desc = knowledge.GetDescriptionOf(theSubject);
      
      Assert.IsNotNull( desc , "description is not null");
      Assert.IsTrue(  desc.IsEmpty() , "description is empty");

    }

    [Test]
    public void newInstanceIsEmpty() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      Assert.IsTrue( knowledge.IsEmpty() );
    }
    
    [Test]
    public void IsEmptyIsNotTrueIfStatementAdded() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(new StatementStub());
      Assert.IsFalse( knowledge.IsEmpty() );
   }

    [Test]
    public void IsEmptyAfterClear() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(new StatementStub());
      knowledge.Clear();
      Assert.IsTrue( knowledge.IsEmpty() );
   }


    [Test]
    public void IncludeRdfXmlViaReader() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      KnowledgeBase knowledge = new KnowledgeBase( new XsltParserFactory() );
      knowledge.Include( reader, "http://example.org/node");
   
      Assert.AreEqual(1, knowledge.Count);   
      
      ResourceDescription desc = knowledge.GetDescriptionOf( new UriRef("http://example.org/node"));
      
      Assert.IsNotNull( desc );
      Assert.IsTrue( desc.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"),  knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.org/type"))) );
    }

    [Test]
    public void ReadAndWriteRdf() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      KnowledgeBase knowledge = new KnowledgeBase( new XsltParserFactory() );
      knowledge.Include( reader, "http://example.org/node");
   
      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
   
      knowledge.Write( writer );
      
      
      Assert.AreEqual("<http://example.org/node> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://example.org/type> .\r\n", output.ToString());   
    }

  
    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Utility.KnowledgeBaseException))]
    public void ParseUriStringConvertsParserExceptions() {
      ParserThrower parser = new ParserThrower();
      ParserFactoryResponder factory = new ParserFactoryResponder(parser);

      KnowledgeBase knowledge = new KnowledgeBase(factory);
      knowledge.Include( "http://example.com");
    }
  
    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Utility.KnowledgeBaseException))]
    public void ParseUriConvertsParserExceptions() {
      ParserThrower parser = new ParserThrower();
      ParserFactoryResponder factory = new ParserFactoryResponder(parser);

      KnowledgeBase knowledge = new KnowledgeBase(factory);
      knowledge.Include( new Uri("http://example.com") );
    }

    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Utility.KnowledgeBaseException))]
    public void ParseTextReaderConvertsParserExceptions() {
      ParserThrower parser = new ParserThrower();
      ParserFactoryResponder factory = new ParserFactoryResponder(parser);

      KnowledgeBase knowledge = new KnowledgeBase(factory);
      knowledge.Include( new StringReader("foo"), "http://base.example.com/" );
    }

    [Test]
    [ExpectedException(typeof(SemPlan.Spiral.Utility.KnowledgeBaseException))]
    public void ParseStreamConvertsParserExceptions() {
      ParserThrower parser = new ParserThrower();
      ParserFactoryResponder factory = new ParserFactoryResponder(parser);

      KnowledgeBase knowledge = new KnowledgeBase(factory);
      knowledge.Include( new MemoryStream(), "http://base.example.com/" );
    }





    [Test]
    public void ParsingTwoIdenticalDocumentsKeepsBlankNodesSeparate() {
      string rdfContent = @"<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#'><rdf:Description><rdf:value>bar</rdf:value></rdf:Description></rdf:RDF>";

      StringReader documentReader1 = new StringReader( rdfContent );
      StringReader documentReader2 = new StringReader( rdfContent );
      
      KnowledgeBase knowledge = new KnowledgeBase( new XsltParserFactory() );
      knowledge.Include( documentReader1, "" );
      knowledge.Include( documentReader2, "" );
      
      KnowledgeBaseVerifier verifier = new KnowledgeBaseVerifier();
      verifier.expect("_:node1 <http://www.w3.org/1999/02/22-rdf-syntax-ns#value> \"bar\" .");
      verifier.expect("_:node2 <http://www.w3.org/1999/02/22-rdf-syntax-ns#value> \"bar\" .");
      //Assert.AreEqual( 4, knowledge.nodeCount );
      Assert.IsTrue( verifier.verify(knowledge) );
    }


    [Test]
    public void writeDoesNotIncludeInferredStatements() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      Statement statement1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      knowledge.Add(statement1);

      Statement inference1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop2"), new UriRef("http://example.com/obj"));
      knowledge.Think();

      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
      knowledge.Write( writer );
      Assert.AreEqual("<http://example.com/subj> <http://example.com/prop> \"value\" .\r\n", output.ToString());   
    }    

    
    [Test]
    public void ThinkImplementsRdfEntailmentRule1() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");

      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(thePredicate, new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Property")) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule2() {

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      UriRef thePredicateDomain = new UriRef("http://example.com/predDomain");

      UriRef rdfsDomain = new UriRef("http://www.w3.org/2000/01/rdf-schema#domain");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Add(thePredicate, rdfsDomain, thePredicateDomain);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, rdfType, thePredicateDomain) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule3() {

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      UriRef thePredicateRange = new UriRef("http://example.com/predRange");

      UriRef rdfsRange = new UriRef("http://www.w3.org/2000/01/rdf-schema#range");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Add(thePredicate, rdfsRange, thePredicateRange);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theObject, rdfType, thePredicateRange) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule4aForUriRefSubjects() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource")) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule4aForBlankNodeSubjects() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      
      BlankNode theSubject = new BlankNode();
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource")) );
    }    


    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule4bForUriRefObjects() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theObject, new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource")) );
    }    


    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule4bForBlankNodeObjects() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      BlankNode theObject = new BlankNode();
      
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theObject, new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource")) );
    }    


    [Test]
    public void ThinkAddsRdfAxioms() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      
      knowledge.Think();

      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
      UriRef rdfProperty = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Property");
      

      Assert.IsTrue( knowledge.Contains(rdfType, rdfType, rdfProperty), "type type is Property"  );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#subject"), rdfType, rdfProperty), "rdf:subject rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate"), rdfType, rdfProperty), "rdf:predicate rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#object"), rdfType, rdfProperty), "rdf:object rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#first"), rdfType, rdfProperty), "rdf:first rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#rest"), rdfType, rdfProperty), "rdf:rest rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#value"), rdfType, rdfProperty), "rdf:value rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_1"), rdfType, rdfProperty), "rdf:_1 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_2"), rdfType, rdfProperty), "rdf:_2 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_3"), rdfType, rdfProperty), "rdf:_3 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_4"), rdfType, rdfProperty), "rdf:_4 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_5"), rdfType, rdfProperty), "rdf:_5 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_6"), rdfType, rdfProperty), "rdf:_6 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_7"), rdfType, rdfProperty), "rdf:_7 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_8"), rdfType, rdfProperty), "rdf:_8 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_9"), rdfType, rdfProperty), "rdf:_9 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_10"), rdfType, rdfProperty), "rdf:_10 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_11"), rdfType, rdfProperty), "rdf:_11 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_12"), rdfType, rdfProperty), "rdf:_12 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_13"), rdfType, rdfProperty), "rdf:_13 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_14"), rdfType, rdfProperty), "rdf:_14 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_15"), rdfType, rdfProperty), "rdf:_15 rdf:type rdf:Property ." );
      Assert.IsTrue( knowledge.Contains(new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#nil"), rdfType, rdfProperty), "rdf:nil rdf:type rdf:Property ." );
    }    

    [Test]
    public void ThinkAddsRdfSchemaAxioms() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      
      knowledge.Think();

      UriRef rdfAlt = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Alt");
      UriRef rdfBag = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Bag");
      UriRef rdfFirst = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#first");
      UriRef rdfList = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#List");
      UriRef rdfObject = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#object");
      UriRef rdfPredicate = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#predicate");
      UriRef rdfProperty = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Property");
      UriRef rdfRest = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#rest");
      UriRef rdfSeq = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Seq");
      UriRef rdfStatement = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Statement");
      UriRef rdfSubject = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#subject");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
      UriRef rdfValue = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#value");
      
      UriRef rdfsClass = new UriRef("http://www.w3.org/2000/01/rdf-schema#Class");
      UriRef rdfsComment = new UriRef("http://www.w3.org/2000/01/rdf-schema#comment");
      UriRef rdfsContainer = new UriRef("http://www.w3.org/2000/01/rdf-schema#Container");
      UriRef rdfsContainerMembershipProperty = new UriRef("http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty");
      UriRef rdfsDatatype = new UriRef("http://www.w3.org/2000/01/rdf-schema#Datatype");
      UriRef rdfsDomain = new UriRef("http://www.w3.org/2000/01/rdf-schema#domain");
      UriRef rdfsIsDefinedBy = new UriRef("http://www.w3.org/2000/01/rdf-schema#isDefinedBy");
      UriRef rdfsLabel = new UriRef("http://www.w3.org/2000/01/rdf-schema#label");
      UriRef rdfsLiteral = new UriRef("http://www.w3.org/2000/01/rdf-schema#Literal");
      UriRef rdfsMember = new UriRef("http://www.w3.org/2000/01/rdf-schema#member");
      UriRef rdfsRange = new UriRef("http://www.w3.org/2000/01/rdf-schema#range");
      UriRef rdfsResource = new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource");
      UriRef rdfsSeeAlso = new UriRef("http://www.w3.org/2000/01/rdf-schema#seeAlso");
      UriRef rdfsSubPropertyOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subPropertyOf");
      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      


      Assert.IsTrue( knowledge.Contains( rdfType, rdfsDomain, rdfsResource), "rdf:type rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsDomain, rdfsDomain, rdfProperty), "rdfs:domain rdfs:domain rdf:Property .");
      Assert.IsTrue( knowledge.Contains( rdfsRange, rdfsDomain, rdfProperty), "rdfs:range rdfs:domain rdf:Property .");
      Assert.IsTrue( knowledge.Contains( rdfsSubPropertyOf, rdfsDomain, rdfProperty), "rdfs:subPropertyOf rdfs:domain rdf:Property .");
      Assert.IsTrue( knowledge.Contains( rdfsSubClassOf, rdfsDomain, rdfsClass), "rdfs:subClassOf rdfs:domain rdfs:Class .");
      Assert.IsTrue( knowledge.Contains( rdfSubject, rdfsDomain, rdfStatement), "rdf:subject rdfs:domain rdf:Statement .");
      Assert.IsTrue( knowledge.Contains( rdfPredicate, rdfsDomain, rdfStatement), "rdf:predicate rdfs:domain rdf:Statement .");
      Assert.IsTrue( knowledge.Contains( rdfObject, rdfsDomain, rdfStatement), "rdf:object rdfs:domain rdf:Statement .");
      Assert.IsTrue( knowledge.Contains( rdfsMember, rdfsDomain, rdfsResource), "rdfs:member rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfFirst, rdfsDomain, rdfList), "rdf:first rdfs:domain rdf:List .");
      Assert.IsTrue( knowledge.Contains( rdfRest, rdfsDomain, rdfList), "rdf:rest rdfs:domain rdf:List .");
      Assert.IsTrue( knowledge.Contains( rdfsSeeAlso, rdfsDomain, rdfsResource), "rdfs:seeAlso rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsIsDefinedBy, rdfsDomain, rdfsResource), "rdfs:isDefinedBy rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsComment, rdfsDomain, rdfsResource), "rdfs:comment rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsLabel, rdfsDomain, rdfsResource), "rdfs:label rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfValue, rdfsDomain, rdfsResource), "rdf:value rdfs:domain rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( rdfType, rdfsRange, rdfsClass), "rdf:type rdfs:range rdfs:Class .");
      Assert.IsTrue( knowledge.Contains( rdfsDomain, rdfsRange, rdfsClass), "rdfs:domain rdfs:range rdfs:Class .");
      Assert.IsTrue( knowledge.Contains( rdfsRange, rdfsRange, rdfsClass ), "rdfs:range rdfs:range rdfs:Class .");
      Assert.IsTrue( knowledge.Contains( rdfsSubPropertyOf, rdfsRange, rdfProperty ), "rdfs:subPropertyOf rdfs:range rdf:Property .");
      Assert.IsTrue( knowledge.Contains( rdfsSubClassOf, rdfsRange, rdfsClass), "rdfs:subClassOf rdfs:range rdfs:Class .");
      Assert.IsTrue( knowledge.Contains( rdfSubject, rdfsRange, rdfsResource), "rdf:subject rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfPredicate, rdfsRange, rdfsResource), "rdf:predicate rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfObject, rdfsRange, rdfsResource), "rdf:object rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsMember, rdfsRange, rdfsResource), "rdfs:member rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfFirst, rdfsRange, rdfsResource), "rdf:first rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfRest, rdfsRange, rdfList), "rdf:rest rdfs:range rdf:List .");
      Assert.IsTrue( knowledge.Contains( rdfsSeeAlso, rdfsRange, rdfsResource), "rdfs:seeAlso rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsIsDefinedBy, rdfsRange, rdfsResource), "rdfs:isDefinedBy rdfs:range rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( rdfsComment, rdfsRange, rdfsLiteral), "rdfs:comment rdfs:range rdfs:Literal .");
      Assert.IsTrue( knowledge.Contains( rdfsLabel, rdfsRange, rdfsLiteral), "rdfs:label rdfs:range rdfs:Literal .");
      Assert.IsTrue( knowledge.Contains( rdfValue, rdfsRange, rdfsResource), "rdf:value rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( rdfAlt, rdfsSubClassOf, rdfsContainer), "rdf:Alt rdfs:subClassOf rdfs:Container .");
      Assert.IsTrue( knowledge.Contains( rdfBag, rdfsSubClassOf, rdfsContainer), "rdf:Bag rdfs:subClassOf rdfs:Container .");
      Assert.IsTrue( knowledge.Contains( rdfSeq, rdfsSubClassOf, rdfsContainer), "rdf:Seq rdfs:subClassOf rdfs:Container .");
      Assert.IsTrue( knowledge.Contains( rdfsContainerMembershipProperty, rdfsSubClassOf, rdfProperty), "rdfs:ContainerMembershipProperty rdfs:subClassOf rdf:Property .");

      Assert.IsTrue( knowledge.Contains( rdfsIsDefinedBy, rdfsSubPropertyOf, rdfsSeeAlso), "rdfs:isDefinedBy rdfs:subPropertyOf rdfs:seeAlso .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral"), rdfType, rdfsDatatype), "rdf:XMLLiteral rdf:type rdfs:Datatype .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#XMLLiteral"), rdfsSubClassOf, rdfsLiteral), "rdf:XMLLiteral rdfs:subClassOf rdfs:Literal .");
      Assert.IsTrue( knowledge.Contains( rdfsDatatype, rdfsSubClassOf, rdfsClass), "rdfs:Datatype rdfs:subClassOf rdfs:Class .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_1"), rdfType, rdfsContainerMembershipProperty), "rdf:_1 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_1"), rdfsDomain, rdfsResource), "rdf:_1 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_1"), rdfsRange, rdfsResource), "rdf:_1 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_2"), rdfType, rdfsContainerMembershipProperty), "rdf:_2 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_2"), rdfsDomain, rdfsResource), "rdf:_2 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_2"), rdfsRange, rdfsResource), "rdf:_2 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_3"), rdfType, rdfsContainerMembershipProperty), "rdf:_3 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_3"), rdfsDomain, rdfsResource), "rdf:_3 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_3"), rdfsRange, rdfsResource), "rdf:_3 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_4"), rdfType, rdfsContainerMembershipProperty), "rdf:_4 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_4"), rdfsDomain, rdfsResource), "rdf:_4 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_4"), rdfsRange, rdfsResource), "rdf:_4 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_5"), rdfType, rdfsContainerMembershipProperty), "rdf:_5 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_5"), rdfsDomain, rdfsResource), "rdf:_5 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_5"), rdfsRange, rdfsResource), "rdf:_5 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_6"), rdfType, rdfsContainerMembershipProperty), "rdf:_6 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_6"), rdfsDomain, rdfsResource), "rdf:_6 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_6"), rdfsRange, rdfsResource), "rdf:_6 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_7"), rdfType, rdfsContainerMembershipProperty), "rdf:_7 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_7"), rdfsDomain, rdfsResource), "rdf:_7 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_7"), rdfsRange, rdfsResource), "rdf:_7 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_8"), rdfType, rdfsContainerMembershipProperty), "rdf:_8 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_8"), rdfsDomain, rdfsResource), "rdf:_8 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_8"), rdfsRange, rdfsResource), "rdf:_8 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_9"), rdfType, rdfsContainerMembershipProperty), "rdf:_9 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_9"), rdfsDomain, rdfsResource), "rdf:_9 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_9"), rdfsRange, rdfsResource), "rdf:_9 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_10"), rdfType, rdfsContainerMembershipProperty), "rdf:_10 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_10"), rdfsDomain, rdfsResource), "rdf:_10 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_10"), rdfsRange, rdfsResource), "rdf:_10 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_11"), rdfType, rdfsContainerMembershipProperty), "rdf:_11 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_11"), rdfsDomain, rdfsResource), "rdf:_11 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_11"), rdfsRange, rdfsResource), "rdf:_11 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_12"), rdfType, rdfsContainerMembershipProperty), "rdf:_12 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_12"), rdfsDomain, rdfsResource), "rdf:_12 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_12"), rdfsRange, rdfsResource), "rdf:_12 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_13"), rdfType, rdfsContainerMembershipProperty), "rdf:_13 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_13"), rdfsDomain, rdfsResource), "rdf:_13 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_13"), rdfsRange, rdfsResource), "rdf:_13 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_14"), rdfType, rdfsContainerMembershipProperty), "rdf:_14 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_14"), rdfsDomain, rdfsResource), "rdf:_14 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_14"), rdfsRange, rdfsResource), "rdf:_14 rdfs:range rdfs:Resource .");

      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_15"), rdfType, rdfsContainerMembershipProperty), "rdf:_15 rdf:type rdfs:ContainerMembershipProperty .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_15"), rdfsDomain, rdfsResource), "rdf:_15 rdfs:domain rdfs:Resource .");
      Assert.IsTrue( knowledge.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#_15"), rdfsRange, rdfsResource), "rdf:_15 rdfs:range rdfs:Resource .");

    }

    [Test]
    public void GetDescriptionOfIncludesInferredProperties() {
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef thePredicate = new UriRef("http://example.com/pred");
      UriRef theObject = new UriRef("http://example.com/obj");
      
      knowledge.Add(theSubject, thePredicate, theObject);
      knowledge.Think();
      ResourceDescription desc = knowledge.GetDescriptionOf(theSubject);
      
      Assert.IsTrue( desc.Contains( thePredicate, knowledge.TripleStore.GetResourceDenotedBy(theObject) ), "Description contains asserted triple");
      Assert.IsTrue( desc.Contains( new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource"))), "Description contains inferred triple" );
    }    


    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule5() {

      UriRef resource1 = new UriRef("http://example.com/subj1");
      UriRef resource2 = new UriRef("http://example.com/subj2");
      UriRef resource3 = new UriRef("http://example.com/subj3");

      UriRef rdfsSubPropertyOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subPropertyOf");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(resource1, rdfsSubPropertyOf, resource2);
      knowledge.Add(resource2, rdfsSubPropertyOf, resource3);
      knowledge.Think();
      Assert.IsTrue( knowledge.Contains(resource1, rdfsSubPropertyOf, resource3) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule6() {

      UriRef theSubject = new UriRef("http://example.com/subj");

      UriRef rdfsSubPropertyOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subPropertyOf");
      UriRef rdfsProperty = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#Property");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, rdfType, rdfsProperty);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, rdfsSubPropertyOf, theSubject) );
    }    



    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule7() {

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef theObject = new UriRef("http://example.com/obj");
      UriRef theProperty = new UriRef("http://example.com/property");
      UriRef theSuperProperty = new UriRef("http://example.com/superProperty");

      UriRef rdfsSubPropertyOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subPropertyOf");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, theProperty, theObject);
      knowledge.Add(theProperty, rdfsSubPropertyOf, theSuperProperty);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, theSuperProperty, theObject) );
    }    


    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule8() {

      UriRef theSubject = new UriRef("http://example.com/subj");

      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      UriRef rdfsClass = new UriRef("http://www.w3.org/2000/01/rdf-schema#Class");
      UriRef rdfsResource = new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, rdfType, rdfsClass);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, rdfsSubClassOf, rdfsResource) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule9() {

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef theType = new UriRef("http://example.com/type");
      UriRef theSuperType = new UriRef("http://example.com/superType");

      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, rdfType, theType );
      knowledge.Add(theType, rdfsSubClassOf, theSuperType);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, rdfType, theSuperType) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule10() {

      UriRef theSubject = new UriRef("http://example.com/subj");

      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      UriRef rdfsClass = new UriRef("http://www.w3.org/2000/01/rdf-schema#Class");
      UriRef rdfsResource = new UriRef("http://www.w3.org/2000/01/rdf-schema#Resource");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, rdfType, rdfsClass);
      knowledge.Think();

      Assert.IsTrue( knowledge.Contains(theSubject, rdfsSubClassOf, theSubject) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule11() {

      UriRef resource1 = new UriRef("http://example.com/subj1");
      UriRef resource2 = new UriRef("http://example.com/subj2");
      UriRef resource3 = new UriRef("http://example.com/subj3");

      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(resource1, rdfsSubClassOf, resource2);
      knowledge.Add(resource2, rdfsSubClassOf, resource3);
      knowledge.Think();
      Assert.IsTrue( knowledge.Contains(resource1, rdfsSubClassOf, resource3) );
    }    


    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule12() {
      UriRef resource1 = new UriRef("http://example.com/subj1");

      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
      UriRef rdfsContainerMembershipProperty = new UriRef("http://www.w3.org/2000/01/rdf-schema#ContainerMembershipProperty");
      UriRef rdfsSubPropertyOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subPropertyOf");
      UriRef rdfsMember = new UriRef("http://www.w3.org/2000/01/rdf-schema#member");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(resource1, rdfType, rdfsContainerMembershipProperty);
      knowledge.Think();
      Assert.IsTrue( knowledge.Contains(resource1, rdfsSubPropertyOf, rdfsMember) );
    }    

    [Test]
    public void ThinkImplementsRdfSchemaEntailmentRule13() {
      UriRef resource1 = new UriRef("http://example.com/subj1");

      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
      UriRef rdfsDatatype = new UriRef("http://www.w3.org/2000/01/rdf-schema#Datatype");
      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      UriRef rdfsLiteral = new UriRef("http://www.w3.org/2000/01/rdf-schema#Literal");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(resource1, rdfType, rdfsDatatype);
      knowledge.Think();
      Assert.IsTrue( knowledge.Contains(resource1, rdfsSubClassOf, rdfsLiteral) );
    }    


    [Test]
    public void ThinkIncorporatesStatementsFromSchemas() {

      UriRef theSubject = new UriRef("http://example.com/subj");
      UriRef theType = new UriRef("http://example.com/type");
      UriRef theSuperType = new UriRef("http://example.com/superType");

      UriRef rdfsSubClassOf = new UriRef("http://www.w3.org/2000/01/rdf-schema#subClassOf");
      UriRef rdfType = new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(theSubject, rdfType, theType );
      knowledge.Schemas.Add( new Statement( theType, rdfsSubClassOf, theSuperType) );
      knowledge.Think();
      Assert.IsTrue( knowledge.Contains(theSubject, rdfType, theSuperType) );
    }    

    [Test]
    public void setDereferencerPassesSuppliedObjectToParserFactory() {
      DereferencerStore dereferencer = new DereferencerStore();

      ParserFactoryStore parserFactory = new ParserFactoryStore();
      
      KnowledgeBase knowledge = new KnowledgeBase(parserFactory);
      knowledge.SetDereferencer(dereferencer);
      knowledge.Include( "http://example.com");
      
      Assert.IsTrue( parserFactory.WasSetDereferencerCalledWith( dereferencer )  );

    }

    [Test]
    public void AddKnowledgeBaseWithNoCommonNodes() {
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
    
      KnowledgeBase knowledgeFirst = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeFirst.Add(statement1);

      KnowledgeBase knowledgeSecond = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeSecond.Add(statement2);
      knowledgeSecond.Add(statement3);

      knowledgeFirst.Add( knowledgeSecond );
      KnowledgeBaseVerifier verifierFirst = new KnowledgeBaseVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierFirst.expect("<any:node5> <any:predicate3> <any:node6> .");

      Assert.IsTrue( verifierFirst.verify(knowledgeFirst),"first knowledge base includes triples from second" );
      
      KnowledgeBaseVerifier verifierSecond = new KnowledgeBaseVerifier();
      verifierSecond.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierSecond.expect("<any:node5> <any:predicate3> <any:node6> .");

      Assert.IsTrue( verifierSecond.verify(knowledgeSecond), "second knowledge base is unchanged" );
      

    }

    [Test]
    public void AddKnowledgeBaseWithCommonNodes() {
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
    
      KnowledgeBase knowledgeFirst = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeFirst.Add(statement1);

      KnowledgeBase knowledgeSecond = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeSecond.Add(statement2);
      knowledgeSecond.Add(statement3);
      knowledgeSecond.Add(statement4);


      knowledgeFirst.Add( knowledgeSecond );
      KnowledgeBaseVerifier verifierFirst = new KnowledgeBaseVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierFirst.expect("<any:node5> <any:predicate3> <any:node6> .");
      verifierFirst.expect("<any:node1> <any:predicate2> <any:node6> .");
      Assert.IsTrue( verifierFirst.verify(knowledgeFirst),"first knowledge base includes triples from second" );
      
      KnowledgeBaseVerifier verifierSecond = new KnowledgeBaseVerifier();
      verifierSecond.expect("<any:node3> <any:predicate2> <any:node4> .");
      verifierSecond.expect("<any:node5> <any:predicate3> <any:node6> .");
      verifierSecond.expect("<any:node1> <any:predicate2> <any:node6> .");

      Assert.IsTrue( verifierSecond.verify(knowledgeSecond), "second knowledge base is unchanged" );
    }


    [Test]
    public void AddKnowledgeBaseWithIdenticalNodesDenotingDifferentResources() {
      UriRef node1 = new  UriRef("any:node1");
      UriRef node2 = new  UriRef("any:node2");
      UriRef node3 = new  UriRef("any:node3");

      UriRef predicate1 = new UriRef("any:predicate1");
      UriRef predicate2 = new UriRef("any:predicate2");
  
      Statement statement1 = new Statement(node1, predicate1, node2);
      Statement statement2 = new Statement(node1, predicate2, node3);
    
      KnowledgeBase knowledgeFirst = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeFirst.Add(statement1);

      Resource node1ResourceBefore = knowledgeFirst.TripleStore.GetResourceDenotedBy( node1 );

      KnowledgeBase knowledgeSecond = new KnowledgeBase( new ParserFactoryStub() );
      knowledgeSecond.Add(statement2);

      knowledgeFirst.Add( knowledgeSecond );

      Resource node1ResourceAfter = knowledgeFirst.TripleStore.GetResourceDenotedBy( node1 );

      Assert.AreEqual( node1ResourceBefore, node1ResourceAfter);

      KnowledgeBaseVerifier verifierFirst = new KnowledgeBaseVerifier();
      verifierFirst.expect("<any:node1> <any:predicate1> <any:node2> .");
      verifierFirst.expect("<any:node1> <any:predicate2> <any:node3> .");
      Assert.IsTrue( verifierFirst.verify(knowledgeFirst),"first knowledge base includes triples from second" );

    }
    
/* 
TODO: test first kb has 2 nodes denoting separate resources, second kb has same two nodes denoting single resource. And vice versa.
*/    

    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void AddDoesNotAllowDuplicatesByDefault() {
      //~ UriRef theSubject = new UriRef("http://example.com/subj");
      //~ UriRef thePredicate = new UriRef("http://example.com/pred");
      //~ UriRef theObject = new UriRef("http://example.com/obj");
  
      //~ Statement statement1 = new Statement(theSubject, thePredicate, theObject);
      //~ Statement statement2 = new Statement(theSubject, thePredicate, theObject);
    
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ knowledge.Add(statement1);
      //~ knowledge.Add(statement2);

      //~ ConciseBoundedDescription description = knowledge.GetDescriptionOf( new UriRef("http://example.com/subj") );
      //~ ResourceDescriptionList values = description.getPropertyValues( new UriRef("http://example.com/pred") );

      //~ Assert.AreEqual( 1, values.Count );
    //~ }


    // Commented out until decided whether to keep ResourceDescriptionList

    //~ [Test]
    //~ public void findByType() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ Statement statement1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://example.com/thetype"));
      //~ knowledge.Add(statement1);

      //~ Statement statement2 = new Statement(new UriRef("http://example.com/other"), new UriRef("http://example.com/prop"), new UriRef("http://example.com/thetype"));
      //~ knowledge.Add(statement2);

      //~ ResourceDescriptionList found = knowledge.findByType(new UriRef("http://example.com/thetype"));
      
      //~ Assert.AreEqual( 1, found.Count );
      //~ Assert.AreEqual( knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.com/subj")),  found[0].resource );
    //~ }



    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void findByPropertyValueUsesInferredStatements() {
    
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ Statement statement1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      //~ knowledge.Add(statement1);

      //~ Statement inference1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop2"), new UriRef("http://example.com/obj"));
      //~ knowledge.inferences.Add(inference1);

      //~ ResourceDescriptionList found = knowledge.findByPropertyValue(new UriRef("http://example.com/prop2"), new UriRef("http://example.com/obj"));
      
      //~ Assert.AreEqual( 1, found.Count );
      //~ Assert.AreEqual( knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.com/subj")),  found[0].resource );
    //~ }    

    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void findByTypeUsesInferredStatements() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ Statement statement1 = new Statement(new UriRef("http://example.com/bogus"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://example.com/theType"));
      //~ knowledge.Add(statement1);

      //~ Statement inference1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new UriRef("http://example.com/theType"));
      //~ knowledge.inferences.Add(inference1);

      //~ ResourceDescriptionList found = knowledge.findByType( new UriRef("http://example.com/theType"));
      
      //~ Assert.AreEqual( 1, found.Count );
      //~ Assert.AreEqual( knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.com/subj")),  found[0].resource );
    //~ }    

    [Test]
    public void AddConciseBoundedDescriptionAboutNewSubjectMergesPropertyValues() {
      KnowledgeBase sourceKnowledge = new KnowledgeBase( new ParserFactoryStub() );
      KnowledgeBase destinationKnowledge = new KnowledgeBase( new ParserFactoryStub() );

      // Source knowledge base has triples about subject 
      sourceKnowledge.Add( new UriRef("http://example.com/subj"), new UriRef("http://example.com/sound"), new UriRef("http://example.com/fizz") );
      
      // Destination knowledge base already has triple about subject
      destinationKnowledge.Add( new UriRef("http://example.com/subj"), new UriRef("http://example.com/colour"), new UriRef("http://example.com/blue") );


      ResourceDescription descSubj = sourceKnowledge.GetDescriptionOf( new UriRef("http://example.com/subj") );       
      destinationKnowledge.Add( descSubj );

      ResourceDescription descObj = destinationKnowledge.GetDescriptionOf( new UriRef("http://example.com/subj") );       

      Assert.IsTrue( descObj.Contains( new UriRef("http://example.com/colour"), new UriRef("http://example.com/blue") ), "Description contains destination triple");
      Assert.IsTrue( descObj.Contains( new UriRef("http://example.com/sound"), new UriRef("http://example.com/fizz") ), "Description contains source triple");

    }

    
    // Commented out - only valid for ResourceDescription not CBD?
    //~ [Test]
    //~ public void AddMakesConciseBoundedDescriptionsForObjects() {
      //~ NodeStub node1 = new NodeStub("node1");
      //~ UriRef thePredicate = new UriRef("thePredicate");
      //~ NodeStub node2 = new NodeStub("node2");
      //~ NodeStub node3 = new NodeStub("node3");
  
      //~ Statement statement1 = new Statement(node1, thePredicate, node2);
      //~ Statement statement2 = new Statement(node2, thePredicate, node3);
    
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ knowledge.Add(statement1);
      //~ knowledge.Add(statement2);

      //~ ConciseBoundedDescription desc1 = knowledge.GetDescriptionOf(node1);
      //~ ConciseBoundedDescription desc2 = desc1.getFirstPropertyValue(thePredicate);

      //~ Assert.IsTrue(  desc2.hasProperty(thePredicate) , "desc2 has property");
      //~ ConciseBoundedDescription desc3 = desc2.getFirstPropertyValue(thePredicate);
      
      //~ Assert.IsNotNull( desc3 );
      
    //~ }
    

    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void AddConciseBoundedDescriptionSetsPropertyValuesToBeKnowledgeBaseGeneratedDescriptions() {
      //~ KnowledgeBase sourceKnowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ KnowledgeBase destinationKnowledge = new KnowledgeBase( new ParserFactoryStub() );

      //~ sourceKnowledge.Add( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") );
      //~ sourceKnowledge.Add( new UriRef("http://example.com/obj"), new UriRef("http://example.com/sound"), new UriRef("http://example.com/fizz") );
      
      //~ ConciseBoundedDescription sourceSubjectDescription = sourceKnowledge.GetDescriptionOf( new UriRef("http://example.com/subj") );       

      //~ destinationKnowledge.Add( sourceSubjectDescription );

      //~ // Modify the object in the destination knowledge base
      //~ destinationKnowledge.Add( new UriRef("http://example.com/obj"), new UriRef("http://example.com/colour"), new UriRef("http://example.com/blue") );

      //~ // Get the description of the subject from the destination knowledge base
      //~ ConciseBoundedDescription destinationSubjectDescription = destinationKnowledge.GetDescriptionOf( new UriRef("http://example.com/subj") );       

      //~ ResourceDescriptionList propertyValues = destinationSubjectDescription.getPropertyValues( new UriRef("http://example.com/pred") );
      
      //~ Assert.AreEqual( 1, propertyValues.Count );
      
      //~ ConciseBoundedDescription destinationObjectDescription = (ConciseBoundedDescription)propertyValues[0];

      //~ // Check that the object description referred to in the subject description reflects the recent modification
      //~ Assert.IsTrue( destinationObjectDescription.Contains( new UriRef("http://example.com/colour"), destinationKnowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.com/blue") )));
    //~ }
  
    
    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void findByPropertyValueReturnsEmptyListAfterClear() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ Statement statement1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      //~ knowledge.Add(statement1);

      //~ ResourceDescriptionList foundBefore = knowledge.findByPropertyValue(new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      //~ Assert.AreEqual( 1, foundBefore.Count );

      //~ knowledge.Clear();

      //~ ResourceDescriptionList foundAfter = knowledge.findByPropertyValue(new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      //~ Assert.AreEqual( 0, foundAfter.Count );
    //~ }
    
    
    [Test]
    public void AddResourceDescriptionTakesCopy() {
      Resource theSubject = new Resource();
      Arc thePredicate = new UriRef("http://example.com/pred");
      Arc thePredicate2 = new UriRef("http://example.com/pred2");
      PlainLiteral theFirstObject = new PlainLiteral("first");
      PlainLiteral theSecondObject = new PlainLiteral("second");

    
      ConciseBoundedDescription desc = new ConciseBoundedDescription( theSubject );
      desc.Add( thePredicate, theFirstObject );

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add( desc );

      desc.Add( thePredicate2, theSecondObject );
      
      ResourceDescription foundDesc = knowledge.GetDescriptionOf( theSubject );
      
      Assert.IsTrue( foundDesc.Contains( thePredicate, theFirstObject) , "Description copy should have properties original had when copied");
      Assert.IsFalse( foundDesc.Contains( thePredicate2, theSecondObject), "Description copy should not be affected by changes to original" );
    }


    /// <summary>This tests that adding two separate resource descriptions about the same resource merge their properties into a single new description</summary>
    [Test]
    public void AddConciseBoundedDescriptionMergesWithExistingDescription() {
      Resource theSubject = new Resource();
      Arc thePredicate = new UriRef("http://example.com/pred");
      Resource theFirstObject = new Resource();
      Resource theSecondObject = new Resource();

    
      ConciseBoundedDescription descOriginal = new ConciseBoundedDescription( theSubject );
      descOriginal.Add(thePredicate,  theFirstObject );

      ConciseBoundedDescription descNew = new ConciseBoundedDescription( theSubject );
      descNew.Add(thePredicate,  theSecondObject );

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add( descOriginal );
      knowledge.Add( descNew );
     
      ResourceDescription foundDesc = knowledge.GetDescriptionOf( theSubject );

      Assert.IsTrue( foundDesc.Contains( thePredicate, theFirstObject) , "Description has original property");
      Assert.IsTrue( foundDesc.Contains( thePredicate, theSecondObject), "Description has additional property" );
    }

    /* Tests for replace method */
    [Test]
    public void ReplaceConciseBoundedDescriptionRemovesExistingDescription() {
      Resource theSubject = new Resource();
      Resource theFirstObject = new Resource();
      Resource theSecondObject = new Resource();
      Arc thePredicate = new UriRef("http://example.com/pred");

    
      ConciseBoundedDescription descOriginal = new ConciseBoundedDescription( theSubject );
      descOriginal.Add(thePredicate,  theFirstObject );

      ConciseBoundedDescription descNew = new ConciseBoundedDescription( theSubject );
      descNew.Add(thePredicate, theSecondObject );

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add( descOriginal );
      knowledge.Replace( descNew );
     
      ResourceDescription foundDesc = knowledge.GetDescriptionOf( theSubject );

      Assert.IsFalse( foundDesc.Contains( thePredicate, theFirstObject) , "Description has original property");
      Assert.IsTrue( foundDesc.Contains( thePredicate, theSecondObject), "Description has additional property" );
    }
    
    [Test]
    public void ReplaceMergesPropertyValueDescriptionsWithExistingDescriptions() {
      BlankNode thePerson = new BlankNode();
      BlankNode theFirstWife = new BlankNode();
      BlankNode theSecondWife = new BlankNode();
      
      UriRef marriedToProperty = new UriRef("http://example.com/marriedTo");

      // Create an assertion:  thePerson -> marriedTo -> theFirstWife
      ConciseBoundedDescription originalPersonDescription = new ConciseBoundedDescription( thePerson );
      originalPersonDescription.Add( marriedToProperty,   theFirstWife );

      // Add assertion to knowledge base
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add( originalPersonDescription );

      // Check it's all there
      Assert.IsTrue( knowledge.Contains( thePerson, marriedToProperty, theFirstWife ), "Contains marriage to first wife");
      Assert.IsTrue( knowledge.Assertions.HasResourceDenotedBy( thePerson ), "Contains resource representing person");
      Assert.IsTrue( knowledge.Assertions.HasResourceDenotedBy( theFirstWife ), "Contains resource representing first wife");

      //Now revise the relationship:  thePerson -> marriedTo -> theSecondWife
      ConciseBoundedDescription revisedPersonDescription = new ConciseBoundedDescription( thePerson );
      revisedPersonDescription.Add( marriedToProperty,  theSecondWife );

      // Replace original description with new one
      knowledge.Replace( revisedPersonDescription );
      
      // Check results
      Assert.IsFalse( knowledge.Contains( thePerson, marriedToProperty, theFirstWife ), "Does not contain marriage to first wife");

      Assert.IsTrue( knowledge.Contains( thePerson, marriedToProperty, theSecondWife ), "Contains marriage to second wife");
      Assert.IsTrue( knowledge.Assertions.HasResourceDenotedBy( thePerson ), "Contains resource representing person");
      Assert.IsTrue( knowledge.Assertions.HasResourceDenotedBy( theSecondWife ), "Contains resource representing second wife");
      
    }
    


    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void findByPropertyValueWithValueReturnsEmptyListIfNoMatch() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );

      //~ ResourceDescriptionList found = knowledge.findByPropertyValue(new UriRef("http://example.com/bogus"), new PlainLiteral("dummy"));
      
      //~ Assert.IsNotNull( found, "list is not null");
      //~ Assert.AreEqual( 0, found.Count );
    //~ }

    // Commented out until decided whether to keep ResourceDescriptionList
    //~ [Test]
    //~ public void findByPropertyValueReturnsNodesWithSpecifiedPropertyAndValue() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      //~ Statement statement1 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      //~ knowledge.Add(statement1);

      //~ Statement statement2 = new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/prop2"), new PlainLiteral("value"));
      //~ knowledge.Add(statement2);

      //~ Statement statement3 = new Statement(new UriRef("http://example.com/subj2"), new UriRef("http://example.com/prop"), new PlainLiteral("value2"));
      //~ knowledge.Add(statement3);



      //~ ResourceDescriptionList found = knowledge.findByPropertyValue(new UriRef("http://example.com/prop"), new PlainLiteral("value"));
      
      //~ Assert.AreEqual( 1, found.Count );
      //~ Assert.AreEqual( knowledge.TripleStore.GetResourceDenotedBy(new UriRef("http://example.com/subj")),  found[0].resource );
    //~ }


    [Test]
    public void AddConciseBoundedDescriptionObeysAllowDuplicatesSetting() {
      BlankNode theSubjectNode = new BlankNode();
      Arc thePredicate = new UriRef("http://example.com/pred");
      Arc thePredicate2 = new UriRef("http://example.com/pred2");
      PlainLiteral theFirstObjectNode = new PlainLiteral("first");
      PlainLiteral theSecondObjectNode = new PlainLiteral("second");

    
      ConciseBoundedDescription firstDescripton = new ConciseBoundedDescription( theSubjectNode );
      firstDescripton.Add(thePredicate,  theFirstObjectNode );

      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add( firstDescripton );

      KnowledgeBaseVerifier verifierBefore = new KnowledgeBaseVerifier();
      verifierBefore.expect(@"_:node1 <http://example.com/pred> ""first"" .");

      Assert.IsTrue( verifierBefore.verify(knowledge), "Store has original triple");

      ConciseBoundedDescription secondDescripton = new ConciseBoundedDescription( theSubjectNode );
      secondDescripton.Add(thePredicate,  theFirstObjectNode );
      secondDescripton.Add(thePredicate2,  theSecondObjectNode );
      knowledge.Add( secondDescripton );

      KnowledgeBaseVerifier verifierAfter = new KnowledgeBaseVerifier();
      verifierAfter.expect(@"_:node1 <http://example.com/pred> ""first"" .");
      verifierAfter.expect(@"_:node1 <http://example.com/pred2> ""second"" .");

      bool verifyAfter = verifierAfter.verify(knowledge);
      if (! verifyAfter ) {
        Assert.Fail("Expecting original triple plus one new triple. Failed because verifier " + verifierAfter.LastFailureDescription);
      }

      Assert.IsTrue( verifyAfter );
    }


  
      //~ [Test]
    //~ public void sourceListAdd2SameMboxCheckTriples() {
      //~ KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );

      //~ ConciseBoundedDescription agent1 = new ConciseBoundedDescription( new BlankNode() );
      //~ agent1[Rdfs.seeAlso] += new Rdfs.Resource("http://example.com/1234");

      //~ ConciseBoundedDescription agent2 = new ConciseBoundedDescription( new BlankNode() );
      //~ agent2[Rdfs.seeAlso] += new Rdfs.Resource("http://example.com/5678");

      //~ agent1[Rdfs.seeAlso] += agent2[Rdfs.seeAlso]; 
      //~ knowledge.Add(agent1);

      //~ KnowledgeBaseVerifier verifier = new KnowledgeBaseVerifier(); 
      //~ verifier.expect(@"_:agent1 <http://www.w3.org/2000/01/rdf-schema#seeAlso> <http://example.com/1234> .");
      //~ verifier.expect(@"_:agent1 <http://www.w3.org/2000/01/rdf-schema#seeAlso> <http://example.com/5678> .");
      
      //~ bool verifyResult = verifier.verify(knowledge); 
      //~ if (!verifyResult)
        //~ Console.WriteLine(verifier.LastFailureDescription);

      //~ Assert.IsTrue(verifyResult); 
    //~ }


    [Test]
    public void GetDescriptionOfDoesNotIncludePropertiesOfUriRefValues() {
      UriRef theSubject = new UriRef("ex:res1");
    
      Statement statement1 = new Statement( theSubject, new UriRef("ex:pred"), new UriRef("ex:res2"));
      Statement statement2 = new Statement( new UriRef("ex:res2"), new UriRef("ex:pred"), new UriRef("ex:res3"));
    
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(statement1);
      knowledge.Add(statement2);
      
      ResourceDescription desc = knowledge.GetDescriptionOf( theSubject );
            
      Assert.IsTrue(  desc.Contains( statement1 ) , "includes first statement");
      Assert.IsTrue( ! desc.Contains( statement2 ) , "does not include second property");
    } 

    [Test]
    public void GetDescriptionOfDoesIncludesPropertiesOfBlankNodeValues() {
      UriRef theSubject = new UriRef("ex:res1");
      BlankNode theNode = new BlankNode();
      
      Statement statement1 = new Statement( theSubject, new UriRef("ex:pred"), theNode);
      Statement statement2 = new Statement( theNode, new UriRef("ex:pred"), new UriRef("ex:res3"));
    
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(statement1);
      knowledge.Add(statement2);
      
      ResourceDescription desc = knowledge.GetDescriptionOf( theSubject );
            
      Assert.IsTrue(  desc.Contains( statement1 ) , "includes first statement");
      Assert.IsTrue(  desc.Contains( statement2 ) , "includes second property");
    } 


    [Test]
    public void GetDescriptionOfRecursesBlankNodes() {
      UriRef theSubject = new UriRef("ex:res1");
      BlankNode theNode1 = new BlankNode();
      BlankNode theNode2 = new BlankNode();
      
      Statement statement1 = new Statement( theSubject, new UriRef("ex:pred"), theNode1);
      Statement statement2 = new Statement( theNode1, new UriRef("ex:pred"), theNode2);
      Statement statement3 = new Statement( theNode2, new UriRef("ex:pred"), new UriRef("ex:res3"));
    
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(statement1);
      knowledge.Add(statement2);
      knowledge.Add(statement3);
      
      ResourceDescription desc = knowledge.GetDescriptionOf( theSubject );
            
      Assert.IsTrue(  desc.Contains( statement1 ) , "includes first statement");
      Assert.IsTrue(  desc.Contains( statement2 ) , "includes second property");
      Assert.IsTrue(  desc.Contains( statement3 ) , "includes third property");
    } 

    [Test]
    public void GetDescriptionOfHandlesBlankNodeLoops() {
      UriRef theSubject = new UriRef("ex:res1");
      BlankNode theNode1 = new BlankNode();
      BlankNode theNode2 = new BlankNode();
      
      Statement statement1 = new Statement( theSubject, new UriRef("ex:pred"), theNode1);
      Statement statement2 = new Statement( theNode1, new UriRef("ex:pred"), theNode2);
      Statement statement3 = new Statement( theNode2, new UriRef("ex:pred"), theNode1);
    
      KnowledgeBase knowledge = new KnowledgeBase( new ParserFactoryStub() );
      knowledge.Add(statement1);
      knowledge.Add(statement2);
      knowledge.Add(statement3);
      
      ResourceDescription desc = knowledge.GetDescriptionOf( theSubject );
      Assert.IsTrue(  desc.Contains( statement1 ) , "includes first statement");
      Assert.IsTrue(  desc.Contains( statement2 ) , "includes second property");
      Assert.IsTrue(  desc.Contains( statement3 ) , "includes third property");
    } 


  }
  
  
  
}