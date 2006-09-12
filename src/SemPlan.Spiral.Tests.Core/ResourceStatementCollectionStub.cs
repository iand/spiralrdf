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
  using System.Collections;
  
	/// <summary>
	/// An instrumented version of StatementCollection that does nothing and returns default values
	/// </summary>
  /// <remarks> 
  /// $Id: ResourceStatementCollectionStub.cs,v 1.1 2006/02/13 23:44:11 ian Exp $
  ///</remarks>
  public class ResourceStatementCollectionStub : ResourceStatementCollection {

    /// <summary>Gets a value indicating whether the StatementCollection is empty or not.</summary>
    /// <returns>true if the StatementCollection contains any statements, false otherwise</returns>
    public bool IsEmpty() {
      return true;
    }
    
    public void Add(ResourceStatement statement) {
      // NOOP
    }

    /// <returns>True if the StatementCollection contains the specified statement</returns>
    public bool Contains(ResourceStatement resourceStatement) {
      return false;
    }

    public IEnumerator GetStatementEnumerator() {
      ArrayList list = new ArrayList();
      return list.GetEnumerator();
    }

    public int StatementCount{ 
      get { return 0; }
    }
      

    public void Remove(ResourceStatement resourceStatement) {
      // NOOP
    }

    public IList ReferencedResources { 
      get { return new ArrayList(); }
    }

    public ResourceMap ResourceMap{ 
      get { return null; }
    }
  }


}
