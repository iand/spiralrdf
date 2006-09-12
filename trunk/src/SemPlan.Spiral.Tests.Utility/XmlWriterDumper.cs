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
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.Xml;
	/// <summary>
	/// An instrumented version of XmlWriter that dumps calls to  methods to the console
	/// </summary>
  /// <remarks>
  /// $Id: XmlWriterDumper.cs,v 1.1 2005/05/23 21:45:55 ian Exp $
  ///</remarks>
  public class XmlWriterDumper : XmlWriter {
    private int itsIndentLevel;

    public XmlWriterDumper() {
      itsIndentLevel = 0;
    }

    public void writeIndent() {
      for (int i = 0; i < itsIndentLevel; i++) {
        Console.Out.Write("  ");
      }
    }
  
    public override WriteState WriteState {
      get {
        return WriteState.Start;
      }
    }
    
    public override string XmlLang {
      get {
        return null;
      }
    }
    
    public override XmlSpace XmlSpace {
      get {
        return XmlSpace.Default;
      }
    }
    
    public override void Close() { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.Close()");  }
    public override void Flush() { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.Flush()");  }
    public override string LookupPrefix(   string ns ) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.LookupPrefix('" + ns + "')");  return ""; }
    public override void WriteBase64(    byte[] buffer,    int index,    int count ) {  }
    public override void WriteBinHex(   byte[] buffer,   int index,   int count) {  }
    
    public override void WriteCData(   string text) {  } 
    
    public override void WriteCharEntity(   char ch) {  }
    
    
    public override void WriteChars(   char[] buffer,   int index,   int count) {  }
    
    public override void WriteComment(   string text) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteComment('" + text + "')");  }
    
    
    public override void WriteDocType(   string name,   string pubid,   string sysid,   string subset) {  }
    
    
    public override void WriteEndAttribute() { --itsIndentLevel;  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteEndAttribute()");   }
    
    public override void WriteEndDocument() {  --itsIndentLevel; writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteEndDocument()"); }
    
    public override void WriteEndElement() {  --itsIndentLevel; writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteEndElement()");  }
    
    public override void WriteEntityRef(   string name)  { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteEntityRef('" + name + "')");  }
    
    public override void WriteFullEndElement() {  --itsIndentLevel; writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteFullEndElement()");   }
    public override void WriteName(   string name) {  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteName('" + name + "')");  }
    
    public override void WriteNmToken(   string name) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteNmToken('" + name + "')");  }
    
    public override void WriteProcessingInstruction(   string name,   string text) {  }
    
    
    public override void WriteQualifiedName(   string localName,   string ns) {  }
    
    public override void WriteRaw(   string data) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteRaw('" + data + "')");  }
    
    
    public override void WriteRaw(   char[] buffer,   int index,   int count) {  }
    
    
    public override void WriteStartAttribute(   string prefix,   string localName,   string ns) {  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteStartAttribute('" + prefix + "', '" + localName + "', '" + ns + "')");  ++itsIndentLevel; }
    
    public override void WriteStartDocument()  {  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteStartDocument()");  ++itsIndentLevel;}
    
    public override void WriteStartDocument(   bool standalone)  {  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteStartDocument('" + standalone + "')"); ++itsIndentLevel; }
    
    
    public override void WriteStartElement(   string prefix,   string localName,   string ns)  {  writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteStartElement('" + prefix + "', '" + localName + "', '" + ns + "')");  ++itsIndentLevel;}
    
    public override void WriteString(   string text) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteString('" + text + "')");  }
    
    
    public override void WriteSurrogateCharEntity(   char lowChar,   char highChar) {  }
    
    
    public override void WriteWhitespace(   string ws) { writeIndent(); Console.Out.WriteLine("XmlWriterDumper.WriteWhitespace('" + ws + "')");  }

  }
}
