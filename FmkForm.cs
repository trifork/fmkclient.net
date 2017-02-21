using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fmkclient.net
{
    public partial class fmk : Form
    {
        private static FmkClient _fmkClient;
        public static FmkClient FmkClientInstance 
        {
            get
            {
                if (_fmkClient == null)
                    _fmkClient = new FmkClient();
                return _fmkClient;
            }
        }

        public fmk()
        {
            InitializeComponent();
        }

        private void btnHent_Click(object sender, EventArgs e)
        {
            txtResult.Text = FmkClientInstance.GetMedicineCard(txtCpr.Text);
        }

        private void buttonHentRecepter_Click(object sender, EventArgs e)
        {
            txtResult.Text = FmkClientInstance.GetPrescriptions(txtCpr.Text);
        }

    }
}
