-- Set database context
if (serverproperty('Edition') != 'SQL Azure') begin
    use [dynamic-schema-ef]
end
go

delete from dbo.[todo_hybrid];
go

declare @t nvarchar(max) = '{
	"title": "test",
	"completed": 0,
	"extension": {
		"order": 1,
		"createdOn": "2025-08-06T22:06:51"
	}
}';

insert into 
	dbo.[todo_hybrid] (todo, completed, [extension])
select
	title,
	completed,
	[extension]
from
	openjson(@t) with 
	(
		title nvarchar(100) '$.title',
		completed bit '$.completed',
		[extension] nvarchar(max) '$.extension' as json
	)
go

declare @t2 nvarchar(max) = '{
	"title": "another test",
	"completed": 1,
	"extension": {
		"order": 2,
		"tags": ["one", "two"]
	}
}';

insert into 
	dbo.[todo_hybrid] (todo, completed, [extension])
select
	title,
	completed,
	[extension]
from
	openjson(@t2) with 
	(
		title nvarchar(100) '$.title',
		completed bit '$.completed',
		[extension] nvarchar(max) '$.extension' as json
	)
go

select * from dbo.[todo_hybrid]
go


