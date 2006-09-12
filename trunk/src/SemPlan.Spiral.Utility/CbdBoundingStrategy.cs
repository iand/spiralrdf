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
namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;

	/// <summary>
	/// Represents an algorithm for generating concise bounded descriptions
	/// </summary>
  /// <remarks>
  /// $Id: CbdBoundingStrategy.cs,v 1.1 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class CbdBoundingStrategy : BoundingStrategy {
    // TODO: optimisations and recursion
    /// <returns>A concise bounded description containing all properties the KnowledgeBase knows about the subject if the subject is known, otherwise an empty description.</returns>
    public ResourceDescription GetDescriptionOf(Resource theResource, TripleStore store) {
      return GetDescriptionOf(  theResource, store, new Hashtable() );
    }

    private ResourceDescription GetDescriptionOf(Resource theResource, TripleStore store, Hashtable processedResources) {

      ConciseBoundedDescription cbd = new ConciseBoundedDescription( theResource );
      
      IList denotingNodes = store.GetNodesDenoting( theResource );
      processedResources[ theResource ] = denotingNodes;
      
      foreach (GraphMember member in  denotingNodes) {
        cbd.AddDenotation( member, theResource );
      }
      
      Query query = new Query();
      query.AddPattern( new Pattern( store.GetBestDenotingNode( theResource ), new Variable("pred"), new Variable("obj") ) );

      IEnumerator solutions = store.Solve( query );
      
      while ( solutions.MoveNext() ) {
        QuerySolution solution = (QuerySolution)solutions.Current;
        
        foreach (GraphMember member in store.GetNodesDenoting( solution["pred"] ) ) {
          cbd.AddDenotation( member, solution["pred"] );
        }
        
        foreach (GraphMember member in store.GetNodesDenoting( solution["obj"] ) ) {
          cbd.AddDenotation( member, solution["obj"] );
        }
    
        cbd.Add( new ResourceStatement( theResource, solution["pred"], solution["obj"] ) );
      
        if ( store.GetBestDenotingNode(  solution["obj"] ) is BlankNode ) {
          if ( ! processedResources.ContainsKey(  solution["obj"] ) ) {
            cbd.Add( (ConciseBoundedDescription)GetDescriptionOf(  solution["obj"], store, processedResources ) );
          }
        }
      
      } 
      
      return cbd;
    }

    public override bool Equals(object other) {
      if ( null == other ) return false;
      if ( this == other ) return true;
      
      return (GetType().Equals( other.GetType() ) );
    }

  }
}