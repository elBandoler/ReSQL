namespace ReSQL.Data
{
    public enum ColumnType
    {
        //Internal ReSQL
        NONE,
        //Numeric
        TINYINT,
        SMALLINT,
        MEDIUMINT,
        INT,
        BIGINT,
        DECIMAL,
        FLOAT,
        DOUBLE,
        REAL,
        BIT,
        BOOLEAN,
        SERIAL,
        // Date&Time
        DATE,
        DATETIME,
        TIMESTAMP,
        TIME,
        YEAR,
        // String
        CHAR,
        VARCHAR,
        TINYTEXT, 
        TEXT,
        MEDIUMTEXT,
        LONGTEXT,
        BINARY,
        VARBINARY,
        TINYBLOB,
        BLOB,
        MEDIUMBLOB,
        LONGBLOB,
        ENUM,
        SET, 
        // Spatial
        GEOMETRY,
        POINT,
        LINESTRING,
        POLYGON,
        MULTIPOINT,
        MULTILINESTRING,
        MULTIPOLYGON,
        GEOMETRYCOLLECTION,
        //JSON
        JSON
    }
}
