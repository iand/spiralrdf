#region Copyright (c) 2004 Ian Davis & James Carlyle
/*----------------------------------------------------------------------------------
Copyright © 2004 Ian Davis & James Carlyle

This software is provided 'as-is', without any express or implied warranty. In no 
event will the author(s) be held liable for any damages arising from the use of this 
software.
 
Permission is granted to anyone to use this software for any purpose, including 
commercial applications, and to alter it and redistribute it freely, subject to the 
following restrictions:

1. The origin of this software must not be misrepresented; you must not claim that 
   you wrote the original software. If you use this software in a product, an 
   acknowledgment (see the following) in the product documentation is required.

   Portions Copyright © 2004 Ian Davis & James Carlyle

2. Altered source versions must be plainly marked as such, and must not be 
   misrepresented as being the original software.

3. This notice may not be removed or altered from any source distribution.
----------------------------------------------------------------------------------*/
#endregion
using SemPlan.Spiral.Core;
using SemPlan.Spiral.Utility;
using SemPlan.Spiral.XsltParser;
using System;
using System.IO;
using System.Xml;


/// <summary>
/// Dumps out some RDF as RDF/XML
/// </summary>
/// <remarks>
/// $Id: RdfDumper.cs,v 1.3 2005/05/26 21:51:26 ian Exp $
///</remarks>
public class RdfDumper {
    
	static void Main(string[] args) {
	  ParserFactory parserFactory = new XsltParserFactory();
    
    SimpleModel model = new SimpleModel(parserFactory);
    
    string rdfUri = "http://iandavis.com/foaf.rdf";
    if (args.Length > 0) {
      rdfUri = args[0];
    }
    
    
    model.Parse( new Uri(rdfUri), rdfUri);

    XmlTextWriter xmlWriter = new XmlTextWriter( Console.Out );
    xmlWriter.Formatting = Formatting.Indented;
    
    RdfXmlWriter writer = new RdfXmlWriter(xmlWriter);
    model.Write( writer );    
    
	
	}



}
