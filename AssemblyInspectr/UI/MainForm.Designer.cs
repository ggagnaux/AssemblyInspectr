namespace AssemblyInspectr.UI
{
    partial class MainForm
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.textBoxAssemblyName = new MetroFramework.Controls.MetroTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelAssemblyDetails = new System.Windows.Forms.Panel();
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.tabPageDetails = new MetroFramework.Controls.MetroTabPage();
            this.tabPageClasses = new MetroFramework.Controls.MetroTabPage();
            this.panelClasses = new System.Windows.Forms.Panel();
            this.tabPageReferencedAssemblies = new MetroFramework.Controls.MetroTabPage();
            this.panelReferencedAssembiles = new System.Windows.Forms.Panel();
            this.buttonAbout = new MetroFramework.Controls.MetroButton();
            this.tabControl.SuspendLayout();
            this.tabPageDetails.SuspendLayout();
            this.tabPageClasses.SuspendLayout();
            this.tabPageReferencedAssemblies.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 98);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(172, 20);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Current Loaded Assembly:";
            // 
            // textBoxAssemblyName
            // 
            this.textBoxAssemblyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.textBoxAssemblyName.CustomButton.Image = null;
            this.textBoxAssemblyName.CustomButton.Location = new System.Drawing.Point(846, 2);
            this.textBoxAssemblyName.CustomButton.Name = "";
            this.textBoxAssemblyName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.textBoxAssemblyName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxAssemblyName.CustomButton.TabIndex = 1;
            this.textBoxAssemblyName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxAssemblyName.CustomButton.UseSelectable = true;
            this.textBoxAssemblyName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBoxAssemblyName.Lines = new string[0];
            this.textBoxAssemblyName.Location = new System.Drawing.Point(243, 98);
            this.textBoxAssemblyName.MaxLength = 32767;
            this.textBoxAssemblyName.Name = "textBoxAssemblyName";
            this.textBoxAssemblyName.PasswordChar = '\0';
            this.textBoxAssemblyName.PromptText = "Select Assembly...";
            this.textBoxAssemblyName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxAssemblyName.SelectedText = "";
            this.textBoxAssemblyName.SelectionLength = 0;
            this.textBoxAssemblyName.SelectionStart = 0;
            this.textBoxAssemblyName.ShortcutsEnabled = true;
            this.textBoxAssemblyName.ShowButton = true;
            this.textBoxAssemblyName.Size = new System.Drawing.Size(872, 28);
            this.textBoxAssemblyName.TabIndex = 1;
            this.textBoxAssemblyName.UseSelectable = true;
            this.textBoxAssemblyName.WaterMark = "Select Assembly...";
            this.textBoxAssemblyName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxAssemblyName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxAssemblyName.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.textBoxAssemblyName_ButtonClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panelAssemblyDetails
            // 
            this.panelAssemblyDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAssemblyDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panelAssemblyDetails.Location = new System.Drawing.Point(3, 7);
            this.panelAssemblyDetails.Name = "panelAssemblyDetails";
            this.panelAssemblyDetails.Size = new System.Drawing.Size(1239, 640);
            this.panelAssemblyDetails.TabIndex = 3;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDetails);
            this.tabControl.Controls.Add(this.tabPageClasses);
            this.tabControl.Controls.Add(this.tabPageReferencedAssemblies);
            this.tabControl.Location = new System.Drawing.Point(23, 151);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1255, 699);
            this.tabControl.TabIndex = 4;
            this.tabControl.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabControl.UseSelectable = true;
            // 
            // tabPageDetails
            // 
            this.tabPageDetails.Controls.Add(this.panelAssemblyDetails);
            this.tabPageDetails.HorizontalScrollbarBarColor = true;
            this.tabPageDetails.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageDetails.HorizontalScrollbarSize = 10;
            this.tabPageDetails.Location = new System.Drawing.Point(4, 38);
            this.tabPageDetails.Name = "tabPageDetails";
            this.tabPageDetails.Size = new System.Drawing.Size(1247, 657);
            this.tabPageDetails.TabIndex = 0;
            this.tabPageDetails.Text = "Assembly Details";
            this.tabPageDetails.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabPageDetails.VerticalScrollbarBarColor = true;
            this.tabPageDetails.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageDetails.VerticalScrollbarSize = 10;
            // 
            // tabPageClasses
            // 
            this.tabPageClasses.Controls.Add(this.panelClasses);
            this.tabPageClasses.HorizontalScrollbarBarColor = true;
            this.tabPageClasses.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageClasses.HorizontalScrollbarSize = 10;
            this.tabPageClasses.Location = new System.Drawing.Point(4, 38);
            this.tabPageClasses.Name = "tabPageClasses";
            this.tabPageClasses.Size = new System.Drawing.Size(1247, 657);
            this.tabPageClasses.TabIndex = 1;
            this.tabPageClasses.Text = "Classes";
            this.tabPageClasses.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabPageClasses.VerticalScrollbarBarColor = true;
            this.tabPageClasses.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageClasses.VerticalScrollbarSize = 10;
            // 
            // panelClasses
            // 
            this.panelClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelClasses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panelClasses.Location = new System.Drawing.Point(3, 8);
            this.panelClasses.Name = "panelClasses";
            this.panelClasses.Size = new System.Drawing.Size(1239, 640);
            this.panelClasses.TabIndex = 4;
            // 
            // tabPageReferencedAssemblies
            // 
            this.tabPageReferencedAssemblies.Controls.Add(this.panelReferencedAssembiles);
            this.tabPageReferencedAssemblies.HorizontalScrollbarBarColor = true;
            this.tabPageReferencedAssemblies.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPageReferencedAssemblies.HorizontalScrollbarSize = 10;
            this.tabPageReferencedAssemblies.Location = new System.Drawing.Point(4, 38);
            this.tabPageReferencedAssemblies.Name = "tabPageReferencedAssemblies";
            this.tabPageReferencedAssemblies.Size = new System.Drawing.Size(1247, 657);
            this.tabPageReferencedAssemblies.TabIndex = 2;
            this.tabPageReferencedAssemblies.Text = "Referenced Assemblies";
            this.tabPageReferencedAssemblies.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabPageReferencedAssemblies.VerticalScrollbarBarColor = true;
            this.tabPageReferencedAssemblies.VerticalScrollbarHighlightOnWheel = false;
            this.tabPageReferencedAssemblies.VerticalScrollbarSize = 10;
            // 
            // panelReferencedAssembiles
            // 
            this.panelReferencedAssembiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelReferencedAssembiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panelReferencedAssembiles.Location = new System.Drawing.Point(2, 8);
            this.panelReferencedAssembiles.Name = "panelReferencedAssembiles";
            this.panelReferencedAssembiles.Size = new System.Drawing.Size(1239, 640);
            this.panelReferencedAssembiles.TabIndex = 4;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.buttonAbout.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.buttonAbout.Location = new System.Drawing.Point(1181, 98);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(97, 28);
            this.buttonAbout.TabIndex = 5;
            this.buttonAbout.Text = "About...";
            this.buttonAbout.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.buttonAbout.UseSelectable = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1301, 876);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.textBoxAssemblyName);
            this.Controls.Add(this.metroLabel1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabControl.ResumeLayout(false);
            this.tabPageDetails.ResumeLayout(false);
            this.tabPageClasses.ResumeLayout(false);
            this.tabPageReferencedAssemblies.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox textBoxAssemblyName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panelAssemblyDetails;
        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroTabPage tabPageDetails;
        private MetroFramework.Controls.MetroTabPage tabPageClasses;
        private MetroFramework.Controls.MetroTabPage tabPageReferencedAssemblies;
        private System.Windows.Forms.Panel panelClasses;
        private System.Windows.Forms.Panel panelReferencedAssembiles;
        private MetroFramework.Controls.MetroButton buttonAbout;
    }
}

