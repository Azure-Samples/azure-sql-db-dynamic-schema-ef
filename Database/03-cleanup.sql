-- Set database context
if (serverproperty('Edition') != 'SQL Azure') begin
    use [dynamic-schema-ef]
end
go

-- Cleanup
drop table if exists dbo.[todo_hybrid];
drop sequence if exists dbo.[global_sequence];
