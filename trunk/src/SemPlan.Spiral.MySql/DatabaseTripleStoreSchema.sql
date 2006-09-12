CREATE TABLE Languages (
  hash INT NOT NULL,
  value VARCHAR(255) NOT NULL,
  PRIMARY KEY(hash)
) TYPE = MYISAM;

CREATE TABLE UriRefs (
  hash INT NOT NULL,
  uri VARCHAR(255) NOT NULL,
  PRIMARY KEY(hash),
  UNIQUE INDEX UriRefs_uri(uri)
) TYPE = MYISAM;

CREATE TABLE Datatypes (
  hash INT NOT NULL,
  value VARCHAR(255) NOT NULL,
  PRIMARY KEY(hash)
) TYPE = MYISAM;

CREATE TABLE Graphs (
  graphId INT NOT NULL,
  description VARCHAR(255) NULL,
  PRIMARY KEY(graphId)
) TYPE = MYISAM;

CREATE TABLE TypedLiterals (
  hash INT NOT NULL,
  datatypeHash INT NOT NULL,
  value TEXT NOT NULL,
  PRIMARY KEY(hash) 
) TYPE = MYISAM;

CREATE TABLE Resources (
  graphId INT NOT NULL,
  resourceHash INT NOT NULL,
  PRIMARY KEY(graphId, resourceHash)
) TYPE = MYISAM;

CREATE TABLE PlainLiterals (
  hash INT NOT NULL,
  languageHash INT NULL,
  value TEXT NOT NULL,
  PRIMARY KEY(hash) 
) TYPE = MYISAM;

CREATE TABLE Statements (
  subjectHash INT NOT NULL,
  predicateHash INT NOT NULL,
  objectHash INT NOT NULL,
  graphId INT NOT NULL,
  PRIMARY KEY(subjectHash, predicateHash, objectHash, graphId) 
) TYPE = MYISAM;

CREATE TABLE ResourceNodes (
  graphId INT NOT NULL,
  resourceHash INT NOT NULL,
  nodeHash INT NOT NULL,
  nodeType CHAR(1) NOT NULL,
  PRIMARY KEY(graphId, resourceHash, nodeHash),
  INDEX ResourceNodes_resourceHash(resourceHash),
  INDEX ResourceNodes_nodeHash(nodeHash)
) TYPE = MYISAM;

