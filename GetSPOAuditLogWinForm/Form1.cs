using System;
using System.Windows.Forms;

namespace GetSPOAuditLogWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (SPOAuditHelper.Init())
                {
                    MessageBox.Show("You may need to wait for several up to 24 hours to initiate the subscription endpoint of Audit.SharePoint");
                }
                this.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridView1.DataSource = SPOAuditHelper.GetAuditData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }


    }
}
