Imports System.ComponentModel

Public Class Form1

    ' Declare a ContextMenuStrip
    Dim contextMenuStrip1 As New ContextMenuStrip()
    Dim dataFruits As New List(Of Object)()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders

        dataFruits.Add(New Fruit() With {.Name = "Apples", .Price = 2})
        dataFruits.Add(New Fruit() With {.Name = "Apples", .Price = 2})
        dataFruits.Add(New Fruit() With {.Name = "Oranges", .Price = 5})
        dataFruits.Add(New Fruit() With {.Name = "Banana", .Price = 1})
        DataGridView1.DataSource = dataFruits

        CreateDropdownChecksMenu()

    End Sub

    Private Sub CreateDropdownChecksMenu()
        Dim stringList As New List(Of String) From {"Option 1", "Option 2", "Option 3"}

        ' Create a new DataGridViewComboBoxColumn
        Dim comboBoxColumn As New DataGridViewComboBoxColumn()

        ' Set column properties
        comboBoxColumn.HeaderText = "Options"
        comboBoxColumn.Name = "OptionsColumn"
        comboBoxColumn.DataSource = stringList

        ' Add the column to the DataGridView
        DataGridView1.Columns.Add(comboBoxColumn)

        ' Create a new DataGridViewCheckBoxColumn
        Dim checkBoxColumn As New DataGridViewCheckBoxColumn()

        ' Set column properties
        checkBoxColumn.HeaderText = "Select"
        checkBoxColumn.Name = "SelectColumn"

        ' Add the CheckBox column to the DataGridView
        DataGridView1.Columns.Add(checkBoxColumn)

        ' Add items to the ContextMenuStrip
        Dim sortAscendingMenuItem As New ToolStripMenuItem("Sort Ascending")
        AddHandler sortAscendingMenuItem.Click, AddressOf SortAscendingMenuItem_Click
        contextMenuStrip1.Items.Add(sortAscendingMenuItem)

        Dim sortDescendingMenuItem As New ToolStripMenuItem("Sort Descending")
        AddHandler sortDescendingMenuItem.Click, AddressOf SortDescendingMenuItem_Click
        contextMenuStrip1.Items.Add(sortDescendingMenuItem)
    End Sub

    'Private Sub dataGridView1_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
    '    If e.RowIndex < 0 Or e.ColumnIndex < 0 Or e.ColumnIndex = DataGridView1.Columns("SelectColumn").Index Then
    '        Return
    '    End If

    '    ' Check if the current cell's value is the same as the cell above.
    '    If e.RowIndex > 0 AndAlso DataGridView1(e.ColumnIndex, e.RowIndex).Value = DataGridView1(e.ColumnIndex, e.RowIndex - 1).Value Then
    '        ' Don't paint this cell to merge it with the cell above.
    '        e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None
    '    End If
    'End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        'If (e.RowIndex > 0) Then
        '    If e.RowIndex > 0 AndAlso DataGridView1(e.ColumnIndex, e.RowIndex).Value = DataGridView1(e.ColumnIndex, e.RowIndex - 1).Value Then

        '        e.Value = ""
        '        e.FormattingApplied = True
        '    End If
        'End If

    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        ' Check if the current cell is in the ComboBox column
        If TypeOf e.Control Is ComboBox AndAlso DataGridView1.CurrentCell.ColumnIndex = DataGridView1.Columns("OptionsColumn").Index Then
            Dim comboBox As ComboBox = DirectCast(e.Control, ComboBox)

            ' Check if the event handler is already subscribed
            If Not comboBox.Tag Is Nothing Then
                Return
            End If

            ' Subscribe to the SelectionChangeCommitted event
            AddHandler comboBox.SelectionChangeCommitted, AddressOf ComboBox_SelectionChangeCommitted

            ' Set a tag to indicate that the event handler is subscribed
            comboBox.Tag = True
        End If
    End Sub

    ' Event handler for SelectionChangeCommitted
    Private Sub ComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs)
        ' Handle the selected value here
        Dim selectedValue As String = DirectCast(sender, ComboBox).SelectedItem.ToString()
        MessageBox.Show($"Selected Value: {selectedValue}")
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        ' Check if the clicked cell is in the CheckBox column
        If e.ColumnIndex = DataGridView1.Columns("SelectColumn").Index AndAlso e.RowIndex >= 0 Then
            ' Handle checkbox click
            Dim checkBoxCell As DataGridViewCheckBoxCell = DirectCast(DataGridView1.Rows(e.RowIndex).Cells("SelectColumn"), DataGridViewCheckBoxCell)
            Dim isChecked As Boolean = Convert.ToBoolean(checkBoxCell.Value)

            ' Perform actions based on the checkbox state
            If isChecked Then
                MessageBox.Show("Checkbox is checked.")
                ' Add your logic here when the checkbox is checked
            Else
                MessageBox.Show("Checkbox is unchecked.")
                ' Add your logic here when the checkbox is unchecked
            End If
        End If
    End Sub

    Private Sub SortAscendingMenuItem_Click(sender As Object, e As EventArgs)
        ' Implement sorting logic for ascending order
        Dim columnIndex As Integer = DataGridView1.CurrentCell.ColumnIndex
        SortDataGridView(columnIndex, SortOrder.Ascending)
    End Sub

    Private Sub SortDescendingMenuItem_Click(sender As Object, e As EventArgs)
        ' Implement sorting logic for descending order
        Dim columnIndex As Integer = DataGridView1.CurrentCell.ColumnIndex
        SortDataGridView(columnIndex, SortOrder.Descending)
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        ' Check if the right mouse button is clicked (Button = 2)
        If e.Button = MouseButtons.Right AndAlso e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            ' Select the cell that was clicked
            DataGridView1.CurrentCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)

            ' Display the context menu at the current mouse position
            contextMenuStrip1.Show(DataGridView1, e.Location)
        End If
    End Sub

    ' Custom method to manually sort the DataGridView
    Private Sub SortDataGridView(columnIndex As Integer, direction As SortOrder)
        Dim propertyName As String = DataGridView1.Columns(columnIndex).DataPropertyName

        If direction = SortOrder.Ascending Then
            dataFruits = dataFruits.OrderBy(Function(f) f.GetType().GetProperty(propertyName).GetValue(f, Nothing)).ToList()
        ElseIf direction = SortOrder.Descending Then
            dataFruits = dataFruits.OrderByDescending(Function(f) f.GetType().GetProperty(propertyName).GetValue(f, Nothing)).ToList()
        End If


        DataGridView1.DataSource = Nothing
        DataGridView1.Rows.Clear()
        DataGridView1.Columns.Clear()
        contextMenuStrip1.Items.Clear()

        DataGridView1.DataSource = dataFruits
        CreateDropdownChecksMenu()
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



