using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WinR
{
    internal class FilteredComboBox : ComboBox
    {
        public FilteredComboBox()
        {
            //DropDownClosed += FilteredComboBox_DropDownClosed;
            //this.dropd
            IsDropDownOpen = true;
            StaysOpenOnEdit = true;

        }

        //private void FilteredComboBox_DropDownClosed(object? sender, System.EventArgs e)
        //{
        //    e.
        //}

        //protected override void DropDown(EventArgs e)
        //{
        //    //IsDropDownOpen = true;
        //    //base.OnDropDownClosed(e);
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //e.Handled = true;
                return;
            }

        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //this.IsDropDownOpen = true;
            if (e.Key == Key.Right && GetCaret() == Text.Length)
            {
                SelectedIndex = 0;
            }
            if (e.Key == Key.Up && SelectedIndex == 0)
            {
                SetCaret(Text.Length);
            }
            if (e.Key == Key.Back)
            {
                var tmp = Text;
                SelectedIndex = -1;
                this.Text = tmp;
                SetCaret(tmp.Length);
            }
            //base .OnKeyUp(e);
            //return;
            if (e.Key == Key.Down)
            {
                if (!IsDropDownOpen)
                {
                    //this.IsDropDownOpen = true;
                    //e.Handled = true;
                }

            }
            else if (e.Key == Key.Up)
            {
                //SelectedIndex = -1;
            }
            else
            {
                RefreshFilter();
            }

        }

        private void RefreshFilter()
        {
            if (this.ItemsSource != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(this.ItemsSource);
                view.Refresh();
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(newValue);
                view.Filter += this.FilterPredicate;
            }

            if (oldValue != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(oldValue);
                view.Filter -= this.FilterPredicate;
            }

            //base.OnItemsSourceChanged(oldValue, newValue);
        }

        private bool FilterPredicate(object value)
        {
            // No filter, no text
            if (value == null)
            {
                return false;
            }

            // No text, no filter
            if (this.Text.Length == 0)
            {
                return true;
            }

            // Case insensitive search
            return value.ToString().ToLower().Contains(this.Text.ToLower());
        }


        //

        TextBox editableTextBox = new TextBox();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var myTextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            if (myTextBox != null)
            {
                this.editableTextBox = myTextBox;
            }
        }

        public void SetCaret(int position)
        {
            this.editableTextBox.SelectionStart = position;
            this.editableTextBox.SelectionLength = 0;
        }
        
        public int GetCaret()
        {
            if (editableTextBox.SelectionLength == 0)
            {
                return this.editableTextBox.SelectionStart;
            }
            return -1;
        }
    }
}
