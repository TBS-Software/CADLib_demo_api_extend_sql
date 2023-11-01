namespace TBS_CADLib_demo_sql_connector
{
    partial class TBS_Demo_Form
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
            this.toolStripMenu_TBS = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip_DemoSQL = new System.Windows.Forms.MenuStrip();
            this.tool_DemoSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_UsingAPI = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_UsingManualSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_UsingAutoSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_DemoSQL.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu_TBS
            // 
            this.toolStripMenu_TBS.Name = "toolStripMenu_TBS";
            this.toolStripMenu_TBS.Size = new System.Drawing.Size(270, 6);
            // 
            // menuStrip_DemoSQL
            // 
            this.menuStrip_DemoSQL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_DemoSQL});
            this.menuStrip_DemoSQL.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_DemoSQL.Name = "menuStrip_DemoSQL";
            this.menuStrip_DemoSQL.Size = new System.Drawing.Size(800, 24);
            this.menuStrip_DemoSQL.TabIndex = 2;
            this.menuStrip_DemoSQL.Text = "menuStrip_DemoSQL";
            // 
            // tool_DemoSQL
            // 
            this.tool_DemoSQL.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenu_TBS,
            this.MenuItem_UsingAPI,
            MenuItem_UsingManualSQL,
            this.MenuItem_UsingAutoSQL});
            this.tool_DemoSQL.Name = "tool_DemoSQL";
            this.tool_DemoSQL.Size = new System.Drawing.Size(97, 20);
            this.tool_DemoSQL.Text = "TBS Demo SQL";
            // 
            // MenuItem_UsingAPI
            // 
            this.MenuItem_UsingAPI.Name = "MenuItem_UsingAPI";
            this.MenuItem_UsingAPI.Size = new System.Drawing.Size(273, 22);
            this.MenuItem_UsingAPI.Text = "Исполнить запрос стандартным API";
            this.MenuItem_UsingAPI.Click += new System.EventHandler(this.MenuItem_UsingAPI_Click);
            // 
            // MenuItem_UsingAutoSQL
            // 
            this.MenuItem_UsingAutoSQL.Name = "MenuItem_UsingAutoSQL";
            this.MenuItem_UsingAutoSQL.Size = new System.Drawing.Size(273, 22);
            this.MenuItem_UsingAutoSQL.Text = "Исполнить запрос автоматически";
            this.MenuItem_UsingAutoSQL.Click += new System.EventHandler(this.MenuItem_UsingAutoSQL_Click);
            // 
            // MenuItem_UsingManualSQL
            // 
            this.MenuItem_UsingManualSQL.Name = "MenuItem_UsingManualSQL";
            this.MenuItem_UsingManualSQL.Size = new System.Drawing.Size(273, 22);
            this.MenuItem_UsingManualSQL.Text = "Исполнить запрос вручную";
            this.MenuItem_UsingManualSQL.Click += new System.EventHandler(this.MenuItem_UsingOpenSQL_Click);
            // 
            // TBS_Demo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip_DemoSQL);
            this.Name = "TBS_Demo_Form";
            this.Text = "TBS_Demo_Form";
            this.menuStrip_DemoSQL.ResumeLayout(false);
            this.menuStrip_DemoSQL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripMenu_TBS;
        private System.Windows.Forms.MenuStrip menuStrip_DemoSQL;
        private System.Windows.Forms.ToolStripMenuItem tool_DemoSQL;

        private System.Windows.Forms.ToolStripMenuItem MenuItem_UsingAPI;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_UsingManualSQL;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_UsingAutoSQL;
    }
}