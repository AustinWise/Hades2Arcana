namespace Hades2Arcana;

partial class FrmMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Label label1;
        graspUpDown = new NumericUpDown();
        groupCards = new GroupBox();
        cardGrid = new CtrlCardGrid();
        btnShowSolution = new Button();
        chkValidSolutionExists = new CheckBox();
        lblGraspUsed = new Label();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)graspUpDown).BeginInit();
        groupCards.SuspendLayout();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 14);
        label1.Name = "label1";
        label1.Size = new Size(65, 15);
        label1.TabIndex = 0;
        label1.Text = "Max &Grasp:";
        // 
        // graspUpDown
        // 
        graspUpDown.Location = new Point(83, 12);
        graspUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        graspUpDown.Name = "graspUpDown";
        graspUpDown.Size = new Size(82, 23);
        graspUpDown.TabIndex = 1;
        graspUpDown.Value = new decimal(new int[] { 29, 0, 0, 0 });
        graspUpDown.ValueChanged += graspUpDown_ValueChanged;
        // 
        // groupCards
        // 
        groupCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupCards.Controls.Add(cardGrid);
        groupCards.Location = new Point(12, 41);
        groupCards.Name = "groupCards";
        groupCards.Size = new Size(660, 608);
        groupCards.TabIndex = 2;
        groupCards.TabStop = false;
        groupCards.Text = "Cards";
        // 
        // cardGrid
        // 
        cardGrid.Dock = DockStyle.Fill;
        cardGrid.Location = new Point(3, 19);
        cardGrid.Name = "cardGrid";
        cardGrid.Size = new Size(654, 586);
        cardGrid.TabIndex = 0;
        cardGrid.CheckedCardsChanged += cardGrid_CheckedCardsChanged;
        // 
        // btnShowSolution
        // 
        btnShowSolution.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnShowSolution.Location = new Point(539, 12);
        btnShowSolution.Name = "btnShowSolution";
        btnShowSolution.Size = new Size(133, 23);
        btnShowSolution.TabIndex = 3;
        btnShowSolution.Text = "&Show Solution";
        btnShowSolution.UseVisualStyleBackColor = true;
        btnShowSolution.Click += btnShowSolution_Click;
        // 
        // chkValidSolutionExists
        // 
        chkValidSolutionExists.AutoSize = true;
        chkValidSolutionExists.Checked = true;
        chkValidSolutionExists.CheckState = CheckState.Checked;
        chkValidSolutionExists.Enabled = false;
        chkValidSolutionExists.Location = new Point(404, 15);
        chkValidSolutionExists.Name = "chkValidSolutionExists";
        chkValidSolutionExists.Size = new Size(129, 19);
        chkValidSolutionExists.TabIndex = 4;
        chkValidSolutionExists.Text = "Valid Solution Exists";
        chkValidSolutionExists.UseVisualStyleBackColor = true;
        // 
        // lblGraspUsed
        // 
        lblGraspUsed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblGraspUsed.AutoSize = true;
        lblGraspUsed.Location = new Point(291, 16);
        lblGraspUsed.Name = "lblGraspUsed";
        lblGraspUsed.Size = new Size(78, 15);
        lblGraspUsed.TabIndex = 5;
        lblGraspUsed.Text = "Grasp Used: 0";
        // 
        // FrmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(684, 661);
        Controls.Add(lblGraspUsed);
        Controls.Add(chkValidSolutionExists);
        Controls.Add(btnShowSolution);
        Controls.Add(groupCards);
        Controls.Add(graspUpDown);
        Controls.Add(label1);
        MinimumSize = new Size(700, 400);
        Name = "FrmMain";
        Text = "Hades Carcana Solver";
        ((System.ComponentModel.ISupportInitialize)graspUpDown).EndInit();
        groupCards.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private NumericUpDown graspUpDown;
    private GroupBox groupCards;
    private Button btnShowSolution;
    private CheckBox chkValidSolutionExists;
    private CtrlCardGrid cardGrid;
    private Label lblGraspUsed;
}
