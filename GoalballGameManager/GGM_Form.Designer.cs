namespace GoalballGameManager
{
    partial class GGM_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListBox lstStatus;

        private System.Windows.Forms.Button btnBroadcast;

        private System.Windows.Forms.TextBox txtBroadcast;
        private System.Windows.Forms.ListBox lstPlayers;

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
            this.lstStatus = new System.Windows.Forms.ListBox();
            this.txtBroadcast = new System.Windows.Forms.TextBox();
            this.btnBroadcast = new System.Windows.Forms.Button();
            this.lstPlayers = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.lblGames = new System.Windows.Forms.Label();
            this.lstGames = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstStatus
            // 
            this.lstStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstStatus.IntegralHeight = false;
            this.lstStatus.Location = new System.Drawing.Point(15, 27);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstStatus.Size = new System.Drawing.Size(251, 123);
            this.lstStatus.TabIndex = 0;
            // 
            // txtBroadcast
            // 
            this.txtBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBroadcast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtBroadcast.Location = new System.Drawing.Point(95, 161);
            this.txtBroadcast.MaxLength = 0;
            this.txtBroadcast.Name = "txtBroadcast";
            this.txtBroadcast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtBroadcast.Size = new System.Drawing.Size(411, 20);
            this.txtBroadcast.TabIndex = 0;
            this.txtBroadcast.WordWrap = false;
            // 
            // btnBroadcast
            // 
            this.btnBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBroadcast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBroadcast.ImageIndex = 0;
            this.btnBroadcast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBroadcast.Location = new System.Drawing.Point(15, 158);
            this.btnBroadcast.Name = "btnBroadcast";
            this.btnBroadcast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnBroadcast.Size = new System.Drawing.Size(74, 25);
            this.btnBroadcast.TabIndex = 0;
            this.btnBroadcast.Text = "Broadcast";
            this.btnBroadcast.Click += new System.EventHandler(this.btnBroadcast_Click);
            // 
            // lstPlayers
            // 
            this.lstPlayers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstPlayers.IntegralHeight = false;
            this.lstPlayers.Location = new System.Drawing.Point(272, 27);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstPlayers.Size = new System.Drawing.Size(114, 123);
            this.lstPlayers.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status";
            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.Location = new System.Drawing.Point(272, 9);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(41, 13);
            this.lblPlayers.TabIndex = 2;
            this.lblPlayers.Text = "Players";
            // 
            // lblGames
            // 
            this.lblGames.AutoSize = true;
            this.lblGames.Location = new System.Drawing.Point(392, 9);
            this.lblGames.Name = "lblGames";
            this.lblGames.Size = new System.Drawing.Size(40, 13);
            this.lblGames.TabIndex = 4;
            this.lblGames.Text = "Games";
            // 
            // lstGames
            // 
            this.lstGames.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstGames.IntegralHeight = false;
            this.lstGames.Location = new System.Drawing.Point(392, 28);
            this.lstGames.Name = "lstGames";
            this.lstGames.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lstGames.Size = new System.Drawing.Size(114, 123);
            this.lstGames.TabIndex = 3;
            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(520, 188);
            this.Controls.Add(this.lblGames);
            this.Controls.Add(this.lstGames);
            this.Controls.Add(this.lblPlayers);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lstPlayers);
            this.Controls.Add(this.btnBroadcast);
            this.Controls.Add(this.txtBroadcast);
            this.Controls.Add(this.lstStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GGM_Form";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Goalball Game Manager";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblPlayers;
        private System.Windows.Forms.Label lblGames;
        private System.Windows.Forms.ListBox lstGames;
    }
}

