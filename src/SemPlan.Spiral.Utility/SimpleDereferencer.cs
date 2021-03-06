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
  using System.Net;
  using System.IO;

	/// <summary>
	/// Represents a simplistic, non caching implementation of Dereferencer
	/// </summary>
  /// <remarks>
  /// $Id: SimpleDereferencer.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  public class SimpleDereferencer : Dereferencer {
  
    /// <summary>
    /// Dereference the supplied URI
    /// </summary>
    public DereferencerResponse Dereference(Uri uri) {
      try {
        WebRequest request = WebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        return new SuccessfulResponse( response.GetResponseStream() );
      }
      catch (Exception e) {
        return new FailedResponse( e.Message );
      
      }
    }
  
    /// <summary>
    /// Dereference the supplied string representation of a URI
    /// </summary>
    public DereferencerResponse Dereference(string uri) {
      return Dereference(new Uri(uri));
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
        get { return "OK"; }
      }
    }
  

    private class FailedResponse : DereferencerResponse {
      string itsMessage;
      public FailedResponse(string aMessage) {
        itsMessage = aMessage;
      }
    
      public bool HasContent {
        get { return false; }
      }
      
      public Stream Stream {
        get { return new MemoryStream(); }
      }
      
      public string Message {
        get { return itsMessage; }
      } 
    }
  
  }
}
