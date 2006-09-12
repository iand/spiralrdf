<?xml version="1.0"?>
<stylesheet version="1.0" exclude-result-prefixes="rdf sp"
	xmlns="http://www.w3.org/1999/XSL/Transform" 
	xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" 
	xmlns:sp="http://www.semanticplanet.com/xsl">
<!--
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
-->

<output method="text" indent="no" encoding="utf-8"/>
<strip-space elements="*"/>
<!-- if there's a parameter $base use it as the base URI in preference to the xml:base attribute -->
<param name="base">baseUriOfDocument</param>

<variable name="escapes" select="document('')/*/sp:escapes/sp:replace"/>
<variable name="literal-escapes" select="document('')/*/sp:literal-escapes/sp:replace"/>
<variable name="rdf-uri">http://www.w3.org/1999/02/22-rdf-syntax-ns#</variable>
<template match="rdf:RDF">
	<!-- start with the top subjects -->
	<apply-templates select="*|comment()" mode="subject"/>
</template>

<template match="*">
	<!-- rdf:RDF is no longer mandatory as the document root -->
	<apply-templates select="self::*" mode="subject"/>
</template>

<!-- #######################################################################
	Process subjects
####################################################################### -->
<template match="*" mode="subject">
	<!-- use the previous mode to see if this node has already been processed as a predicate -->
	<param name="previousMode"/>
	<!-- normally the subject String will be the same as the previous object, except for the top level subjects -->
	<param name="subjectString">
		<apply-templates select="." mode="resource-string"/>
	</param>
	<!-- write out a type for this subject if it isn't an untyped rdf Description and wasn't just a rdf resource attribute and wasn't a predicate abbreviated to a subject -->
	<if test="$previousMode != 'predicate'">
		<apply-templates select="self::*[not(self::rdf:Description[not(@rdf:type)])][not(@rdf:parseType = 'Resource')]" mode="type">
			<with-param name="subjectString" select="$subjectString"/>
		</apply-templates>
	</if>
	<!-- with this subject, call each non-rdf attribute predicate.  Need to process using pull model so that position() numbering is correct for reification -->
	<apply-templates select="@*[not(namespace-uri() = namespace-uri(parent::*/@xml:base) and local-name() = 'base')][not(namespace-uri() = namespace-uri(ancestor::rdf:*) and (local-name() = 'ID' or local-name() = 'nodeID' or local-name() = 'resource' or local-name() = 'about' or local-name() = 'parseType' or local-name() = 'type' or local-name() = 'bagID'))]" mode="predicate">
		<with-param name="subjectString" select="$subjectString"/>
	</apply-templates>
	<!-- need to call bagID separately so it is not included in position() count for attributes to be reified -->
	<apply-templates select="@rdf:bagID" mode="predicate"/>
	<!-- with this subject, call each element predicate -->
	<apply-templates select="*" mode="predicate">
		<with-param name="subjectString" select="$subjectString"/>
	</apply-templates>
</template>

<!-- #######################################################################
	Process predicates
####################################################################### -->
<template match="node()|@*" mode="predicate">
	<param name="subjectString"/>
	<variable name="predicateString">
		<apply-templates select="." mode="predicate-string"/>
	</variable>
	<variable name="objectString">
		<apply-templates select="." mode="object-string"/>
	</variable>
	<!-- must write out the subject once for each predicate, so write here and not at subject level -->
	<value-of select="$subjectString"/>
	<text> </text>
	<value-of select="$predicateString"/>
	<text> </text>
	<value-of select="$objectString"/>
	<text> .&#xA;</text>
	<!-- test to see whether to write reification triples here (if predicate was identified by rdf:ID attribute, or subject was identified by rdf:bagID) -->
	<if test="parent::*/@rdf:bagID | @rdf:ID">
		<!-- use call-template so as not to disturb position() of attributes from mode=subject call to predicate attibutes, needed to assign sequential pointers -->
		<call-template name="reifyTriple">
			<with-param name="subjectString" select="$subjectString"/>
			<with-param name="predicateString" select="$predicateString"/>
			<with-param name="objectString" select="$objectString"/>
			<with-param name="reificationType"><choose><when test="parent::*/@rdf:bagID">bag</when><otherwise>single</otherwise></choose></with-param>
		</call-template>
	</if>
	<!-- process first child of any collection -->
	<apply-templates select="*[parent::*/@rdf:parseType='Collection'][1]" mode="collection">
		<with-param name="subjectString" select="$subjectString"/>
		<with-param name="predicateString" select="$predicateString"/>
	</apply-templates>
	<!--- try and continue processing with this predicate node as a source of objects -->
	<apply-templates select="." mode="object">
		<with-param name="objectString" select="$objectString"/>
	</apply-templates>
</template>

<!-- don't process special attributes as predicates (@rdf:ID|@rdf:nodeID|@rdf:resource|@rdf:about|@rdf:parseType now unnecessary by explicit exclusion in apply-templates -->
<template match="@xml:lang" mode="predicate"/>

<!-- #######################################################################
	Process objects
####################################################################### -->
<!-- For child XML nodes with no children, continue processing node as a subject -->
<template match="*[not(node())]" mode="object">
	<param name="objectString"/>
	<apply-templates select="." mode="subject">
		<with-param name="subjectString" select="$objectString"/>
		<with-param name="previousMode" select="'predicate'"/>
	</apply-templates>
</template>

<!-- For child XML nodes with a parseType attribute, continue processing node as a subject -->
<template match="*[@rdf:parseType]" mode="object">
	<param name="objectString"/>
	<!-- process any non literal nodes -->
	<apply-templates select="self::*[not(@rdf:parseType = 'Literal')]" mode="subject">
		<with-param name="subjectString" select="$objectString"/>
		<with-param name="previousMode" select="'predicate'"/>
	</apply-templates>
</template>

<!-- For child XML nodes with a parseType=Collection, continue processing nodes as subjects -->
<template match="*[@rdf:parseType='Collection']" mode="object">
	<param name="objectString"/>
	<!-- process contained elements of the collection -->
	<apply-templates select="*" mode="subject">
		<!-- cannot pass object string as subject because collections can have multiple objects -->
	</apply-templates>
</template>

<!-- For the regular case, continue processing children nodes as subjects -->
<template match="*" mode="object">
	<param name="objectString"/>
	<!-- process child elements unless they are xml literal elements -->
	<apply-templates select="*" mode="subject">
		<with-param name="subjectString" select="$objectString"/>
	</apply-templates>
</template>

<template match="@*" mode="object"/>

<!-- #######################################################################
	Generate strings for objects 
####################################################################### -->
<!-- In the case where this XML node has already been processed as a predicate, select 
this node but specify that node has been processed as a predicate so any rdf:ID attribute 
is not used to identify the subject.  slightly above normal priority-->
<template match="*[@rdf:parseType or not(node()[not(self::processing-instruction() or self::comment())])]" mode="object-string" priority="0.5">
	<apply-templates select="." mode="resource-string">
		<with-param name="previousMode" select="'predicate'"/>
	</apply-templates>
</template>

<!-- If the only attribute is a rdf:ID and there is no text node then this is an empty literal 
(above normal priority than no node() match above) -->
<template match="*[not(node())][not((@rdf:ID and count(@*) > 1) or (not(@rdf:ID) and @*))]" mode="object-string" priority="1">
	<text>&quot;&quot;</text>
</template>

<!-- For attributes, select the attribute itself -->
<template match="@*" mode="object-string">
	<apply-templates select="." mode="resource-string"/>
</template>

<!-- For child nodes select the child node, as long as the child node is not just a processing instruction or comment.
there should be an attribute to use as the object instead -->
<template match="node()" mode="object-string">
	<apply-templates select="node()" mode="resource-string"/>
</template>

<!-- For collections, generate an ID for the first element of the collection -->
<template match="*[@rdf:parseType='Collection']" mode="object-string">
	<value-of select="concat('_:',translate(generate-id(*[1]),':',''))"/>
</template>

<!-- #######################################################################
	Generate strings for predicates
####################################################################### -->
<!-- For list elements, ID comes from the index number -->
<template match="rdf:li" mode="predicate-string">
	<text>&lt;</text><apply-templates select="." mode="rdf-namespace-uri"/><value-of select="concat('_',count(preceding-sibling::rdf:li)+1)"/><text>&gt;</text>
</template>

<!-- For the predicate, the ID comes from the element or attribute name -->
<template match="*|@*" mode="predicate-string">
	<text>&lt;</text><value-of select="concat(namespace-uri(),local-name())"/><text>&gt;</text>
</template>

<!-- #######################################################################
	Generate strings for resources
####################################################################### -->
<!-- Unspecified node: use the XSLT unique identifier for the node in the document -->
<template match="*" mode="resource-string">
	<text>_:</text><value-of select="translate(generate-id(),':','')"/>
</template>

<!-- String literal objects -->
<template match="text()" mode="resource-string">
	<text>&quot;</text>
	<call-template name="replace-nodes">
		<with-param name="string" select="self::text()"/>
		<with-param name="nodes" select="$literal-escapes"/>
	</call-template>
	<text>&quot;</text>
	<!-- Output lang and datatype qualifiers if appropriate -->
	<choose>
		<when test="parent::*/@rdf:datatype"><value-of select="concat('^^&lt;',parent::*/@rdf:datatype,'&gt;')"/></when>
		<when test="ancestor-or-self::*/@xml:lang"><value-of select="concat('@',ancestor-or-self::*[@xml:lang][1]/@xml:lang)"/></when>
	</choose>
</template>

<!-- String literal objects -->
<template match="@*" mode="resource-string">
	<text>&quot;</text><value-of select="."/><text>&quot;</text>
	<if test="ancestor-or-self::*/@xml:lang"><value-of select="concat('@',ancestor-or-self::*[@xml:lang][1]/@xml:lang)"/></if>
</template>

<!-- Blank node resource -->
<template match="*[@rdf:nodeID]" mode="resource-string">
	<text>_:</text><value-of select="@rdf:nodeID"/>
</template>

<!-- Node identifier -->
<template match="*[@rdf:ID]" mode="resource-string">
	<param name="previousMode"/>
	<!-- If the XML node was previously processed as a predicate, any rdf:ID attribute refers 
	to reification, not an identifier -->
	<choose>
		<when test="$previousMode = 'predicate'">
			<choose>
				<when test="@rdf:nodeID">
					<text>_:</text><value-of select="@rdf:nodeID"/>
				</when>
				<otherwise>
					<text>_:</text><value-of select="translate(generate-id(),':','')"/>
				</otherwise>
			</choose>
		</when>
		<otherwise>
			<text>&lt;</text><apply-templates select="." mode="base"/><text>#</text><value-of select="@rdf:ID"/><text>&gt;</text>
		</otherwise>
	</choose>
</template>

<!-- Resource identifier -->
<template match="*[@rdf:resource]" mode="resource-string">
	<text>&lt;</text><apply-templates select="@rdf:resource" mode="fullUri"/><text>&gt;</text>
</template>

<!-- Resource identified outside document -->
<template match="*[@rdf:about]" mode="resource-string">
	<text>&lt;</text><apply-templates select="@rdf:about" mode="fullUri"/><text>&gt;</text>
</template>

<!-- Literal XML resource -->
<template match="*[@rdf:parseType = 'Literal']" mode="resource-string">
	<text>&quot;</text><apply-templates select="node()" mode="serialise"/><text>&quot;^^&lt;</text><apply-templates select="." mode="rdf-namespace-uri"/><text>XMLLiteral&gt;</text>
</template>

<!-- don't process xml:lang attributes as resource -->
<template match="@xml:lang|@rdf:parseType|@rdf:datatype" mode="resource-string"/>

<!-- #######################################################################
	Reification templates
####################################################################### -->
<!-- State that the bag reification has a type of Bag -->
<template match="@rdf:bagID" mode="predicate">
	<text>&lt;</text>
	<apply-templates select="." mode="base"/><text>#</text><value-of select="."/><text>&gt; </text>
	<text>&lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>type&gt; &lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>Bag&gt; .&#xA;</text>
</template>

<!-- Generalised template for reification -->
<template name="reifyTriple">
	<param name="subjectString"/>
	<param name="predicateString"/>
	<param name="objectString"/>
	<param name="reificationType"/>
	<!-- Generate a value for the triple ID -->
	<variable name="tripleID">
		<choose>
			<when test="@rdf:ID">
				<text>&lt;</text><apply-templates select="." mode="base"/><text>#</text><value-of select="@rdf:ID"/><text>&gt;</text>
			</when>
			<otherwise>
				<text>_:t</text><value-of select="translate(generate-id(),':','')"/>
			</otherwise>
		</choose>
	</variable>
	<!-- state if this triple belongs to a bag -->
	<if test="$reificationType='bag'">
		<!-- now write out a triple linking the bag with the triple -->
		<text>&lt;</text>
		<apply-templates select="." mode="base"/><text>#</text><value-of select="ancestor-or-self::*[@rdf:bagID][1]/@rdf:bagID"/>
		<text>&gt; &lt;</text>
		<apply-templates select="." mode="rdf-namespace-uri"/>
		<!-- The index number calculation will depend on whether the statement being reified comes from an attribute or element -->
		<choose>
			<when test="self::*">
				<value-of select="concat('_', count(preceding-sibling::*[not(self::rdf:*)]) + count(parent::*/@*[namespace-uri() != namespace-uri(ancestor::rdf:*)]) + count(parent::*[not(self::rdf:*)]) + 1)"/>
			</when>
			<otherwise>
				<value-of select="concat('_', position() + count(parent::*[not(self::rdf:*)][child::*]))"/>
			</otherwise>
		</choose>
		<text>&gt; </text>
		<value-of select="$tripleID"/><text> .&#xA;</text>
	</if>
	<!-- State that the reification triple is of type statement -->
	<value-of select="$tripleID"/>
	<text> &lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>type&gt; &lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>Statement&gt; .&#xA;</text>
	<!-- Now write out the components of the reification: first, subject -->
	<value-of select="$tripleID"/>
	<text> &lt;</text><apply-templates select="." mode="rdf-namespace-uri"/>subject&gt; <value-of select="$subjectString"/><text> .&#xA;</text>
	<!-- predicate -->
	<value-of select="$tripleID"/>
	<text> &lt;</text><apply-templates select="." mode="rdf-namespace-uri"/><text>predicate&gt; </text>
	<value-of select="$predicateString"/><text> .&#xA;</text>
	<!-- object -->
	<value-of select="$tripleID"/>
	<text> &lt;</text><apply-templates select="." mode="rdf-namespace-uri"/>object&gt; <value-of select="$objectString"/><text> .&#xA;</text>
</template>

<!-- ####################################################################### 
	Collection templates
####################################################################### -->
<template match="*" mode="collection">
	<param name="subjectString"/>
	<param name="predicateString"/>
	<!-- Generate ID of first element of the collection-->
	<variable name="objectString" select="concat('_:',translate(generate-id(.),':',''))"/>
	<!-- Point to the first -->
	<value-of select="$objectString"/>
	<text> &lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>first&gt; </text>
	<apply-templates select="." mode="resource-string"/>
	<text> .&#xA;</text>
	<!-- Point to the rest -->
	<value-of select="$objectString"/>
	<text> &lt;</text>
	<apply-templates select="." mode="rdf-namespace-uri"/>
	<text>rest&gt; </text>
	<!-- Allow for a null termination -->
	<choose>
		<when test="following-sibling::*">
			<text>_:</text><value-of select="translate(generate-id(following-sibling::*[1]),':','')"/>
		</when>
		<otherwise>
			<text>&lt;</text><apply-templates select="." mode="rdf-namespace-uri"/>
			<text>nil&gt;</text>
		</otherwise>
	</choose>
	<text> .&#xA;</text>
	<!-- Process any subsequent members -->
	<if test="following-sibling::*">
		<apply-templates select="following-sibling::*[1]" mode="collection">
			<with-param name="subjectString" select="$subjectString"/>
			<with-param name="predicateString" select="$predicateString"/>
		</apply-templates>
	</if>
</template>

<!-- ####################################################################### 
	Miscellaneous templates
####################################################################### -->
<!-- Write out a statement detailing the type of a resource -->
<template match="*" mode="type">
	<param name="subjectString"/>
	<variable name="predicateString">
		<text>&lt;</text><apply-templates select="." mode="rdf-namespace-uri"/><text>type&gt;</text>
	</variable>
	<variable name="objectString">
		<text>&lt;</text>
		<choose>
			<when test="@rdf:type">
				<value-of select="@rdf:type"/>
			</when>
			<otherwise>
				<value-of select="concat(namespace-uri(.),local-name(.))"/>
			</otherwise>
		</choose>
		<text>&gt;</text>
	</variable>
	<value-of select="$subjectString"/>
	<text> </text>
	<value-of select="$predicateString"/>
	<text> </text>
	<value-of select="$objectString"/>
	<text> .&#xA;</text>
	<!-- Test to see whether to write reification triples here (if predicate was identified by rdf:ID attribute, or subject was identified by rdf:bagID) -->
	<if test="@rdf:bagID">
		<!-- Use call-template so as not to disturb position() of attributes, needed to assign sequential pointers -->
		<call-template name="reifyTriple">
			<with-param name="subjectString" select="$subjectString"/>
			<with-param name="predicateString" select="$predicateString"/>
			<with-param name="objectString" select="$objectString"/>
			<with-param name="reificationType" select="'bag'"/>
		</call-template>
	</if>
</template>

<!-- Template that outputs the correct uri for the node, regardless of whether the uri is absolute or relative or a fragment identifier -->
<template match="@rdf:about|@rdf:resource|@rdf:ID|@rdf:datatype" mode="fullUri">
	<choose>
		<when test="starts-with(., '#')">
			<apply-templates select="." mode="base"/>
			<value-of select="."/>
		</when>
		<when test=". = ''">
			<apply-templates select="." mode="base"/>
		</when>
		<when test="not(contains(., ':'))">
			<!-- no scheme, so use the base uri -->
			<variable name="baseUri"><apply-templates select="." mode="base"/></variable>
			<variable name="baseUriDomainPath" select="substring-after($baseUri,'.')"/>
			<variable name="baseUriPath" select="substring-after($baseUriDomainPath,'/')"/>
			<choose>
				<when test="starts-with(., '//')">
					<!-- absolute uri, so add the base scheme only -->
					<value-of select="concat(substring-before($baseUri,':'), ':', .)"/>
				</when>
				<when test="starts-with(., '/')">
					<!-- absolute uri, so add the base scheme and domain 
					take the first . and then the first / after that -->
					<value-of select="substring($baseUri, 1, string-length($baseUri) - (string-length($baseUriPath) + 1))"/>				
					<value-of select="."/>
				</when>
				<otherwise>
					<!-- relative uri, so add the base scheme and domain and relative path -->
					<choose>
						<when test="$baseUriPath = ''">
							<!-- no path in base, so simply concat base and relative -->
							<value-of select="concat($baseUri, '/', .)"/>
						</when>
						<otherwise>
							<!-- path in base, so calculate relative uri and base -->	
							<call-template name="remove-uri-parent-specifiers">
								<with-param name="relativeUri" select="."/>
								<with-param name="baseUri" select="$baseUri"/>		
							</call-template>
						</otherwise>
					</choose>
				</otherwise>
			</choose>
		</when>
		<otherwise>
			<value-of select="."/>
		</otherwise>
	</choose>
</template>

<!-- Template that generates the correct xml:base string, regardless of where in the document it is defined -->
<template match="node()[ancestor-or-self::*/@xml:base]|@*[ancestor::*/@xml:base]" mode="base">
	<call-template name="uri-before-fragment">
		<with-param name="uri" select="string(ancestor-or-self::*[@xml:base][1]/@xml:base)"/>	
	</call-template>
</template>

<!-- Use the supplied base parameter if none is found -->
<template match="node()|@*" mode="base">
	<call-template name="uri-before-fragment">
		<with-param name="uri" select="$base"/>	
	</call-template>
</template>

<!-- Template to generate the correct string for the nearest declaration of the RDF uri -->
<template match="node()|@*" mode="rdf-namespace-uri">
	<choose>
		<when test="namespace-uri(ancestor::rdf:*)"><value-of select="namespace-uri(ancestor::rdf:*)"/></when>
		<otherwise><value-of select="$rdf-uri"/></otherwise>
	</choose>
</template>

<!-- Template to write out comments -->
<template match="comment()" mode="subject">
	<text>#</text>
	<call-template name="replace-nodes">
		<with-param name="string" select="."/>
		<with-param name="nodes" select="$literal-escapes"/>
	</call-template>
	<text>&#xA;</text>
</template>

<!-- Series of templates to serialise XML literal content (needed because with output mode of text, 
any copy-of will write the text content of the XML and not the XML itself) -->
<template match="*" mode="serialise">
	<text/>&lt;<value-of select="name()"/>
	<!-- Need more work here to restrict namespace output to only what is needed -->
	<for-each select="namespace::*[name() != 'xml']">
		<if test="not(../../namespace::*[name() = name(current()) and . = current()])">
			<text> xmlns</text>
			<if test="name()">
				<text />:<value-of select="name()" />
			</if>
			<value-of select="concat('=\&quot;', ., '\&quot;')" />
		</if>
	</for-each>
	<for-each select="@*">
		<value-of select="concat(' ',name(),'=\&quot;')"/>
		<call-template name="replace-nodes">
			<with-param name="string" select="."/>
			<with-param name="nodes" select="$escapes"/>
		</call-template>
		<text>\&quot;</text>
	</for-each>
	<choose>
		<when test="node()">
			<text>></text>
			<apply-templates select="node()" mode="serialise"/>
			<text/>&lt;/<value-of select="name()"/><text>></text>
		</when>
		<otherwise>
			<text>/></text>
		</otherwise>
	</choose>
</template>

<!-- Text nodes can be serialised as is, except that special characters are escaped -->
<template match="text()" mode="serialise">
	<call-template name="replace-nodes">
		<with-param name="string" select="."/>
		<with-param name="nodes" select="$escapes"/>
	</call-template>
</template>

<!-- Template that returns a substring up to but not including the last incidence of a specified string -->
<template name="substring-before-last"> 
	<param name="input" />
	<param name="substr" />
	<if test="$substr and contains($input, $substr)">
		<variable name="temp" select="substring-after($input, $substr)" />
		<value-of select="substring-before($input, $substr)" />
			<if test="contains($temp, $substr)">
			<value-of select="$substr" />
			<call-template name="substring-before-last">
				<with-param name="input" select="$temp" />
				<with-param name="substr" select="$substr" />
			</call-template>
		</if>
	</if>
</template>
  
<!-- Template that combines a relative and base uri by removing any parent relative specifiers -->
<template name="remove-uri-parent-specifiers"> 
	<param name="relativeUri" />
	<param name="baseUri" />
	<choose>
		<when test="starts-with($relativeUri, '../')">
			<call-template name="remove-uri-parent-specifiers">
				<with-param name="relativeUri" select="substring-after($relativeUri, '../')"/>
				<with-param name="baseUri">
					<call-template name="substring-before-last">
						<with-param name="input" select="$baseUri" />
						<with-param name="substr" select="'/'" />
					</call-template>
				</with-param>
			</call-template>
		</when>
		<otherwise>
			<call-template name="substring-before-last">
				<with-param name="input" select="$baseUri" />
				<with-param name="substr" select="'/'" />
			</call-template>
			<text>/</text>
			<value-of select="$relativeUri"/>
		</otherwise>
	</choose>
</template>

<!-- Template that returns the uri supplied with any optional fragment identifier removed -->
<template name="uri-before-fragment"> 
	<param name="uri" />
	<choose>
		<when test="contains($uri, '#')">
			<value-of select="substring-before($uri, '#')"/>
		</when>
		<otherwise>
			<value-of select="$uri"/>
		</otherwise>
	</choose>
</template>
  
<!-- Template that replaces characters in strings, using escapes declaration -->
<template name="replace-nodes">
	<param name="string" select="string()"/>
	<param name="nodes"/>
	<variable name="first" select="$nodes[1]"/>
	<variable name="rest" select="$nodes[position()>1]"/>
	<choose>
		<when test="$first and $string">
			<choose>
				<when test="contains($string, $first/sp:from)">
					<call-template name="replace-nodes">
						<with-param name="string" select="substring-before($string, $first/sp:from)"/>
						<with-param name="nodes" select="$rest"/>
					</call-template>
					<copy-of select="$first/sp:to"/>
					<if test="$string != substring-after($string, $first/sp:from)">
						<call-template name="replace-nodes">
							<with-param name="string" select="substring-after($string, $first/sp:from)"/>
							<with-param name="nodes" select="$nodes"/>
						</call-template>
					</if>
				</when>
				<otherwise>
					<call-template name="replace-nodes">
						<with-param name="string" select="$string"/>
						<with-param name="nodes" select="$rest"/>
					</call-template>
				</otherwise>
			</choose>
		</when>
		<otherwise>
			<value-of select="$string"/>
		</otherwise>
	</choose>
</template>

<!-- Escapes declaration -->
<sp:escapes>
	<sp:replace><sp:from>&lt;</sp:from><sp:to>&amp;lt;</sp:to></sp:replace>
	<sp:replace><sp:from>&gt;</sp:from><sp:to>&amp;gt;</sp:to></sp:replace>
	<sp:replace><sp:from>&quot;</sp:from><sp:to>&amp;quot;</sp:to></sp:replace>
	<sp:replace><sp:from>&amp;</sp:from><sp:to>&amp;amp;</sp:to></sp:replace>
</sp:escapes>
<sp:literal-escapes>
	<sp:replace><sp:from>&#xD;</sp:from><sp:to>\r</sp:to></sp:replace>
	<sp:replace><sp:from>&#xA;</sp:from><sp:to>\n</sp:to></sp:replace>
</sp:literal-escapes>
</stylesheet>


