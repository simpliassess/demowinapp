using DemoWindowsApp.Demo_App_Models;
using DemoWindowsApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DemoWindowsApp.Demo_App_Models.LoginDataModel;

namespace DemoWindowsApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblLoginError.Hide();
            lblEmailValidation.Hide();
            lblPasswordValidation.Hide();
        }
        /// <summary>
        /// This click event is used when user click on the login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, EventArgs e)
        {
            lblLoginError.Hide();
            lblLoginError.Text = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            var emailRegEx = new System.Text.RegularExpressions.Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            var isValid = false;

            if (string.IsNullOrWhiteSpace(txtUserEmail.Text))
            {
                lblEmailValidation.Text = "Email is required";
                lblEmailValidation.ForeColor = Color.Red;
                lblEmailValidation.Show();
            }
            else if (!emailRegEx.IsMatch(txtUserEmail.Text))
            {
                lblEmailValidation.Text = "Invalid email";
                lblEmailValidation.ForeColor = Color.Red;
                lblEmailValidation.Show();
            }
            else
            {
                isValid = true;
                lblEmailValidation.Text = string.Empty;
                lblEmailValidation.Hide();
            }

            if (string.IsNullOrWhiteSpace(txtUserpassword.Text))
            {
                isValid = false;
                lblPasswordValidation.Text = "Password is required";
                lblPasswordValidation.ForeColor = Color.Red;
                lblPasswordValidation.Show();
            }
            else
            {
                lblPasswordValidation.Text = string.Empty;
                lblPasswordValidation.Hide();
            }

            if (isValid)
            {
                using (var client = new HttpClient())
                {
                    var req = new { emailId = txtUserEmail.Text, password = txtUserpassword.Text };
                    client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/login");
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8,
                                        "application/json");
                    var httpRequestMessage2 = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        Content = httpContent,
                    };
                    client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                    client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                    HttpResponseMessage response;
                    response = client.SendAsync(httpRequestMessage2).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync();
                        LoginDataModel.Login login = JsonConvert.DeserializeObject<LoginDataModel.Login>(result.Result);
                        Schedules form = new Schedules(login);
                        form.Show();
                        this.Visible = false;
                    }
                    else
                    {
                        var result = response.Content.ReadAsStringAsync();
                        LoginErrorModel login = JsonConvert.DeserializeObject<LoginErrorModel>(result.Result);
                        lblLoginError.Show();
                        lblLoginError.Text = login.message;//"Your email or password is incorrect. Try again";
                        lblLoginError.ForeColor = Color.Red;
                    }
                }
            }
            Cursor.Current = Cursors.Default;

        }

        /// <summary>
        /// On Close login button click ends the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
