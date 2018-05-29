namespace PingPoint
{
    partial class Endgame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Endgame));
            this.button_endgame = new System.Windows.Forms.Button();
            this.button_rematch = new System.Windows.Forms.Button();
            this.label_static_end = new System.Windows.Forms.Label();
            this.label_winner = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_endgame
            // 
            this.button_endgame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.button_endgame.Font = new System.Drawing.Font("Calibri", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_endgame.ForeColor = System.Drawing.Color.White;
            this.button_endgame.Location = new System.Drawing.Point(-7, 206);
            this.button_endgame.Name = "button_endgame";
            this.button_endgame.Size = new System.Drawing.Size(300, 66);
            this.button_endgame.TabIndex = 0;
            this.button_endgame.Text = "Zakończ mecz";
            this.button_endgame.UseVisualStyleBackColor = false;
            this.button_endgame.Click += new System.EventHandler(this.button_endgame_Click);
            // 
            // button_rematch
            // 
            this.button_rematch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.button_rematch.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_rematch.ForeColor = System.Drawing.Color.White;
            this.button_rematch.Location = new System.Drawing.Point(-7, 160);
            this.button_rematch.Name = "button_rematch";
            this.button_rematch.Size = new System.Drawing.Size(300, 40);
            this.button_rematch.TabIndex = 1;
            this.button_rematch.Text = "Rewanż";
            this.button_rematch.UseVisualStyleBackColor = false;
            this.button_rematch.Click += new System.EventHandler(this.button_rematch_Click);
            // 
            // label_static_end
            // 
            this.label_static_end.Font = new System.Drawing.Font("Calibri", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_static_end.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(224)))), ((int)(((byte)(117)))));
            this.label_static_end.Location = new System.Drawing.Point(-7, 9);
            this.label_static_end.Name = "label_static_end";
            this.label_static_end.Size = new System.Drawing.Size(300, 60);
            this.label_static_end.TabIndex = 2;
            this.label_static_end.Text = "Zwycięzca:";
            this.label_static_end.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_winner
            // 
            this.label_winner.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_winner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(125)))));
            this.label_winner.Location = new System.Drawing.Point(-7, 79);
            this.label_winner.Name = "label_winner";
            this.label_winner.Size = new System.Drawing.Size(300, 60);
            this.label_winner.TabIndex = 3;
            this.label_winner.Text = "Radohan";
            this.label_winner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Endgame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.label_winner);
            this.Controls.Add(this.label_static_end);
            this.Controls.Add(this.button_rematch);
            this.Controls.Add(this.button_endgame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "Endgame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_endgame;
        private System.Windows.Forms.Button button_rematch;
        private System.Windows.Forms.Label label_static_end;
        private System.Windows.Forms.Label label_winner;
    }
}