﻿/*
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
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Load";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 41);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
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
			this.lstLog.Location = new System.Drawing.Point(12, 279);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(230, 173);
			this.lstLog.TabIndex = 4;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 456);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.listlbl);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "MSBash";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.ListBox lstLog;
		private System.Windows.Forms.Label listlbl;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
	}
}
