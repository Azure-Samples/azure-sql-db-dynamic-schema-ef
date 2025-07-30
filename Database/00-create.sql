-- Create database
if (serverproperty('Edition') != 'SQL Azure') begin
    create database [dynamic-schema-ef]
end
go

-- Set database context
if (serverproperty('Edition') != 'SQL Azure') begin
    use [dynamic-schema-ef]
end
go

-- Create schema
create schema [web] authorization [dbo];
go

-- Create application user 
if (serverproperty('Edition') = 'SQL Azure') begin

    if not exists (select * from sys.database_principals where [type] in ('E', 'S') and [name] = 'dynamic-schema-test-user')
    begin 
        create user [dynamic-schema-test-user] with password = 'Super_Str0ng*P@ZZword!'
    end

end else begin

    if not exists (select * from sys.server_principals where [type] in ('E', 'S') and [name] = 'dynamic-schema-test-user')
    begin 
        create login [dynamic-schema-test-user] with password = 'Super_Str0ng*P@ZZword!'
    end    

    if not exists (select * from sys.database_principals where [type] in ('E', 'S') and [name] = 'dynamic-schema-test-user')
    begin
        create user [dynamic-schema-test-user] from login [dynamic-schema-test-user]            
    end

end

-- Grant permission to the application user
alter role db_datareader add member [dynamic-schema-test-user]
alter role db_datawriter add member [dynamic-schema-test-user]
go

