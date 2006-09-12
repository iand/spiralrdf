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
  using System.Text;
	/// <summary>
	/// Represents a solution to a query
	/// </summary>
  /// <remarks>
  /// $Id: QuerySolution.cs,v 1.6 2006/02/13 23:42:49 ian Exp $
  ///</remarks>

  public class QuerySolution {
    private Hashtable itsBindings;
    private Hashtable itsResourceNodes;
    
    public QuerySolution() {
      itsBindings = new Hashtable();
      itsResourceNodes = new Hashtable();
    }
    
    ///<summary>Provides access to the solution results by bound variable name</summary>
    public Resource this[string variableName] {
      get {
        return (Resource)itsBindings[variableName];
      }
      
      set {
        itsBindings[variableName] = value;
      }
      
    }
    
    public Bindings Bindings {
      get { 
        Bindings bindings = new Bindings();
        IDictionaryEnumerator enumerator = itsBindings.GetEnumerator();
        
        while (enumerator.MoveNext()) {
          bindings.Bind( new Variable( (string)enumerator.Key ) , (Atom)enumerator.Value);
        }      
        return bindings; 
      }
    }
    

    ///<summary>Provides access to the best denoting node for the matching resource</summary>
    public GraphMember GetNode(string variableName) {
      return (GraphMember)itsResourceNodes[ itsBindings[variableName] ];
    }
      
    public void SetNode(string variableName, GraphMember member) {
      itsResourceNodes[ itsBindings[variableName] ] = member;
    }


    public int BindingCount {
      get {
        return itsBindings.Keys.Count;
      }
    }

    public bool IsBound(string variableName) {
      return itsBindings.Contains( variableName );
    }

    public override bool Equals(object other) {
      if (this == other) {
        return true;
      } 
      
      if (GetType().Equals( other.GetType() ) ) {
        QuerySolution otherSpecific = (QuerySolution)other;
        if (itsBindings.Count != otherSpecific.itsBindings.Count) {
          return false;
        }
        
        IDictionaryEnumerator enumerator = itsBindings.GetEnumerator();
        
        while (enumerator.MoveNext()) {
          if ( ! otherSpecific[(string)enumerator.Key].Equals( enumerator.Value ) ) {
            return false;
          }
        }
        
        return true;
      }

      return false;
    }

    public override int GetHashCode() {
      int hashcode = 12345678;

      IDictionaryEnumerator enumerator = itsBindings.GetEnumerator();
      
      while (enumerator.MoveNext()) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator.Key.GetHashCode();
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator.Value.GetHashCode();
      }
      
      return hashcode;
    }

    public override string ToString() {
      StringBuilder buffer = new StringBuilder("QuerySolution: ");
      if ( itsBindings.Count > 0) {
        IDictionaryEnumerator enumerator = itsBindings.GetEnumerator();
        
        while (enumerator.MoveNext()) {
          buffer.Append("[");
          buffer.Append(enumerator.Key.ToString());
          buffer.Append("=");
          buffer.Append(GetNode(enumerator.Key.ToString()));
          buffer.Append(" (");
          buffer.Append(enumerator.Value.ToString());
          buffer.Append(")] ");
        }
      }
      else {
        buffer.Append("[ no bindings ]");
      }      
      return buffer.ToString();
    }


  }

}