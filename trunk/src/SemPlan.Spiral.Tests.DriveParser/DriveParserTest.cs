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


namespace SemPlan.Spiral.Tests.DriveParser {
  using Drive.Rdf;
  using NUnit.Framework;
  using SemPlan.Spiral.DriveParser;
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.IO;
	/// <summary>
	/// Programmer tests for DriveParser class
	/// </summary>
  /// <remarks>
  /// $Id: DriveParserTest.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
	[TestFixture]
  public class DriveParserTest {


    [Test]
    public void parserUsesResourceFactoryToCreatePredicate() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");

      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://www.w3.org/1999/02/22-rdf-syntax-ns#type" ) );
    }

    [Test]
    public void parserUsesResourceFactoryToCreateUriRefSubject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");

      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://example.org/node" ) );
    }


    [Test]
    public void parserUsesResourceFactoryToCreateUriRefObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");

      Assert.IsTrue( resourceFactory.WasMakeUriRefCalledWith( "http://example.org/type" ) );
    }


    [Test]
    public void parserUsesResourceFactoryToCreateBlankSubject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeBlankNodeCalledWith( "drive10000" ) );
    }

    [Test]
    public void parserUsesResourceFactoryToCreateBlankObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type><rdf:Description /></rdf:type></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakeBlankNodeCalledWith( "drive10000" ) );
    }

    [Test]
    public void parserUsesResourceFactoryToCreatePlainLiteralWithNoLanguageObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");
      
      Assert.IsTrue( resourceFactory.WasMakePlainLiteralCalledWith( "foo" ) );
    }

    [Test]
    public void parserUsesResourceFactoryToCreatePlainLiteralWithLanguageObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type xml:lang=\"pt\">foo</rdf:type></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");

      Assert.IsTrue( resourceFactory.WasMakePlainLiteralCalledWith( "foo", "pt" ) );
    }

    [Test]
    public void parserUsesResourceFactoryToCreateTypedLiteralObject() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");

      ResourceFactoryStore resourceFactory = new ResourceFactoryStore();
      DriveParser parser = new DriveParser(resourceFactory, new StatementFactoryStub() );
      parser.Parse(  reader, "");

      Assert.IsTrue( resourceFactory.WasMakeTypedLiteralCalledWith( "foo", "http://example.com/type" ) );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateUUUStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementUUUCalled );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateBUUStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementBUUCalled );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateUUBStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type><rdf:Description/></rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementUUBCalled );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateBUBStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type><rdf:Description/></rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementBUBCalled );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateUUPStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementUUPCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUPStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type>foo</rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementBUPCalled );
    }

    [Test]
    public void parserUsesStatementFactoryToCreateUUTStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementUUTCalled );
    }
    
    [Test]
    public void parserUsesStatementFactoryToCreateBUTStatement() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:type rdf:datatype=\"http://example.com/type\">foo</rdf:type></rdf:Description></rdf:RDF>");

      StatementFactoryCounter statementFactory = new StatementFactoryCounter();
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), statementFactory );
      parser.Parse(  reader, "");

      Assert.AreEqual( 1, statementFactory.MakeStatementBUTCalled );
    }

    
    [Test]
    public void parserPassesResourceFactoryObjectsToStatementFactory() {
      StringReader reader = new StringReader("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description rdf:about=\"http://example.org/node\"><rdf:type rdf:resource=\"http://example.org/type\"/></rdf:Description></rdf:RDF>");

      StatementFactoryStore statementFactory = new StatementFactoryStore();
      ResourceFactoryResponder resourceFactory = new ResourceFactoryResponder();
      
      
      DriveParser parser = new DriveParser(resourceFactory, statementFactory );
      parser.Parse(  reader, "");

      Assert.IsTrue( statementFactory.WasMakeStatementCalledWith( resourceFactory.itsUriRefResponse,  resourceFactory.itsUriRefResponse, resourceFactory.itsUriRefResponse) );
    }    

    [Test]
    public void parserUsesSuppliedDereferencerToGetRepresentationsOfUris() {
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), new StatementFactoryStub() );

      DereferencerStore dereferencer = new DereferencerStore();
      
      parser.SetDereferencer(dereferencer);
      parser.Parse(new Uri("http://example.com/"), "");

      Assert.IsTrue( dereferencer.WasDereferenceCalledWith( new Uri("http://example.com/") ) );
    }

    [Test]
    public void parserUsesSuppliedDereferencerToGetRepresentationsOfStringUris() {
      DriveParser parser = new DriveParser(new ResourceFactoryStub(), new StatementFactoryStub() );

      DereferencerStore dereferencer = new DereferencerStore();
      
      parser.SetDereferencer(dereferencer);
      parser.Parse( "http://example.com/", "");

      Assert.IsTrue( dereferencer.WasDereferenceCalledWith( "http://example.com/" ) );
    }
    
  }
  
  
  
}