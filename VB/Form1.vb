Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsApplication2
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
			SetDataSource()
		End Sub

		Private Sub SetDataSource()
			Dim dataSource As New DataSet()
			dataSource.ReadXml(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName & "\Contacts.xml")
			gridControl1.DataSource = dataSource.Tables(0)
			gridView1.Columns("Description").Visible = False
			gridView1.BestFitColumns()
		End Sub
	End Class
End Namespace
