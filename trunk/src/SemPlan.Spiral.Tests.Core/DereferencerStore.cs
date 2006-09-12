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


namespace SemPlan.Spiral.Tests.Core {
  using SemPlan.Spiral.Core;
  using System;
  using System.IO;

	/// <summary>
	/// An instrumented version of Dereferencer that can verify arguments to method calls. 
	/// </summary>
  /// <remarks>
  /// $Id: DereferencerStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class DereferencerStore : Dereferencer {
    private MethodCallStore itsMethodCalls;
  
  
    public DereferencerStore() {
      itsMethodCalls = new MethodCallStore();
    }
  
    public DereferencerResponse Dereference(Uri uri) {
      itsMethodCalls.RecordMethodCall("Dereference", uri);
      return new DeferencerResponseStub();
    }

    public bool WasDereferenceCalledWith(Uri uri) {
      return itsMethodCalls.WasMethodCalledWith("Dereference", uri);
    }
  
    public DereferencerResponse Dereference(string uri) {
      itsMethodCalls.RecordMethodCall("Dereference", uri);
      return new DeferencerResponseStub();
    }
  
    public bool WasDereferenceCalledWith(string uri) {
      return itsMethodCalls.WasMethodCalledWith("Dereference", uri);
    }
  
    private class DeferencerResponseStub : DereferencerResponse {
      private const string itsStubRdf = @"<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#'><rdf:Description rdf:value='foo'/></rdf:RDF>";
      
      public bool HasContent {
        get { return true; }
      }
      
      public Stream Stream {
        get {
          MemoryStream result = new MemoryStream();
          StreamWriter writer = new StreamWriter( result );
          writer.Write( itsStubRdf );
          writer.Flush();
          result.Position = 0;
          return result;
        }
      }

      public string Message {
        get { return "{stub message}"; }
      }

    }
  

  }
}
