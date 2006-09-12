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
	/// Represents a set of triples
	/// </summary>
  /// <remarks>
  /// $Id: TripleStore.cs,v 1.4 2006/02/13 23:42:49 ian Exp $
  ///</remarks>
  public interface TripleStore : ResourceStatementCollection, ResourceMap, ICloneable, IDisposable {

    StatementHandler GetStatementHandler();
  
    IEnumerator Solve(Query query);
    void Evaluate(Rule rule);

    /// <summary>Write this TripleStore to the supplied RdfWriter.</summary>
    void Write(RdfWriter writer);

    /// <summary>Adds a statement of knowledge about a resource to the TripleStore</summary>
    /// <param name="statement">The statement to add to the TripleStore</param>
    void Add(Statement statement);
    bool Contains( Statement statement );
    

    void Add(TripleStore other);
    void Add(ResourceDescription description);
    void Merge(ResourceStatementCollection statements, ResourceMap map);

   /// <summary>Clears all triples, nodes and resources from this triple store</summary>
    void Clear();

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    ResourceDescription GetDescriptionOf(Resource theResource, BoundingStrategy strategy);

    /// <returns>A bounded description generated according to the specified strategy.</returns>
    ResourceDescription GetDescriptionOf(Node theNode, BoundingStrategy strategy);


  }
}