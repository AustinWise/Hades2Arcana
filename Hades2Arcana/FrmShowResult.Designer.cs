﻿namespace Hades2Arcana
{
    partial class FrmShowResult
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
            cardGrid = new CtrlCardGrid();
            SuspendLayout();
            // 
            // cardGrid
            // 
            cardGrid.Dock = DockStyle.Fill;
            cardGrid.Enabled = false;
            cardGrid.Location = new Point(0, 0);
            cardGrid.Name = "cardGrid";
            cardGrid.Size = new Size(671, 561);
            cardGrid.TabIndex = 0;
            // 
            // FrmShowResult
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(671, 561);
            Controls.Add(cardGrid);
            Name = "FrmShowResult";
            Text = "Arcana Result";
            ResumeLayout(false);
        }

        #endregion

        private CtrlCardGrid cardGrid;
    }
}