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


namespace SemPlan.Spiral.Tests.Parser {
  using NUnit.Framework;
  using SemPlan.Spiral.XsltParser;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO;
  using System.Xml;
  /// <summary>
  /// Programmer tests for Parser class
  /// </summary>
  /// <remarks>
  /// $Id: XsltParserTests.cs,v 1.4 2006/01/20 10:37:45 ian Exp $
  ///</remarks>
  [TestFixture]
  public class XsltParserTest {
  
    
    [Test]
    public void parserUsesResourceFactoryToCreatePredicate() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub());
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://www.w3.org/1999/02/22-rdf-syntax-ns#type" ) );
    }
    
    [Test]
    public void parserUsesResourceFactoryToCreateUriRefSubject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://example.org/node" ) );
    }
    
    
    [Test]
    public void parserUsesResourceFactoryToCreateUriRefObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://example.org/type" ) );
    }
    
    
    [Test]
    public void parserUsesResourceFactoryToCreateBlankSubject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeBlankNodeCalledWithANodeId() );
    }
    
    [Test]
    public void parserUsesResourceFactoryToCreateBlankObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type><rdf:Description /></rdf:type></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeBlankNodeCalledWithANodeId() );
    }
    
    [Test]
    public void parserUsesResourceFactoryToCreatePlainLiteralWithNoLanguageObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakePlainLiteralCalledWith( "foo" ) );
    }
    
    [Test]
    public void parserUsesResourceFactoryToCreatePlainLiteralWithLanguageObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type xml:lang=\"pt\">foo</rdf:type></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakePlainLiteralCalledWith( "foo", "pt" ) );
    }
    
    [Test]
    public void parserUsesResourceFactoryToCreateTypedLiteralObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");
      
      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeTypedLiteralCalledWith( "foo", "http://example.com/type" ) );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateUUUStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementUUUCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUUStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementBUUCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateUUBStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type><rdf:Description/></rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementUUBCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUBStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type><rdf:Description/></rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementBUBCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateUUPStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementUUPCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUPStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementBUPCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateUUTStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementUUTCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUTStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");
      
      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(reader, "");
      
      Assert.AreEqual( 1, statementFactory.MakeStatementBUTCalled );
    }
    
    
    [Test]
    public void parserPassesResourceFactoryObjectsToStatementFactory() {
    StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");
    
    StatementFactoryStore statementFactory = new StatementFactoryStore();
    ResourceFactoryResponder resourceFactory = new ResourceFactoryResponder();
    
    
    XsltParserFactory parserFactory = new XsltParserFactory();
    Parser parser = parserFactory.MakeParser(resourceFactory, statementFactory );
    parser.Parse(reader, "");
    
    Assert.IsTrue( statementFactory.WasMakeStatementCalledWith( resourceFactory.itsUriRefResponse,  resourceFactory.itsUriRefResponse, resourceFactory.itsUriRefResponse) );
    }    


    [Test]
    public void parseReaderWithoutEntities() {
      
      string rdf = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
	<rdf:Description rdf:about=""http://example.com"">
		<rdf:value>Test</rdf:value>
	</rdf:Description>
</rdf:RDF>";
      
    
      SimpleModel model = new SimpleModel(new XsltParserFactory());

      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory() );
      parser.NewStatement += new StatementHandler(model.Add);
      parser.Parse( new StringReader(rdf),"");


      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect( @"<http://example.com> <http://www.w3.org/1999/02/22-rdf-syntax-ns#value> ""Test"" .");

      foreach (string ntriple in model) {
        verifier.Receive(ntriple);
      }
    
      bool testPassed = verifier.Verify();
      string failureDescription = verifier.GetLastFailureDescription() + "\r\nTriples received:\r\n" + model.ToString();
    
      if ( ! testPassed) {
        Console.WriteLine("XsltParserTest.parseWithEntities FAILED because ");
        Console.WriteLine( failureDescription );
      }

      Assert.IsTrue( testPassed );
    
    }

    [Test]
    public void parseStreamWithEntitiesInNamespaceDeclaration() {
      
      string rdf = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE rdf:RDF [
	<!ENTITY ex 'http://example.com/ns/'>
]>
<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"" xmlns:ex=""&ex;"">
	<rdf:Description rdf:about=""http://example.com"">
		<ex:value>Test</ex:value>
	</rdf:Description>
</rdf:RDF>";
      
    
      SimpleModel model = new SimpleModel(new XsltParserFactory());

      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
      parser.NewStatement += new StatementHandler(model.Add);

      MemoryStream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter( stream );
      writer.Write( rdf );
      writer.Flush();
      stream.Position = 0;
      

      parser.Parse( stream, "" );


      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect( @"<http://example.com> <http://example.com/ns/value> ""Test"" .");
      foreach (string ntriple in model) {
        verifier.Receive(ntriple);
      }
    
      bool testPassed = verifier.Verify();
      string failureDescription = verifier.GetLastFailureDescription() + "\r\nTriples received:\r\n" + model.ToString();
    
      if ( ! testPassed) {
        Console.WriteLine("XsltParserTest.parseWithEntities FAILED because ");
        Console.WriteLine( failureDescription );
      }

      Assert.IsTrue( testPassed );
    
    }


    [Test]
    public void entitiesInNamespaceDeclarationResultInZeroTriples() {
      
      string rdf = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE rdf:RDF [
	<!ENTITY ex 'http://example.com/ns/'>
	<!ENTITY rdf 'http://www.w3.org/1999/02/22-rdf-syntax-ns#'>
]>
<rdf:RDF xmlns:rdf=""&rdf;"" xmlns:ex=""&ex;"">
	<rdf:Description rdf:about=""http://example.com"">
		<ex:value>Test</ex:value>
	</rdf:Description>
</rdf:RDF>";
      
    
      SimpleModel model = new SimpleModel(new XsltParserFactory());

      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
      parser.NewStatement += new StatementHandler(model.Add);

      MemoryStream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter( stream );
      writer.Write( rdf );
      writer.Flush();
      stream.Position = 0;
      
      try {
        parser.Parse( stream, "" );
      } catch (SemPlan.Spiral.Core.ParserException ) {
        // swallow error output from validation stylesheet
        //Console.WriteLine(e);
      }
      Assert.AreEqual( 0, model.Count );
   
    }
  
  
    [Test] [Category("KnownFailures")] 
    [Ignore("Known bug in .NET 1.1 framework, entities in namespace declaration not expanded")]
    public void parseStreamWithEntitiesInNamespaceDeclarationOfRootElementNamespace() {
      
      string rdf = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE rdf:RDF [
	<!ENTITY ex 'http://example.com/ns/'>
	<!ENTITY rdf 'http://www.w3.org/1999/02/22-rdf-syntax-ns#'>
]>
<rdf:RDF xmlns:rdf=""&rdf;"" xmlns:ex=""&ex;"">
	<rdf:Description rdf:about=""http://example.com"">
		<ex:value>Test</ex:value>
	</rdf:Description>
</rdf:RDF>";
      
    
      SimpleModel model = new SimpleModel(new XsltParserFactory());

      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
      parser.NewStatement += new StatementHandler(model.Add);

      MemoryStream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter( stream );
      writer.Write( rdf );
      writer.Flush();
      stream.Position = 0;
      
			try {
				parser.Parse( stream, "" );
			}
			catch (Exception) { }


      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect( @"<http://example.com> <http://example.com/ns/value> ""Test"" .");
      foreach (string ntriple in model) {
        verifier.Receive(ntriple);
      }
    
      bool testPassed = verifier.Verify();
      string failureDescription = verifier.GetLastFailureDescription() + "\r\nTriples received:\r\n" + model.ToString();
    
      if ( testPassed) {
        Console.WriteLine("XsltParserTest.parseWithEntities UNEXPECTEDLY PASSED - entity in namespace declaration problem might have been fixed ");
      }

      Assert.IsTrue( ! testPassed ); // We exoect this to fail
    
    }

  
  
    [Test]
    public void testPaoloMassa() {
      
      string rdf = @"<?xml version=""1.0"" encoding=""iso-8859-1"" ?>
<!DOCTYPE rdf:RDF [
	<!ENTITY rdf 'http://www.w3.org/1999/02/22-rdf-syntax-ns#'>
	<!ENTITY rdfs 'http://www.w3.org/2000/01/rdf-schema#'>
	<!ENTITY cc 'http://web.resource.org/cc/'>
	<!ENTITY foaf 'http://xmlns.com/foaf/0.1/'>
	<!ENTITY dc 'http://purl.org/dc/elements/1.1/'>
	<!ENTITY dcterms 'http://purl.org/dc/terms/'>
]>
<rdf:RDF
	xmlns=""http://xmlns.com/foaf/0.1/""
	xmlns:quaff=""http://purl.org/net/schemas/quaffing/""
	xmlns:bio=""http://purl.org/vocab/bio/0.1/""
	xmlns:wot=""http://xmlns.com/wot/0.1/""
	xmlns:k=""http://opencyc.sourceforge.net/daml/cyc.daml#""
	xmlns:rss=""http://purl.org/rss/1.0/""
	xmlns:lang=""http://purl.org/net/inkel/rdf/schemas/lang/1.1#""
	xmlns:zodiac=""http://www.ideaspace.net/users/wkearney/schema/astrology/0.1#""
	xmlns:airport=""http://www.daml.org/2001/10/html/airport-ont#""
	xmlns:contact=""http://www.w3.org/2000/10/swap/pim/contact#""
	xmlns:geo=""http://www.w3.org/2003/01/geo/wgs84_pos#""
	xmlns:bt=""http://jibbering.com/vocabs/bluetooth#""
	xmlns:dcterms=""&dcterms;""
	xmlns:dc=""&dc;""
	xmlns:cc=""&cc;""
        xmlns:foaf=""http://xmlns.com/foaf/0.1/"" 
        xmlns:owl=""http://www.w3.org/2002/07/owl#"" 
        xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"" 
        xmlns:rdfs=""http://www.w3.org/2000/01/rdf-schema#"" 
        xmlns:trust=""http://trust.mindswap.org/ont/trust.owl#""> 
 
 <PersonalProfileDocument rdf:about="""">
	<dc:title>FoaF Document for Paolo Massa</dc:title>

	<dc:description>Friend-of-a-Friend description of Paolo Massa</dc:description>
	<topic rdf:nodeID=""paolomassa""/>
	<Maker rdf:nodeID=""paolomassa""/>
	<dcterms:created>2004-09-21T13:45:21Z</dcterms:created>
	<cc:license rdf:resource=""http://creativecommons.org/licenses/nc/1.0""/>
 </PersonalProfileDocument>
 </rdf:RDF>";
      
    
      SimpleModel model = new SimpleModel(new XsltParserFactory());

      XsltParserFactory parserFactory = new XsltParserFactory();
      Parser parser = parserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
      parser.NewStatement += new StatementHandler(model.Add);

      MemoryStream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter( stream );
      writer.Write( rdf );
      writer.Flush();
      stream.Position = 0;
        Console.WriteLine(model.ToString());
      
      try {
        parser.Parse( stream, "" );
      } catch (SemPlan.Spiral.Core.ParserException e) {
        // swallow error output from validation stylesheet
        Console.WriteLine(e);
      }
      Assert.AreEqual( 0, model.Count );
   
    }
  
  }



}