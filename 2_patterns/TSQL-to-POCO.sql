----------------------------------------------------------------------      TSQL-to-POCO      ----------------------------------------------------------------------
--https://stackoverflow.com/a/19123064

SQL SERVER version ()

declare @TableName sysname = 'TableName'
declare @result varchar(max) = 'public class ' + @TableName + '
{'

select @result = @result + '
    public ' + ColumnType + ' ' + ColumnName + ' { get; set; }
'
from
(
    select 
        replace(col.name, ' ', '_') ColumnName,
        column_id,
        case typ.name 
            when 'bigint' then 'long'
            when 'binary' then 'byte[]'
            when 'bit' then 'bool'
            when 'char' then 'string'
            when 'date' then 'DateTime'
            when 'datetime' then 'DateTime'
            when 'datetime2' then 'DateTime'
            when 'datetimeoffset' then 'DateTimeOffset'
            when 'decimal' then 'decimal'
            when 'float' then 'float'
            when 'image' then 'byte[]'
            when 'int' then 'int'
            when 'money' then 'decimal'
            when 'nchar' then 'char'
            when 'ntext' then 'string'
            when 'numeric' then 'decimal'
            when 'nvarchar' then 'string'
            when 'real' then 'double'
            when 'smalldatetime' then 'DateTime'
            when 'smallint' then 'short'
            when 'smallmoney' then 'decimal'
            when 'text' then 'string'
            when 'time' then 'TimeSpan'
            when 'timestamp' then 'DateTime'
            when 'tinyint' then 'byte'
            when 'uniqueidentifier' then 'Guid'
            when 'varbinary' then 'byte[]'
            when 'varchar' then 'string'
            else 'UNKNOWN_' + typ.name
        end + 
        CASE
            WHEN col.is_nullable=1 AND
                 typ.name NOT IN (
                     'binary', 'varbinary', 'image',
                     'text', 'ntext',
                     'varchar', 'nvarchar', 'char', 'nchar')
            THEN '?'
            ELSE '' END AS [ColumnType]
    from sys.columns col
        join sys.types typ on
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id 
    where object_id = object_id(@TableName)
) t
order by column_id

set @result = @result  + '
}'

print @result

------------

MYSQL version (https://stackoverflow.com/a/28743225)

set @schema := 'schema_name';
set @table := 'table_name';
SET group_concat_max_len = 2048;
SELECT 
    concat('public class ', @table, '\n{\n', GROUP_CONCAT(a.property_ SEPARATOR '\n'), '\n}') class_
FROM 
    (select
        CONCAT(
        '\tpublic ',
        case 
            when DATA_TYPE = 'bigint' then 'long'
            when DATA_TYPE = 'BINARY' then 'byte[]'
            when DATA_TYPE = 'bit' then 'bool'
            when DATA_TYPE = 'char' then 'string'
            when DATA_TYPE = 'date' then 'DateTime'
            when DATA_TYPE = 'datetime' then 'DateTime'
            when DATA_TYPE = 'datetime2' then 'DateTime'
            when DATA_TYPE = 'datetimeoffset' then 'DateTimeOffset'
            when DATA_TYPE = 'decimal' then 'decimal'
            when DATA_TYPE = 'double' then 'double'
            when DATA_TYPE = 'float' then 'float'
            when DATA_TYPE = 'image' then 'byte[]'
            when DATA_TYPE = 'int' then 'int'
            when DATA_TYPE = 'money' then 'decimal'
            when DATA_TYPE = 'nchar' then 'char'
            when DATA_TYPE = 'ntext' then 'string'
            when DATA_TYPE = 'numeric' then 'decimal'
            when DATA_TYPE = 'nvarchar' then 'string'
            when DATA_TYPE = 'real' then 'double'
            when DATA_TYPE = 'smalldatetime' then 'DateTime'
            when DATA_TYPE = 'smallint' then 'short'
            when DATA_TYPE = 'smallmoney' then 'decimal'
            when DATA_TYPE = 'text' then 'string'
            when DATA_TYPE = 'time' then 'TimeSpan'
            when DATA_TYPE = 'timestamp' then 'DateTime'
            when DATA_TYPE = 'tinyint' then 'byte'
            when DATA_TYPE = 'uniqueidentifier' then 'Guid'
            when DATA_TYPE = 'varbinary' then 'byte[]'
            when DATA_TYPE = 'varchar' then 'string'
            else '_UNKNOWN_'
        end, ' ', 
        COLUMN_NAME, ' {get; set;}') as property_
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE table_name = @table AND table_schema = @schema) a
;

---------------------------------------------------------------------- FOR ALL TABLES & VIEWS ----------------------------------------------------------------------
----------------------------------------------------------------------      TSQL-to-POCO      ----------------------------------------------------------------------

src - https://gist.github.com/joey-qc/6710702

declare @tableName varchar(200)
declare @columnName varchar(200)
declare @nullable varchar(50)
declare @datatype varchar(50)
declare @maxlen int

declare @sType varchar(50)
declare @sProperty varchar(200)

DECLARE table_cursor CURSOR FOR 
SELECT TABLE_NAME
FROM [INFORMATION_SCHEMA].[TABLES]

OPEN table_cursor

FETCH NEXT FROM table_cursor 
INTO @tableName

WHILE @@FETCH_STATUS = 0
BEGIN

PRINT 'public class ' + @tableName + ' {'

    DECLARE column_cursor CURSOR FOR 
    SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE, isnull(CHARACTER_MAXIMUM_LENGTH,'-1') 
  from [INFORMATION_SCHEMA].[COLUMNS] 
	WHERE [TABLE_NAME] = @tableName
	order by [ORDINAL_POSITION]

    OPEN column_cursor
    FETCH NEXT FROM column_cursor INTO @columnName, @nullable, @datatype, @maxlen

    WHILE @@FETCH_STATUS = 0
    BEGIN

	-- datatype
	select @sType = case @datatype
	when 'int' then 'Int32'
	when 'decimal' then 'Decimal'
	when 'money' then 'Decimal'
	when 'char' then 'String'
	when 'nchar' then 'String'
	when 'varchar' then 'String'
	when 'nvarchar' then 'String'
	when 'uniqueidentifier' then 'Guid'
	when 'datetime' then 'DateTime'
	when 'bit' then 'Boolean'
	else 'String'
	END

		If (@nullable = 'NO')
			PRINT '[Required]'
		if (@sType = 'String' and @maxLen <> '-1')
			Print '[MaxLength(' +  convert(varchar(4),@maxLen) + ')]'
		SELECT @sProperty = 'public ' + @sType + ' ' + @columnName + ' { get; set;}'
		PRINT @sProperty

		print ''
		FETCH NEXT FROM column_cursor INTO @columnName, @nullable, @datatype, @maxlen
	END
    CLOSE column_cursor
    DEALLOCATE column_cursor

	print '}'
	print ''
    FETCH NEXT FROM table_cursor 
    INTO @tableName
END
CLOSE table_cursor
DEALLOCATE table_cursor
