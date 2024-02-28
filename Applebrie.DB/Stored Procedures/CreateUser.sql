CREATE PROCEDURE [users].[CreateUser]
	@name   nvarchar(25),
	@sex    bit,
	@role  tinyint,
	@status tinyint
as
	begin
		insert into [users].[Users]([Name], [Sex], [Role], [Status], [Created], [Updated])
			values(LTRIM(rtrim(@name)), @sex, @role, @status, GETUTCDATE(), GETUTCDATE())
	
		SELECT TOP 1 * FROM [users].[Users] ORDER BY ID DESC
				FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
	end;