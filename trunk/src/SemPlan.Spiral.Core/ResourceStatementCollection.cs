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
	/// Represents a collection of resource statements
	/// </summary>
  /// <remarks> 
  /// $Id: ResourceStatementCollection.cs,v 1.1 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public interface ResourceStatementCollection {

    /// <summary>Gets a value indicating whether the StatementCollection is empty or not.</summary>
    /// <returns>true if the StatementCollection contains any statements, false otherwise</returns>
    bool IsEmpty();
    
    
    /// <summary>Adds a statement of knowledge about a resource to the StatementCollection</summary>
    /// <param name="statement">The resource statement to add to the StatementCollection</param>
    void Add(ResourceStatement statement);

    /// <returns>True if the StatementCollection contains the specified statement</returns>
    bool Contains(ResourceStatement resourceStatement);

    /// <summary>Gets an enumerator of all the ResourceStatements in the StatementCollection</summary>
    IEnumerator GetStatementEnumerator();

    /// <summary>Counts the number of distinct resource statements in the StatementCollection</summary>
    /// <value>The number of distinct resource statements in the StatementCollection</value>
    int StatementCount{ get; }

    void Remove(ResourceStatement resourceStatement);

    /// <summary>Gets a list of all the resources referenced by statements in the collection</summary>
    IList ReferencedResources { get; }

    ResourceMap ResourceMap{ get; }
  }


}
