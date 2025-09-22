using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOGGINN
{
    public partial class LOGIN_LandingPage : Form
    {

        CONTROLLER CONTROLLER = new CONTROLLER() ;
        Form2 form2 = new Form2();
        public LOGIN_LandingPage()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BeginInvoke((MethodInvoker)delegate
            {
                userName_txtbox.Focus();
            });
        }
       
        private void signup_btn_Click(object sender, EventArgs e)
        {
            SIGN_UP signUpForm = new SIGN_UP();
            signUpForm.Show();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName_txtbox.Text) || string.IsNullOrWhiteSpace(password_txtbox.Text))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }
            else
            {
                bool verify = true;
                if (verify = CONTROLLER.verifyUSer(userName_txtbox.Text, password_txtbox.Text))
                {
                    MessageBox.Show("Login successful!");
                    form2.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }

            }

        }
    }
}
       
