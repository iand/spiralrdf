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
  using System.Text;

	/// <summary>
	/// A test utility that can record and verify method calls and arguments 
	/// </summary>
  /// <remarks>
  /// $Id: MethodCallStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class MethodCallStore {

    private Hashtable itsMethodCalls;

    public MethodCallStore() {
      itsMethodCalls = new Hashtable();
    }

    



    public void RecordMethodCall(string methodName, object argument1) {
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
    

    public void RecordMethodCall(string methodName, object argument1, object argument2) {
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

    public void RecordMethodCall(string methodName, object argument1, object argument2,  object argument3) {
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
      methodCall["argument3"] = argument3;
      calls.Add( methodCall );
      
    }

    public void RecordMethodCall(string methodName, object argument1, object argument2,  object argument3, object argument4) {
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
      methodCall["argument3"] = argument3;
      methodCall["argument4"] = argument4;
      calls.Add( methodCall );
      
    }


    
    public bool WasMethodCalledWith(string methodName, object argument1) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 1) {
            if (methodCall.Contains("argument1")) {
              if (AreEqual(methodCall["argument1"], argument1) ) {
                return true;
              }
            }
          }
          
        }
      }
      return false;
      
    }    

    public bool WasMethodCalledWith(string methodName, object argument1, object argument2) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 2) {
            if (methodCall.Contains("argument1")) {
              if (AreEqual(methodCall["argument1"], argument1) ) {
                if (methodCall.Contains("argument2")) {
                  if (AreEqual(methodCall["argument2"], argument2) ) {
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

    
    
    public bool WasMethodCalledWith(string methodName, object argument1, object argument2,  object argument3) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 3) {
            if (methodCall.Contains("argument1")) {
              if ( AreEqual(methodCall["argument1"], argument1) ) {
                if (methodCall.Contains("argument2")) {
                  if (AreEqual(methodCall["argument2"], argument2) ) {
                    if (methodCall.Contains("argument3")) {
                      if (AreEqual(methodCall["argument3"], argument3)) {
                        return true;
                      }
                    }
                  }
                }
              }
            }
          }
          
        }
      }
      return false;
      
    }
    
    public bool WasMethodCalledWith(string methodName, object argument1, object argument2,  object argument3, object argument4) {
      
      if (itsMethodCalls.Contains(methodName)) {
        ArrayList calls = (ArrayList)itsMethodCalls[methodName];
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          
          if (methodCall.Keys.Count == 4) {
            if (methodCall.Contains("argument1")) {
              if ( AreEqual(methodCall["argument1"], argument1) ) {
                if (methodCall.Contains("argument2")) {
                  if (AreEqual(methodCall["argument2"], argument2) ) {
                    if (methodCall.Contains("argument3")) {
                      if (AreEqual(methodCall["argument3"], argument3)) {
                        if (methodCall.Contains("argument4")) {
                          if (AreEqual(methodCall["argument4"], argument4)) {
                            return true;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          
        }
      }
      return false;
      
    }


    private bool AreEqual( object obj1, object obj2) {
      return ( (obj1 ==  null && obj2 == null )|| (obj1 !=  null && obj1.Equals(obj2) ));
    }
    
    public string getMethodCalls() {
      StringBuilder buffer = new StringBuilder();
      IDictionaryEnumerator methodCallsEnum = itsMethodCalls.GetEnumerator();
      while ( methodCallsEnum.MoveNext() ) {

        ArrayList calls = (ArrayList)methodCallsEnum.Value;
        for (int index = 0; index < calls.Count; ++index) {
          Hashtable methodCall = (Hashtable)calls[index];
          buffer.Append( (string)methodCallsEnum.Key).Append("(");
          for (int i = 1; i <= methodCall.Keys.Count; ++i) {
            if (i > 1) {
              buffer.Append(", ");
            }
            buffer.Append( methodCall["argument" + i]);
          }
          buffer.Append(")\n");
        }
      }
      return buffer.ToString();
    }
    
  }




}
