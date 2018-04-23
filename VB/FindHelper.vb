Imports DevExpress.Utils.Paint
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

Namespace WindowsFormsApplication2
	Public Class FindHelper
		Inherits Component

		Public Sub New()
			BackGroundColor = Color.Green
			HighLightColor = Color.Gold

			timer.Interval = 1000
			AddHandler timer.Tick, AddressOf timer_Tick
		End Sub



		Private edit As ButtonEdit
		Private grid As GridControl
		Private view As GridView
		Private showResult As EditorButton
		Private timer As New Timer()
		Private filterCellText As String = String.Empty
		Private findList As New Dictionary(Of GridCell, Boolean)()

		Private ReadOnly Property FindListIsEmpty() As Boolean
			Get
				Return findList.Keys.Count = 0
			End Get
		End Property
		Public Property BackGroundColor() As Color
		Public Property HighLightColor() As Color
		Public Property AutomaticallyPerformSearchAfter() As Integer
			Get
				Return timer.Interval
			End Get
			Set(ByVal value As Integer)
				timer.Interval = value
			End Set
		End Property

		Public Property TargetControl() As GridControl
			Get
				Return grid
			End Get
			Set(ByVal value As GridControl)
				SubscibeViewEvent(False)
				grid = value
				ResetActiveGrid()
			End Set
		End Property

		Public Property SearchControl() As ButtonEdit
			Get
				Return edit
			End Get
			Set(ByVal value As ButtonEdit)
				SubscribeRIEvent(False)
				edit = value
				ResetActiveRI()
			End Set
		End Property

		Private Sub ResetActiveRI()
			If DesignMode Then
				Return
			End If
			edit.Properties.Buttons.Clear()
			showResult = New EditorButton(ButtonPredefines.Glyph, "0 of 0", 0, False, True, False, ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(Keys.None), Nothing, "", Nothing, Nothing, True)
			edit.Properties.Buttons.AddRange(New EditorButton() {
				New EditorButton(ButtonPredefines.Search),
				New EditorButton(ButtonPredefines.Clear),
				New EditorButton(ButtonPredefines.SpinLeft),
				New EditorButton(ButtonPredefines.SpinRight),
				showResult
			})

			SubscribeRIEvent(True)
		End Sub

		Private Sub SubscribeRIEvent(ByVal subscribe As Boolean)
			If edit Is Nothing Then
				Return
			End If
			RemoveHandler edit.Properties.ButtonClick, AddressOf ButtonClick
			RemoveHandler edit.Properties.KeyUp, AddressOf Editor_KeyUp
			RemoveHandler edit.Properties.KeyDown, AddressOf Editor_KeyDown
			If subscribe Then
				AddHandler edit.Properties.KeyUp, AddressOf Editor_KeyUp
				AddHandler edit.Properties.KeyDown, AddressOf Editor_KeyDown
				AddHandler edit.Properties.ButtonClick, AddressOf ButtonClick
			End If
		End Sub

		Private Sub Editor_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			StopTimer()
			Select Case e.KeyData
				Case Keys.Enter
					edit.PerformClick(edit.Properties.Buttons(0))
				Case (Keys.Control Or Keys.Left)
					edit.PerformClick(edit.Properties.Buttons(1))
			End Select
		End Sub


		Private Sub Editor_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
			StratTimer()
		End Sub


		Private Sub ResetActiveGrid()
			If grid Is Nothing Then
				Return
			End If
			view = TryCast(grid.MainView, GridView)
			SubscibeViewEvent(True)
		End Sub

		Private Sub SubscibeViewEvent(ByVal subscribe As Boolean)
			If view Is Nothing Then
				Return
			End If
			RemoveHandler view.CustomDrawCell, AddressOf CustomDrawCell
			If subscribe Then
				AddHandler view.CustomDrawCell, AddressOf CustomDrawCell
			End If
		End Sub

		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			SubscibeViewEvent(False)
			SubscribeRIEvent(False)
			MyBase.Dispose(disposing)
		End Sub

		Private Sub CustomDrawCell(ByVal sender As Object, ByVal e As RowCellCustomDrawEventArgs)
			If FindListIsEmpty Then
				Return
			End If

			Dim filterTextIndex As Integer = e.DisplayText.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase)
			If filterTextIndex = -1 Then
				Return
			End If
			Dim temp As New GridCell(e.RowHandle, e.Column)

			If NeedHighLight(temp) Then
				e.Appearance.BackColor = BackGroundColor
				e.Cache.FillRectangle(BackGroundColor, e.Bounds)
			End If

			Dim gci As GridCellInfo = TryCast(e.Cell, GridCellInfo)
			Dim tevi As TextEditViewInfo = TryCast(gci.ViewInfo, TextEditViewInfo)
			If tevi Is Nothing Then
				Return
			End If
			Dim textRect As New Rectangle(e.Bounds.X + tevi.MaskBoxRect.X, e.Bounds.Y + tevi.MaskBoxRect.Y, tevi.MaskBoxRect.Width, tevi.MaskBoxRect.Height)
			e.Cache.Paint.DrawMultiColorString(e.Cache, textRect, e.DisplayText, filterCellText, e.Appearance,e.Appearance.ForeColor, HighLightColor, False, filterTextIndex)

			e.Handled = True
		End Sub

		Private Sub ButtonClick(ByVal sender As Object, ByVal e As ButtonPressedEventArgs)
			Dim edit As ButtonEdit = TryCast(sender, ButtonEdit)
			Select Case e.Button.Kind
				Case ButtonPredefines.Search
					PerformSearch(edit.EditValue)
				Case ButtonPredefines.Clear
					ClearEditValue(edit)
					PerformSearch(Nothing)
				Case ButtonPredefines.SpinLeft
					HighLightPrevious()
				Case ButtonPredefines.SpinRight
					HighLightNext()
			End Select
		End Sub

		Private Sub timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
			PerformSearch(edit.EditValue)
		End Sub

		Private Sub UpdateShowResult()
			Dim index As Integer
			For index = 0 To findList.Keys.Count - 1
				If findList(findList.Keys.ElementAt(index)) Then
					index += 1
					Exit For
				End If
			Next index
			showResult.Caption = String.Format("{0} of {1}", index, findList.Keys.Count)
		End Sub

		Private Sub HighLightPrevious()

			If FindListIsEmpty Then
				Return
			End If

			Dim currItem As GridCell = findList.Keys.ElementAt(0)
			Dim targetItem As GridCell = findList.Keys.ElementAt(findList.Keys.Count - 1)
			Dim temp As GridCell
			For i As Integer = 1 To findList.Keys.Count - 1
				temp = findList.Keys.ElementAt(i)
				If findList(temp) Then
					targetItem = findList.Keys.ElementAt(i - 1)
					currItem = temp
					Exit For
				End If
			Next i

			findList(currItem) = False
			findList(targetItem) = True
			EnsureCellVisible(targetItem)
			RefreshGridView()
			UpdateShowResult()
		End Sub

		Private Sub HighLightNext()
			If FindListIsEmpty Then
				Return
			End If

			Dim needBreak As Boolean = False
			Dim currItem As GridCell = Nothing
			Dim targetItem As GridCell = Nothing
			For Each item As GridCell In findList.Keys
				If needBreak Then
					targetItem = item
					Exit For
				End If
				If findList(item) Then
					currItem = item
					needBreak = True
				End If
			Next item

			If targetItem Is Nothing Then
				targetItem = findList.Keys.ElementAt(0)
			End If

			findList(currItem) = False
			findList(targetItem) = True
			EnsureCellVisible(targetItem)

			RefreshGridView()
			UpdateShowResult()
		End Sub

		Private Sub EnsureCellVisible(ByVal cell As GridCell)
			view.MakeRowVisible(cell.RowHandle)
			view.MakeColumnVisible(cell.Column)
		End Sub

		Private Sub ClearEditValue(ByVal edit As ButtonEdit)
			edit.EditValue = Nothing
		End Sub

		Private Sub PerformSearch(ByVal val As Object)
			findList.Clear()
			StopTimer()
			If val Is Nothing Then
				val = String.Empty
			End If
			filterCellText = val.ToString()
			InitFindList()
			If Not FindListIsEmpty Then
				EnsureCellVisible(findList.Keys.ElementAt(0))
			End If
			RefreshGridView()
			UpdateShowResult()
		End Sub

		Private Sub InitFindList()
			If String.IsNullOrEmpty(filterCellText) Then
				Return
			End If
			Dim text As String
			For i As Integer = 0 To view.RowCount - 1
				For Each col As GridColumn In view.Columns
					If Not col.Visible OrElse (TryCast(col.RealColumnEdit, RepositoryItemTextEdit)) Is Nothing Then
						Continue For
					End If

					text = view.GetRowCellDisplayText(i, col)

					Dim filterTextIndex As Integer = text.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase)
					If filterTextIndex <> -1 Then
						findList.Add(New GridCell(i, col), False)
					End If
				Next col
			Next i
			If FindListIsEmpty Then
				Return
			End If
			findList(findList.Keys.ElementAt(0)) = True
		End Sub

		Private Sub RefreshGridView()
			view.LayoutChanged()
		End Sub

		Private Function NeedHighLight(ByVal cell As GridCell) As Boolean
			For Each item As GridCell In findList.Keys
				If item.RowHandle = cell.RowHandle AndAlso item.Column Is cell.Column Then
					Return findList(item)
				End If
			Next item
			Return False
		End Function

		Private Sub StopTimer()
			timer.Stop()
		End Sub

		Private Sub StratTimer()
			timer.Start()
		End Sub
	End Class
End Namespace
