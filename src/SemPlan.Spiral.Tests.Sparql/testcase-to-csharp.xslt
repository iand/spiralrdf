<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:d="http://www.w3.org/2001/sw/DataAccess/tests/test-manifest#" xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#" xmlns:t="http://www.w3.org/2001/sw/DataAccess/tests/test-query#">
	<xsl:output method="text" version="1.0" encoding="UTF-8" indent="no"/>
	
	<xsl:param name="group" select="'unsaid-inference'" />
	
	<xsl:template match="/">
		<xsl:for-each select="rdf:RDF/rdf:Description[d:name]">
		
			<xsl:variable name="nodeID" select="@rdf:nodeID" />
			<xsl:variable name="actionNodeID" select="/rdf:RDF/rdf:Description[@rdf:nodeID=$nodeID]/d:action/@rdf:nodeID" />

			<xsl:text>
    [Test]
    /// &lt;summary&gt;</xsl:text>
			<xsl:value-of select="$group"/>
			<xsl:text>/</xsl:text>
			<xsl:value-of select="d:name"/>
			<xsl:text>&lt;/summary&gt;</xsl:text>

			<xsl:if test="/rdf:RDF/rdf:Description[@rdf:nodeID=$nodeID]/rdfs:comment">
				<xsl:text>
    /// &lt;remarks&gt;</xsl:text>
				<xsl:value-of select="/rdf:RDF/rdf:Description[@rdf:nodeID=$nodeID]/rdfs:comment" />
				<xsl:text>&lt;/remarks&gt;</xsl:text>				
			</xsl:if>

			<xsl:text>
    public void </xsl:text>
			<xsl:value-of select="translate($group, '-', '_')"/>
			<xsl:text>_</xsl:text>
			<xsl:value-of select="translate(d:name, '-.', '__')"/>
			<xsl:text>() {
        TestQuery( "</xsl:text>
			<xsl:value-of select="$group"/>
			<xsl:text>\\</xsl:text>
			<xsl:value-of select="/rdf:RDF/rdf:Description[@rdf:nodeID=$actionNodeID]/t:query/@rdf:resource" />
			<xsl:text>", "</xsl:text>
			<xsl:value-of select="$group"/>
			<xsl:text>\\</xsl:text>
			<xsl:value-of select="/rdf:RDF/rdf:Description[@rdf:nodeID=$actionNodeID]/t:data/@rdf:resource" />
			<xsl:text>", "</xsl:text>
			<xsl:value-of select="$group"/>
			<xsl:text>\\</xsl:text>
			<xsl:value-of select="/rdf:RDF/rdf:Description[@rdf:nodeID=$nodeID]/d:result/@rdf:resource" />
			<xsl:text>" );
    }
</xsl:text>

		</xsl:for-each>
	</xsl:template>
	
	
</xsl:stylesheet>
