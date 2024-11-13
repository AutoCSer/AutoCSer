namespace AutoCSer.Web.DeployClient
{
    partial class DeployForm
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
            this.htmlButton = new System.Windows.Forms.Button();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.webButton = new System.Windows.Forms.Button();
            this.httpButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.exampleButton = new System.Windows.Forms.Button();
            this.deployServerButton = new System.Windows.Forms.Button();
            this.gameWebButton = new System.Windows.Forms.Button();
            this.gameServerButton = new System.Windows.Forms.Button();
            this.clearMessageButton = new System.Windows.Forms.Button();
            this.openExampleButton = new System.Windows.Forms.Button();
            this.autoCSerZipButton = new System.Windows.Forms.Button();
            this.nugetPackButton = new System.Windows.Forms.Button();
            this.getNugetVersionButton = new System.Windows.Forms.Button();
            this.setNugetVersionButton = new System.Windows.Forms.Button();
            this.nugetStandardPushButton = new System.Windows.Forms.Button();
            this.AutoCSer2PushButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // htmlButton
            // 
            this.htmlButton.Location = new System.Drawing.Point(13, 13);
            this.htmlButton.Name = "htmlButton";
            this.htmlButton.Size = new System.Drawing.Size(116, 32);
            this.htmlButton.TabIndex = 0;
            this.htmlButton.Text = "HTML";
            this.htmlButton.UseVisualStyleBackColor = true;
            this.htmlButton.Click += new System.EventHandler(this.deploy);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Location = new System.Drawing.Point(135, 12);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageTextBox.Size = new System.Drawing.Size(364, 299);
            this.MessageTextBox.TabIndex = 2;
            // 
            // webButton
            // 
            this.webButton.Location = new System.Drawing.Point(12, 51);
            this.webButton.Name = "webButton";
            this.webButton.Size = new System.Drawing.Size(116, 32);
            this.webButton.TabIndex = 3;
            this.webButton.Text = "Web";
            this.webButton.UseVisualStyleBackColor = true;
            this.webButton.Click += new System.EventHandler(this.deploy);
            // 
            // httpButton
            // 
            this.httpButton.Location = new System.Drawing.Point(12, 89);
            this.httpButton.Name = "httpButton";
            this.httpButton.Size = new System.Drawing.Size(116, 32);
            this.httpButton.TabIndex = 4;
            this.httpButton.Text = "Web/Http";
            this.httpButton.UseVisualStyleBackColor = true;
            this.httpButton.Click += new System.EventHandler(this.deploy);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(12, 127);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(116, 32);
            this.searchButton.TabIndex = 5;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.deploy);
            // 
            // exampleButton
            // 
            this.exampleButton.Location = new System.Drawing.Point(13, 165);
            this.exampleButton.Name = "exampleButton";
            this.exampleButton.Size = new System.Drawing.Size(116, 32);
            this.exampleButton.TabIndex = 6;
            this.exampleButton.Text = "Example";
            this.exampleButton.UseVisualStyleBackColor = true;
            this.exampleButton.Click += new System.EventHandler(this.deploy);
            // 
            // deployServerButton
            // 
            this.deployServerButton.Location = new System.Drawing.Point(12, 203);
            this.deployServerButton.Name = "deployServerButton";
            this.deployServerButton.Size = new System.Drawing.Size(116, 32);
            this.deployServerButton.TabIndex = 7;
            this.deployServerButton.Text = "DeployServer";
            this.deployServerButton.UseVisualStyleBackColor = true;
            this.deployServerButton.Click += new System.EventHandler(this.deploy);
            // 
            // gameWebButton
            // 
            this.gameWebButton.Location = new System.Drawing.Point(12, 241);
            this.gameWebButton.Name = "gameWebButton";
            this.gameWebButton.Size = new System.Drawing.Size(116, 32);
            this.gameWebButton.TabIndex = 8;
            this.gameWebButton.Text = "GameWeb";
            this.gameWebButton.UseVisualStyleBackColor = true;
            this.gameWebButton.Click += new System.EventHandler(this.deploy);
            // 
            // gameServerButton
            // 
            this.gameServerButton.Location = new System.Drawing.Point(12, 279);
            this.gameServerButton.Name = "gameServerButton";
            this.gameServerButton.Size = new System.Drawing.Size(116, 32);
            this.gameServerButton.TabIndex = 9;
            this.gameServerButton.Text = "GameServer";
            this.gameServerButton.UseVisualStyleBackColor = true;
            this.gameServerButton.Click += new System.EventHandler(this.deploy);
            // 
            // clearMessageButton
            // 
            this.clearMessageButton.Location = new System.Drawing.Point(505, 279);
            this.clearMessageButton.Name = "clearMessageButton";
            this.clearMessageButton.Size = new System.Drawing.Size(137, 32);
            this.clearMessageButton.TabIndex = 10;
            this.clearMessageButton.Text = "清除消息";
            this.clearMessageButton.UseVisualStyleBackColor = true;
            this.clearMessageButton.Click += new System.EventHandler(this.clearMessageButton_Click);
            // 
            // openExampleButton
            // 
            this.openExampleButton.Location = new System.Drawing.Point(505, 13);
            this.openExampleButton.Name = "openExampleButton";
            this.openExampleButton.Size = new System.Drawing.Size(137, 32);
            this.openExampleButton.TabIndex = 11;
            this.openExampleButton.Text = "测试用例";
            this.openExampleButton.UseVisualStyleBackColor = true;
            this.openExampleButton.Click += new System.EventHandler(this.openExampleButton_Click);
            // 
            // autoCSerZipButton
            // 
            this.autoCSerZipButton.Location = new System.Drawing.Point(505, 51);
            this.autoCSerZipButton.Name = "autoCSerZipButton";
            this.autoCSerZipButton.Size = new System.Drawing.Size(137, 32);
            this.autoCSerZipButton.TabIndex = 12;
            this.autoCSerZipButton.Text = "AutoCSer.zip";
            this.autoCSerZipButton.UseVisualStyleBackColor = true;
            this.autoCSerZipButton.Click += new System.EventHandler(this.autoCSerZipButton_Click);
            // 
            // nugetPackButton
            // 
            this.nugetPackButton.Location = new System.Drawing.Point(505, 165);
            this.nugetPackButton.Name = "nugetPackButton";
            this.nugetPackButton.Size = new System.Drawing.Size(137, 32);
            this.nugetPackButton.TabIndex = 13;
            this.nugetPackButton.Text = "Nuget 打包";
            this.nugetPackButton.UseVisualStyleBackColor = true;
            this.nugetPackButton.Click += new System.EventHandler(this.nugetPackButton_Click);
            // 
            // getNugetVersionButton
            // 
            this.getNugetVersionButton.Location = new System.Drawing.Point(505, 89);
            this.getNugetVersionButton.Name = "getNugetVersionButton";
            this.getNugetVersionButton.Size = new System.Drawing.Size(137, 32);
            this.getNugetVersionButton.TabIndex = 14;
            this.getNugetVersionButton.Text = "获取 Nuget 版本";
            this.getNugetVersionButton.UseVisualStyleBackColor = true;
            this.getNugetVersionButton.Click += new System.EventHandler(this.getNugetVersionButton_Click);
            // 
            // setNugetVersionButton
            // 
            this.setNugetVersionButton.Location = new System.Drawing.Point(505, 127);
            this.setNugetVersionButton.Name = "setNugetVersionButton";
            this.setNugetVersionButton.Size = new System.Drawing.Size(137, 32);
            this.setNugetVersionButton.TabIndex = 15;
            this.setNugetVersionButton.Text = "重算 Nuget 版本";
            this.setNugetVersionButton.UseVisualStyleBackColor = true;
            this.setNugetVersionButton.Click += new System.EventHandler(this.setNugetVersionButton_Click);
            // 
            // nugetStandardPushButton
            // 
            this.nugetStandardPushButton.Location = new System.Drawing.Point(505, 203);
            this.nugetStandardPushButton.Name = "nugetStandardPushButton";
            this.nugetStandardPushButton.Size = new System.Drawing.Size(137, 32);
            this.nugetStandardPushButton.TabIndex = 16;
            this.nugetStandardPushButton.Text = "Nuget 标准包";
            this.nugetStandardPushButton.UseVisualStyleBackColor = true;
            this.nugetStandardPushButton.Click += new System.EventHandler(this.nugetStandardPushButton_Click);
            // 
            // AutoCSer2PushButton
            // 
            this.AutoCSer2PushButton.Location = new System.Drawing.Point(505, 241);
            this.AutoCSer2PushButton.Name = "AutoCSer2PushButton";
            this.AutoCSer2PushButton.Size = new System.Drawing.Size(137, 32);
            this.AutoCSer2PushButton.TabIndex = 17;
            this.AutoCSer2PushButton.Text = "AutoCSer2";
            this.AutoCSer2PushButton.UseVisualStyleBackColor = true;
            this.AutoCSer2PushButton.Click += new System.EventHandler(this.AutoCSer2PushButton_Click);
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 317);
            this.Controls.Add(this.AutoCSer2PushButton);
            this.Controls.Add(this.nugetStandardPushButton);
            this.Controls.Add(this.setNugetVersionButton);
            this.Controls.Add(this.getNugetVersionButton);
            this.Controls.Add(this.nugetPackButton);
            this.Controls.Add(this.autoCSerZipButton);
            this.Controls.Add(this.openExampleButton);
            this.Controls.Add(this.clearMessageButton);
            this.Controls.Add(this.gameServerButton);
            this.Controls.Add(this.gameWebButton);
            this.Controls.Add(this.deployServerButton);
            this.Controls.Add(this.exampleButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.httpButton);
            this.Controls.Add(this.webButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.htmlButton);
            this.Name = "DeployForm";
            this.Text = "AutoCSer 网站发布客户端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button htmlButton;
        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button webButton;
        private System.Windows.Forms.Button httpButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button exampleButton;
        private System.Windows.Forms.Button deployServerButton;
        private System.Windows.Forms.Button gameWebButton;
        private System.Windows.Forms.Button gameServerButton;
        private System.Windows.Forms.Button clearMessageButton;
        private System.Windows.Forms.Button openExampleButton;
        private System.Windows.Forms.Button autoCSerZipButton;
        private System.Windows.Forms.Button nugetPackButton;
        private System.Windows.Forms.Button getNugetVersionButton;
        private System.Windows.Forms.Button setNugetVersionButton;
        private System.Windows.Forms.Button nugetStandardPushButton;
        private System.Windows.Forms.Button AutoCSer2PushButton;
    }
}