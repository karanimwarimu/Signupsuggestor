using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LOGGINN.CONTROLLER;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LOGGINN
{
    public partial class SIGN_UP : Form
    {
        CONTROLLER ctrl = new CONTROLLER();
        private readonly UsernameGenerator gen = new UsernameGenerator();

        private string hashedPassword;
        public SIGN_UP()
        {
            InitializeComponent();

            //submit_signup_Btn.Enabled = true;

            firstName_signup.TextChanged += new EventHandler(Input_TextChanged);
            lastname_signup.TextChanged += new EventHandler(Input_TextChanged);
            username_signup.TextChanged += new EventHandler(Input_TextChanged);
            email_signup.TextChanged += new EventHandler(Input_TextChanged);
            password_signup.TextChanged += new EventHandler(Input_TextChanged);
            confirmpasssowrd_signup.TextChanged += new EventHandler(Input_TextChanged);
            userSelect_signup.SelectedIndexChanged += new EventHandler(Input_TextChanged);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BeginInvoke((MethodInvoker)delegate
            {
                firstName_signup.Focus();
                ValidateAllInputs();
            });
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Input_TextChanged(object sender, EventArgs e)
        {
            ValidateAllInputs();
        }


        private bool ValidateAllInputs()
        {
            bool allInputsValid = true;


            Action<dynamic, bool> setControlValidityColor = (control, isValidField) =>
            {
                if (isValidField)
                {
                    control.FillColor = Color.FloralWhite;
                }
                else
                {
                    control.FillColor = Color.Gainsboro;
                }
            };


            bool isFirstNameValid = !String.IsNullOrWhiteSpace(firstName_signup.Text);
            setControlValidityColor(firstName_signup, isFirstNameValid);
            if (!isFirstNameValid) allInputsValid = false;

            bool isLastNameValid = !String.IsNullOrWhiteSpace(lastname_signup.Text);
            setControlValidityColor(lastname_signup, isLastNameValid);
            if (!isLastNameValid) allInputsValid = false;


            bool isUsernameValid = !String.IsNullOrWhiteSpace(username_signup.Text);
            setControlValidityColor(username_signup, isUsernameValid);
            if (!isUsernameValid) allInputsValid = false;

            bool isEmailValid = !String.IsNullOrWhiteSpace(email_signup.Text) && email_signup.Text.Contains("@") && email_signup.Text.Contains(".");
            setControlValidityColor(email_signup, isEmailValid);
            if (!isEmailValid) allInputsValid = false;


            bool isPasswordValid = !String.IsNullOrWhiteSpace(password_signup.Text);
            setControlValidityColor(password_signup, isPasswordValid);
            if (!isPasswordValid) allInputsValid = false;


            bool isConfirmPasswordValid = !String.IsNullOrWhiteSpace(confirmpasssowrd_signup.Text);
            setControlValidityColor(confirmpasssowrd_signup, isConfirmPasswordValid);
            if (!isConfirmPasswordValid) allInputsValid = false;

            if (isPasswordValid && isConfirmPasswordValid)
            {
                if (password_signup.Text != confirmpasssowrd_signup.Text)
                {
                    setControlValidityColor(confirmpasssowrd_signup, false);
                    allInputsValid = false;
                }

            }
            else if (isPasswordValid && !isConfirmPasswordValid)
            {

                allInputsValid = false;
            }



            bool isUserRoleSelected = userSelect_signup.SelectedIndex != -1;

            setControlValidityColor(userSelect_signup, isUserRoleSelected);
            if (!isUserRoleSelected) allInputsValid = false;



            submit_signup_Btn.ForeColor = allInputsValid ? Color.Black : Color.Gray;
            submit_signup_Btn.BackColor = allInputsValid ? Color.LightGreen : Color.LightGray;
            return allInputsValid;
        }


        private void submit_signup_Btn_Click_1(object sender, EventArgs e)
        {

            if (ValidateAllInputs()) // Re-run validation one last time
            {
                MessageBox.Show("Sign up successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                hashedPassword = ctrl.SecurePassword(password_signup.Text.Trim());

                string username = username_signup.Text.Trim();
                string firstName = firstName_signup.Text.Trim().ToUpper();
                string lastName = lastname_signup.Text.Trim().ToUpper();
                string email = email_signup.Text.Trim().ToLower();
                string userRole = userSelect_signup.SelectedItem.ToString();

                ctrl.SaveUserData(username, firstName, lastName, email, hashedPassword, userRole);
                clearFields();

                this.Hide();

                LOGIN_LandingPage lOGIN_LandingPage = new LOGIN_LandingPage();
                lOGIN_LandingPage.Show();

            }
            else
            {

                string errorMessage = "Please correct the following issues:\n\n";
                Control firstInvalidControl = null;

                if (String.IsNullOrWhiteSpace(firstName_signup.Text))
                {
                    errorMessage += "- First Name is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = firstName_signup;
                }
                if (String.IsNullOrWhiteSpace(lastname_signup.Text))
                {
                    errorMessage += "- Last Name is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = lastname_signup;
                }
                if (String.IsNullOrWhiteSpace(username_signup.Text))
                {
                    errorMessage += "- Username is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = username_signup;
                }
                if (String.IsNullOrWhiteSpace(email_signup.Text) || !email_signup.Text.Contains("@") || !email_signup.Text.Contains("."))
                {
                    errorMessage += "- Valid Email is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = email_signup;
                }
                if (String.IsNullOrWhiteSpace(password_signup.Text))
                {
                    errorMessage += "- Password is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = password_signup;
                }
                if (String.IsNullOrWhiteSpace(confirmpasssowrd_signup.Text))
                {
                    errorMessage += "- Confirm Password is required.\n";
                    if (firstInvalidControl == null) firstInvalidControl = confirmpasssowrd_signup;
                }
                if (password_signup.Text != confirmpasssowrd_signup.Text && !String.IsNullOrWhiteSpace(password_signup.Text) && !String.IsNullOrWhiteSpace(confirmpasssowrd_signup.Text))
                {
                    errorMessage += "- Passwords do not match.\n";
                    if (firstInvalidControl == null) firstInvalidControl = confirmpasssowrd_signup;
                }
                if (userSelect_signup.SelectedIndex == -1)
                {
                    errorMessage += "- Please select a User Role.\n";
                    if (firstInvalidControl == null) firstInvalidControl = userSelect_signup;
                }

                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Set focus to the first invalid control found
                firstInvalidControl?.Focus();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void clearFields()
        {
            firstName_signup.Clear();
            lastname_signup.Clear();
            username_signup.Clear();
            email_signup.Clear();
            password_signup.Clear();
            confirmpasssowrd_signup.Clear();
            userSelect_signup.SelectedIndex = -1;
        }

        private void email_signup_TextChanged(object sender, EventArgs e)
        {

            if (!ctrl.verifyEmail(email_signup.Text.Trim()))
            {
                // If the email does not exist, we can proceed with the signup
                submit_signup_Btn.Enabled = true;
                lblEmailError.Visible = false;
            }
            else
            {
                submit_signup_Btn.Enabled = false;
                lblEmailError.Text = "Email already exists.";
                lblEmailError.ForeColor = Color.Red;
                lblEmailError.Visible = true;

                return;
            }
        }

        private void username_signup_TextChanged(object sender, EventArgs e)
        {
            /* if (!ctrl.verifyUserName(username_signup.Text.Trim()))
             {
                 submit_signup_Btn.Enabled = true;
                 username_error.Visible = false;
             }
             else
             {
                 submit_signup_Btn.Enabled = false;
                 username_error.Text = "Username already exists.";
                 username_error.ForeColor = Color.Red;
                 username_error.Visible = true;
                 return;
             }
            */
            if (verifyUserName(username_signup.Text.Trim()))
            {
                errorProvider1.SetError(username_signup, "Username already exists.");
            }
            else
            {
                errorProvider1.SetError(username_signup, string.Empty);
            }
        }

        private void password_signup_TextChanged(object sender, EventArgs e)
        {

        }
        private void validatePassowrdStrength()
        {
            string enterdpassword = password_signup.Text.Trim();

            if (string.IsNullOrEmpty(enterdpassword))
                password_signup.Focus();
            Password_errorProvider.Clear(); // Clear previous error messages

            if (enterdpassword.Length < 4)

            {
                Password_errorProvider.SetError(password_signup, "Password must be at least 8 characters long.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(enterdpassword, @"[A-Z]"))
            {
                Password_errorProvider.SetError(password_signup, "Password must contain at least one uppercase letter.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(enterdpassword, @"[a-z]"))
            {
                Password_errorProvider.SetError(password_signup, "Password must contain at least one lowercase letter.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(enterdpassword, @"[0-9]"))
            {
                Password_errorProvider.SetError(password_signup, "Password must contain at least one number.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(enterdpassword, @"[\W_]"))
            {
                Password_errorProvider.SetError(password_signup, "Password must contain at least one special character.");
            }
            else
            {
                Password_errorProvider.SetError(password_signup, string.Empty);
            }
        }
        private void password_signup_Leave(object sender, EventArgs e)
        {
            validatePassowrdStrength();
        }


        private async void lastname_signup_Leave(object sender, EventArgs e)
        {
            string fn = firstName_signup.Text.Trim();
            string ln = lastname_signup.Text.Trim();
            string eus = email_signup.Text.Trim();
            List<string> result = await gen.SuggestPasswordAsync(fn, ln, eus);
            foreach (string suggestion in result)
            {
                listBox1.Items.Add("");
                listBox1.Items.Add(suggestion.ToUpper());
                listBox1.Items.Add("");
            }

            listBox1.Visible = result.Count > 0;
         
        }

        private void username_signup_Click(object sender, EventArgs e)
        {
            listBox1.Visible = false;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox1.SelectedItem.ToString()))
            {
                string selected = listBox1.SelectedItem.ToString().ToUpper();
                username_signup.Text = selected;
                username_signup.Focus();
                username_signup.SelectionStart = selected.Length;

                listBox1.Visible = false; // hide after click
            }
        }

        /*  private async void button1_Click(object sender, EventArgs e)
          {
              string fn = firstName_signup.Text.Trim();
              string ln = lastname_signup.Text.Trim();
              string eus = email_signup.Text.Trim();
              List<string> result = await gen.SuggestPasswordAsync(fn, ln, eus);
              foreach (string suggestion in result)
              {
                  listBox1.Items.Add("");
                  listBox1.Items.Add(suggestion.ToUpper());
                  listBox1.Items.Add("");
              }

              listBox1.Visible = result.Count > 0;
          } */
    }
}
