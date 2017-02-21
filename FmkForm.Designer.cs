namespace fmkclient.net
{
    partial class fmk
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnHent = new System.Windows.Forms.Button();
            this.txtCpr = new System.Windows.Forms.TextBox();
            this.lblCPR = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.buttonHentRecepter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnHent
            // 
            this.btnHent.Location = new System.Drawing.Point(254, 19);
            this.btnHent.Name = "btnHent";
            this.btnHent.Size = new System.Drawing.Size(176, 25);
            this.btnHent.TabIndex = 0;
            this.btnHent.Text = "Hent medicinkort";
            this.btnHent.UseVisualStyleBackColor = true;
            this.btnHent.Click += new System.EventHandler(this.btnHent_Click);
            // 
            // txtCpr
            // 
            this.txtCpr.Location = new System.Drawing.Point(50, 22);
            this.txtCpr.Name = "txtCpr";
            this.txtCpr.Size = new System.Drawing.Size(188, 20);
            this.txtCpr.TabIndex = 1;
            // 
            // lblCPR
            // 
            this.lblCPR.AutoSize = true;
            this.lblCPR.Location = new System.Drawing.Point(15, 25);
            this.lblCPR.Name = "lblCPR";
            this.lblCPR.Size = new System.Drawing.Size(29, 13);
            this.lblCPR.TabIndex = 2;
            this.lblCPR.Text = "CPR";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 117);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(822, 502);
            this.txtResult.TabIndex = 3;
            this.txtResult.Text = "";
            // 
            // buttonHentRecepter
            // 
            this.buttonHentRecepter.Location = new System.Drawing.Point(452, 19);
            this.buttonHentRecepter.Name = "buttonHentRecepter";
            this.buttonHentRecepter.Size = new System.Drawing.Size(176, 25);
            this.buttonHentRecepter.TabIndex = 4;
            this.buttonHentRecepter.Text = "Hent recepter";
            this.buttonHentRecepter.UseVisualStyleBackColor = true;
            this.buttonHentRecepter.Click += new System.EventHandler(this.buttonHentRecepter_Click);
            // 
            // fmk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 631);
            this.Controls.Add(this.buttonHentRecepter);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblCPR);
            this.Controls.Add(this.txtCpr);
            this.Controls.Add(this.btnHent);
            this.Name = "fmk";
            this.Text = "Fælles medicinkort 1.4.6";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHent;
        private System.Windows.Forms.TextBox txtCpr;
        private System.Windows.Forms.Label lblCPR;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Button buttonHentRecepter;
    }
}

