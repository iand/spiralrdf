<?xml version="1.0" encoding="UTF-8"?>
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

<stylesheet version="1.0" 
	xmlns="http://www.w3.org/1999/XSL/Transform"
	xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#">
	<output method="text" indent="no" encoding="utf-8"/>
	
<!--
  <template match="/rdf:RDF" priority="1">
    <variable name="nsEntity">
      <for-each select="namespace::*">
        <if test="contains(., '&amp;')">
          <value-of select="."/>
        </if>
      </for-each>
    </variable>
    <choose>
      <when test="$nsEntity != ''">
        <apply-templates select="." mode="serialise"/>
        <text># WARNING .Net framework will not resolve entities in root element namespace declarations.&#xA;</text>
      </when>
      <otherwise>
        <apply-templates select="node()|@*"/>
      </otherwise>
    </choose>
  </template>
  -->

  <variable name="illegal-name-punctuation">
		<text>,:;!?&#x9;&#xA;&#xD;&#xA0; &quot;'()[]&lt;>{}/</text>
	</variable>

	<template match="text()"/>
	
  <template match="node()|@*">
    <apply-templates select="node()[contains(namespace-uri(), '&amp;')]|@*[contains(namespace-uri(), '&amp;')]" mode="namespace"/>
    <apply-templates select="node()|@*"/>
  </template>

  <template match="node()|@*" mode="namespace">
		<apply-templates select="." mode="serialise"/>
		<text># WARNING .Net framework will not resolve entities in namespace declarations.&#xA;</text>
  </template>

	<template match="@rdf:li">
		<apply-templates select="parent::*" mode="serialise"/>
		<text># ERROR rdf-containers-syntax-vs-schema/error001.rdf : rdf:li is not allowed as as an attribute.&#xA;</text>
	</template>

	<template match="rdf:li">
		<apply-templates select="self::node()" mode="resource-element">
			<with-param name="test-name">#ERROR rdf-containers-syntax-vs-schema/error002.rdf : rdf:li elements as typed nodes - a bizarre case, see also rdfms-rdf-names-use/error008.rdf : li</with-param>
		</apply-templates>
	</template>

	<template match="@rdf:aboutEach">
		<apply-templates select="parent::*" mode="serialise"/>
		<text># ERROR rdfms-abouteach/error001.rdf : aboutEach removed from the RDF specifications.&#xA;</text>
	</template>

	<template match="@rdf:aboutEachPrefix">
		<apply-templates select="parent::*" mode="serialise"/>
		<text># ERROR rdfms-abouteach/error002.rdf : aboutEachPrefix removed from the RDF specifications.&#xA;</text>
	</template>

	<template match="@rdf:ID">
		<choose>
			<when test="ancestor::*[@xml:base]">
				<if test="ancestor::*[@xml:base]//@rdf:ID[. = current() and generate-id() != generate-id(current())]">
					<apply-templates select="parent::*" mode="serialise"/>
					<text># ERROR rdfms-difference-between-ID-and-about/error1.rdf : two elements cannot use the same ID within same xml:base.&#xA;</text>
				</if>
			</when>
			<otherwise>
				<if test="//@rdf:ID[. = current() and generate-id() != generate-id(current())][not(ancestor::*[@xml:base])]">
					<apply-templates select="parent::*" mode="serialise"/>
					<text># ERROR rdfms-difference-between-ID-and-about/error1.rdf : two elements cannot use the same ID.&#xA;</text>
				</if>
			</otherwise>
		</choose>
		<!-- rdfms-rdf-id/error005.rdf : Entities are legal within an XML Name, but not as the first character.
		This is impossible to detect using XSLT, since the XML parser will convert entities to BaseChar before the XSLT processor sees the document -->
		<if test="string-length(translate(substring(.,1,1),'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_','')) != 0">
			<apply-templates select="parent::*" mode="serialise"/>
			<text># ERROR rdfms-rdf-id/error005.rdf : Entities are legal within an XML Name, but not as the first character..&#xA;</text>
		</if>
		<call-template name="name-production">
			<with-param name="test-name"># ERROR rdfms-rdf-id/error001.rdf + 002 + 003 + 004 : The value of rdf:ID</with-param>
			<with-param name="name" select="."/>
			<with-param name="context-node" select="parent::*"/>
		</call-template>
	</template>

	<template match="*[@rdf:parseType[. = 'Literal'] and @rdf:resource]">
		<apply-templates select="parent::*" mode="serialise"/>
		<text># ERROR rdfms-empty-property-elements/error001.rdf and error002.rdf : specifying an rdf:parseType of "Literal" and an rdf:resource attribute at the same time is an error.&#xA;</text>
	</template>

	<template match="*[@rdf:parseType[. = 'Literal'] and (count(@*) - count(@rdf:*) - count(@xml:*)) &gt; 0]">
		<apply-templates select="parent::*" mode="serialise"/>
		<text># ERROR rdfms-empty-property-elements/error003.rdf : rdf:parseType="Literal" is forbidden here since we're creating an additional resource node.&#xA;</text>
	</template>

	<template match="@rdf:bagID">
		<call-template name="name-production">
			<with-param name="test-name"># ERROR rdfms-rdf-id/error006.rdf + 007  : The value of rdf:bagID</with-param>
			<with-param name="name" select="."/>
			<with-param name="context-node" select="parent::*"/>
		</call-template>
	</template>

	<template match="rdf:ID">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error002.rdf : ID is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error013.rdf : ID is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:about">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error003.rdf : about is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error014.rdf : about is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:bagID">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error004.rdf : bagID is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error015.rdf : bagID is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:parseType">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error005.rdf : parseType is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error016.rdf : parseType is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:resource">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error006.rdf : resource is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error017.rdf : resource is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:nodeID">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error007.rdf : nodeID is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error018.rdf : nodeID is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:aboutEach">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error009.rdf : aboutEach is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error019.rdf : aboutEach is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:aboutEachPrefix">
		<apply-templates select="." mode="serialise"/>
		<apply-templates select="." mode="forbidden-name">
			<with-param name="resource-test-name">rdfms-rdf-names-use/error010.rdf : aboutEachPrefix is forbidden as a node element name.</with-param>
			<with-param name="property-test-name">rdfms-rdf-names-use/error020.rdf : aboutEachPrefix is forbidden as a property element name.</with-param>
		</apply-templates>
	</template>

	<template match="rdf:RDF[ancestor::*]">
		<apply-templates select="." mode="serialise"/>
		<text># ERROR rdfms-rdf-names-use/error012.rdf : RDF is forbidden as a property element name.&#xA;</text>
	</template>

	<template match="rdf:Description">
		<apply-templates select="self::node()" mode="property-element">
			<with-param name="test-name"># ERROR rdfms-rdf-names-use/error011.rdf : Description</with-param>
		</apply-templates>
		<apply-templates select="node()|@*"/>
	</template>

	<template match="@rdf:nodeID">
		<call-template name="name-production">
			<with-param name="test-name"># ERROR rdfms-syntax-incomplete/error001.rdf + 002 + 003 : The value of rdf:nodeID</with-param>
			<with-param name="name" select="."/>
			<with-param name="context-node" select="parent::*"/>
		</call-template>
	</template>

	<template match="*[@rdf:nodeID and @rdf:ID]">
		<variable name="property-or-resource"><apply-templates select="self::node()" mode="property-or-resource"/></variable>
		<if test="$property-or-resource = 'resource'">
			<apply-templates select="." mode="serialise"/>
			<text># ERROR rdfms-syntax-incomplete/error004.rdf : Cannot have rdf:nodeID and rdf:ID.&#xA;</text>
		</if>
	</template>

	<template match="*[@rdf:nodeID and @rdf:about]">
		<apply-templates select="." mode="serialise"/>
		<text># ERROR rdfms-syntax-incomplete/error005.rdf : Cannot have rdf:nodeID and rdf:about.&#xA;</text>
	</template>

	<template match="*[@rdf:nodeID and @rdf:resource]">
		<apply-templates select="." mode="serialise"/>
		<text># ERROR rdfms-syntax-incomplete/error006.rdf : Cannot have rdf:nodeID and rdf:resource.&#xA;</text>
	</template>

	<template match="*" mode="forbidden-name">
		<param name="resource-test-name"/>
		<param name="property-test-name"/>
		<variable name="property-or-resource"><apply-templates select="." mode="property-or-resource"/></variable>
		<text># ERROR </text>
		<choose>
			<when test="$property-or-resource = 'resource'">
				<value-of select="$resource-test-name"/>
			</when>
			<otherwise>
				<value-of select="$property-test-name"/>
			</otherwise>
		</choose>
		<text>&#xA;</text>
	</template>

	<template name="name-production">
		<param name="test-name"/>
		<param name="name"/>
		<param name="context-node"/>
		<if test="string-length(translate($name,$illegal-name-punctuation,'')) != string-length(.)">
			<apply-templates select="$context-node" mode="serialise"/>
			<value-of select="concat($test-name, ' must match the XML Name production.&#xA;')"/>
		</if>
		<if test="string-length(translate(substring($name,1,1),'0123456789-.','')) != 1">
			<apply-templates select="$context-node" mode="serialise"/>
			<value-of select="concat($test-name, ' must match the XML Name production for first characters.&#xA;')"/>
		</if>
	</template>

	<template match="node()" mode="property-element">
		<param name="test-name"/>
		<variable name="property-or-resource"><apply-templates select="self::node()" mode="property-or-resource"/></variable>
		<if test="$property-or-resource = 'property'">
			<apply-templates select="." mode="serialise"/>
			<value-of select="concat($test-name, ' is forbidden as a property element name.&#xA;')"/>
		</if>
	</template>

	<template match="node()" mode="resource-element">
		<param name="test-name"/>
		<variable name="property-or-resource"><apply-templates select="self::node()" mode="property-or-resource"/></variable>
		<if test="$property-or-resource = 'resource'">
			<apply-templates select="." mode="serialise"/>
			<value-of select="concat($test-name, ' is forbidden as a node element name.&#xA;')"/>
		</if>
	</template>

	<template match="node()" mode="property-or-resource">
		<choose>
			<when test="self::node()[(count(ancestor::*[not(self::rdf:RDF)]) + count(ancestor::*[@rdf:parseType='Literal' or @rdf:parseType='Collection'])) mod 2 = 0][not(ancestor::*[@rdf:parseType='Literal'])]">
				<text>resource</text>
			</when>
			<otherwise>
				<text>property</text>
			</otherwise>
		</choose>
	</template>

	<template match="*" mode="serialise">
		<text/># ERROR &lt;<value-of select="name()"/>
		<for-each select="@*">
			<value-of select="concat(' ',name(),'=&quot;',.,'&quot;')"/>
		</for-each>
		<text>/>&#xA;</text>
	</template>

</stylesheet>
