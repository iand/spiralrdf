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
  using SemPlan.Spiral.ICalParser;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.XsltParser;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO;
  using System.Xml;

  /// <summary>
  /// Programmer tests for Parser class
  /// </summary>
  /// <remarks>
  /// $Id: ICalParserTests.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  [TestFixture]
  public class ICalParserTest {

    [Test]
    public void parseWorksWithIcalxOutputViaTextReader() {
      ICalParserFactory factory = new ICalParserFactory();
      Parser parser =  factory.MakeParser( new ResourceFactoryStub(), new StatementFactoryStub());
      
      try {
        StringReader icalxReader = new StringReader( icalx );
        parser.Parse( icalxReader, "");
        Assert.IsTrue( true );
      }
      catch (Exception e) {
        Assert.Fail(e.ToString());
      }

    }

    [Test]
    public void parseWorksWithIcalxOutputViaStream() {
      StringReader icalxReader = new StringReader( icalx );

      ICalParserFactory factory = new ICalParserFactory();
      Parser parser =  factory.MakeParser( new ResourceFactoryStub(), new StatementFactoryStub());
      
      try {
        MemoryStream stream = new MemoryStream();
        StreamWriter writer = new StreamWriter(stream);
        writer.Write( icalx );
        writer.Flush();
        stream.Position = 0;
        
        parser.Parse( stream, "");
        Assert.IsTrue( true );
      }
      catch (Exception e) {
        Assert.Fail(e.ToString());
      }

    }


    private string icalx = @"BEGIN:VCALENDAR
VERSION

 :2.0

PRODID

 :-//Mozilla.org/NONSGML Mozilla Calendar V1.0//EN

BEGIN:VEVENT
UID

 :3f6cb680-0a20-11d9-9a56-810dfb6de997

SUMMARY

 :Wight Off Road MTB race

LOCATION

 :Isle Of Wight

STATUS

 :TENTATIVE

CLASS

 :PUBLIC

DTSTART

 :20041030T110000

DTEND

 :20041030T130000

DTSTAMP

 :20040919T094205Z

LAST-MODIFIED

 :20040919T094253Z

END:VEVENT

END:VCALENDAR

";
  
  }
    


}