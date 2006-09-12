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

namespace SemPlan.Spiral.Core {
  using System;
  using System.Collections;
	/// <summary>
	/// Represents a resource, i.e. anything that assertions can be made about or with
	/// </summary>
  /// <remarks>
  /// $Id: Resource.cs,v 1.5 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class Resource : Atom, PatternTerm {
    private Object itsValue;
    private int itsHashCode;
    
    public Resource()  {
      itsValue = new Object();
      itsHashCode = Guid.NewGuid().GetHashCode();
    }
    public Resource(int hashCode)  {
      itsValue = new Object();
      itsHashCode = hashCode;
    }
    
    public Resource(Object value) {
      itsValue = value;
      itsHashCode = itsValue.GetHashCode();
    }
  
    public override string ToString() {
      string ret = itsValue.ToString();
      if (ret.Equals("System.Object")) {
        return "res:" + GetHashCode() ;
      }
      else {
        return ret;
      }
    }
    
    public string GetLabel() {
      return ToString();
    }

    public object Value {
      get {
        return itsValue;
      }
    }
    
    public override int GetHashCode() {
      return itsHashCode;
    }
    
    public override bool Equals(object other) {
        if (  this == other ) return true;

        if ( other is Resource ) {
          return ( GetHashCode() == other.GetHashCode()  );
        }
        return false;   
    } 
    
    public Bindings Unify(Atom other, Bindings bindings, ResourceMap map) {
      if ( null == bindings) {
        return null;
      }
      else if (Equals(other) ) {
        return bindings;
      }
      else if (other is Variable) {
        return (( Variable)other).Unify( this, bindings, map);
      }
      else {
        return null;
      }
    
    }
    
  }
}