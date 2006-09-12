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
  using System.Collections;
  using System.IO;

	/// <summary>
	/// An instrumented version of Dereferencer that responds with known content
	/// </summary>
  /// <remarks>
  /// $Id: DereferencerResponder.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class DereferencerResponder : Dereferencer {
    private Hashtable itsUriMap;
    
    public DereferencerResponder() {
      itsUriMap = new Hashtable();
    }
    
    
    public void MapUriToContent(Uri uri, string content) {
      itsUriMap[uri.ToString()] = content;
    }
  
    /// <summary>
    /// Dereference the supplied URI
    /// </summary>
    public DereferencerResponse Dereference(Uri uri) {
      return Dereference( uri.ToString() );
    }
  
    /// <summary>
    /// Dereference the supplied string representation of a URI
    /// </summary>
    public DereferencerResponse Dereference(string uri) {
      if (itsUriMap.Contains(uri)) {
        MemoryStream result = new MemoryStream();
        StreamWriter writer = new StreamWriter( result );
        writer.Write( (string)itsUriMap[ uri ] );
        writer.Flush();
        result.Position = 0;
        return new SuccessfulResponse(result);
      }
      else  {
        throw new Exception("No content found for URI " + uri);
      }
      
    }

    private class SuccessfulResponse : DereferencerResponse {
      Stream itsStream;
      
      public SuccessfulResponse( Stream aStream) {
        itsStream = aStream;
      }
      public bool HasContent {
        get { return true; }
      }
      
      public Stream Stream {
        get { return itsStream; }
      }
      
      public string Message {
        get { return ""; }
      }
    }
  

  }
}
