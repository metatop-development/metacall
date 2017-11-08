using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public class PhoneNumberTextBox: TextBox
    {
       
        private bool nonNumberEntered = false;

        public override string Text
        {
            get
            {
                return TrimText(base.Text).Trim();
            }
            set
            {
                if (value != null)
                    base.Text = TrimText(value.Trim());
                else
                    base.Text = TrimText(value) ;
            }
        }

        private string TrimText(string value)
        {
            if (value == null)
                return null;
            
            char[] chars = value.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] < '0' || chars[i] > '9')
                    chars[i] = ' ';
            }

            return new string(chars);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
        }
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            // Initialize the flag to false.
            nonNumberEntered = false;

            Console.WriteLine(e.KeyCode.ToString());

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if ((e.KeyCode != Keys.Back) &&
                        (e.KeyCode != Keys.Space))
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }


        }
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (nonNumberEntered)
            {
                e.Handled = true;
            }
        }

    }
}
