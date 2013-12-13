using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Util;
using Android.Widget;

namespace polymorphandroid
{
    class MultiSelectionSpinner : Spinner, IDialogInterfaceOnMultiChoiceClickListener
    {
        private string[] items;
        private bool[] selection;
        private ArrayAdapter<string> simpleAdapter;
        private Context context;

        public MultiSelectionSpinner(IntPtr a, Android.Runtime.JniHandleOwnership b) : base(a, b)
        {
        }

        public MultiSelectionSpinner(Context c) : base(c, null)
        {
            context = c;
            simpleAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem);
            Adapter = simpleAdapter;
        }

        public MultiSelectionSpinner(Context c, IAttributeSet attrs) : base(c, attrs)
        {
            context = c;
            simpleAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem);
            Adapter = simpleAdapter;
        }

        public MultiSelectionSpinner(Context c, IAttributeSet attr, int defStyle) : 
            base(c, attr, defStyle)
        {
            context = c;
            simpleAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem);
            Adapter = simpleAdapter;
        }

        public void OnClick(IDialogInterface dialog, int which, bool isChecked)
        {
            if (selection != null && which < selection.Length)
            {
                selection[which] = isChecked;
                simpleAdapter.Clear();
                simpleAdapter.Add(buildSelectedItemString());
            }
            else
            {
                throw new Exception("Argument 'which' is out of bounds");
            }
        }

        public override bool PerformClick()
        {
            var builder = new AlertDialog.Builder(context);
            builder.SetMultiChoiceItems(items, selection, this);
            builder.Show();
            return true;
        }

        public override ISpinnerAdapter Adapter
        {
            get
            {
                return base.Adapter;
            }
            set
            {
                throw new Exception("setAdapter is not supported by MultiSelectSpinner");
            }
        }

        public void SetItems(string[] i)
        {
            items = i;
            selection = new bool[items.Length];
            simpleAdapter.Clear();
            simpleAdapter.Add(items[0]);
            selection = selection.Select(t => false).ToArray();
        }

        public void SetItems(List<string> i)
        {
            items = i.ToArray();
            selection = new bool[items.Length];
            simpleAdapter.Clear();
            simpleAdapter.Add(items[0]);
            selection = selection.Select(t => false).ToArray();
        }

        public void SetSelection(string[] sel)
        {
            foreach (var c in sel)
            {
                for (int j = 0; j < items.Length; ++j)
                    if (items[j] == c)
                        selection[j] = true;
            }
        }

        public void SetSelection(List<string> sel)
        {
            selection = selection.Select(t => false).ToArray();
            foreach (var s in sel)
            {
                for (int j = 0; j < items.Length; ++j)
                    if (items[j] == s)
                        selection[j] = true;
            }
            simpleAdapter.Clear();
            simpleAdapter.Add(buildSelectedItemString());
        }

        public void SetSelection(int[] selectedIndices)
        {
            selection = selection.Select(t => false).ToArray();
            foreach (var index in selectedIndices)
            {
                if (index >= 0 && index < selection.Length)
                    selection[index] = true;
                else
                {
                    throw new Exception(string.Format("Index {0} is out of bounds", index));
                }
            }
            simpleAdapter.Clear();
            simpleAdapter.Add(buildSelectedItemString());
        }

        public void SetSelection(int index)
        {
            selection = selection.Select(t => false).ToArray();
            if (index >= 0 && index < selection.Length)
                selection[index] = true;
            else
            {
                throw new Exception(string.Format("Index {0} is out of bounds", index));
            }
            simpleAdapter.Clear();
            simpleAdapter.Add(buildSelectedItemString());
        }

        public List<string> GetSelectedString()
        {
            var select = new List<string>();
            for (int i = 0; i < items.Length; ++i)
                if (selection[i])
                    select.Add(items[i]);
            return select;
        }

        public List<int> GetSelectedIndices()
        {
            var select = new List<int>();
            for (int i = 0; i < items.Length; ++i)
                if (selection[i])
                    select.Add(i);
            return select;
        }

        private string buildSelectedItemString()
        {
            var sb = new StringBuilder();
            bool foundOne = false;
            for (int i = 0; i < items.Length; ++i)
            {
                if (selection[i])
                {
                    if (foundOne)
                        sb.Append(", ");
                    foundOne = true;
                    sb.Append(items[i]);
                }
            }
            return sb.ToString();
        }
    }
}
