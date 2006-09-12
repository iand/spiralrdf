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

namespace SemPlan.Spiral.Tests.Utility {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.Collections;
  using System.IO;
  using System.Xml;
  
	/// <summary>
	/// An instrumented version of KnowledgeBase that returns default values for methods
	/// </summary>
  /// <remarks>
  /// $Id: KnowledgeBaseStub.cs,v 1.2 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class KnowledgeBaseStub : KnowledgeBase {

    public KnowledgeBaseStub() : base(new ParserFactoryStub()) {

    }
    
    public override void SetDereferencer( Dereferencer dereferencer ) {
        // NOOP
    }
    
    public override void Add(Node theSubject, Arc thePredicate, Node theObject) {
        // NOOP
    }

    public override void Add(ResourceDescription description) {
        // NOOP
    }

    public override void Replace(ConciseBoundedDescription description) {
        // NOOP
    }

    public override void Clear() {
        // NOOP
    }

    public override bool IsEmpty() {
      return true;
    }

    //~ public override ResourceDescription GetDescriptionOf(Resource theResource) {
      //~ return new ConciseBoundedDescription( theResource );
    //~ }


		public override void Investigate(ConciseBoundedDescription resourceDescription, Investigator investigator) {
        // NOOP
    }
  
    public override void Think() {
        // NOOP
    }

		public override void Include(TextReader reader, string baseUri) {
        // NOOP
    }

		public override void Include(Uri uri) {
        // NOOP
    }
    
		public override void Include(Stream stream, string baseUri) {
        // NOOP
    }

		public override void Include(string uri) {
        // NOOP
    }


		public override void IncludeSchema(TextReader reader, string baseUri) {
        // NOOP
    }
    
		public override void IncludeSchema(Uri uri) {
        // NOOP
    }
    
		public override void IncludeSchema(Stream stream, string baseUri) {
        // NOOP
    }

		public override void IncludeSchema(string uri) {
        // NOOP
    }


    public override void Write(RdfWriter writer) {
        // NOOP
    }
  
    public override string ToString() {
      return "KnowledgeBaseStub";
    }
  
  }
}