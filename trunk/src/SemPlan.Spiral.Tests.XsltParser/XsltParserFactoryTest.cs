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


namespace SemPlan.Spiral.Tests.XsltParser {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.XsltParser;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO;
  /// <summary>
  /// Programmer tests for XsltParserFactory class
  /// </summary>
  /// <remarks>
  /// $Id: XsltParserFactoryTest.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  [TestFixture]
  public class XsltParserFactoryTest {
  
    
    [Test]
    public void makeParserReturnsParserThatUsesSuppliedResourceFactory() {
      string rdfContent = @"<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#'><rdf:Description><rdf:value>bar</rdf:value></rdf:Description></rdf:RDF>";
      
      XsltParserFactory parserFactory = new XsltParserFactory();
      
      ResourceFactoryCounter resourceFactoryToBeIgnored = new ResourceFactoryCounter();
      Parser parser1 = parserFactory.MakeParser( resourceFactoryToBeIgnored, new StatementFactoryStub() );
      Parser parser2 = parserFactory.MakeParser( new ResourceFactoryStub(), new StatementFactoryStub() );
      
      parser2.Parse( new StringReader( rdfContent), "");
      
      Assert.AreEqual( 0, resourceFactoryToBeIgnored.MakeBlankNodeWithNodeIdCalled, "First resource factory should not be used");
   
   
    }

  }



}