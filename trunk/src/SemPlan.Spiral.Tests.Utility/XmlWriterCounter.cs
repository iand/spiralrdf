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
	/// An instrumented version of XmlWriter that counts calls to  methods. 
	/// </summary>
  /// <remarks>
  /// $Id: XmlWriterCounter.cs,v 1.1 2005/05/23 21:45:55 ian Exp $
  ///</remarks>
  public class XmlWriterCounter : XmlWriter {


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
    
    public override void Close() {  }
    public override void Flush() {  }
    public override string LookupPrefix(   string ns ) { return ""; }
    public override void WriteBase64(    byte[] buffer,    int index,    int count ) {  }
    public override void WriteBinHex(   byte[] buffer,   int index,   int count) {  }
    
    public override void WriteCData(   string text) {  } 
    
    public override void WriteCharEntity(   char ch) {  }
    
    
    public override void WriteChars(   char[] buffer,   int index,   int count) {  }
    
    public override void WriteComment(   string text) {  }
    
    
    public override void WriteDocType(   string name,   string pubid,   string sysid,   string subset) {  }
    
    
    public override void WriteEndAttribute() {  }
    
    public int WriteEndDocumentCalled = 0;
    public override void WriteEndDocument() {  ++WriteEndDocumentCalled; }
    
    public int WriteEndElementCalled = 0;
    public override void WriteEndElement() {  ++WriteEndElementCalled; }
    
    public override void WriteEntityRef(   string name) {  }
    
    public override void WriteFullEndElement() {  }
    public override void WriteName(   string name) {  }
    
    public override void WriteNmToken(   string name) {  }
    
    public override void WriteProcessingInstruction(   string name,   string text) {  }
    
    
    public override void WriteQualifiedName(   string localName,   string ns) {  }
    
    public override void WriteRaw(   string data) {  }
    
    
    public override void WriteRaw(   char[] buffer,   int index,   int count) {  }
    
    
    public override void WriteStartAttribute(   string prefix,   string localName,   string ns) {  }
    
    public override void WriteStartDocument() {  }
    
    
    public override void WriteStartDocument(   bool standalone) {  }
    
    
    public override void WriteStartElement(   string prefix,   string localName,   string ns) {  }
    
    public override void WriteString(   string text) {  }
    
    
    public override void WriteSurrogateCharEntity(   char lowChar,   char highChar) {  }
    
    
    public override void WriteWhitespace(   string ws) {  }

  }
}
