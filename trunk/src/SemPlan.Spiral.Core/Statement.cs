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
	/// <summary>
	/// Represents an RDF statement comprising a subject, a predicate and an object
	/// </summary>
  /// <remarks>
  /// $Id: Statement.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class Statement {
    private Node itsSubject;
    private Arc itsPredicate;
    private Node itsObject;


    public Statement(Node theSubject, Arc  thePredicate, Node theObject) {
      itsSubject = theSubject;
      itsPredicate = thePredicate;
      itsObject = theObject;
    }
  
    public Node GetSubject() {
      return itsSubject;  
    }

    public Arc GetPredicate() {
      return itsPredicate;  
    }

    public Node GetObject() {
      return itsObject;  
    }
    
    public override string ToString() {
      return GetSubject().GetLabel() + " " + GetPredicate().GetLabel() + " " + GetObject().GetLabel() + " .";
    }

    public override bool Equals(object other) {
      if (this == other) return true;
      
      if (other is Statement) {
        Statement otherSpecific = (Statement)other;
        
        return GetSubject().Equals(otherSpecific.GetSubject()) && GetPredicate().Equals(otherSpecific.GetPredicate()) && GetObject().Equals(otherSpecific.GetObject());
      }
      return false;
    }

    public virtual void Write(RdfWriter writer) {
      writer.StartSubject();
        itsSubject.Write(writer);
        writer.StartPredicate();
          itsPredicate.Write(writer);
          writer.StartObject();
            itsObject.Write(writer);
          writer.EndObject();  
        writer.EndPredicate();  
      writer.EndSubject();  
    }

    public override int GetHashCode() {
      return ToString().GetHashCode();
    }

  }
}