using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class DateTimePickerExt : UserControl
    {
        public event EventHandler ValueChanged;

        private Button[] hourButtons;
        private Button[] minuteButtons;

        private int[] minutes = new int[]{0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55};
        private int[] hours = new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 , 21, 22};

        private int vPosHours;
        private int vPosMinutes;

        private DateTime currentDate;

        /// <summary>
        /// Liefert das aktuelle Datum oder legt dieses fest. (Betrifft nur die Kalenderanzeige
        /// </summary>
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; }
        }
        
        private int currentHourIndex;
        private int currentMinuteIndex;

        public DateTimePickerExt()
        {
            InitializeComponent();

            this.monthCalendar1.TrailingForeColor = Color.Gray;
            this.monthCalendar1.MaxSelectionCount = 1;
            /* berechnen des Platzbedarfs für die Stunden/Minutenanzeige */
            Size btnSize = TextRenderer.MeasureText("MM", this.Font);
            btnSize.Height += (2 * DefaultMargin.All);
            btnSize.Width += (2 * DefaultMargin.All);

            vPosHours = monthCalendar1.Right;
            vPosMinutes = vPosHours + (2 * btnSize.Width) + 3;

            Size = new Size(vPosHours + (btnSize.Width * 4) + 3 + Margin.Vertical, monthCalendar1.Bottom);
            MinimumSize = this.Size;
            MaximumSize = MinimumSize;

            CreateHourButtons();
            CreateMinuteButtons();

        }

        /// <summary>
        /// Liefert das aktuelle Datum und Uhrzeit oder legt dieses fest. (Betrifft Kalender und Zeitanzeige)
        /// </summary>
        public DateTime Value
        {
            get {

                return this.CurrentDate;
            }
            set {
                this.monthCalendar1.SelectionStart = value.Date;
                this.currentMinuteIndex = FindArrayIndex(minutes, value.Minute);
                this.currentHourIndex = FindArrayIndex(hours, value.Hour);

                SetSelectedDate();
                UpdateControls();

                OnValueChanged(EventArgs.Empty);
                
            }
        }

        /// <summary>
        /// liefert die Liste der verfügbaren Minuten
        /// </summary>
        public int[] Minutes
        {
            get
            {
                return this.minutes;
            }
        }

        /// <summary>
        /// Liefert die Liste der verfügbaren Stunden
        /// </summary>
        public int[] Hours
        {
            get
            {
                return this.hours;
            }
        }

        /// <summary>
        /// ruft das maximale zulässige Datum ab, oder legt dieses fest.
        /// </summary>
        public DateTime MaxDate
        {
            get
            {
                return this.monthCalendar1.MaxDate;
            }
            set
            {
                if (value.Date <= DateTime.Today.Date)
                    this.monthCalendar1.MaxDate = DateTime.Today;
                else
                    this.monthCalendar1.MaxDate = value;
            }
        }

        /// <summary>
        /// ruft das minimale zulässige Datum ab oder legt dieses fest.
        /// </summary>
        public DateTime MinDate
        {
            get
            {
                return this.monthCalendar1.MinDate;
            }
            set
            {
                this.monthCalendar1.MinDate = value.Date;
            }
        }

        private void UpdateControls()
        {
            this.SuspendLayout();
            DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
           // this.lblHeader.Text = string.Format("{0:d} {0:t}", this.CurrentDate);

            //this.monthCalendar1.SelectionStart = currentDate;

            foreach (Button btn in hourButtons)
            {
                if (Array.IndexOf(hourButtons, btn)  == currentHourIndex)
                {
                    btn.BackColor = ControlPaint.ContrastControlDark;
                }
                else
                {
                    btn.BackColor = Color.Transparent;
                }

            }

            foreach (Button btn in minuteButtons)
            {
                if (Array.IndexOf(minuteButtons, btn) == currentMinuteIndex)
                {
                    btn.BackColor = ControlPaint.ContrastControlDark;
                }
                else
                {
                    btn.BackColor = Color.Transparent;
                }
            }

            this.ResumeLayout();
        }

        private void SetSelectedDate()
        {
            DateTime date = this.monthCalendar1.SelectionStart.Date;
            date = date.AddHours(hours[this.currentHourIndex]);
            date = date.AddMinutes((double) this.minutes[this.currentMinuteIndex]);
            
            Console.WriteLine("Date set {0}", date);
            
            this.CurrentDate = date;
        }

        private void CreateHourButtons()
        {
            int hPos = monthCalendar1.Top;
            int vPos = vPosHours;

            hourButtons = new Button[hours.Length];

            for (int i = 0; i < hours.Length; i++)
            {
                Button btn = CreateButton();
                btn.Name = "HourButton" + i.ToString();
                btn.Text = hours[i].ToString();

                btn.Left = vPos;
                btn.Top = hPos;

                btn.Click+=new EventHandler(HourButton_Click);

                this.Controls.Add(btn);
                hourButtons[i] = btn;


                if ((i % 2) == 0)
                {
                    vPos += btn.Width;
                }
                else
                {
                    vPos = vPosHours;
                    hPos += btn.Height;
                }
                
            }
        }

        private void CreateMinuteButtons()
        {
            int hPos = monthCalendar1.Top;

            int vPos = vPosMinutes;

            minuteButtons = new Button[minutes.Length];

            for (int i = 0; i < minutes.Length; i++)
            {
                Button btn = CreateButton();
                btn.Name = "MinuteButton" + i.ToString();
                btn.Text = minutes[i].ToString();
                btn.Left = vPos;
                btn.Top = hPos;

                btn.Click += new EventHandler(MinuteButton_Click);

                this.Controls.Add(btn);
                minuteButtons[i] = btn;


                if ((i % 2) == 0)
                {
                    vPos += btn.Width;
                }
                else
                {
                    vPos = vPosMinutes;
                    hPos += btn.Height;
                }
            }
        }

        private Button CreateButton()
        {
            Font btnFont = this.Font;
            Size btnSize = TextRenderer.MeasureText("MM", btnFont);
            btnSize.Height += 2 * DefaultMargin.All;
            btnSize.Width += 2 * DefaultMargin.All;

            Button btn = new Button();
            btn.Visible = true;
            btn.Font = btnFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = btnSize;
            return btn;

        }

        private void HourButton_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(hourButtons, sender);
            this.currentHourIndex = index;
            SetSelectedDate();

            UpdateControls();

            OnValueChanged(EventArgs.Empty);
        }

        private void MinuteButton_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(minuteButtons, sender);
            this.currentMinuteIndex = index;
            SetSelectedDate();
            UpdateControls();

            OnValueChanged(EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            Graphics gfx = e.Graphics;

            Pen pen = new Pen(System.Windows.Forms.ProfessionalColors.SeparatorDark, 1.0f);

            int hPos;

            hPos = (monthCalendar1.Right + Margin.Vertical) + (Width - (monthCalendar1.Right + Margin.Vertical)) / 2;
            hPos -= 3;
            gfx.DrawLine(pen,
                new Point(hPos, monthCalendar1.Top),
                new Point(hPos, monthCalendar1.Bottom + 10)
                );
            hPos++;

            pen = new Pen(System.Windows.Forms.ProfessionalColors.SeparatorLight, 1.0f);
            gfx.DrawLine(pen,
                new Point(hPos, monthCalendar1.Top),
                new Point(hPos, monthCalendar1.Bottom)
                );        
        }

        private int FindArrayIndex(int[] array, int value)
        {
            int index = 0;
            for (int i = 0; i < array.Length; i++)
            {
                index = i;
                if (array[i] >= value)
                {
                    break;
                }
            }

            return index;
        }
        
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            //this.currentDate = e.Start.Date;
            //SetSelectedDate();
            //UpdateControls();

            //OnValueChanged(EventArgs.Empty);
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            //this.currentDate = e.Start.Date;
            SetSelectedDate();
            OnValueChanged(EventArgs.Empty);
        }

        protected void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);

        }

    }
}
