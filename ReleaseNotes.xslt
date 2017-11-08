<?xml version="1.0" encoding="utf-8"?>
<!--
xmlns:xsl="http://www.w3.org/TR/WD-xsl"             IE       
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"    XSLT
xmlns:http://madanet.de/ReleaseNotes/
-->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://www.w3.org/1999/xhtml" 
    xmlns:rn="http://madanet.de/ReleaseNotes/"
    >

  <xsl:output  method="html" omit-xml-declaration="yes"   media-type="online" version="1.0" indent="yes"/>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="rn:Application" >
    <HTML>
      <title>
        ReleaseNotes <xsl:value-of select="@name"/>
      </title>
      <style type="text/css">
        .description
        {
        font-family:verdana
        font-size:9pt;
        margin-left:3em;
        }
        .heading
        {
        font-family:verdana
        font-size:10pt;
        margin-left:2em;
        }
        font-family:verdana
        H1.caption
        {
        font-family:verdana
        font-size:12pt;
        }
        font-family:verdana
        H2.caption
        {
        font-family:verdana
        font-size:11pt;
        }
        font-family:verdana
        H3.caption
        {
        font-family:verdana
        font-size:10pt;
        }
        .block
        {
        margin-top:1em;
        margin-left:1em;
        padding:0.5em;
        border:solid 1pt;
        }
        .comment
        {
        font-family:verdana
        font-size:9pt;
        font-style:italic;
        margin-left:3em;
        }
      </style>
      <BODY>
        <h1 class="caption">
          ReleaseNotes <b>
            <xsl:value-of select="@name"/>
          </b>
        </h1>
        Letzte Änderung: <xsl:value-of select="substring(@LastChanged, 11, 14)"/>
        <hr/>
        <div>
          <H2 class="caption">Offene Punkte</H2>
          <xsl:apply-templates select="rn:OpenIssues"/>
        </div>
        <hr/>
        <div>
          <H2 class="caption">durchgeführte Änderungen in den Versionen</H2>
          <xsl:apply-templates select="rn:Version"/>
        </div>
      </BODY>
    </HTML>
  </xsl:template>
  <xsl:template match="rn:Version">
      <div class="block">
        <H3 class="caption"><b>
          Version <xsl:value-of select="@VersionNumber"/>
        </b><br/>-<xsl:value-of select="@entwicklungsstand"/>-</H3>
        <xsl:if test="rn:Comment[.]">
          <div class="comment">
            <b>
              Kommentar:<br/>
            </b>
            <xsl:value-of select="rn:Comment"/>
          </div>
        </xsl:if>
        <xsl:apply-templates select="rn:Feature"/>
        <xsl:apply-templates select="rn:Bug"/>
      </div>
    </xsl:template>
  <xsl:template match="rn:OpenIssues">
    <div class="block">
      <xsl:apply-templates select="rn:Feature"/>
      <xsl:apply-templates select="rn:Bug"/>
    </div>
  </xsl:template>
   <xsl:template match="rn:Feature" >
    <p>
      <div class="heading">
      <em>
        Feature:
        <u>
          <xsl:value-of select="@name" disable-output-escaping="yes"/>
        </u>
      </em>
      </div>
      <p class="description">
        <xsl:value-of select="." disable-output-escaping="yes"/>
      </p>
    </p>
  </xsl:template>
  <xsl:template match="rn:Bug">
    <p>
      <div class="heading">
        <em>
          BUG:
          <u>
            <xsl:value-of select="@name"/>
          </u>
        </em>
      </div>
      <p class="description">
        <xsl:copy-of select="." />
      </p>
    </p>
  </xsl:template>
</xsl:stylesheet>
