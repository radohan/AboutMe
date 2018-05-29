namespace PingPoint
{
    partial class Wait
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wait));
            this.label_change = new System.Windows.Forms.Label();
            this.label_next_set = new System.Windows.Forms.Label();
            this.label_info_win = new System.Windows.Forms.Label();
            this.label_static_info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_change
            // 
            this.label_change.Font = new System.Drawing.Font("Calibri", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_change.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(88)))), ((int)(((byte)(106)))));
            this.label_change.Location = new System.Drawing.Point(0, 80);
            this.label_change.Name = "label_change";
            this.label_change.Size = new System.Drawing.Size(289, 80);
            this.label_change.TabIndex = 0;
            this.label_change.Text = "Dokonaj zmian";
            this.label_change.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_change.Click += new System.EventHandler(this.label_change_Click);
            // 
            // label_next_set
            // 
            this.label_next_set.Font = new System.Drawing.Font("Calibri", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_next_set.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(224)))), ((int)(((byte)(117)))));
            this.label_next_set.Location = new System.Drawing.Point(295, 80);
            this.label_next_set.Name = "label_next_set";
            this.label_next_set.Size = new System.Drawing.Size(295, 80);
            this.label_next_set.TabIndex = 1;
            this.label_next_set.Text = "Zamknij i przejdź dalej";
            this.label_next_set.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_next_set.Click += new System.EventHandler(this.label_next_set_Click);
            // 
            // label_info_win
            // 
            this.label_info_win.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_info_win.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.label_info_win.Location = new System.Drawing.Point(-10, 0);
            this.label_info_win.Name = "label_info_win";
            this.label_info_win.Size = new System.Drawing.Size(600, 80);
            this.label_info_win.TabIndex = 2;
            this.label_info_win.Text = "Set wygrywa: Radohan";
            this.label_info_win.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_info
            // 
            this.label_static_info.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_static_info.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.label_static_info.Location = new System.Drawing.Point(-10, 0);
            this.label_static_info.Name = "label_static_info";
            this.label_static_info.Size = new System.Drawing.Size(600, 18);
            this.label_static_info.TabIndex = 3;
            this.label_static_info.Text = "Zmiana stron";
            this.label_static_info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Wait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 162);
            this.ControlBox = false;
            this.Controls.Add(this.label_static_info);
            this.Controls.Add(this.label_next_set);
            this.Controls.Add(this.label_info_win);
            this.Controls.Add(this.label_change);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 200);
            this.Name = "Wait";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_change;
        private System.Windows.Forms.Label label_next_set;
        private System.Windows.Forms.Label label_info_win;
        private System.Windows.Forms.Label label_static_info;
    }
}