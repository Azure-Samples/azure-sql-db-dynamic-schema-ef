create schema [web] authorization [dbo];
go

create user [dynamic-schema-test-user] with password = 'Super_Str0ng*P@ZZword!'
go

alter role db_datareader add member [dynamic-schema-test-user]
alter role db_datawriter add member [dynamic-schema-test-user]
go

create sequence dbo.[global_sequence]
as int
start with 1
increment by 1;
go

