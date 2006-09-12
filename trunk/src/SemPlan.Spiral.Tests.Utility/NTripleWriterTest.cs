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
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO;
	/// <summary>
	/// Programmer tests for NTripleWriter class
	/// </summary>
  /// <remarks>
  /// $Id: NTripleWriterTest.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  [TestFixture]
  public class NTripleWriterTest {

    [Test]
    public void escapeSpecials() {
      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
      
      Assert.AreEqual( @"\t", writer.Escape("\u0009") );
      Assert.AreEqual( @"\n", writer.Escape("\u000A") );
      Assert.AreEqual( @"\r", writer.Escape("\u000D") );
      Assert.AreEqual( @"\""", writer.Escape("\u0022") );
      Assert.AreEqual( @"\\", writer.Escape("\u005C") );
    }

    [Test]
    public void escapeVeryLowAscii() {
      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
      
      Assert.AreEqual( @"\u0000", writer.Escape("\u0000") );
      Assert.AreEqual( @"\u0001", writer.Escape("\u0001") );
      Assert.AreEqual( @"\u0002", writer.Escape("\u0002") );
      Assert.AreEqual( @"\u0003", writer.Escape("\u0003") );
      Assert.AreEqual( @"\u0004", writer.Escape("\u0004") );
      Assert.AreEqual( @"\u0005", writer.Escape("\u0005") );
      Assert.AreEqual( @"\u0006", writer.Escape("\u0006") );
      Assert.AreEqual( @"\u0007", writer.Escape("\u0007") );
      Assert.AreEqual( @"\u0008", writer.Escape("\u0008") );
    }

    [Test]
    public void escapeHighUnicode() {
      StringWriter output = new StringWriter();
      NTripleWriter writer = new NTripleWriter(output);
      
      Assert.AreEqual( @"\u0080", writer.Escape("\u0080") );
      Assert.AreEqual( @"\uC2C1", writer.Escape("\uC2C1") );
    }
  }
}
