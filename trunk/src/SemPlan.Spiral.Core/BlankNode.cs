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
	/// Represents an anonymous node in an RDF graph
	/// </summary>
  /// <remarks>
  /// $Id: BlankNode.cs,v 1.6 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class BlankNode : Node {
    private int itsHashCode;
    public BlankNode() {
      itsHashCode = Guid.NewGuid().GetHashCode();
    }

    public BlankNode(int hashCode) {
      itsHashCode = hashCode;
    }


  	/// <summary>Returns the value of the label attached to the node.</summary>
  	/// <returns>A string representing the node's label.</returns>
    ///<remarks></remarks>
    public virtual string GetLabel() {
      return String.Format(@"spiral{0}", GetHashCode());
    }      
  
    public virtual bool IsSelfDenoting() {
      return false;
    }
  
    /// <summary>Determines whether two BlankNode instances are equal.</summary>
  	/// <returns>True if the two BlankNodes are equal, False otherwise</returns>
    /// <remarks>Two BlankNodes are equal if and only if they have the same hash code.</remarks>
    public override bool Equals(object other) {
        if (  this == other ) return true;
        if ( other is BlankNode ) {
          return ( GetHashCode() == other.GetHashCode() );
        }
        return false;   
    } 

    ///<summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
     public override int GetHashCode() {
        return itsHashCode;
     }

    /// <summary>Writes a representation of this BlankNode to the supplied RdfWriter.</summary>
    /// <remarks></remarks>
    public virtual void Write(RdfWriter writer) {
      writer.WriteBlankNode( "spiral" + GetHashCode() );
    }

    /// <summary>Returns a String that represents this BlankNode.</summary>
  	/// <returns>The hasshcode of the blank node prefixed with _:carp.</returns>
    /// <remarks></remarks>
    public override string ToString() {
      return "_:" + GetLabel();
    }

    public virtual bool Matches(ResourceSpecifier specifier) {
      return specifier.MatchesBlankNode(  );
    }

    public virtual Bindings Unify(Atom other, Bindings bindings, ResourceMap map) {
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