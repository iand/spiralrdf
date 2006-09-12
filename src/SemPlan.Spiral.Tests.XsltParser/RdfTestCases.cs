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
  using SemPlan.Spiral.XsltParser;
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.IO;
	/// <summary>
	/// Tests against the RDF test suite
	/// </summary>
  /// <remarks>
  /// $Id: RdfTestCases.cs,v 1.3 2006/01/10 12:26:53 ian Exp $
  ///</remarks>
	[TestFixture]
  public class RdfTestCases {

    [Test] [Category("KnownFailures")]
    [Ignore("3 Known Failures in RDF core test cases")]
    public void parserPassesRdfCoreTestCases() {
    
      RdfTestSuite suite = new RdfTestSuite(new XsltParserFactory());
      
      if ( suite.Count != suite.RunTests() ) {
        // Uncomment for verbose description of failures
        Console.Out.WriteLine("3 known XML Literal failures"); // suite.getFailureDescription() 
      }
      
      // 3 known XML Literal failures
      Assert.AreEqual( suite.Count-3, suite.RunTests());
      
    }
    
  }
  
  
  
}