Public Class Summary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadPage()
    End Sub

    Sub LoadPage()
        Dim SqlSection = "
            DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+=QUOTENAME(Section) + ','
        FROM 
            [Manpower_Mecha2].[dbo].[Emp_Master]
        GROUP BY 
            Section
		;
		SELECT
		@columns+=QUOTENAME('Total') + ','
		
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        With TableA as
			(SELECT 
				Convert(varchar(5),[Date],5) as [Date]
				,[Emp_Master].[Section]
				,Sum([Hours])as Hours
			FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

			LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master]
			ON [Request_OT_Detail].[EmpNo] = [Emp_Master].[EmpNo]

			WHERE
            Month([Date]) = datepart(MONTH,dateadd(MONTH,0, GETDATE()))
			AND
            Year([Date]) = Year(GETDATE())

			GROUP BY Convert(varchar(5),[Date],5)
				,[Emp_Master].[Section]
			),
			TableB as
			(Select
				Convert(varchar(5),[Date],5) as [Date]
				,''Total'' as [Section]
				,Sum([Hours])as Hours
			FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

			WHERE 
            Month([Date]) = datepart(MONTH,dateadd(MONTH,0, GETDATE()))
			AND
            Year([Date]) = Year(GETDATE())

			GROUP BY Convert(varchar(5),[Date],5)
			)

        SELECT * FROM
        (
			SELECT * FROM TableA
			UNION
			SELECT * FROM TableB
        )t 
        PIVOT(
            SUM (Hours) 
            FOR [Section] IN ('+ @columns +')
        ) AS pivot_table
        Order By pivot_table.[Date] asc;';
        PRINT @sql;
        -- execute the dynamic SQL
        EXECUTE sp_executesql @sql;
        "
        StandardFunction.fillDataTableToDataGrid(GrdSection, SqlSection, "0")

        Dim SqlProcess = "
        DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+= QUOTENAME(Convert(varchar(5),[Date],5)) + ','
        FROM 
            [Manpower_Mecha2].[dbo].[Request_OT_Detail]
		WHERE
			Month([Date]) = Month(GETDATE())
        GROUP BY 
            Convert(varchar(5),[Date],5)
		ORDER BY
			Convert(varchar(5),[Date],5)
		;
 
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        with TableA as
        (
        SELECT 
			UPPER([Emp_Master].[Process]) as [Process]
			,Convert(varchar(5),[Date],5) as [Date]
			,Sum([Hours])as Hours
		FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

		LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master]
		ON [Request_OT_Detail].[EmpNo] = [Emp_Master].[EmpNo]

		WHERE Month([Date]) = Month(GETDATE())
		AND Year([Date]) = Year(GETDATE())

		GROUP BY Convert(varchar(5),[Date],5)
			,[Emp_Master].[Process]
        )

        SELECT * FROM
        (
        SELECT	[Process],
				[Date],
		        [Hours]
        FROM TableA

        )t 
        PIVOT(
            SUM (Hours) 
            FOR [Date] IN ('+ @columns +')
        ) AS pivot_table
        Order By pivot_table.[Process] asc;';
        PRINT @sql;
        -- execute the dynamic SQL
        EXECUTE sp_executesql @sql;
        "

        StandardFunction.fillDataTableToDataGrid(GrdProcess, SqlProcess, "0")

        Dim SqlSectionNext = "
            DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+=QUOTENAME(Section) + ','
        FROM 
            [Manpower_Mecha2].[dbo].[Emp_Master]
        GROUP BY 
            Section
		;
		SELECT
		@columns+=QUOTENAME('Total') + ','
		
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        With TableA as
			(SELECT 
				Convert(varchar(5),[Date],5) as [Date]
				,[Emp_Master].[Section]
				,Sum([Hours])as Hours
			FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

			LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master]
			ON [Request_OT_Detail].[EmpNo] = [Emp_Master].[EmpNo]

			WHERE Month([Date]) = datepart(MONTH,dateadd(MONTH,1, GETDATE()))
			AND Year([Date]) = Year(GETDATE())

			GROUP BY Convert(varchar(5),[Date],5)
				,[Emp_Master].[Section]
			),
			TableB as
			(Select
				Convert(varchar(5),[Date],5) as [Date]
				,''Total'' as [Section]
				,Sum([Hours])as Hours
			FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

			WHERE Month([Date]) = datepart(MONTH,dateadd(MONTH,1, GETDATE()))
			AND Year([Date]) = Year(GETDATE())

			GROUP BY Convert(varchar(5),[Date],5)
			)

        SELECT * FROM
        (
			SELECT * FROM TableA
			UNION
			SELECT * FROM TableB
        )t 
        PIVOT(
            SUM (Hours) 
            FOR [Section] IN ('+ @columns +')
        ) AS pivot_table
        Order By pivot_table.[Date] asc;';
        PRINT @sql;
        -- execute the dynamic SQL
        EXECUTE sp_executesql @sql;
        "
        StandardFunction.fillDataTableToDataGrid(GrdSectionNext, SqlSectionNext, "0")

        Dim SqlProcessNext = "
        DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+= QUOTENAME(Convert(varchar(5),[Date],5)) + ','
        FROM 
            [Manpower_Mecha2].[dbo].[Request_OT_Detail]
		WHERE
			Month([Date]) = datepart(MONTH,dateadd(MONTH,1, GETDATE()))
        GROUP BY 
            Convert(varchar(5),[Date],5)
		ORDER BY
			Convert(varchar(5),[Date],5)
		;
 
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        with TableA as
        (
        SELECT 
			UPPER([Emp_Master].[Process]) as [Process]
			,Convert(varchar(5),[Date],5) as [Date]
			,Sum([Hours])as Hours
		FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

		LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master]
		ON [Request_OT_Detail].[EmpNo] = [Emp_Master].[EmpNo]

		WHERE Month([Date]) = datepart(MONTH,dateadd(MONTH,1, GETDATE()))
		AND Year([Date]) = Year(GETDATE())

		GROUP BY Convert(varchar(5),[Date],5)
			,[Emp_Master].[Process]
        )

        SELECT * FROM
        (
        SELECT	[Process],
				[Date],
		        [Hours]
        FROM TableA

        )t 
        PIVOT(
            SUM (Hours) 
            FOR [Date] IN ('+ @columns +')
        ) AS pivot_table
        Order By pivot_table.[Process] asc;';
        PRINT @sql;
        -- execute the dynamic SQL
        EXECUTE sp_executesql @sql;
        "

        StandardFunction.fillDataTableToDataGrid(GrdProcessNext, SqlProcessNext, "0")

        Dim SqlManProcess = "
            DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

                -- select the category names
                SELECT 
                    @columns+= QUOTENAME(Convert(varchar(5),[Date],5)) + ','
                FROM 
                    [Manpower_Mecha2].[dbo].[Request_OT_Detail]
		        WHERE
			        Month([Date]) = Month(GETDATE())
                GROUP BY 
                    Convert(varchar(5),[Date],5)
		        ORDER BY
			        Convert(varchar(5),[Date],5)
		        ;
 
                -- remove the last comma
                SET @columns = LEFT(@columns, LEN(@columns) - 1);

                PRINT @columns;
                -- construct dynamic SQL
                SET @sql ='
                with TableA as
                (
                SELECT
	                ISNULL(t2.[Process],t1.[Process]) as [Process]
	                ,Convert(varchar(5),[Date],5) as [Date]
	                ,Count([EmpNo]) as [Person]
                  FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail] as t1

                  LEFT JOIN [Manpower_Mecha2].[dbo].[Request_OT] as t2
                  ON t1.[Req_Id] = t2.[Request_Id]

                WHERE
	                Not t1.Req_Id = 0
                AND
	                Month([Date]) = Month(GETDATE())
                AND 
	                Year([Date]) = Year(GETDATE())
                GROUP BY
	                ISNULL(t2.[Process],t1.[Process])
	                ,Convert(varchar(5),[Date],5)
                )

                SELECT * FROM
                (
                SELECT	[Process],
				        [Date],
		                [Person]
                FROM TableA

                )t 
                PIVOT(
                    SUM (Person) 
                    FOR [Date] IN ('+ @columns +')
                ) AS pivot_table
                Order By pivot_table.[Process] asc;';
                PRINT @sql;
                -- execute the dynamic SQL
                EXECUTE sp_executesql @sql;
        "

        StandardFunction.fillDataTableToDataGrid(GrdManProcess, SqlManProcess, "0")
    End Sub
End Class