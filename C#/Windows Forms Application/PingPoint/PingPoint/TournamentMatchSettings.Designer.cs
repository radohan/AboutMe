namespace PingPoint
{
    partial class TournamentMatchSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentMatchSettings));
            this.button_choose = new System.Windows.Forms.Button();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_tournament = new System.Windows.Forms.DataGridView();
            this.Typ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ile_punktow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ile_setow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Opis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tournament)).BeginInit();
            this.SuspendLayout();
            // 
            // button_choose
            // 
            this.button_choose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.button_choose.Font = new System.Drawing.Font("Calibri", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_choose.ForeColor = System.Drawing.Color.White;
            this.button_choose.Location = new System.Drawing.Point(-9, 160);
            this.button_choose.Name = "button_choose";
            this.button_choose.Size = new System.Drawing.Size(480, 61);
            this.button_choose.TabIndex = 1;
            this.button_choose.Text = "Wybierz turniej";
            this.button_choose.UseVisualStyleBackColor = false;
            this.button_choose.Click += new System.EventHandler(this.button1_Click);
            // 
            // Type
            // 
            this.Type.FillWeight = 50F;
            this.Type.Frozen = true;
            this.Type.HeaderText = "Typ";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // dataGridView_tournament
            // 
            this.dataGridView_tournament.AllowUserToAddRows = false;
            this.dataGridView_tournament.AllowUserToDeleteRows = false;
            this.dataGridView_tournament.AllowUserToResizeColumns = false;
            this.dataGridView_tournament.AllowUserToResizeRows = false;
            this.dataGridView_tournament.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_tournament.ColumnHeadersHeight = 20;
            this.dataGridView_tournament.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_tournament.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Typ,
            this.Ile_punktow,
            this.Ile_setow,
            this.Opis,
            this.Id});
            this.dataGridView_tournament.Location = new System.Drawing.Point(13, 12);
            this.dataGridView_tournament.MultiSelect = false;
            this.dataGridView_tournament.Name = "dataGridView_tournament";
            this.dataGridView_tournament.ReadOnly = true;
            this.dataGridView_tournament.RowHeadersWidth = 38;
            this.dataGridView_tournament.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_tournament.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tournament.Size = new System.Drawing.Size(441, 142);
            this.dataGridView_tournament.TabIndex = 3;
            this.dataGridView_tournament.SelectionChanged += new System.EventHandler(this.dataGridView_tournament_SelectionChanged);
            // 
            // Typ
            // 
            this.Typ.HeaderText = "Typ";
            this.Typ.Name = "Typ";
            this.Typ.ReadOnly = true;
            // 
            // Ile_punktow
            // 
            this.Ile_punktow.HeaderText = "Ile punktów";
            this.Ile_punktow.Name = "Ile_punktow";
            this.Ile_punktow.ReadOnly = true;
            // 
            // Ile_setow
            // 
            this.Ile_setow.HeaderText = "Ile setów";
            this.Ile_setow.Name = "Ile_setow";
            this.Ile_setow.ReadOnly = true;
            // 
            // Opis
            // 
            this.Opis.HeaderText = "Opis";
            this.Opis.Name = "Opis";
            this.Opis.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // TournamentMatchSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(464, 212);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridView_tournament);
            this.Controls.Add(this.button_choose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(480, 250);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 250);
            this.Name = "TournamentMatchSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tournament)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_choose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridView dataGridView_tournament;
        private System.Windows.Forms.DataGridViewTextBoxColumn Typ;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ile_punktow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ile_setow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Opis;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    }
}