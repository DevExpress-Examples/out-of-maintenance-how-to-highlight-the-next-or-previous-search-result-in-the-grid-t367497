using DevExpress.Utils.Paint;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public class FindHelper : Component
    {
        public FindHelper()
        {
            BackGroundColor = Color.Green;
            HighLightColor = Color.Gold;
            
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }



        ButtonEdit edit;
        GridControl grid;
        GridView view;
        EditorButton showResult;
        Timer timer = new Timer();
        string filterCellText = string.Empty;
        Dictionary<GridCell, bool> findList = new Dictionary<GridCell, bool>();

        bool FindListIsEmpty { get { return findList.Keys.Count == 0; } }
        public Color BackGroundColor { get; set; }
        public Color HighLightColor { get; set; }
        public int AutomaticallyPerformSearchAfter
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                timer.Interval = value;
            }
        }

        public GridControl TargetControl
        {
            get
            {
                return grid;
            }
            set
            {
                SubscibeViewEvent(false);
                grid = value;
                ResetActiveGrid();
            }
        }

        public ButtonEdit SearchControl
        {
            get
            {
                return edit;
            }
            set
            {
                SubscribeRIEvent(false);
                edit = value;
                ResetActiveRI();
            }
        }

        private void ResetActiveRI()
        {
            if (DesignMode) return;
            edit.Properties.Buttons.Clear();
            showResult = new EditorButton(ButtonPredefines.Glyph, "0 of 0", 0, false, true, false, ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(Keys.None), null, "", null, null, true);
            edit.Properties.Buttons.AddRange(new EditorButton[] {
            new EditorButton(ButtonPredefines.Search),
            new EditorButton(ButtonPredefines.Clear),
            new EditorButton(ButtonPredefines.SpinLeft),
            new EditorButton(ButtonPredefines.SpinRight),
            showResult
            });

            SubscribeRIEvent(true);
        }

        private void SubscribeRIEvent(bool subscribe)
        {
            if (edit == null) return;
            edit.Properties.ButtonClick -= ButtonClick;
            edit.Properties.KeyUp -= Editor_KeyUp;
            edit.Properties.KeyDown -= Editor_KeyDown;
            if (subscribe)
            {
                edit.Properties.KeyUp += Editor_KeyUp;
                edit.Properties.KeyDown += Editor_KeyDown;
                edit.Properties.ButtonClick += ButtonClick;
            }
        }

        void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            StopTimer();
            switch (e.KeyData)
            {
                case Keys.Enter:
                    edit.PerformClick(edit.Properties.Buttons[0]);
                    break;
                case (Keys.Control | Keys.Left):
                    edit.PerformClick(edit.Properties.Buttons[1]);
                    break;
            }
        }

     
        void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            StratTimer();
        }


        private void ResetActiveGrid()
        {
            if (grid == null) return;
            view = grid.MainView as GridView;
            SubscibeViewEvent(true);
        }

        private void SubscibeViewEvent(bool subscribe)
        {
            if (view == null) return;
            view.CustomDrawCell -= CustomDrawCell;
            if(subscribe)
                view.CustomDrawCell += CustomDrawCell;
        }

        protected override void Dispose(bool disposing)
        {
            SubscibeViewEvent(false);
            SubscribeRIEvent(false);
            base.Dispose(disposing);
        }
      
        private void CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (FindListIsEmpty) return;

            int filterTextIndex = e.DisplayText.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase);
            if (filterTextIndex == -1)
                return;
            GridCell temp = new GridCell(e.RowHandle, e.Column);

            if(NeedHighLight(temp)) {
                e.Appearance.BackColor = BackGroundColor;
                e.Cache.FillRectangle(BackGroundColor, e.Bounds);
            }

            GridCellInfo gci = e.Cell as GridCellInfo;
            TextEditViewInfo tevi = gci.ViewInfo as TextEditViewInfo;
            if (tevi == null)
                return;
            Rectangle textRect = new Rectangle(e.Bounds.X + tevi.MaskBoxRect.X, e.Bounds.Y + tevi.MaskBoxRect.Y, tevi.MaskBoxRect.Width, tevi.MaskBoxRect.Height);
            e.Cache.Paint.DrawMultiColorString(e.Cache, textRect, e.DisplayText, filterCellText, e.Appearance,e.Appearance.ForeColor , HighLightColor, false, filterTextIndex);            

            e.Handled = true;
        }

        void ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit edit = sender as ButtonEdit;
            switch (e.Button.Kind)
            {
                case ButtonPredefines.Search:
                    PerformSearch(edit.EditValue);
                    break;
                case ButtonPredefines.Clear:
                    ClearEditValue(edit);
                    PerformSearch(null);
                    break;
                case ButtonPredefines.SpinLeft:
                    HighLightPrevious();
                    break;
                case ButtonPredefines.SpinRight:
                    HighLightNext();
                    break;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            PerformSearch(edit.EditValue);
        }

        private void UpdateShowResult()
        {
            int index;
            for (index = 0; index < findList.Keys.Count; index++)
                if (findList[findList.Keys.ElementAt(index)])
                {
                    index++;
                    break;
                }
            showResult.Caption = string.Format("{0} of {1}", index, findList.Keys.Count);
        }

        private void HighLightPrevious()
        {

            if (FindListIsEmpty) return;

            GridCell currItem = findList.Keys.ElementAt(0);
            GridCell targetItem = findList.Keys.ElementAt(findList.Keys.Count - 1);
            GridCell temp;
            for (int i = 1; i < findList.Keys.Count; i++)
            {
                temp = findList.Keys.ElementAt(i);
                if (findList[temp])
                {
                    targetItem = findList.Keys.ElementAt(i - 1);
                    currItem = temp;
                    break;
                }
            }

            findList[currItem] = false;
            findList[targetItem] = true;
            EnsureCellVisible(targetItem);
            RefreshGridView();
            UpdateShowResult();
        }

        private void HighLightNext()
        {
            if (FindListIsEmpty) return;

            bool needBreak = false;
            GridCell currItem = null;
            GridCell targetItem = null;
            foreach (GridCell item in findList.Keys)
            {
                if (needBreak)
                {
                    targetItem = item;
                    break;
                }
                if (findList[item])
                {
                    currItem = item;
                    needBreak = true;
                }
            }

            if (targetItem == null)
                targetItem = findList.Keys.ElementAt(0);

            findList[currItem] = false;
            findList[targetItem] = true;
            EnsureCellVisible(targetItem);

            RefreshGridView();
            UpdateShowResult();
        }

        private void EnsureCellVisible(GridCell cell)
        {
            view.MakeRowVisible(cell.RowHandle);
            view.MakeColumnVisible(cell.Column);
        }

        private void ClearEditValue(ButtonEdit edit)
        {
            edit.EditValue = null;
        }

        private void PerformSearch(object val)
        {
            findList.Clear();
            StopTimer();
            if (val == null) val = string.Empty;
            filterCellText = val.ToString();
            InitFindList();
            if (!FindListIsEmpty)
                EnsureCellVisible(findList.Keys.ElementAt(0));
            RefreshGridView();
            UpdateShowResult();
        }

        private void InitFindList()
        {
            if (String.IsNullOrEmpty(filterCellText))
                return;   
            string text;
            for (int i = 0; i < view.RowCount; i++)
                foreach (GridColumn col in view.Columns)
                {
                    if (!col.Visible || (col.RealColumnEdit as RepositoryItemTextEdit) == null) 
                        continue;
                   
                    text = view.GetRowCellDisplayText(i, col);
                   
                    int filterTextIndex = text.IndexOf(filterCellText, StringComparison.CurrentCultureIgnoreCase);
                    if (filterTextIndex != -1)
                        findList.Add(new GridCell(i, col), false);
                }
            if (FindListIsEmpty) return;
            findList[findList.Keys.ElementAt(0)] = true;
        }  

        private void RefreshGridView()
        {
            view.LayoutChanged();
        }

        private bool NeedHighLight(GridCell cell)
        {
            foreach (GridCell item in findList.Keys)
                if (item.RowHandle == cell.RowHandle && item.Column == cell.Column)
                    return findList[item];
            return false;
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private void StratTimer()
        {
            timer.Start();
        }
    }
}
