Namespace WindowsFormsApplication2
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.buttonEdit1 = New DevExpress.XtraEditors.ButtonEdit()
			Me.layoutControl1 = New DevExpress.XtraLayout.LayoutControl()
			Me.gridControl1 = New DevExpress.XtraGrid.GridControl()
			Me.gridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.layoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
			Me.layoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
			Me.layoutControlItem2 = New DevExpress.XtraLayout.LayoutControlItem()
			Me.emptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
			Me.findHelper1 = New WindowsFormsApplication2.FindHelper()
			CType(Me.buttonEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.layoutControl1.SuspendLayout()
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.layoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' buttonEdit1
			' 
			Me.buttonEdit1.Location = New System.Drawing.Point(12, 12)
			Me.buttonEdit1.Name = "buttonEdit1"
			Me.buttonEdit1.Size = New System.Drawing.Size(392, 20)
			Me.buttonEdit1.StyleController = Me.layoutControl1
			Me.buttonEdit1.TabIndex = 5
			' 
			' layoutControl1
			' 
			Me.layoutControl1.Controls.Add(Me.buttonEdit1)
			Me.layoutControl1.Controls.Add(Me.gridControl1)
			Me.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.layoutControl1.Location = New System.Drawing.Point(0, 0)
			Me.layoutControl1.Name = "layoutControl1"
			Me.layoutControl1.Root = Me.layoutControlGroup1
			Me.layoutControl1.Size = New System.Drawing.Size(1623, 422)
			Me.layoutControl1.TabIndex = 0
			Me.layoutControl1.Text = "layoutControl1"
			' 
			' gridControl1
			' 
			Me.gridControl1.Location = New System.Drawing.Point(12, 36)
			Me.gridControl1.MainView = Me.gridView1
			Me.gridControl1.Name = "gridControl1"
			Me.gridControl1.Size = New System.Drawing.Size(1599, 374)
			Me.gridControl1.TabIndex = 4
			Me.gridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gridView1})
			' 
			' gridView1
			' 
			Me.gridView1.GridControl = Me.gridControl1
			Me.gridView1.Name = "gridView1"
			' 
			' layoutControlGroup1
			' 
			Me.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
			Me.layoutControlGroup1.GroupBordersVisible = False
			Me.layoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() { Me.layoutControlItem1, Me.layoutControlItem2, Me.emptySpaceItem1})
			Me.layoutControlGroup1.Location = New System.Drawing.Point(0, 0)
			Me.layoutControlGroup1.Name = "layoutControlGroup1"
			Me.layoutControlGroup1.Size = New System.Drawing.Size(1623, 422)
			Me.layoutControlGroup1.TextVisible = False
			' 
			' layoutControlItem1
			' 
			Me.layoutControlItem1.Control = Me.gridControl1
			Me.layoutControlItem1.Location = New System.Drawing.Point(0, 24)
			Me.layoutControlItem1.Name = "layoutControlItem1"
			Me.layoutControlItem1.Size = New System.Drawing.Size(1603, 378)
			Me.layoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
			Me.layoutControlItem1.TextVisible = False
			' 
			' layoutControlItem2
			' 
			Me.layoutControlItem2.Control = Me.buttonEdit1
			Me.layoutControlItem2.Location = New System.Drawing.Point(0, 0)
			Me.layoutControlItem2.Name = "layoutControlItem2"
			Me.layoutControlItem2.Size = New System.Drawing.Size(396, 24)
			Me.layoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
			Me.layoutControlItem2.TextVisible = False
			' 
			' emptySpaceItem1
			' 
			Me.emptySpaceItem1.AllowHotTrack = False
			Me.emptySpaceItem1.Location = New System.Drawing.Point(396, 0)
			Me.emptySpaceItem1.Name = "emptySpaceItem1"
			Me.emptySpaceItem1.Size = New System.Drawing.Size(1207, 24)
			Me.emptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
			' 
			' findHelper1
			' 
			Me.findHelper1.BackGroundColor = System.Drawing.Color.Green
			Me.findHelper1.HighLightColor = System.Drawing.Color.Gold
			Me.findHelper1.SearchControl = Me.buttonEdit1
			Me.findHelper1.TargetControl = Me.gridControl1
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1623, 422)
			Me.Controls.Add(Me.layoutControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			CType(Me.buttonEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.layoutControl1.ResumeLayout(False)
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.layoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private gridControl1 As DevExpress.XtraGrid.GridControl
		Private gridView1 As DevExpress.XtraGrid.Views.Grid.GridView
		Private layoutControl1 As DevExpress.XtraLayout.LayoutControl
		Private buttonEdit1 As DevExpress.XtraEditors.ButtonEdit
		Private layoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
		Private layoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
		Private layoutControlItem2 As DevExpress.XtraLayout.LayoutControlItem
		Private emptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
		Private findHelper1 As FindHelper
	End Class
End Namespace

