CREATE PROCEDURE [users].[UpsertUsers]	
	@id     int = null,
	@name   nvarchar(25),
	@sex    bit,
	@role   tinyint,
	@status tinyint
as
begin
	if(ISNULL(@id, 0) = 0)

		begin
			insert into [users].[Users]([Name], [Sex], [Role], [Status], [Created], [Updated])
				values(LTRIM(rtrim(@name)), @sex, @role, @status, GETUTCDATE(), GETUTCDATE())
	
			SELECT TOP 1 * FROM [users].[Users] ORDER BY ID DESC
					FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
		end
	else
		begin
			update u
				set [Name] = LTRIM(rtrim(@name)),
				[Sex] = @sex,
				[Role] = @role,
				[Status] = @status,
				[Updated] = GETUTCDATE()
					from [users].[Users] as u
				where [id] = @id

			SELECT * FROM [users].[Users] where [id] = @id
					FOR JSON PATH, WITHOUT_ARRAY_WRAPPER

		end
end;