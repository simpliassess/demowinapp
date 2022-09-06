using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using DemoWindowsApp.Demo_App_Models;
using DemoWindowsApp.Extensions;
using DemoWindowsApp.Properties;

namespace DemoWindowsApp
{
    public partial class Questions : Form
    {
        private readonly LoginDataModel.Login _loginDetails;
        private readonly UserDeliverTestResponse _userDeliverTestResponse;
        List<string> ItemUuidList = new List<string>();
        public Questions(LoginDataModel.Login login, UserDeliverTestResponse userDeliverTestResponse)
        {
            InitializeComponent();
            // StartPosition was set to FormStartPosition.Manual in the properties window.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point(0, 0); //this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);
            this.MaximumSize = this.Size;
            _loginDetails = login;
            _userDeliverTestResponse = userDeliverTestResponse;
        }
        /// <summary>
        /// Method used to load first page of the test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQuestions_Load(object sender, EventArgs e)
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point(0, 0);
            this.Size = new Size(w, h);
            this.MaximumSize = this.Size;
            var firstitemUuid = string.Empty;
            this.listView1.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.None);
            // API call to get the Item list
            using (var client = new HttpClient())
            {
                var Testuuid = _userDeliverTestResponse.testbankuuId;
                var Usertestuuid = _userDeliverTestResponse.usertestuuId;

                client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/test/" + Testuuid + "/items?userTestuuId=" + Usertestuuid);
                client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage response;
                response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var TestItemDetailsResponse = JsonConvert.DeserializeObject<TestItemDetailsResponse>(result.Result);
                    if (TestItemDetailsResponse != null)
                    {
                        lblTestName.Text = _userDeliverTestResponse.testTitle;
                        lblTestDescription.Text = _userDeliverTestResponse.testDescription;
                        listView1.FullRowSelect = true;
                        ListViewExtender extender = new ListViewExtender(listView1);

                        extender.ListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                        var buttonAction = new ListViewButtonColumn(1)
                        {
                            FixedWidth = true
                        };
                        extender.AddColumn(buttonAction);
                        foreach (var ques in TestItemDetailsResponse.items)
                        {
                            listView1.Items.Add(ques.itemBankName).Name = JsonConvert.SerializeObject(ques);
                            ItemUuidList.Add(ques.itemuuId);
                        }
                        firstitemUuid = TestItemDetailsResponse.items[0].itemuuId;
                    }
                }
            }
            // Api call to get the questions of first item 
            using (var client = new HttpClient())
            {
                btnback.Hide();
                var Testuuid = _userDeliverTestResponse.testbankuuId;
                var Usertestuuid = _userDeliverTestResponse.usertestuuId;

                client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/testDemo/" + Usertestuuid + "/items/" + firstitemUuid + "/questions");
                client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                };

                HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();
                    var TestItemDetailsResponse = JsonConvert.DeserializeObject<ItemQuestionModel>(result.Result);
                    if (TestItemDetailsResponse != null)
                    {
                        foreach (var item in TestItemDetailsResponse.items)
                        {
                            if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "yes or no")
                            {
                                GroupBox groupBox = new GroupBox
                                {
                                    AutoSize = true,
                                    Top = 70,
                                    Left = 40
                                };

                                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                {
                                    Margin = new Padding(10, 30, 10, 10),
                                    FlowDirection = FlowDirection.TopDown
                                };
                                groupBox.FlatStyle = FlatStyle.Standard;
                                flowLayoutPanel.AutoSize = true;
                                Label label = new Label
                                {
                                    Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                    Visible = true,
                                    TextAlign = (ContentAlignment)HorizontalAlignment.Center,
                                    AutoSize = true,
                                    Top = 70,
                                    Left = 40,
                                    Name = item.questionuuId + "," + item.attempted
                                };
                                flowLayoutPanel.Controls.Add(label);
                                foreach (var options in item.options)
                                {
                                    RadioButton radio = new RadioButton
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                        Checked = options.selected,
                                        Visible = true,
                                        AutoSize = true,
                                        Margin = new Padding(25, 5, 5, 0),
                                        Name = item.questionuuId + "," + firstitemUuid + "," + options.optiId
                                    };
                                    groupBox.Controls.Add(radio);
                                    flowLayoutPanel.Controls.Add(radio);
                                }
                                flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                            }
                            else if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "standard")
                            {
                                GroupBox groupBox = new GroupBox
                                {
                                    AutoSize = true,
                                    Top = 70,
                                    Left = 40,
                                    FlatStyle = FlatStyle.Standard
                                };

                                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                {
                                    Margin = new Padding(10, 30, 10, 10),
                                    FlowDirection = FlowDirection.TopDown,
                                    AutoSize = true
                                };
                                Label label = new Label
                                {
                                    Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                    Visible = true,
                                    AutoSize = true,
                                    Top = 50,
                                    Left = 40,
                                    Name = item.questionuuId + "," + item.attempted
                                };
                                flowLayoutPanel.Controls.Add(label);
                                foreach (var options in item.options)
                                {
                                    RadioButton radio = new RadioButton
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                        Checked = options.selected,
                                        Visible = true,
                                        AutoSize = true,
                                        Margin = new Padding(25, 5, 5, 0),
                                        Name = item.questionuuId + "," + firstitemUuid + "," + options.optiId
                                    };
                                    groupBox.Controls.Add(radio);
                                    flowLayoutPanel.Controls.Add(radio);
                                }
                                flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                            }
                            else if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "multiple options")
                            {
                                GroupBox groupBox = new GroupBox
                                {
                                    AutoSize = true,
                                    Top = 70,
                                    Left = 40,
                                    FlatStyle = FlatStyle.Standard
                                };

                                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                {
                                    Margin = new Padding(10, 30, 10, 10)
                                };

                                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                                flowLayoutPanel.AutoSize = true;
                                Label label = new Label
                                {
                                    Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                    Visible = true,
                                    AutoSize = true,
                                    Top = 50,
                                    Left = 40,
                                    Name = item.questionuuId + "," + item.attempted
                                };
                                flowLayoutPanel.Controls.Add(label);
                                foreach (var options in item.options)
                                {
                                    CheckBox radio = new CheckBox
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                        Checked = options.selected,
                                        Visible = true,
                                        AutoSize = true,
                                        Margin = new Padding(25, 5, 5, 0),
                                        Name = item.questionuuId + "," + firstitemUuid + "," + options.optiId
                                    };
                                    groupBox.Controls.Add(radio);
                                    flowLayoutPanel.Controls.Add(radio);
                                }
                                flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                            }

                        }
                        listView1.Focus();
                        listView1.Items[0].Selected = true;
                    }
                    if (TestItemDetailsResponse != null)
                    {
                        if (ItemUuidList.First().Equals(firstitemUuid))
                        {
                            var findIndex = ItemUuidList.FindIndex(c => c.Contains(firstitemUuid));
                            btnNext.Text = "Next";
                            btnNext.Name = ItemUuidList.ElementAtOrDefault(findIndex + 1);
                            btnback.Hide();
                            btnback.Visible = false;
                            btnback.Text = "Back";
                            btnback.Name = ItemUuidList.ElementAtOrDefault(findIndex - 1);
                        }
                        else if (ItemUuidList.Last().Equals(firstitemUuid))
                        {
                            btnNext.Text = "Finish";
                            btnNext.Name = firstitemUuid;
                            var findIndex = ItemUuidList.FindIndex(c => c.Contains(firstitemUuid));
                            btnback.Text = "Back";
                            btnback.Name = ItemUuidList.ElementAtOrDefault(findIndex - 1);
                        }
                        else
                        {
                            var findIndex = ItemUuidList.FindIndex(c => c.Contains(firstitemUuid));
                            btnNext.Text = "Next";
                            btnNext.Name = ItemUuidList.ElementAtOrDefault(findIndex + 1);
                            btnback.Text = "Back";
                            btnback.Name = ItemUuidList.ElementAtOrDefault(findIndex - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show item questions for the selected item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///  Method to get the next item questions or to finish the test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            string checkdata = btnNext.Name;
            if (btnNext.Text == "Finish")
            {
                Cursor.Current = Cursors.WaitCursor;
                // save for the last form
                NewTestAnswerRequest newTestAnswerRequest = new NewTestAnswerRequest
                {
                    userTestuuId = _userDeliverTestResponse.usertestuuId,
                    testuuId = _userDeliverTestResponse.testbankuuId
                };
                List<NewAnswers> answers = new List<NewAnswers>();
                foreach (var ctls in flowLayoutPanel1.Controls)
                {
                    var s = (FlowLayoutPanel)ctls;
                    var questionlable = s.Controls.OfType<Label>().FirstOrDefault();
                    var radiochecked = s.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    var checkboxchecked = s.Controls.OfType<CheckBox>().ToList().Where(r => r.Checked).ToList();

                    NewAnswers newAnswers = new NewAnswers
                    {
                        questionuuId = questionlable.Name.Split(',')[0],
                        optionId = new List<short>()
                    };

                    var isAttempted = !string.IsNullOrEmpty(questionlable.Name.Split(',')[1]) ? Convert.ToBoolean(questionlable.Name.Split(',')[1]) : false;
                    List<short> optionId = new List<short>();

                    if (isAttempted)
                    {
                        if (checkboxchecked != null)
                        {
                            if (checkboxchecked.Count > 0)
                            {
                                foreach (var chck in checkboxchecked.ToList())
                                {
                                    optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                    newAnswers.optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                }
                            }
                        }
                        if (radiochecked != null)
                        {
                            optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            newAnswers.optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                        }
                        answers.Add(newAnswers);
                    }
                    else
                    {
                        if (checkboxchecked != null)
                        {
                            if (checkboxchecked.Count > 0)
                            {

                                foreach (var chck in checkboxchecked.ToList())
                                {
                                    optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                    newAnswers.optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                }
                                answers.Add(newAnswers);
                            }
                        }
                        if (radiochecked != null)
                        {
                            optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            newAnswers.optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            answers.Add(newAnswers);
                        }
                    }
                }
                if (answers != null)
                {
                    newTestAnswerRequest.answers.AddRange(answers);
                    // send the data to api to save the answers
                    if (answers.Count > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(newTestAnswerRequest), Encoding.UTF8, "application/json");
                            var httpRequestMessage = new HttpRequestMessage()
                            {
                                Method = HttpMethod.Post,
                                //Headers.
                                Content = httpContent
                            };

                            client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/questions/answers");
                            client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                            client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                            HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                            if (response.IsSuccessStatusCode)
                            {
                                var result = response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
                // finish test
                // the finish test api
                flowLayoutPanel1.Controls.Clear();
                using (var client = new HttpClient())
                {
                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                    };

                    client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/users/" + _userDeliverTestResponse.usertestuuId + "/submit-test");
                    client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                    client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync();
                        var finalSubmit = JsonConvert.DeserializeObject<ScoreDto>(result.Result);
                        if (finalSubmit.submitStatus.Value)
                        {
                            listView1.Hide();
                            Label finishlable = new Label
                            {
                                AutoSize = true,
                                Text = "Test submitted successfully",
                                ForeColor = Color.Green
                            };
                            btnback.Hide();
                            btnNext.Hide();
                            Button exit = new Button
                            {
                                AutoSize = true,
                                Visible = true,
                                Text = "Close"
                            };
                            exit.Click += OnExitButtonActionClick;
                            flowLayoutPanel1.Controls.Add(finishlable);
                            flowLayoutPanel1.Controls.Add(exit);
                        }
                        else
                        {
                            listView1.Hide();
                            Label finishlable = new Label
                            {
                                Text = "Error in submitting test",
                                ForeColor = Color.Red
                            };
                            btnback.Hide();
                            btnNext.Hide();
                            flowLayoutPanel1.Controls.Add(finishlable);
                        }
                    }
                    else
                    {
                        listView1.Hide();
                        Label finishlable = new Label
                        {
                            Text = "Error in submitting test",
                            ForeColor = Color.Red
                        };
                        btnback.Hide();
                        btnNext.Hide();
                        flowLayoutPanel1.Controls.Add(finishlable);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            else
            {
                int count = 0;
                foreach (var s in listView1.Items)
                {
                    var itemDetails = ((ListViewItem)s);
                    var listviewItem = ((ListViewItem)s).Name;
                    var jsondata = JsonConvert.DeserializeObject<TestItemDetailsResponseProjection>(listviewItem);
                    var index = listView1.Items.IndexOf(itemDetails);
                    if (checkdata == jsondata.itemuuId)
                    {
                        listView1.Items[index].Selected = true;
                        listView1.Focus();
                    }
                    else
                    {
                        listView1.SelectedIndices.Remove(index);
                        listView1.Focus();
                    }
                    count++;
                }
            }
        }


        /// <summary>
        /// Method used to get previous item questions on click of back/previous button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnback_Click(object sender, EventArgs e)
        {
            // check for the answers and save 
            Cursor.Current = Cursors.WaitCursor;
            var btnbackdata = btnback.Name;
            int count = 0;
            foreach (var s in listView1.Items)
            {
                var itemDetails = ((ListViewItem)s);
                var listviewItem = ((ListViewItem)s).Name;
                var jsondata = JsonConvert.DeserializeObject<TestItemDetailsResponseProjection>(listviewItem);
                var index = listView1.Items.IndexOf(itemDetails);
                if (btnbackdata == jsondata.itemuuId)
                {
                    listView1.Items[index].Selected = true;
                    listView1.Focus();
                }
                else
                {
                    listView1.SelectedIndices.Remove(index);
                    listView1.Focus();
                }
                count++;
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Method to open list of test schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonActionClick(object sender, EventArgs e)
        {
            Schedules formreload = new Schedules(_loginDetails);
            formreload.Show();
            formreload.TopMost = true;
            Hide();
        }



        /// <summary>
        /// Closes question form and opens list of test schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Questions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Schedules formreload = new Schedules(_loginDetails);
            formreload.Show();
            formreload.TopMost = true;
        }

        private void grpQuestions_Enter(object sender, EventArgs e)
        {

        }
        private void ListView1_ItemDrag(Object sender, ItemDragEventArgs e)
        {

            //System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            //messageBoxCS.AppendFormat("{0} = {1}", "Button", e.Button);
            //messageBoxCS.AppendLine();
            //messageBoxCS.AppendFormat("{0} = {1}", "Item", e.Item);
            //messageBoxCS.AppendLine();
            //MessageBox.Show(messageBoxCS.ToString(), "ItemDrag Event");
        }

        private void ListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                Cursor.Current = Cursors.WaitCursor;
                // get the seleced values for the controls and then remove and call the save answers api
                var newTestAnswerRequest = new NewTestAnswerRequest
                {
                    userTestuuId = _userDeliverTestResponse.usertestuuId,
                    testuuId = _userDeliverTestResponse.testbankuuId
                };
                List<NewAnswers> answers = new List<NewAnswers>();
                foreach (var ctls in flowLayoutPanel1.Controls)
                {
                    var s = (FlowLayoutPanel)ctls;
                    var questionlable = s.Controls.OfType<Label>().FirstOrDefault();
                    var radiochecked = s.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                    var checkboxchecked = s.Controls.OfType<CheckBox>().ToList().Where(r => r.Checked).ToList();

                    NewAnswers newAnswers = new NewAnswers
                    {
                        questionuuId = questionlable.Name.Split(',')[0],
                        optionId = new List<short>()
                    };

                    var isAttemted = !string.IsNullOrEmpty(questionlable.Name.Split(',')[1]) ? Convert.ToBoolean(questionlable.Name.Split(',')[1]) : false;
                    List<short> optionId = new List<short>();

                    if (isAttemted)
                    {
                        if (checkboxchecked != null)
                        {
                            if (checkboxchecked.Count > 0)
                            {
                                foreach (var chck in checkboxchecked.ToList())
                                {
                                    optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                    newAnswers.optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                }
                            }
                        }
                        if (radiochecked != null)
                        {
                            optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            newAnswers.optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                        }
                        answers.Add(newAnswers);
                    }
                    else
                    {
                        if (checkboxchecked != null)
                        {
                            if (checkboxchecked.Count > 0)
                            {
                                foreach (var chck in checkboxchecked.ToList())
                                {
                                    optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                    newAnswers.optionId.Add(Convert.ToInt16(chck.Name.Split(',')[2].ToString()));
                                }
                                answers.Add(newAnswers);
                            }
                        }
                        if (radiochecked != null)
                        {
                            optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            newAnswers.optionId.Add(Convert.ToInt16(radiochecked.Name.Split(',')[2].ToString()));
                            answers.Add(newAnswers);
                        }
                    }
                }
                if (answers != null)
                {
                    newTestAnswerRequest.answers.AddRange(answers);
                    // send the data to api to save the answers
                    if (answers.Count > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(newTestAnswerRequest), Encoding.UTF8, "application/json");
                            var httpRequestMessage = new HttpRequestMessage()
                            {
                                Method = HttpMethod.Post,
                                Content = httpContent
                            };

                            client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/questions/answers");
                            client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                            client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                            HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                            if (response.IsSuccessStatusCode)
                            {
                                var result = response.Content.ReadAsStringAsync();
                            }
                        }
                    }
                }
                var listviewItem = ((ListViewItem)e.Item).Name;
                var jsondata = JsonConvert.DeserializeObject<TestItemDetailsResponseProjection>(listviewItem);
                var LastItem = ItemUuidList.Last();
                var firstItem = ItemUuidList.First();
                if (firstItem == jsondata.itemuuId)
                {
                    btnback.Hide();
                    var findIndex = ItemUuidList.FindIndex(c => c.Contains(firstItem));
                    btnNext.Text = "Next";
                    btnNext.Name = ItemUuidList.ElementAtOrDefault(findIndex + 1);
                }
                else if (LastItem == jsondata.itemuuId)
                {
                    btnNext.Text = "Finish";
                    btnNext.Name = LastItem;
                    var findIndex = ItemUuidList.FindIndex(c => c.Contains(LastItem));
                    btnback.Text = "Back";
                    btnback.Name = ItemUuidList.ElementAtOrDefault(findIndex - 1);
                    btnback.Show();
                }
                else
                {
                    var findIndex = ItemUuidList.FindIndex(c => c.Contains(jsondata.itemuuId));
                    btnNext.Text = "Next";
                    btnNext.Name = ItemUuidList.ElementAtOrDefault(findIndex + 1);
                    btnback.Text = "Back";
                    btnback.Name = ItemUuidList.ElementAtOrDefault(findIndex - 1);
                    btnback.Show();
                }
                flowLayoutPanel1.Controls.Clear();
                using (var client = new HttpClient())
                {
                    flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                    var Testuuid = _userDeliverTestResponse.testbankuuId;
                    var Usertestuuid = _userDeliverTestResponse.usertestuuId;

                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Get,
                    };

                    client.BaseAddress = new Uri(Settings.Default.TanayDelivaryBaseAddress + "/test-delivery/testDemo/" + Usertestuuid + "/items/" + jsondata.itemuuId + "/questions");
                    client.DefaultRequestHeaders.Add("subDomainName", Settings.Default.subDomainName);
                    client.DefaultRequestHeaders.Add("TimezoneOffset", Settings.Default.TimezoneOffset);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _loginDetails.data.accessToken);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                    HttpResponseMessage response = client.SendAsync(httpRequestMessage).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync();
                        var TestItemDetailsResponse = JsonConvert.DeserializeObject<ItemQuestionModel>(result.Result);
                        if (TestItemDetailsResponse != null)
                        {
                            foreach (var item in TestItemDetailsResponse.items)
                            {
                                if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "yes or no")
                                {
                                    FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                    {
                                        FlowDirection = FlowDirection.TopDown,
                                        Margin = new Padding(10, 30, 10, 10),
                                        AutoSize = true
                                    };
                                    GroupBox groupBox = new GroupBox
                                    {
                                        AutoSize = true,
                                        FlatStyle = FlatStyle.Standard,
                                        Top = 60,
                                        Left = 40
                                    };
                                    Label label = new Label
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                        Visible = true,
                                        TextAlign = (ContentAlignment)HorizontalAlignment.Center,
                                        AutoSize = true,
                                        Top = 50,
                                        Left = 40,
                                        Name = item.questionuuId + "," + item.attempted
                                    };

                                    flowLayoutPanel.Controls.Add(label);
                                    foreach (var options in item.options)
                                    {
                                        RadioButton radio = new RadioButton
                                        {
                                            Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                            Checked = options.selected,
                                            Visible = true,
                                            AutoSize = true,
                                            Margin = new Padding(25, 5, 5, 0),
                                            Name = item.questionuuId + "," + jsondata.itemuuId + "," + options.optiId
                                        };
                                        groupBox.Controls.Add(radio);
                                        flowLayoutPanel.Controls.Add(radio);
                                    }
                                    flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                                }
                                else if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "standard")
                                {
                                    FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                    {
                                        FlowDirection = FlowDirection.TopDown,
                                        AutoSize = true,
                                        Margin = new Padding(10, 30, 10, 10)
                                    };
                                    GroupBox groupBox = new GroupBox
                                    {
                                        AutoSize = true,
                                        FlatStyle = FlatStyle.Standard,
                                        Top = 60,
                                        Left = 40
                                    };
                                    Label label = new Label
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                        Visible = true,
                                        AutoSize = true,
                                        Top = 50,
                                        Left = 40,
                                        Name = item.questionuuId + "," + item.attempted
                                    };
                                    flowLayoutPanel.Controls.Add(label);
                                    foreach (var options in item.options)
                                    {
                                        RadioButton radio = new RadioButton
                                        {
                                            Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                            Checked = options.selected,
                                            Visible = true,
                                            AutoSize = true,
                                            Margin = new Padding(25, 5, 5, 0),
                                            Name = item.questionuuId + "," + jsondata.itemuuId + "," + options.optiId
                                        };
                                        groupBox.Controls.Add(radio);
                                        flowLayoutPanel.Controls.Add(radio);
                                    }
                                    flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                                }
                                else if (item.questionType.Trim().ToLower() == "multiple choice" && item.questionSubType.Trim().ToLower() == "multiple options")
                                {
                                    FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                    {
                                        FlowDirection = FlowDirection.TopDown,
                                        AutoSize = true,
                                        Margin = new Padding(10, 30, 10, 10)
                                    };
                                    GroupBox groupBox = new GroupBox
                                    {
                                        AutoSize = true,
                                        FlatStyle = FlatStyle.Standard,
                                        Top = 60,
                                        Left = 40
                                    };
                                    Label label = new Label
                                    {
                                        Text = StripHtmlHelper.StripHtmlTags(item.questiondescription.description),
                                        Visible = true,
                                        AutoSize = true,
                                        Top = 50,
                                        Left = 40,
                                        Name = item.questionuuId + "," + item.attempted
                                    };
                                    flowLayoutPanel.Controls.Add(label);
                                    foreach (var options in item.options)
                                    {
                                        CheckBox radio = new CheckBox
                                        {
                                            Text = StripHtmlHelper.StripHtmlTags(options.answerDescription.description),
                                            Checked = options.selected,
                                            Visible = true,
                                            Margin = new Padding(25, 5, 5, 0),
                                            AutoSize = true,
                                            Name = item.questionuuId + "," + jsondata.itemuuId + "," + options.optiId
                                        };
                                        groupBox.Controls.Add(radio);
                                        flowLayoutPanel.Controls.Add(radio);
                                    }
                                    flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                                }
                            }
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
           
        }
    }

}
