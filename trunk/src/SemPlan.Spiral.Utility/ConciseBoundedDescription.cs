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
  using System.Collections;
  using SemPlan.Spiral.Core;
    
  
	/// <summary>
	/// Represents a self-contained set of triples related to a single subject.
	/// </summary>
  /// <remarks> 
  /// $Id: ConciseBoundedDescription.cs,v 1.2 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class ConciseBoundedDescription : ResourceDescription {
    private MemoryTripleStore itsStore;
  
    private Resource itsResource;

    public ConciseBoundedDescription(Resource resource) {
      itsResource = resource;
      itsStore = new MemoryTripleStore();
    }

    public ConciseBoundedDescription(Node denotingNode) {
      itsStore = new MemoryTripleStore();
      itsResource = itsStore.GetResourceDenotedBy( denotingNode );
    }

    public virtual Resource Resource {
      get {
        return itsResource;
      }
    }


    public virtual IList DenotingNodes {
      get {
        return itsStore.GetNodesDenoting( itsResource );
      }
    }

    public bool Contains( GraphMember thePredicate, GraphMember theObject ) {

      if ( ! itsStore.HasResourceDenotedBy( theObject ) ) {
        return false;
      }

      Resource theObjectResource = itsStore.GetResourceDenotedBy( theObject );

      return Contains( thePredicate, theObjectResource );
      
    }

    public bool Contains( GraphMember thePredicate, Resource theObjectResource ) {
      if ( ! itsStore.HasResourceDenotedBy( thePredicate ) ) {
        return false;
      }

      Resource thePredicateResource = itsStore.GetResourceDenotedBy(  thePredicate );

      ResourceStatement testStatement = new ResourceStatement( itsResource, thePredicateResource, theObjectResource);

      return itsStore.Contains( testStatement );
      
    }
    
    public void Add( GraphMember thePredicate, GraphMember theObject ) {

      Resource theObjectResource;
      if ( itsStore.HasResourceDenotedBy( theObject ) ) {
        theObjectResource = itsStore.GetResourceDenotedBy( theObject );
      }
      else {
        theObjectResource = itsStore.MakeNewResourceForNode( theObject );
        itsStore.AddDenotation( theObject, theObjectResource );
      }

      Add( thePredicate, theObjectResource );
    }
    
    public void Add( GraphMember thePredicate, Resource theObjectResource ) {
      Resource thePredicateResource;
      if ( itsStore.HasResourceDenotedBy( thePredicate ) ) {
        thePredicateResource = itsStore.GetResourceDenotedBy( thePredicate );
      }
      else {
        thePredicateResource = itsStore.MakeNewResourceForNode( thePredicate );
        itsStore.AddDenotation( thePredicate, thePredicateResource );
      }

      ResourceStatement resourceStatement = new ResourceStatement( itsResource,  thePredicateResource, theObjectResource );
      itsStore.Add( resourceStatement );
    }

    public void Add(ResourceStatement statement) {
      itsStore.Add(statement);
    }

    public void Add(ConciseBoundedDescription description) {
      itsStore.Add(description.itsStore);
    }


    public void Add(Statement statement) {
      itsStore.Add(statement);
    }

    public bool Contains(Statement statement) {
      return itsStore.Contains(statement);
    }

    public bool Contains(ResourceStatement statement) {
      return itsStore.Contains(statement);
    }

    public void AddDenotation(GraphMember member, Resource theResource) {
      itsStore.AddDenotation(member, theResource);
    }
    public void Write(RdfWriter writer) {
      itsStore.Write(writer);
    }

    public bool IsEmpty() {
      return itsStore.IsEmpty();
    }

    public IList ReferencedResources { 
      get {
        return itsStore.ReferencedResources;
      }
    }

    public IEnumerator GetStatementEnumerator() {
      return itsStore.GetStatementEnumerator();
    }

    public IList GetNodesDenoting(Resource theResource) {
      return itsStore.GetNodesDenoting(theResource);
    }

    public GraphMember GetBestDenotingNode(Resource theResource) {
      return itsStore.GetBestDenotingNode(theResource);
    }
    
    public int NodeCount { 
      get {
        return itsStore.NodeCount;
      }
    }

    public int ResourceCount {
      get {
        return itsStore.ResourceCount;
      }
    }

    public int StatementCount { 
      get {
        return itsStore.StatementCount;
      }
    }
    

    public void Clear() {
      itsStore.Clear();
    }

    public void Remove(ResourceStatement resourceStatement) {
      itsStore.Remove(resourceStatement);
    }

    public ResourceMap ResourceMap{ 
      get { return itsStore; }
    }

  }


}
