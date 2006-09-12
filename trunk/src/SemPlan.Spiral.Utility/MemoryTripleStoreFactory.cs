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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
	/// <summary>
	/// Represents a factory for controlling the creation of MemoryTripleStore instances
	/// </summary>
  /// <remarks>
  /// $Id: MemoryTripleStoreFactory.cs,v 1.3 2006/01/24 13:28:11 ian Exp $
  ///</remarks>
  public class MemoryTripleStoreFactory : TripleStoreFactory {
    
    /// <summary>Make a new, empty instance of a MemoryTripleStore</summary>
    public TripleStore MakeTripleStore() {
      return new MemoryTripleStore();
    }

    public TripleStore GetTripleStore(string graphUri) {
      return new MemoryTripleStore();
    }

    
    public SemPlan.Spiral.Core.ResourceFactory MakeResourceFactory() {
      return new ResourceFactory();
    }

  }
}
