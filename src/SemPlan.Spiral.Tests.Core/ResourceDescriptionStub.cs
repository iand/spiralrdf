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
	/// An instrumented version of ResourceDescription that does nothing and returns default values
	/// </summary>
  /// <remarks>
  /// $Id: ResourceDescriptionStub.cs,v 1.2 2006/02/13 23:44:11 ian Exp $
  ///</remarks>
  public class ResourceDescriptionStub : ResourceStatementCollectionStub, ResourceDescription {
    public Resource Resource { 
      get { return new Resource(); }
    }
    
    public IList DenotingNodes { 
      get { return new ArrayList(); }
    }


    public bool Contains( Statement statement ) {
      return false;
    }

    public bool Contains( GraphMember thePredicate, GraphMember theObject ) {
      return false;
    }
    
    public bool Contains( GraphMember thePredicate, Resource theObjectResource ) {
      return false;
    }

  }
}
