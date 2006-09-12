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
  using SemPlan.Spiral.XsltParser;
  using System;
  using System.IO;
  using System.Xml;
	/// <summary>
	/// Programmer tests for RdfXmlWriter class
	/// </summary>
  /// <remarks>
  /// $Id: RdfXmlWriterTest.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  [TestFixture]
  public class RdfXmlWriterTest {
    public class RdfXmlWriterTestHarness {
      private StringWriter itsOutputWriter;
      private RdfXmlWriter itsRdfWriter;
      private NTripleListVerifier itsVerifier;
      
      public RdfXmlWriterTestHarness() {
        itsOutputWriter = new StringWriter();
        XmlTextWriter xmlWriter = new XmlTextWriter(itsOutputWriter);
        itsRdfWriter = new RdfXmlWriter( xmlWriter );
        itsVerifier = new NTripleListVerifier();
      }
      
      public RdfWriter getRdfWriter() {
        return itsRdfWriter;
      }
      
      public void expect(string ntriple) {
        itsVerifier.Expect(ntriple);
      }
      
      public bool verify() {
        return verify(false);
      }

      public bool verify(bool verbose) {

        SimpleModel model = new SimpleModel(new XsltParserFactory());
        model.ParseString( itsOutputWriter.ToString(), "");

        if (verbose) {
          Console.WriteLine( itsOutputWriter.ToString() );
        }
        foreach (string ntriple in model) {
          itsVerifier.Receive(ntriple);
        }
  
        return itsVerifier.Verify();

      }

      public string getOutput() {
        return itsOutputWriter.ToString();
      }
    }


    public void writeSingleUUUTriple(RdfWriter rdfWriter) {
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();
    }

    public void writeSingleUUBTriple(RdfWriter rdfWriter) {
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteBlankNode("jazz");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();
    }


    public void writeTwoUUUTriplesWithSameSubjectAndPredicate(RdfWriter rdfWriter) {
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
            rdfWriter.EndObject();
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj2");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();
    }

    public void writeTwoUUUTriplesWithSameSubject(RdfWriter rdfWriter) {
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
            rdfWriter.EndObject();
         rdfWriter.EndPredicate();
         rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred2");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj2");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();
    }


    [Test]
    public void writerWritesXmlPrologAsStandalone() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartDocumentCalledWith(true) );
    }

    [Test]
    public void writerStartsRdfRootElement() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#") );
    }

    [Test]
    public void writerWritesEndDocument() {
      XmlWriterCounter xmlWriter = new XmlWriterCounter();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.AreEqual( 1, xmlWriter.WriteEndDocumentCalled );
    }

    [Test]
    public void writerWritesRdfDescriptionForUntypedSubject() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#") );
    }

    [Test]
    public void writerWritesRdfAboutForSubject() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartAttributeCalledWith("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "start attribute" );
Assert.IsTrue( xmlWriter.WasWriteStringCalledWith("http://example.com/subj"), "attribute content" );
    }

    [Test]
    public void writerWritesPredicateElement() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("ns1", "pred", "http://example.com/") );
    }


    [Test]
    public void writerWritesRdfResourceForUriRefObject() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUUTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartAttributeCalledWith("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "start attribute" );
      Assert.IsTrue( xmlWriter.WasWriteStringCalledWith("http://example.com/obj"), "attribute content" );
    }

    [Test]
    public void writerWritesRdfNodeIdForBlankNodeObject() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      writeSingleUUBTriple(rdfWriter);
      
      Assert.IsTrue( xmlWriter.WasWriteStartAttributeCalledWith("rdf", "nodeID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "start attribute" );
      Assert.IsTrue( xmlWriter.WasWriteStringCalledWith("genid1"), "attribute content" );
    }

    [Test]
    public void writerWritesOneRdfDescriptionPerUniqueSubject() {
      XmlWriterCounter xmlWriter = new XmlWriterCounter();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
            rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj2");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
            rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj2");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();
      
      // elements should be: rdf:RDF, rdf:Description * 2, ns1:pred * 3
      Assert.AreEqual( 6, xmlWriter.WriteEndElementCalled );
    }

    [Test]
    public void roundTripWriteSingleUUUTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> <http://example.com/obj> .");

      writeSingleUUUTriple(harness.getRdfWriter());

      Assert.IsTrue( harness.verify() );
    }

    [Test]
    public void roundTripTwoUUUTriplesWithSameSubjectAndPredicate() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> <http://example.com/obj> .");
      harness.expect("<http://example.com/subj> <http://example.com/pred> <http://example.com/obj2> .");
      writeTwoUUUTriplesWithSameSubjectAndPredicate(harness.getRdfWriter());

      Assert.IsTrue( harness.verify() );
    }

    [Test]
    public void roundTripTwoUUUTriplesWithSameSubject() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> <http://example.com/obj> .");
      harness.expect("<http://example.com/subj> <http://example.com/pred2> <http://example.com/obj2> .");

      RdfWriter rdfWriter = harness.getRdfWriter();
      writeTwoUUUTriplesWithSameSubject(harness.getRdfWriter());

      Assert.IsTrue( harness.verify() );
    }

    [Test]
    public void roundTripWriteSingleUUPWithoutLanguageTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\" .");
      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }


    [Test]
    public void roundTripWriteTwoUUPWithoutLanguageTriplesWithSameSubjectAndPredicate() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\" .");
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"bang\" .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz");
           rdfWriter.EndObject();
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("bang");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }

    [Test]
    public void roundTripWriteTwoUUPWithoutLanguageTriplesWithSameSubject() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\" .");
      harness.expect("<http://example.com/subj> <http://example.com/pred2> \"bang\" .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz");
            rdfWriter.EndObject();
          rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred2");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("bang");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }

    [Test]
    public void roundTripWriteSingleUUPWithLanguageTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"@de .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz", "de");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }

    [Test]
    public void roundTripWriteTwoUUPWithLanguageTriplesWithSameSubjectAndPredicate() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"@fr .");
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"bang\"@it .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz", "fr");
           rdfWriter.EndObject();
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("bang", "it");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }

    [Test]
    public void roundTripWriteTwoUUPWithLanguageTriplesWithSameSubject() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"@fr .");
      harness.expect("<http://example.com/subj> <http://example.com/pred2> \"bang\"@it .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("fizz", "fr");
            rdfWriter.EndObject();
          rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred2");
            rdfWriter.StartObject();
              rdfWriter.WritePlainLiteral("bang", "it");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );
    }

    [Test]
    public void roundTripWriteSingleUUTTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"^^<http://www.w3.org/2001/XMLSchema#integer> .");
      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteTypedLiteral("fizz", "http://www.w3.org/2001/XMLSchema#integer");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();

      Assert.IsTrue( testPassed );

    }

    [Test]
    public void roundTripWriteTwoUUTTriplesWithSameSubjectAndPredicate() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"^^<http://www.w3.org/2001/XMLSchema#integer> .");
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"bang\"^^<http://www.w3.org/2001/XMLSchema#integer> .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteTypedLiteral("fizz", "http://www.w3.org/2001/XMLSchema#integer");
           rdfWriter.EndObject();
            rdfWriter.StartObject();
              rdfWriter.WriteTypedLiteral("bang", "http://www.w3.org/2001/XMLSchema#integer");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );

    }

    [Test]
    public void roundTripWriteTwoUUTTriplesWithSameSubject() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> \"fizz\"^^<http://www.w3.org/2001/XMLSchema#integer> .");
      harness.expect("<http://example.com/subj> <http://example.com/pred2> \"bang\"^^<http://www.w3.org/2001/XMLSchema#integer> .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteTypedLiteral("fizz", "http://www.w3.org/2001/XMLSchema#integer");
            rdfWriter.EndObject();
          rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred2");
            rdfWriter.StartObject();
              rdfWriter.WriteTypedLiteral("bang", "http://www.w3.org/2001/XMLSchema#integer");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();


      bool testPassed = harness.verify();
      Assert.IsTrue( testPassed );

    }

    [Test]
    public void roundTripWriteSingleBUUTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("_:genid1 <http://example.com/pred> <http://example.com/obj> .");

      RdfWriter rdfWriter = harness.getRdfWriter();
      
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteBlankNode("foo");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( harness.verify() );
    }


    [Test]
    public void roundTripWriteSingleUUBTriple() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://example.com/pred> _:genid1 .");

      RdfWriter rdfWriter = harness.getRdfWriter();
      
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteBlankNode("foo");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( harness.verify() );
    }

    [Test]
    public void roundTripWriteSingleBUBTripleWithSameNodeId() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("_:genid1 <http://example.com/pred> _:genid1 .");

      RdfWriter rdfWriter = harness.getRdfWriter();
      
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteBlankNode("foo");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteBlankNode("foo");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( harness.verify() );
    }


    [Test]
    public void roundTripTwoUUUTriplesWithSameSubjectDifferentPredicateNamespaces() {
      RdfXmlWriterTestHarness harness = new RdfXmlWriterTestHarness();
      harness.expect("<http://example.com/subj> <http://ex1.example.com/pred> <http://example.com/obj> .");
      harness.expect("<http://example.com/subj> <http://ex2.example.com/pred> <http://example.com/obj2> .");

      RdfWriter rdfWriter = harness.getRdfWriter();

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://ex1.example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
            rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://ex2.example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj2");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();


      Assert.IsTrue( harness.verify() );
    }

    [Test]
    public void registerNamespacePrefix() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      rdfWriter.RegisterNamespacePrefix( "http://foo.example.com/", "foo");
      
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("foo", "name", "http://foo.example.com/") );
    }


    [Test]
    public void commonNamespacePrefixesAlreadyRegistered() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://xmlns.com/foaf/0.1/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://xmlns.com/wot/0.1/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/2000/01/rdf-schema#prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/2002/07/owl#prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://purl.org/vocab/bio/0.1/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://purl.org/dc/elements/1.1/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
         
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://purl.org/dc/terms/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://web.resource.org/cc/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://purl.org/vocab/relationship/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();


          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/2003/01/geo/wgs84_pos#prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://purl.org/rss/1.0/prop");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();         
         
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("foaf", "prop", "http://xmlns.com/foaf/0.1/") , "foaf prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("wot", "prop", "http://xmlns.com/wot/0.1/") , "wot prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rdf", "prop", "http://www.w3.org/1999/02/22-rdf-syntax-ns#") , "rdf prefix"); 
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rdfs", "prop", "http://www.w3.org/2000/01/rdf-schema#") , "rdf schema prefix"); 
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("owl", "prop", "http://www.w3.org/2002/07/owl#") , "owl prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("bio", "prop", "http://purl.org/vocab/bio/0.1/") , "bio prefix"); 
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("dc", "prop", "http://purl.org/dc/elements/1.1/") , "dublin core prefix"); 
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("dct", "prop", "http://purl.org/dc/terms/" ) , "dublin core terms prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("cc", "prop", "http://web.resource.org/cc/") , "creative commons prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rel", "prop", "http://purl.org/vocab/relationship/") , "relationship prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("geo", "prop", "http://www.w3.org/2003/01/geo/wgs84_pos#") , "geo prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("rss", "prop", "http://purl.org/rss/1.0/") , "rss prefix");

    }


    [Test]
    public void newNamespacesAreAssignedNewPrefixes() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://bar.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("ns1", "name", "http://foo.example.com/"),"first new namespace assigned" );
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("ns2", "name", "http://bar.example.com/"),"second new namespace assigned" );
    }

    [Test]
    public void newNamespacePrefixesAreRememberedAndReused() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://bar.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

         rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/place");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();

        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("ns1", "place", "http://foo.example.com/"),"first new namespace reused" );
    }


    [Test]
    public void rdfNamespacePrefixCanBeChanged() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );
      rdfWriter.RegisterNamespacePrefix( "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "wtf");

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("wtf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "RDF element uses registered prefix");
      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("wtf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "Description element uses registered prefix" );
      Assert.IsTrue( xmlWriter.WasWriteStartAttributeCalledWith("wtf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "about attribute uses registered prefix" );
      Assert.IsTrue( xmlWriter.WasWriteStartAttributeCalledWith("wtf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"), "resource attribute uses registered prefix" );
    }

    [Test]
    public void subjectUsedTypedNodeIfRdfTypeSpecified() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );
      rdfWriter.RegisterNamespacePrefix( "http://example.com/", "ex");

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/Thing");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsTrue( xmlWriter.WasWriteStartElementCalledWith("ex", "Thing", "http://example.com/"));
    }

    [Test]
    public void subjectUsesOnlyFirstRdfTypePropertyToDetermineTypedNode() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );
      rdfWriter.RegisterNamespacePrefix( "http://example.com/", "ex");

      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteUriRef("http://example.com/subj");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://foo.example.com/name");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/Thing");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/Other");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsFalse( xmlWriter.WasWriteStartElementCalledWith("ex", "Other", "http://example.com/"));
    }

    [Test]
    public void writerMapsNodeIdsToNewValues() {
      XmlWriterStore xmlWriter = new XmlWriterStore();
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );
      rdfWriter.RegisterNamespacePrefix( "http://example.com/", "ex");
      
      rdfWriter.StartOutput();
        rdfWriter.StartSubject();
          rdfWriter.WriteBlankNode("foo");
          rdfWriter.StartPredicate();
            rdfWriter.WriteUriRef("http://example.com/pred");
            rdfWriter.StartObject();
              rdfWriter.WriteUriRef("http://example.com/obj");
           rdfWriter.EndObject();
         rdfWriter.EndPredicate();
        rdfWriter.EndSubject();
      rdfWriter.EndOutput();

      Assert.IsFalse( xmlWriter.WasWriteStringCalledWith("foo"), "attribute content" );
    }




  }

}
