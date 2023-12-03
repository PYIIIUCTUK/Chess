namespace Chess
{
    partial class Placement
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
            this.SuspendLayout();
            // 
            // Placement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Placement";
            this.Text = "Placement";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Placement_FormClosed);
            this.Shown += new System.EventHandler(this.Placement_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Placement_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Placement_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Placement_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}