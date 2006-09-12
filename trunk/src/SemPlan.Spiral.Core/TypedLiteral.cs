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
	/// Represents an RDF typed literal node which is a string combined with a datatype URI.
	/// </summary>
  /// <remarks>
  /// $Id: TypedLiteral.cs,v 1.6 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class TypedLiteral :  Node {
    private string itsLexicalValue;
    private string itsDataTypeUriRef;
    private int itsHashCode;
    
    public TypedLiteral(string lexicalValue, string dataTypeUriRef) {
      itsLexicalValue = lexicalValue;
      itsDataTypeUriRef = dataTypeUriRef;
      itsHashCode = itsLexicalValue.GetHashCode() ^ itsDataTypeUriRef.GetHashCode();
    }

  	/// <summary>Returns the value of the label attached to the node.</summary>
  	/// <returns>A string representing the node's label.</returns>
    ///<remarks></remarks>
    public string GetLabel() {
      return itsLexicalValue;
    }      

    public string GetDataType() {
      return itsDataTypeUriRef;
    }          

    public bool IsSelfDenoting() {
      return true;
    }

    /// <summary>Determines whether two TypedLiteral instances are equal.</summary>
  	/// <returns>True if the two TypedLiterals are equal, False otherwise</returns>
    /// <remarks>Two typed literal nodes are equal if their lexical values compare equal character by character and their datatype URI references are equal character by character..</remarks>
    public override bool Equals(object other) {
        if (other == null) return false;
    
        if (this.GetType() != other.GetType()) return false;
        
        if ( GetHashCode() != other.GetHashCode()) return false;
     
        TypedLiteral specific = (TypedLiteral)other;     
    
        if ( ! itsLexicalValue.Equals(specific.itsLexicalValue) ) return false;

        if ( ! itsDataTypeUriRef.Equals(specific.itsDataTypeUriRef) ) return false;
    
        return true;
    }    


    ///<summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
   public override int GetHashCode() {
      return itsHashCode;
   }

    /// <summary>Writes a representation of this TypedLiteral to the supplied RdfWriter.</summary>
    /// <remarks></remarks>
    public void Write(RdfWriter writer) {
      writer.WriteTypedLiteral(itsLexicalValue, itsDataTypeUriRef);
    }

    /// <summary>Returns a String that represents this TypedLiteral.</summary>
  	/// <returns>The lexical value of the literal enclosed in quotes with the datatype uri appended.</returns>
    /// <remarks></remarks>
    public override string ToString() {
      return String.Format(@"""{0}""^^<{1}>", itsLexicalValue, itsDataTypeUriRef);
    }

    public bool Matches(ResourceSpecifier specifier) {
      return specifier.MatchesTypedLiteral( itsLexicalValue, itsDataTypeUriRef );
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