CREATE PROCEDURE [users].[GetUsers]
	@Search nvarchar(25) = '',
	@OrderBy nvarchar(10) = '',
	@Desc bit = 0,
	@Page int = 1,
	@PageSize int = 25
AS
	begin 
	    SELECT u.Id
            ,u.[Name]
            ,u.Sex
			,u.[Role]
			,u.[Status]
			,u.Created
			,u.Updated
        INTO #temp
            FROM
            [users].[Users] as u
        where (@Search IS NULL OR @Search = '' OR [Name] LIKE '%' + @Search + '%')

        declare @count int
        set @count = (select count(*) from #temp)

		SELECT
           @count as 'Count',
           JSON_QUERY( (SELECT * FROM #temp
                        ORDER BY
                                CASE WHEN @OrderBy = 'Id' AND @Desc = 0 THEN Id END,
                                CASE WHEN @OrderBy = 'Id' AND @Desc = 1 THEN Id END DESC,
                                CASE WHEN @OrderBy = 'Name' AND @Desc = 0 THEN [Name] END,
                                CASE WHEN @OrderBy = 'Name' AND @Desc = 1 THEN [Name] END DESC,
                                CASE WHEN @OrderBy = 'Sex' AND @Desc = 0 THEN Sex END,
                                CASE WHEN @OrderBy = 'Sex' AND @Desc = 1 THEN Sex END DESC,
                                CASE WHEN @OrderBy = 'Role' AND @Desc = 0 THEN [Role] END,
                                CASE WHEN @OrderBy = 'Role' AND @Desc = 1 THEN [Role] END DESC, 
                                CASE WHEN @OrderBy = 'Status' AND @Desc = 0 THEN [Status] END,
                                CASE WHEN @OrderBy = 'Status' AND @Desc = 1 THEN [Status] END DESC,
                                CASE WHEN @OrderBy = 'Created' AND @Desc = 0 THEN Created END,
                                CASE WHEN @OrderBy = 'Created' AND @Desc = 1 THEN Created END DESC,
                                CASE WHEN @OrderBy = 'Updated' AND @Desc = 0 THEN [Updated] END,
                                CASE WHEN @OrderBy = 'Updated' AND @Desc = 1 THEN [Updated] END DESC
                        OFFSET (@Page-1)*@PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY
                        FOR JSON PATH
                    )) as 'Users'
            FOR JSON PATH , WITHOUT_ARRAY_WRAPPER
	end