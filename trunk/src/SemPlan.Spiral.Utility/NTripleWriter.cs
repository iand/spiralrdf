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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using System;
  using System.IO;
  using System.Text;
	/// <summary>
	/// Represents a Writer that outputs triples in the NTriples format
	/// </summary>
  /// <remarks>
  /// $Id: NTripleWriter.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  public class NTripleWriter : RdfWriter {
    private TextWriter itsWriter;   
    
    private string itsCurrentSubject;
    private string itsCurrentPredicate;
    private string itsCurrentObject;
    private int itsNextBlankNodeIndex;
    
    public NTripleWriter(TextWriter writer) {
      itsWriter = writer;
      itsCurrentSubject = "";
      itsCurrentPredicate = "";
      itsCurrentObject = "";
      itsNextBlankNodeIndex = 1;
    }
    public void StartSubject() {
      itsCurrentSubject = "";
    }

    public void StartPredicate() {
      itsCurrentPredicate = "";
    }
    
    public void StartObject() {
      itsCurrentObject = "";
    }
    
    
    public void EndObject() {
      itsWriter.Write( itsCurrentSubject );
      itsWriter.Write( " " );
      itsWriter.Write( itsCurrentPredicate );
      itsWriter.Write( " " );
      itsWriter.Write( itsCurrentObject );
      itsWriter.WriteLine(" .");    
    }
    public void EndPredicate() {

    }
    public void EndSubject() {
      itsCurrentSubject = "";
    }
      
    public void WriteUriRef(string uriRef) {
      string escapedUriRef = Escape(uriRef);
      if (itsCurrentSubject == "") {
        itsCurrentSubject = "<" + escapedUriRef + ">";
      }      
      else if (itsCurrentPredicate == "") {
        itsCurrentPredicate = "<" + escapedUriRef + ">";
      }      
      else {
        itsCurrentObject = "<" + escapedUriRef + ">";
      }
    }

    public void WritePlainLiteral(string lexicalValue) {
        itsCurrentObject = "\"" + Escape(lexicalValue) + "\"";
    }

    public void WritePlainLiteral(string lexicalValue, string language) {
        itsCurrentObject = "\"" + Escape(lexicalValue) + "\"@" + Escape(language.ToLower());
    }

    public void WriteTypedLiteral(string lexicalValue, string uriRef) {
        itsCurrentObject = "\"" + Escape(lexicalValue) + "\"^^<" + Escape(uriRef) + ">";
    }

    public void WriteBlankNode(string nodeId) {
        if (itsCurrentSubject == "") {
          itsCurrentSubject = "_:" + Escape(nodeId);
       }
       else {
          itsCurrentObject = "_:" + Escape(nodeId);
       }
    }

    public string GenerateNodeId() {
      return "genid" + itsNextBlankNodeIndex++;
    }

    public void StartOutput() {
    
    }
    
    public void EndOutput() {
    
    }

    public string Escape(string input) {
      StringBuilder output = new StringBuilder();
      
      CharEnumerator enumerator= input.GetEnumerator();
      
      while (enumerator.MoveNext()) {
        char c = enumerator.Current;
        
        if (c == '\u005C') {
          output.Append( @"\\");
        }
        else if (c == '\u0009') {
          output.Append( @"\t");
        }
        else if (c == '\u000A') {
          output.Append( @"\n");
        }
        else if (c == '\u000D') {
          output.Append( @"\r");
        }
        else if (c == '\u0022') {
          output.Append( @"\""");
        }
        else if (c >= '\u0020' && c <= '\u007E') {
          output.Append( c );
        }
        else {
          output.Append( String.Format(@"\u{0:X4}", (int)c));
        }
      }
      return output.ToString();    
    }


  }
}