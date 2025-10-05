namespace Hades2Arcana
{
    partial class CtrlCardGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            table = new TableLayoutPanel();
            SuspendLayout();
            // 
            // table
            // 
            table.ColumnCount = 5;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            table.Dock = DockStyle.Fill;
            table.Location = new Point(0, 0);
            table.Name = "table";
            table.RowCount = 5;
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            table.Size = new Size(309, 219);
            table.TabIndex = 0;
            // 
            // CtrlCardGrid
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(table);
            Name = "CtrlCardGrid";
            Size = new Size(309, 219);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel table;
    }
}
