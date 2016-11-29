namespace CMPE2800DAllanLab01
{
    partial class Form1
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
            this.lab_BlockPosition = new System.Windows.Forms.Label();
            this.lab_WinPosition = new System.Windows.Forms.Label();
            this.lab_BlockCount = new System.Windows.Forms.Label();
            this.gBx_BlockPosition = new System.Windows.Forms.GroupBox();
            this.gBx_WinPosition = new System.Windows.Forms.GroupBox();
            this.gBx_BlockCount = new System.Windows.Forms.GroupBox();
            this.gBx_BlockPosition.SuspendLayout();
            this.gBx_WinPosition.SuspendLayout();
            this.gBx_BlockCount.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_BlockPosition
            // 
            this.lab_BlockPosition.AutoSize = true;
            this.lab_BlockPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_BlockPosition.Location = new System.Drawing.Point(12, 27);
            this.lab_BlockPosition.Name = "lab_BlockPosition";
            this.lab_BlockPosition.Size = new System.Drawing.Size(159, 31);
            this.lab_BlockPosition.TabIndex = 0;
            this.lab_BlockPosition.Text = "{ X=0, Y=0 }";
            // 
            // lab_WinPosition
            // 
            this.lab_WinPosition.AutoSize = true;
            this.lab_WinPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_WinPosition.Location = new System.Drawing.Point(12, 27);
            this.lab_WinPosition.Name = "lab_WinPosition";
            this.lab_WinPosition.Size = new System.Drawing.Size(159, 31);
            this.lab_WinPosition.TabIndex = 1;
            this.lab_WinPosition.Text = "{ X=0, Y=0 }";
            // 
            // lab_BlockCount
            // 
            this.lab_BlockCount.AutoSize = true;
            this.lab_BlockCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lab_BlockCount.Location = new System.Drawing.Point(12, 27);
            this.lab_BlockCount.Name = "lab_BlockCount";
            this.lab_BlockCount.Size = new System.Drawing.Size(29, 31);
            this.lab_BlockCount.TabIndex = 2;
            this.lab_BlockCount.Text = "0";
            // 
            // gBx_BlockPosition
            // 
            this.gBx_BlockPosition.Controls.Add(this.lab_BlockPosition);
            this.gBx_BlockPosition.Location = new System.Drawing.Point(12, 13);
            this.gBx_BlockPosition.Name = "gBx_BlockPosition";
            this.gBx_BlockPosition.Size = new System.Drawing.Size(260, 84);
            this.gBx_BlockPosition.TabIndex = 3;
            this.gBx_BlockPosition.TabStop = false;
            this.gBx_BlockPosition.Text = "Block Position";
            // 
            // gBx_WinPosition
            // 
            this.gBx_WinPosition.Controls.Add(this.lab_WinPosition);
            this.gBx_WinPosition.Location = new System.Drawing.Point(12, 97);
            this.gBx_WinPosition.Name = "gBx_WinPosition";
            this.gBx_WinPosition.Size = new System.Drawing.Size(260, 84);
            this.gBx_WinPosition.TabIndex = 4;
            this.gBx_WinPosition.TabStop = false;
            this.gBx_WinPosition.Text = "Window Position";
            // 
            // gBx_BlockCount
            // 
            this.gBx_BlockCount.Controls.Add(this.lab_BlockCount);
            this.gBx_BlockCount.Location = new System.Drawing.Point(12, 181);
            this.gBx_BlockCount.Name = "gBx_BlockCount";
            this.gBx_BlockCount.Size = new System.Drawing.Size(260, 84);
            this.gBx_BlockCount.TabIndex = 5;
            this.gBx_BlockCount.TabStop = false;
            this.gBx_BlockCount.Text = "Block Count";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 279);
            this.Controls.Add(this.gBx_BlockCount);
            this.Controls.Add(this.gBx_WinPosition);
            this.Controls.Add(this.gBx_BlockPosition);
            this.Name = "Form1";
            this.Text = "CMPE2800 - Review Lab";
            this.gBx_BlockPosition.ResumeLayout(false);
            this.gBx_BlockPosition.PerformLayout();
            this.gBx_WinPosition.ResumeLayout(false);
            this.gBx_WinPosition.PerformLayout();
            this.gBx_BlockCount.ResumeLayout(false);
            this.gBx_BlockCount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lab_BlockPosition;
        private System.Windows.Forms.Label lab_WinPosition;
        private System.Windows.Forms.Label lab_BlockCount;
        private System.Windows.Forms.GroupBox gBx_BlockPosition;
        private System.Windows.Forms.GroupBox gBx_WinPosition;
        private System.Windows.Forms.GroupBox gBx_BlockCount;
    }
}

