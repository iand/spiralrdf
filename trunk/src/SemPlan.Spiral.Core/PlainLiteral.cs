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
	/// Represents an RDF plain literal node which is a string combined with an optional language tag.
	/// </summary>
  /// <remarks>
  /// $Id: PlainLiteral.cs,v 1.6 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public class PlainLiteral : Node { 
    private string itsLexicalValue;
    private string itsLanguage;
    private int itsHashCode;
    

  	/// <summary>Constructs a new PlainLiteralNode instance without any specific language tag using the supplied lexical value.</summary>
    /// <param name="lexicalValue">The literal's lexical value.</param>
    public PlainLiteral(string lexicalValue) {
      itsLexicalValue = lexicalValue;
      itsLanguage = null;
      itsHashCode = lexicalValue.GetHashCode();
    }


  	/// <summary>Constructs a new PlainLiteralNode instance using the supplied lexical value and language code.</summary>
    /// <param name="lexicalValue">The literal's lexical value.</param>
    /// <param name="language">The language tag for the literal. Valid values are specified in http://www.isi.edu/in-notes/rfc3066.txt and are normalized to lower case.</param>
    public PlainLiteral(string lexicalValue, string language) {
      itsLexicalValue = lexicalValue;
      itsLanguage = language;
      if ( null == itsLanguage) {
        itsHashCode = lexicalValue.GetHashCode();
      }
      else {
        itsHashCode = lexicalValue.GetHashCode() ^ language.GetHashCode();
      }
    }    
    
  	/// <summary>Returns the value of the label attached to the node.</summary>
  	/// <returns>A string representing the node's label.</returns>
    ///<remarks>A plain literal's label is equal to its lexical value</remarks>
    public string GetLabel() {
      return itsLexicalValue;
    }      

    public string GetLanguage() {
      return itsLanguage;
    }      

    public bool IsSelfDenoting() {
      return true;
    }

    /// <summary>Determines whether two PlainLiteralNode instances are equal.</summary>
  	/// <returns>True if the two PlainLiteralNodes are equal, False otherwise</returns>
    /// <remarks>Two plain literal nodes are equal if they represent the same node. Two instances may be equivalent but not represent the same node</remarks>
    public override bool Equals(object other) {
        if (other == null) return false;
    
        if (this.GetType() != other.GetType()) return false;
        if ( GetHashCode() != other.GetHashCode()) return false;
    
        PlainLiteral specific = (PlainLiteral)other;     
    
        if ( ! itsLexicalValue.Equals(specific.itsLexicalValue) ) return false;

        if ( itsLanguage == null && specific.itsLanguage != null ) return false;
    
        if ( itsLanguage != null ) {
           if ( specific.itsLanguage == null ) return false;

           if ( ! itsLanguage.Equals( specific.itsLanguage )) return false;
        }
    
        return true;
    }


    ///<summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
     public override int GetHashCode() {
        return itsHashCode;
     }

    /// <summary>Writes a representation of this PlainLiteral to the supplied RdfWriter.</summary>
    /// <remarks></remarks>
    public virtual void Write(RdfWriter writer) {
      if ( itsLanguage == null ) {
         writer.WritePlainLiteral(itsLexicalValue);  
      }
      else {
         writer.WritePlainLiteral(itsLexicalValue, itsLanguage);  
      }
    }

    /// <summary>Returns a String that represents this PlainLiteral.</summary>
  	/// <returns>The lexical value of the literal enclosed in quotes with any language code appended.</returns>
    /// <remarks></remarks>
    public override string ToString() {
      if ( itsLanguage == null ) {
        return itsLexicalValue;
      }
      else {
        return String.Format(@"""{0}""@{1}", itsLexicalValue, itsLanguage);
      }
    }

    public bool Matches(ResourceSpecifier specifier) {
      if ( itsLanguage == null ) {
        return specifier.MatchesPlainLiteral( itsLexicalValue );
      }
      else {
        return specifier.MatchesPlainLiteral( itsLexicalValue, itsLanguage );
      }
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