using DemoWindowsApp.Demo_App_Models;
using DemoWindowsApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DemoWindowsApp
{
    public partial class Schedules : Form
    {
        private readonly LoginDataModel.Login _loginDetails;
        public Schedules(LoginDataModel.Login login)
        {
            InitializeComponent();
            _loginDetails = login;
        }


        /// <summary>
        /// Load active schedule list for loggedin user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Schedules_Load(object sender, EventArgs e)
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            //this.groupBox1.Location = new Point(0, 0);
            this.groupBox1.Size = new Size(w, h);
            this.groupBox1.MaximumSize = this.Size;
            // API call to get loggedin user's list of active tests and schedules.
            using (var client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                };

                client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/GetActiveTestListForDemo/" + _loginDetails.data.examineeId);
                client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var UserDeliverTestResponse = JsonConvert.DeserializeObject<List<UserDeliverTestResponse>>(result.Result);
                    if (UserDeliverTestResponse.Count > 0)
                    {
                        foreach (var s in UserDeliverTestResponse)
                        {
                            if (s.IsScheduleStarted.Value && !s.examCompleted && string.IsNullOrEmpty(s.testStartDate.ToString()))
                            {
                                dataGridView1.Rows.Add(s.testTitle, "", "Start", JsonConvert.SerializeObject(s));
                            }
                            else if (s.IsScheduleStarted.Value && !s.examCompleted && !string.IsNullOrEmpty(s.testStartDate.ToString()))
                            {

                                dataGridView1.Rows.Add(s.testTitle, "", "Restart", JsonConvert.SerializeObject(s));
                            }
                            else if (s.examCompleted)
                            {
                                dataGridView1.Rows.Add(s.testTitle, "Completed", "", "");

                            }
                        }
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[1] != null && !string.IsNullOrWhiteSpace((string)row.Cells[1].Value) && row.Cells[1].Value.ToString().Trim().ToLower() == "completed")
                            {
                                DataGridViewDisableButtonCell buttonCell = (DataGridViewDisableButtonCell)dataGridView1.Rows[row.Index].Cells[2];
                                buttonCell.Enabled = false;
                            }

                        }
                    }
                    else
                    {
                        dataGridView1.Rows.Add("No active schedule(s) found. Please contact your system administrator.");
                    }
                }
                else
                {
                    var result = response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<ErrorClassModel>(result.Result);
                    if (error.message.Trim().ToLower() == "schedeule not found")
                    {
                        dataGridView1.Rows.Add("No active schedule(s) found. Please contact your system administrator.");
                    }
                    else
                    {
                        dataGridView1.Rows.Add("Something went wrong. Please try again.");
                    }
                }
            }

        }


        /// <summary>
        /// On Schedule close button click, close the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Schedules_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = e.RowIndex;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index == a)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (row.Cells[1].Value.ToString().Trim().ToLower() != "completed")
                    {

                        var jsondata = JsonConvert.DeserializeObject<UserDeliverTestResponse>(row.Cells[3].Value.ToString());
                        Questions formQuestion = new Questions(_loginDetails, jsondata);
                        formQuestion.Show();
                        formQuestion.TopMost = false;
                        this.Hide();
                        this.Visible = false;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void lblScheduleTitle_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell = (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground = new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text.
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
