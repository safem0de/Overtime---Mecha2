Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        LoadChart()
    End Sub

    Sub LoadChart()
        Dim Sql = "
            DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+=QUOTENAME(Section) + ','
        FROM 
            [Manpower_Mecha2].[dbo].[Emp_Master]
        GROUP BY 
            Section;
 
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        with TableA as
        (
        SELECT 
	        Convert(varchar(10),[Date],23) as [Date]
	        ,[Emp_Master].[Section]
	        ,Sum([Hours])as Hours
        FROM [Manpower_Mecha2].[dbo].[Request_OT_Detail]

        LEFT JOIN [Manpower_Mecha2].[dbo].[Emp_Master]
        ON [Request_OT_Detail].[EmpNo] = [Emp_Master].[EmpNo]

        WHERE Month([Date]) = Month(GETDATE())
		OR Month([Date]) = Month(GETDATE())-1
		OR Month([Date]) = Month(GETDATE())+1

        GROUP BY Convert(varchar(10),[Date],23)
	        ,[Emp_Master].[Section]
        )

        SELECT * FROM
        (
        SELECT	[Date],
		        [Section],
		        [Hours]
        FROM TableA

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

        'StandardFunction.fillDataTableToDataGrid(GrdTest, Sql, "0")
        Dim GetChart = StandardFunction.GetGraph(Sql)
        Dim Columnz = GetChart(0)
        Dim Dataz = GetChart(1)
        Dim Rowz = GetChart(2)

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "

            var x_row = " & Rowz & "
            var x_column = " & Columnz & "
            var stack = " & Dataz & "

            //alert(x_row);
            //alert(x_column);
            //alert(stack);

            var colorSet = []
            var backgroundSet = []

            for (i = 0; i < x_column.length; i++) {
                var minR = 100, maxR = 255;
                var minG = 0, maxG = 150;
                var minB = 0, maxB = 150;

                var Red = Math.round(Math.random() * ((maxR - minR) + 1) + minR);
                var Green = Math.round(Math.random() * ((maxG - minG) + 1) + minG);
                var Blue = Math.round(Math.random() * ((maxB - minB) + 1) + minB);

                colorSet.push('rgba(' + Red + ',' + Green + ',' + Blue + ',0.5)');
                backgroundSet.push('rgba(' + Red + ',' + Green + ',' + Blue + ',1)');
            }
            colorSet.sort()
            backgroundSet.sort()

            var setData = []
            for (i = 0; i < (x_column.length); i++) {
                setData.push(
                    {
                        label: x_column[i],
                        data: stack[i],
                        backgroundColor: colorSet[i],
                        borderColor: backgroundSet[i],
                        borderWidth: 1
                    }
                )
            }

            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',

                data: {
                    labels: x_row,
                    datasets: setData
                },
                options: {
                    scales: {
                        yAxes: [{
                            stacked: true,
                            ticks: {
                                beginAtZero: true,
                                stepSize: 10
                            },

                            scaleLabel: {
                                display: true,
                                labelString: 'ชั่วโมง',
                            },
                        }],

                        xAxes: [{
                            stacked: true,
                            barPercentage: 0.5,
                            scaleLabel: {
                                display: true,
                                labelString: 'วันที่',
                            },
                        }]
                    },
                }
            });
        ", True)
    End Sub
End Class