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

  public struct Schema {
    public struct rdf {
      public const string _nsprefix = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
      
      // Properties
      public static readonly UriRef first = new UriRef(_nsprefix + "first");
      /// Note naming of this field changed because of .NET limitations
      public static readonly UriRef object_ = new UriRef(_nsprefix + "object"); 
      public static readonly UriRef predicate = new UriRef(_nsprefix + "predicate");
      public static readonly UriRef rest = new UriRef(_nsprefix + "rest");
      public static readonly UriRef subject = new UriRef(_nsprefix + "subject");
      public static readonly UriRef type = new UriRef(_nsprefix + "type");
      public static readonly UriRef value = new UriRef(_nsprefix + "value");

      // Classes
      public static readonly UriRef Alt = new UriRef(_nsprefix + "Alt");
      public static readonly UriRef Bag = new UriRef(_nsprefix + "Bag");
      public static readonly UriRef List = new UriRef(_nsprefix + "List");
      public static readonly UriRef Object = new UriRef(_nsprefix + "Object");
      public static readonly UriRef Predicate = new UriRef(_nsprefix + "Predicate");
      public static readonly UriRef Property = new UriRef(_nsprefix + "Property");
      public static readonly UriRef Seq = new UriRef(_nsprefix + "Seq");
      public static readonly UriRef Statement = new UriRef(_nsprefix + "Statement");
      public static readonly UriRef Subject = new UriRef(_nsprefix + "Subject");
      public static readonly UriRef XMLLiteral = new UriRef(_nsprefix + "XMLLiteral");

      // Instances
      public static readonly UriRef nil = new UriRef(_nsprefix + "nil");

    }

    public struct rdfs {
      public const string _nsprefix = "http://www.w3.org/2000/01/rdf-schema#";

      // Properties
      public static readonly UriRef comment = new UriRef(_nsprefix + "comment"); 
      public static readonly UriRef domain = new UriRef(_nsprefix + "domain");        
      public static readonly UriRef isDefinedBy = new UriRef(_nsprefix + "isDefinedBy");  
      public static readonly UriRef label = new UriRef(_nsprefix + "label");        
      public static readonly UriRef member = new UriRef(_nsprefix + "member");   
      public static readonly UriRef range = new UriRef(_nsprefix + "range");        
      public static readonly UriRef seeAlso = new UriRef(_nsprefix + "seeAlso");   
      public static readonly UriRef subClassOf = new UriRef(_nsprefix + "subClassOf"); 
      public static readonly UriRef subPropertyOf = new UriRef(_nsprefix + "subPropertyOf"); 

      // Classes
      public static readonly UriRef Container = new UriRef(_nsprefix + "Container");   
      public static readonly UriRef ContainerMembershipProperty = new UriRef(_nsprefix + "ContainerMembershipProperty");   
      public static readonly UriRef Class = new UriRef(_nsprefix + "Class");        
      public static readonly UriRef Datatype = new UriRef(_nsprefix + "Datatype");        
      public static readonly UriRef Literal = new UriRef(_nsprefix + "Literal");        
      public static readonly UriRef Resource = new UriRef(_nsprefix + "Resource");        

    }


  }

}