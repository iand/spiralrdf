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

	/// <summary>
	/// Represents somthing that maps resources to graph nodes
	/// </summary>
  /// <remarks>
  /// $Id: ResourceMap.cs,v 1.2 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public interface ResourceMap {

    /// <returns>A resource denoted by the subject if the subject is known, otherwise a new resource</returns>
    Resource GetResourceDenotedBy(GraphMember theMember);

    IDictionary GetResourcesDenotedBy(ICollection nodeList );

    /// <returns>True if the map has a resource mapped to the supplied node</returns>
    bool HasResourceDenotedBy(GraphMember theMember);

    /// <returns>True if the map has a node mapped to the given resource</returns>
    bool HasNodeDenoting(Resource theResource);

    void AddDenotation(GraphMember member, Resource theResource);

    GraphMember GetBestDenotingNode(Resource theResource);
    
    IList GetNodesDenoting(Resource theResource);

    /// <summary>Counts the number of distinct nodes in the ResourceMap</summary>
    /// <value>The number of distinct nodes in the ResourceMap</value>
    int NodeCount{ get; }

    /// <summary>Gets a list of all the resources in the ResourceMap</summary>
    IList Resources { get; }

    /// <summary>Counts the number of distinct resources in the ResourceMap</summary>
    /// <value>The number of distinct resources in the ResourceMap</value>
    int ResourceCount{ get; }

  }
}