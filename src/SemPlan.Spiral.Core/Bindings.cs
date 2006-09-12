#region Copyright (c) 2006 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006 Ian Davis and James Carlyle

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
	/// Represents 
	/// </summary>
  /// <remarks>
  /// $Id: Bindings.cs,v 1.2 2006/02/16 15:25:24 ian Exp $
  ///</remarks>
  public class Bindings {
    private Hashtable itsResourcesIndexedByAtom;
    private Hashtable itsNodesIndexedByAtom;
    
    public Bindings() {
      itsResourcesIndexedByAtom = new Hashtable();
      itsNodesIndexedByAtom = new Hashtable();
    }
    
    public virtual IDictionaryEnumerator GetEnumerator() {
      return itsResourcesIndexedByAtom.GetEnumerator();
    }

    public virtual bool Contains(Atom atom) {
      return itsResourcesIndexedByAtom.Contains( atom );
    }    

    public virtual bool IsBound(Atom atom) {
      return itsResourcesIndexedByAtom.Contains( atom );
    }    
    
    public virtual Atom Get( Atom atom ) {
      return (Atom)itsResourcesIndexedByAtom[ atom ];
    }
  
    public virtual Node GetNode( Atom atom ) {
      return (Node)itsNodesIndexedByAtom[ atom ];
    }


    public virtual void Bind( Atom key, Atom value ) {
      itsResourcesIndexedByAtom[ key ] = value;
    }

    public virtual void BindNode( Atom key, GraphMember value ) {
      itsNodesIndexedByAtom[ key ] = value;
    }

    public virtual Bindings Clone() {
      Bindings clone = new Bindings();
      clone.itsResourcesIndexedByAtom = (Hashtable)itsResourcesIndexedByAtom.Clone();
      clone.itsNodesIndexedByAtom = (Hashtable)itsNodesIndexedByAtom.Clone();
      return clone;
    }


    public override bool Equals(object other) {
      if (this == other) {
        return true;
      } 
      
      if (GetType().Equals( other.GetType() ) ) {
        Bindings otherSpecific = (Bindings)other;
        if (itsResourcesIndexedByAtom.Count != otherSpecific.itsResourcesIndexedByAtom.Count) {
          return false;
        }
        
        if (itsNodesIndexedByAtom.Count != otherSpecific.itsNodesIndexedByAtom.Count) {
          return false;
        }

        IDictionaryEnumerator enumerator1 = itsResourcesIndexedByAtom.GetEnumerator();
        while (enumerator1.MoveNext()) {
          if ( ! otherSpecific.itsResourcesIndexedByAtom[enumerator1.Key].Equals( enumerator1.Value ) ) {
            return false;
          }
        }

        IDictionaryEnumerator enumerator2 = itsNodesIndexedByAtom.GetEnumerator();
        while (enumerator2.MoveNext()) {
          if ( ! otherSpecific.itsNodesIndexedByAtom[enumerator2.Key].Equals( enumerator2.Value ) ) {
            return false;
          }
        }
        
        return true;
      }

      return false;
    }

    public override int GetHashCode() {
      int hashcode = 12345678;

      IDictionaryEnumerator enumerator1 = itsResourcesIndexedByAtom.GetEnumerator();
      
      while (enumerator1.MoveNext()) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator1.Key.GetHashCode();
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator1.Value.GetHashCode();
      }
      

      IDictionaryEnumerator enumerator2 = itsNodesIndexedByAtom.GetEnumerator();
      
      while (enumerator2.MoveNext()) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator2.Key.GetHashCode();
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ enumerator2.Value.GetHashCode();
      }

      return hashcode;
    }

    public override string ToString() {
      StringBuilder buffer = new StringBuilder("Bindings: ");
      if ( itsResourcesIndexedByAtom.Count > 0) {
        IDictionaryEnumerator enumerator = itsResourcesIndexedByAtom.GetEnumerator();
        
        while (enumerator.MoveNext()) {
          buffer.Append("[");
          buffer.Append(enumerator.Key.ToString());
          buffer.Append("=");
          buffer.Append(GetNode((Atom)enumerator.Key));
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