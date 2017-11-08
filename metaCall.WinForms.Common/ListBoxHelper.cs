using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms
{
    public static class ListBoxHelper
    {

        public static void SwitchListBoxItems<T>(ListBox source, ListBox target) where T : class
        {
            //Prüfen der ListBoxeinträge
            if ((source.Items.Count > 0) &&
                (typeof(T) != ListBindingHelper.GetListItemType(source.Items)))
            {
                throw new InvalidCastException("ListType ist nicht gleich T");
            }

            //Prüfen der ListBoxeinträge
            if ((target.Items.Count > 0) &&
                (typeof(T) != ListBindingHelper.GetListItemType(target.Items)))
            {
                throw new InvalidCastException("ListType ist nicht gleich T");
            }

            T item = source.SelectedItem as T;

            if (item != null)
            {
                target.Items.Add(item);
                target.SelectedItem = item;

                source.Items.Remove(item);
                if (source.Items.Count > 0)
                    source.SelectedIndex = 0;
            }
        }

    }
}
