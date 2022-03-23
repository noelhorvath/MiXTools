using Serilog;

namespace MiXTools.View
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.languagesComboBox = new System.Windows.Forms.ComboBox();
            this.applicationSettingsLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.darkModeLabel = new System.Windows.Forms.Label();
            this.enableDarkModeCheckBox = new System.Windows.Forms.CheckBox();
            this.logConsoleCheckBox = new System.Windows.Forms.CheckBox();
            this.logConsoleOnStartupLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.languageLabel = new System.Windows.Forms.Label();
            this.devSettingsLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.openLogConsoleButton = new System.Windows.Forms.Button();
            this.resetAppDataButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // languagesComboBox
            // 
            this.languagesComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languagesComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.languagesComboBox.FormattingEnabled = true;
            this.languagesComboBox.Location = new System.Drawing.Point(439, 3);
            this.languagesComboBox.Name = "languagesComboBox";
            this.languagesComboBox.Size = new System.Drawing.Size(196, 49);
            this.languagesComboBox.TabIndex = 0;
            // 
            // applicationSettingsLabel
            // 
            this.applicationSettingsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.applicationSettingsLabel.Location = new System.Drawing.Point(3, 0);
            this.applicationSettingsLabel.Name = "applicationSettingsLabel";
            this.applicationSettingsLabel.Size = new System.Drawing.Size(638, 111);
            this.applicationSettingsLabel.TabIndex = 1;
            this.applicationSettingsLabel.Text = "APPLICATION_SETTINGS";
            this.applicationSettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.79624F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.20376F));
            this.tableLayoutPanel1.Controls.Add(this.darkModeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.enableDarkModeCheckBox, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 175);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(638, 96);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // darkModeLabel
            // 
            this.darkModeLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.darkModeLabel.Location = new System.Drawing.Point(3, 0);
            this.darkModeLabel.Name = "darkModeLabel";
            this.darkModeLabel.Size = new System.Drawing.Size(535, 96);
            this.darkModeLabel.TabIndex = 4;
            this.darkModeLabel.Text = "DARK_MODE";
            this.darkModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // enableDarkModeCheckBox
            // 
            this.enableDarkModeCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enableDarkModeCheckBox.Location = new System.Drawing.Point(544, 3);
            this.enableDarkModeCheckBox.Name = "enableDarkModeCheckBox";
            this.enableDarkModeCheckBox.Size = new System.Drawing.Size(91, 90);
            this.enableDarkModeCheckBox.TabIndex = 6;
            this.enableDarkModeCheckBox.UseVisualStyleBackColor = true;
            this.enableDarkModeCheckBox.CheckedChanged += new System.EventHandler(this.EnableDarkModeCheckBox_CheckedChanged);
            // 
            // logConsoleCheckBox
            // 
            this.logConsoleCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.logConsoleCheckBox.Location = new System.Drawing.Point(546, 3);
            this.logConsoleCheckBox.Name = "logConsoleCheckBox";
            this.logConsoleCheckBox.Size = new System.Drawing.Size(89, 92);
            this.logConsoleCheckBox.TabIndex = 7;
            this.logConsoleCheckBox.UseVisualStyleBackColor = true;
            this.logConsoleCheckBox.CheckedChanged += new System.EventHandler(this.LogConsoleCheckBox_CheckedChanged);
            // 
            // logConsoleOnStartupLabel
            // 
            this.logConsoleOnStartupLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logConsoleOnStartupLabel.Location = new System.Drawing.Point(3, 0);
            this.logConsoleOnStartupLabel.Name = "logConsoleOnStartupLabel";
            this.logConsoleOnStartupLabel.Size = new System.Drawing.Size(537, 98);
            this.logConsoleOnStartupLabel.TabIndex = 5;
            this.logConsoleOnStartupLabel.Text = "OPEN_LOG_CONSOLE_ON_STARTUP";
            this.logConsoleOnStartupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.applicationSettingsLabel);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Controls.Add(this.devSettingsLabel);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.openLogConsoleButton);
            this.flowLayoutPanel1.Controls.Add(this.resetAppDataButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(641, 623);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.33855F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.66144F));
            this.tableLayoutPanel2.Controls.Add(this.languageLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.languagesComboBox, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 114);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(638, 55);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // languageLabel
            // 
            this.languageLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.languageLabel.Location = new System.Drawing.Point(3, 0);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(429, 46);
            this.languageLabel.TabIndex = 2;
            this.languageLabel.Text = "LANGUAGE";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // devSettingsLabel
            // 
            this.devSettingsLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.devSettingsLabel.Location = new System.Drawing.Point(3, 274);
            this.devSettingsLabel.Name = "devSettingsLabel";
            this.devSettingsLabel.Size = new System.Drawing.Size(638, 105);
            this.devSettingsLabel.TabIndex = 5;
            this.devSettingsLabel.Text = "DEV_SETTINGS";
            this.devSettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.10972F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.89028F));
            this.tableLayoutPanel3.Controls.Add(this.logConsoleCheckBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.logConsoleOnStartupLabel, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 382);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(638, 98);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // openLogConsoleButton
            // 
            this.openLogConsoleButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.openLogConsoleButton.Location = new System.Drawing.Point(3, 486);
            this.openLogConsoleButton.Name = "openLogConsoleButton";
            this.openLogConsoleButton.Size = new System.Drawing.Size(638, 63);
            this.openLogConsoleButton.TabIndex = 6;
            this.openLogConsoleButton.Text = "OPEN_LOG_CONSOLE";
            this.openLogConsoleButton.UseVisualStyleBackColor = true;
            this.openLogConsoleButton.Click += new System.EventHandler(this.OpenLogConsoleButton_Click);
            // 
            // resetAppDataButton
            // 
            this.resetAppDataButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.resetAppDataButton.Location = new System.Drawing.Point(3, 555);
            this.resetAppDataButton.Name = "resetAppDataButton";
            this.resetAppDataButton.Size = new System.Drawing.Size(638, 63);
            this.resetAppDataButton.TabIndex = 7;
            this.resetAppDataButton.Text = "RESET_APPDATA";
            this.resetAppDataButton.UseVisualStyleBackColor = true;
            this.resetAppDataButton.Click += new System.EventHandler(this.ResetAppDataButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 644);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox languagesComboBox;
        private Label applicationSettingsLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label languageLabel;
        private CheckBox logConsoleCheckBox;
        private Label logConsoleOnStartupLabel;
        private Label darkModeLabel;
        private CheckBox enableDarkModeCheckBox;
        private Label devSettingsLabel;
        private Button openLogConsoleButton;
        private Button resetAppDataButton;
        private TableLayoutPanel tableLayoutPanel3;
    }
}