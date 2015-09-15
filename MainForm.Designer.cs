/*
 * Created by SharpDevelop.
 * User: jonas
 * Date: 2015-08-12
 * Time: 09:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace MSBash
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listlbl = new System.Windows.Forms.Label();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetWowWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 33);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "GO/STOP";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 62);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Reset";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 189);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "Focus";
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 204);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Focus";
			this.label2.Click += new System.EventHandler(this.Label1Click);
			// 
			// listlbl
			// 
			this.listlbl.Location = new System.Drawing.Point(13, 219);
			this.listlbl.Name = "listlbl";
			this.listlbl.Size = new System.Drawing.Size(229, 57);
			this.listlbl.TabIndex = 3;
			this.listlbl.Text = "loglbl";
			// 
			// lstLog
			// 
			this.lstLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstLog.FormattingEnabled = true;
			this.lstLog.Location = new System.Drawing.Point(12, 258);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(230, 173);
			this.lstLog.TabIndex = 4;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fileToolStripMenuItem,
			this.editToolStripMenuItem,
			this.toolsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(254, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItemClick);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.loadToonToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// loadToonToolStripMenuItem
			// 
			this.loadToonToolStripMenuItem.Name = "loadToonToolStripMenuItem";
			this.loadToonToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.loadToonToolStripMenuItem.Text = "Load Toon";
			this.loadToonToolStripMenuItem.Click += new System.EventHandler(this.LoadToonToolStripMenuItemClick);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.resetWowWindowToolStripMenuItem,
			this.captureToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// resetWowWindowToolStripMenuItem
			// 
			this.resetWowWindowToolStripMenuItem.Name = "resetWowWindowToolStripMenuItem";
			this.resetWowWindowToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.resetWowWindowToolStripMenuItem.Text = "Reset Wow Window";
			// 
			// captureToolStripMenuItem
			// 
			this.captureToolStripMenuItem.Name = "captureToolStripMenuItem";
			this.captureToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
			this.captureToolStripMenuItem.Text = "Capture";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 434);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(254, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "txt";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "Toon Files|*.txt|All files|*.*";
			this.openFileDialog1.InitialDirectory = "C:\\Users\\jonas\\Documents\\WOW Scripts\\MSBash";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1FileOk);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 456);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.listlbl);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "MSBash";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.ListBox lstLog;
		private System.Windows.Forms.Label listlbl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem resetWowWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripMenuItem loadToonToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}
