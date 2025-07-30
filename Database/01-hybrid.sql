-- Set database context
if (serverproperty('Edition') != 'SQL Azure') begin
    use [dynamic-schema-ef]
end
go

-- Remove existing objects
drop table if exists dbo.[todo_hybrid];
drop sequence if exists dbo.[global_sequence];

-- Create sequence object
create sequence dbo.[global_sequence]
	as int
	start with 1
	increment by 1;
go

-- Create table
drop table if exists [dbo].[todo_hybrid];
create table [dbo].[todo_hybrid]
(
	[id] [int] not null,
	[todo] [nvarchar](max) not null,
	[completed] [tinyint] not null,
	[extension] json null,
)
go
alter table [dbo].[todo_hybrid] add constraint pk__todo_hybrid primary key clustered ([id] asc) with (optimize_for_sequential_key = on)
go
alter table [dbo].[todo_hybrid] add constraint df__todo_hybrid__id default (next value for [global_sequence]) for [id]
go
alter table [dbo].[todo_hybrid] add constraint df__todo_hybrid__completed default ((0)) for [completed]
go

