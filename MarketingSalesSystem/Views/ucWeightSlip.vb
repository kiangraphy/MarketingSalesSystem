Imports DevExpress.XtraTab
Imports DevExpress.XtraGrid
Imports DevExpress.XtraLayout

Public Class ucWeightSlip
    Inherits ucBase

    Private tabControl As XtraTabControl

    Sub New(ByVal title As String)
        InitializeComponent()

        MyBase.title = title
        LabelControl1.Text = title


        Dim grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }

        LayoutControl2.Controls.Add(grid)

        Dim layoutItem As LayoutControlItem = LayoutControl2.AddItem("", grid)
        layoutItem.TextVisible = False
    End Sub

    Function setTab(ByRef tab As XtraTabPage) As GridControl
        Dim layout = New LayoutControl With {
                        .Dock = DockStyle.Fill
                    }
        tab.Controls.Add(layout)
        Dim grid = New GridControl() With {
            .Dock = DockStyle.Fill
        }
        layout.Controls.Add(grid)
        Dim layoutItem As LayoutControlItem = layout.AddItem("", grid)
        layoutItem.TextVisible = False

        Return grid
    End Function


End Class
