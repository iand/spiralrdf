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
  
	/// <summary>
	/// An instrumented version of RdfWriter that can verify arguments to method calls. 
	/// </summary>
  /// <remarks>
  /// $Id: RdfWriterStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class RdfWriterStore : RdfWriter {
    private Hashtable itsMethodCalls;
    
    public RdfWriterStore() {
      itsMethodCalls = new Hashtable();
    }
    
    public void StartOutput() { /*  NOOP   */  }
    public void EndOutput() { /*  NOOP   */  }
    public void StartSubject() { /*  NOOP   */  }
    public void EndSubject() { /*  NOOP   */  }
    public void StartPredicate() { /*  NOOP   */  }
    public void EndPredicate() { /*  NOOP   */  }
    public void StartObject() { /*  NOOP   */  }
    public void EndObject() { /*  NOOP   */  }

    
    private void recordSingleArgumentMethodCall(string methodName, object argument1) {
      ArrayList calls;
      
      if (itsMethodCalls.Contains(methodName)) {
        calls = (ArrayList)itsMethodCalls[methodName];
      }
      else {
        calls = new ArrayList();
        itsMethodCalls[methodName] = calls;
      }
      
      Hashtable  methodCall = new Hashtable();
      methodCall["argument1"] = argument1;
      calls.Add( methodCall );
      
    }
    
    private bool singleArgumentMethodCallExists(string methodName, object argument1) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 1) {
            if (methodCall.Contains("argument1")) {
              if (methodCall["argument1"].Equals(argument1)) {
                return true;
              }
            }
          }
          
        }
      }
      return false;
      
    }
    
    private void recordDoubleArgumentMethodCall(string methodName, object argument1, object argument2) {
      ArrayList calls;
      
      if (itsMethodCalls.Contains(methodName)) {
        calls = (ArrayList)itsMethodCalls[methodName];
      }
      else {
        calls = new ArrayList();
        itsMethodCalls[methodName] = calls;
      }
      
      Hashtable  methodCall = new Hashtable();
      methodCall["argument1"] = argument1;
      methodCall["argument2"] = argument2;
      calls.Add( methodCall );
      
    }


    private bool doubleArgumentMethodCallExists(string methodName, object argument1, object argument2) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 2) {
            if (methodCall.Contains("argument1")) {
              if (methodCall["argument1"].Equals(argument1)) {
                if (methodCall.Contains("argument2")) {
                  if (methodCall["argument2"].Equals(argument2)) {
                    return true;
                  }
                }
              }
            }
          }
          
        }
      }
      return false;
      
    }
    
    
    public void WriteUriRef(string uriRef) {
      recordSingleArgumentMethodCall("WriteUriRef", uriRef);
    }

    public bool WasWriteUriRefCalledWith(string uriRef) {
      return singleArgumentMethodCallExists("WriteUriRef", uriRef);
    }


    public void WritePlainLiteral(string lexicalValue) {
      recordSingleArgumentMethodCall("WritePlainLiteral", lexicalValue);
    }

    public bool WasWritePlainLiteralCalledWith(string lexicalValue) {
      return singleArgumentMethodCallExists("WritePlainLiteral", lexicalValue);
    }


    public void WritePlainLiteral(string lexicalValue, string language) {
      recordDoubleArgumentMethodCall("WritePlainLiteral", lexicalValue, language);
    }

    public bool WasWritePlainLiteralCalledWith(string lexicalValue, string language) {
      return doubleArgumentMethodCallExists("WritePlainLiteral", lexicalValue, language);
    }

    
    public void WriteTypedLiteral(string lexicalValue, string uriRef) {
      recordDoubleArgumentMethodCall("WriteTypedLiteral", lexicalValue, uriRef);
    }

    public bool WasWriteTypedLiteralCalledWith(string lexicalValue, string uriRef) {
      return doubleArgumentMethodCallExists("WriteTypedLiteral", lexicalValue, uriRef);
    }

    
    public void WriteBlankNode(string nodeId) {
      recordSingleArgumentMethodCall("WriteBlankNode", nodeId);
    }
    
    public bool WasWriteBlankNodeCalledWith(string nodeId) {
      return singleArgumentMethodCallExists("WriteBlankNode", nodeId);
    }
    
  }
}