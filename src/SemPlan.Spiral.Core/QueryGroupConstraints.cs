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
	/// Represents some part of a query
	/// </summary>
  /// <remarks> 
  /// $Id: QueryGroupConstraints.cs,v 1.2 2006/02/16 15:25:24 ian Exp $
  ///</remarks>
  public class QueryGroupConstraints : QueryGroup {
    private ArrayList itsConstraints;
    public QueryGroupConstraints() {
      itsConstraints = new ArrayList();
    }

    public virtual void Add( Constraint  constraint ) {
      itsConstraints.Add( constraint );
    }

    public QueryGroup Resolve(ResourceMap map) {
      QueryGroupConstraints newGroup = new QueryGroupConstraints();
      foreach (Constraint constraint in itsConstraints) {
        newGroup.Add( constraint );
      }
      return newGroup;
    }


    public void Accept(QueryGroupVisitor visitor) {
      visitor.visit(this);
    }

    public virtual IList Constraints{ 
      get {
        ArrayList listCopy = new ArrayList();
        listCopy.AddRange( itsConstraints );
        return listCopy;
      }
    }

     public IList GetMentionedVariables() {
      return new ArrayList();
    }    


    public override bool Equals(object other) {
      if (null == other) return false;
      if (this == other) return true;
      if (! GetType().Equals(other.GetType() ) ) return false;
      
      IList otherConstraints = ((QueryGroupConstraints)other).itsConstraints;
      foreach (Constraint item in itsConstraints) {
        if (! otherConstraints.Contains( item ) ) {
          return false;
        }
      }
      
      return true;

    }

    public override int GetHashCode() {
      int hashcode = 9654321;
      
      foreach (Constraint item in itsConstraints) {
        hashcode = hashcode >> 1;
        hashcode = hashcode ^ item.GetHashCode();
      }

      return hashcode;
    }


  }
}