Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders


        Dim data As New List(Of Object)()

        data.Add(New Fruit() With {.Name = "Apples", .Price = 3})
        data.Add(New Fruit() With {.Name = "Apples", .Price = 2})
        data.Add(New Fruit() With {.Name = "Oranges", .Price = 5})
        data.Add(New Fruit() With {.Name = "Banana", .Price = 1})
        DataGridView1.DataSource = data

    End Sub

    Private Sub dataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then
            Return
        End If

        ' Check if the current cell's value is the same as the cell above.
        If e.RowIndex > 0 AndAlso DataGridView1(e.ColumnIndex, e.RowIndex).Value = DataGridView1(e.ColumnIndex, e.RowIndex - 1).Value Then
            ' Don't paint this cell to merge it with the cell above.
            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None
        End If
    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        If (e.RowIndex > 0) Then
            If e.RowIndex > 0 AndAlso DataGridView1(e.ColumnIndex, e.RowIndex).Value = DataGridView1(e.ColumnIndex, e.RowIndex - 1).Value Then

                e.Value = ""
                e.FormattingApplied = True
            End If
        End If

    End Sub


End Class

Class Fruit
    Private _name As String
    Private _count As Integer

    Public Property Price() As Integer
        Get
            Return _count
        End Get
        Set(ByVal value As Integer)
            _count = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
End Class



