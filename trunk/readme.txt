Semantic Planet Spiral - provides common services for RDF parsing and manipulation
(formerly Semantic Planet RDF Lib)

DEPENDENCIES
RDF Lib has no mandatory dependencies. The following DLLs are required for optional components:

* Drive.Rdf.dll if you plan to use the Drive RDF parser
* Semaview.Shared.ICalParser.dll if you plan to use the iCalendar parser
* MySql.Data.dll if you plan to use the mySQL triple store

BUILDING
We use NAnt to build RDFLib on both Mono 1.0 and MS.NET 1.1. The following targets might be useful:
  build - build all the assembiles
  test - build and test the assemblies
  dist - build the assemblies and distribution packages
  doc - build the documentation (uses NDoc)