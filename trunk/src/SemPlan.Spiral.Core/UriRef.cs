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
	/// Represents a URI reference used for identifying resources in RDF
	/// </summary>
  /// <remarks>
  /// $Id: UriRef.cs,v 1.5 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class UriRef : Node, Arc {
    private string itsUriRef;
    private int itsHashCode;
    
    /// <summary>Constructs a new UriRef instance from the given string representtion of a Uri reference.</summary>
    /// <remarks></remarks>
    public UriRef(string uriRef) {
        itsUriRef = String.Intern(uriRef);
        itsHashCode = itsUriRef.GetHashCode();
    }

    public virtual bool IsSelfDenoting() {
      return false;
    }
    
  	/// <summary>Returns the value of the label attached to the UriRef.</summary>
  	/// <returns>The label of the UriRef.</returns>
    /// <remarks>The label equals the character representation of the uriref</remarks>
    public virtual string GetLabel() {
      return itsUriRef;
    }       

    /// <summary>Determines whether two UriRef instances are equal.</summary>
  	/// <returns>True if the two UriRefs are equal, False otherwise</returns>
    /// <remarks>Two RDF URI references are equal if and only if they compare as equal, character by character, as Unicode strings.</remarks>
    public override bool Equals(object other) {
        if (other == null) return false;
    
        if (this.GetType() != other.GetType()) return false;
        if ( GetHashCode() != other.GetHashCode()) return false;

        UriRef specific = (UriRef)other;     
    
        return ( itsUriRef == specific.itsUriRef);
//        if (!itsUriRef.Equals(specific.itsUriRef)) return false;
    
//        return true;
    } 

    
    ///<summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
     public override int GetHashCode() {
       return itsHashCode;
     }
    
    /// <summary>Writes a representation of this UriRef to the supplied RdfWriter.</summary>
    public virtual void Write(RdfWriter writer) {
      writer.WriteUriRef(itsUriRef);
    }

    /// <summary>Returns a String that represents the current UriRef.</summary>
  	/// <returns>The UriRef's label enclosed in angle brackets.</returns>
    public override string ToString() {
      return String.Format("<{0}>", itsUriRef);
    }
  
    public virtual bool Matches(ResourceSpecifier specifier) {
      return specifier.MatchesUriRef( itsUriRef );
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
