
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Data.SqlClient
Imports Microsoft.Extensions.Logging

Public Class Product
    Public Property Id As Integer
    Public Property Name As String
    Public Property Barcode As String
    Public Property InStock As Integer
End Class

Public Class ProductUpdateModel
    Public Property id As Integer
    Public Property addStock As Integer
End Class


<ApiController>
<Route("api/[controller]")>
Public Class ProductController
    Inherits ControllerBase



    Dim connectionString As String = "Server=tcp:productapi.database.windows.net,1433;Initial Catalog=productDB;Persist Security Info=False;User ID=myadmin;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    Private ReadOnly _logger As ILogger(Of ProductController)

    Public Sub New(ByVal logger As ILogger(Of ProductController))
         _logger = logger
    End Sub


    <HttpGet("all")>
    Public Function GetAllProducts() As IEnumerable(Of Product)

        Dim productList As New List(Of Product)()


        Using connection As New SqlConnection(connectionString)

            Dim sql As String = "SELECT * FROM products"

            Using command As New SqlCommand(sql, connection)

                connection.Open()

                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()

                    Dim product As New Product With {
                        .Id = CInt(reader("id")),
                        .Name = CStr(reader("name")),
                        .Barcode = CStr(reader("barcode")),
                        .InStock = CInt(reader("instock"))
                    }

                    productList.Add(product)

                End While
                reader.Close()

            End Using

        End Using

        Return productList
    End Function

    <HttpGet("by-barcode/{barcode}")>
    Public Function GetProductByBarcode(ByVal barcode As String) As IEnumerable(Of Product)

        Dim products As New List(Of Product)()


        Using connection As New SqlConnection(connectionString)

            Dim sql As String = "SELECT * FROM products WHERE barcode = @BARCODE"

            Using command As New SqlCommand(sql, connection)

                command.Parameters.AddWithValue("@BARCODE", barcode)

                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                While reader.Read()
                    Dim product As New Product With {
                        .Id = CInt(reader("id")),
                        .Name = CStr(reader("name")),
                        .Barcode = CStr(reader("barcode")),
                        .InStock = CInt(reader("instock"))
                    }
                    products.Add(product)
                End While
                reader.Close()
            End Using
        End Using

        Return products

    End Function

    <HttpPatch("update-stock")>
    Public Function UpdateProductStock(<FromBody> model As ProductUpdateModel) As Boolean
        
        _logger.LogInformation("Start updating product's stock")
        _logger.LogInformation("Product Id: " + model.id.toString())
        _logger.LogInformation("Adding : " + model.addStock.toString())
        
        Dim result As Boolean = False

        Using connection As New SqlConnection(connectionString)

            Dim sql As String = "UPDATE products SET instock = instock + @ADDSTOCK WHERE id = @ID"

            Using command As New SqlCommand(sql, connection)

                command.Parameters.AddWithValue("@ADDSTOCK", model.addStock)
                command.Parameters.AddWithValue("@ID", model.id)

                connection.Open()

                If command.ExecuteNonQuery() > 0 Then
                    ' The query affected at least one row, so the update was successful
                    result = True
                End If

            End Using
        End Using

        Return result
    End Function

End Class
