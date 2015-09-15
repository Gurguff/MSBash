/*
 * Created by SharpDevelop.
 * User: jonas
 * Date: 2015-08-12
 * Time: 09:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;

namespace MSBash
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	/// 

	public partial class MainForm : Form
	{
		int ConsoleLogLevel=1; // -1:Superdebug, 0:Debug, 1:Info, 2: Warning, 3:Critical, 4:Error, 5: Fatal
		int ScreenLogLevel=1;
		int LogfileLogLevel=1;
		const string LogfileFileName = @"C:\Users\jonas\Documents\WOW Scripts\MSBash\MSBash.log";
		
		
		Rectangle boundingbox;
		GlobalKeyboardHook gkh = new GlobalKeyboardHook();
		Timer myTimer = new Timer();
		//int mode = 0;
		
		List<String> logBuffer = new List<String>();
		List<Point> pixpos = new List<Point>();
		
		Spells spells = null;
		int lastspell = -1; 
		int lastspellcounter=0;
		
		int ax = 974; 		// left edge of the button
		int ay = 798; 		// top edge of the buttons
        int bd = 40;  		// distance between buttons
		int sd = 12; 		// distance between samples
		int os = 6; 		// offset from button corner for first sample point
 		
		bool InBashMode = false;
		int  TargetMode = 0;
		
		public MainForm()
		{
			int i;

			InitializeComponent();

	 		int x1 = ax+os;
	 		int x2 = x1+sd;
	 		int y1 = ay+os;
	 		int y2 = y1+sd;
	
	 		for(i=0; i<4; i++)
	 		{
	 			pixpos.Add( new Point( x1,y1 ));
	 			pixpos.Add( new Point( x2,y1 ));
	 			pixpos.Add( new Point( x1,y2 ));
	 			pixpos.Add( new Point( x2,y2 ));
	 			
				x1 += bd;
				x2 += bd;
	 		}

	 		boundingbox = WindowHelper.BoundingBox( pixpos );

	 		myTimer.Tick += new EventHandler(myEvent);
    		myTimer.Interval = 100;
    		myTimer.Enabled = true;
    		myTimer.Start();
	 		
    		spells = new Spells();
    		if (!spells.Load(@"C:\Users\jonas\Documents\WOW Scripts\MSBash\Gurguff-BM.txt"))
    			spells = null;


	 		/* 
 			//This was to check the time for getting these pixel values - 0.004 to 0.008 s
	 		Stopwatch sw = Stopwatch.StartNew();
	 		List<uint> pxs = WindowHelper.GetSCListPixelColors( pixpos,bb );
	 		sw.Stop();
	 		Log( String.Format("Time Version 4: {0}", sw.Elapsed ));
	 		*/
	 		
	 		this.Load += MainForm_Load;
		}

		private void MainForm_Load(object sender, EventArgs e) {
			gkh.HookedKeys.Add(Keys.Oem1); //The starter - the only one always active!
			gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
		}
		
		static Keys[] AppKeys = { 
			Keys.Oem2,
			Keys.OemMinus
		};
		
		private void GrabKeys()
		{
			for (int i=0; i<AppKeys.Length; i++)
				gkh.HookedKeys.Add(AppKeys[i]); //OemQuestion
		}
		
		private void ReleaseKeys()
		{
			for (int i=0; i<AppKeys.Length; i++)
				gkh.HookedKeys.Remove(AppKeys[i]);
		}
		
		void Log( String msg, int level=0 )
		{
			if (level>=ConsoleLogLevel)
			{
				Debug.WriteLine(msg);
			}
			
			/*
			int visible = (lstLog.ClientSize.Height / lstLog.ItemHeight)-2;
			if (lstLog.Items.Count>visible) 
			{
				lstLog.Items.RemoveAt(0);
			}
			lstLog.Items.Add( msg );
			*/
			//loglbl.Text += String.Format("{0}\n",msg) ;

			if (level>=LogfileLogLevel)
			{
				using (StreamWriter w = File.AppendText(LogfileFileName))
		        {
		            w.WriteLine(msg);
		        }
			}
		}
		
		
		void gkh_KeyDown(object sender, KeyEventArgs e) 
		{	
			Log("Getting key pressed",-1);
			if (e.KeyCode == Keys.Oem1) {
				if (spells!=null)					
				{
					Log("*BASH*");
					button1.BackColor = Color.Red;
					button1.Text = "*BASH*";
					InBashMode = true;
					TargetMode = 0;
					GrabKeys();
				}
			}
			else if ( e.KeyCode == Keys.Oem2 )
			{
				Log("*STOP*");
				button1.BackColor = Color.Transparent;
				button1.Text = "Pause";
				InBashMode = false;
				ReleaseKeys();
			}
			else if ( e.KeyCode == Keys.OemMinus )
			{
				Log( "Setting Targetmode", -1 );
				if (InBashMode)
				{
					if (TargetMode==0)
					{
						Log("Going into AOE mode");
						TargetMode=1;
					}
					else
					{
						Log("Going into AOE mode");
						TargetMode=0;
					}
				}
			}

			e.Handled = true;
		}		
		
		// This method will be called every 50th ms. Lets bail out early if we need to!
		void myEvent(object source, EventArgs e)
		{
			if (!InBashMode || spells==null)
			{
		 		Log( "Skipped as not bashing",-1);
				return;
			}

	 		Stopwatch sw = Stopwatch.StartNew();
	 		List<uint> pxs = WindowHelper.GetSCListPixelColors( pixpos, boundingbox );
	 		
	 		Log( String.Format("{0}\t{1}\t{2}\t{3}",pxs[0], pxs[1], pxs[2], pxs[3] ), 0 );
	 		
	 		int i,setspell=-1;

			//TODO: We can have different lists for different target modes, then it would not 
	 		for (i=0; i<spells.spells.Count; i++)
	 		{

	 			if (spells.spells[i].pos==2 && TargetMode==1)
	 				continue;
	 			if (spells.spells[i].pos==3 && TargetMode==0) 
	 				continue;

	 			int k=(spells.spells[i].pos-1)*4;
				if (
 					(spells.spells[i].vals[0] == pxs[k+0]) &&
 					(spells.spells[i].vals[1] == pxs[k+1]) &&
 					(spells.spells[i].vals[2] == pxs[k+2]) &&
 					(spells.spells[i].vals[3] == pxs[k+3]) 					
 				)
 				{
	 				if (i==lastspell) 
	 				{
	 					if (lastspellcounter>5) //not again (5th time), better skip it!
	 					{
	 						continue;
	 					}
	 				}
	 				setspell=i;
 					break;
	 			}
	 		}
	 		if (setspell==-1) return; // No spell to cast

	 		if (setspell==lastspell && lastspellcounter>5)
	 		{
	 			//Only here if no other spell found and spammed... 
	 			Log("WTF....",4);
	 		}
			else
			{	 				
				MySendKey(spells.spells[setspell].key, spells.spells[setspell].name);
				if (lastspell!=setspell)
					lastspellcounter=0;
				else
					lastspellcounter++;
				lastspell=setspell;
			}
	 		
	 		sw.Stop();
		 	Log( String.Format("Time to decide: {0}", sw.Elapsed ),0);
		}

		void MySendKey(string key, string spellname)
		{
			Dictionary<string, short> keytable = new Dictionary<string,short>()
			{
				{ "1", 		0x31 },
				{ "2", 		0x32 },
				{ "3", 		0x33 },
				{ "4", 		0x34 },
				{ "5", 		0x35 },
				{ "6", 		0x36 },
				{ "7", 		0x37 },
				{ "8", 		0x38 },
				{ "9", 		0x39 },
				{ "0", 		0x30 },
				{ "{F1}", 	0x70 },
				{ "{F2}", 	0x71 },
				{ "{F3}", 	0x72 },
				{ "{F4}", 	0x73 },
				{ "{F5}", 	0x74 },
				{ "{F6}", 	0x75 },
				{ "{F7}", 	0x76 },
				{ "{F8}", 	0x77 },
				{ "{F9}", 	0x78 },
				{ "{F10}", 	0x79 },
				{ "{F11}", 	0x7A },
				{ "{F12}",	0x7B },
			};
			
	 		if (WindowHelper.ActivateWoW())
	 		{
	 			short keycode;
	 			if (keytable.TryGetValue(key, out keycode))
	 			{
		 			Log( String.Format("\t{0}\t{1}", key, spellname ), 1);
	 			}
	 			else
	 			{
	 				SendKeys.Send(key);
		 			Log( String.Format("\t{0}\t{1}", key, spellname ), 3);
	 			}
	 		}
			
		}
		
		void UpdatePlayerStatus()
		{
			/*
			int i;
			
			//Health:
			int health=0;
			for (i=80; i<89; i++)
			{
				if (pixcol[i]!=0x000C0C0C)
					health += (328980/10);
			}
			
			//Focus:
			int focus=0;
			for (i=90; i<99; i++)
			{
				if (pixcol[i]==0x0000FF00)
					focus += (120/10);
			}
			
			//Spell Statuses
			//for (i=100; i<108; i++)
			//{
				//spellstatuses[i-100].ready = (pixcol[i]==0x0000FF01);
			//}
			
			//short v = (short) (((currenttime-lastPlayerStatus.timestamp)*2)/100);
			//lastPlayerStatus.value += v;
			//if (lastPlayerStatus.value>120) 
			//	lastPlayerStatus.value = 120;
			//Log("Adding {0} focus => {1}", v, lastPlayerStatus.value);
			*/
		}
		
		void UpdateStatus()
		{
			UpdatePlayerStatus();
		}


		void Tick()
		{
			// if ( in attackmode!)
			//List<uint> pixcol = WindowHelper.GetSCListPixelColors(pixpos);
			//int spellno = 0, cast = -1;
			/*
			foreach (Spell spell in spells.spells)

				// Get statuses
			
			UpdateStatus();
			
			// TODO: gcd check here !
			
			// SpellStatus
			for (int i=0; i<spells.Length; i++)
			{
				if (spell.pos==1 && attackmode & USE_POS1 )
				{
					if ( pixcol[0]==spell.vals[0] && pixcol[1]==spell.vals[1] && pixcol[2]==spell.vals[2] && pixcol[3]==spell.vals[3] )
					{
						cast = spellno;
						break;
					}
				}

				if (spell.pos==2 && attackmode & USE_POS2 )
				{
					if ( pixcol[4]==spell.vals[0] && pixcol[5]==spell.vals[1] && pixcol[6]==spell.vals[2] && pixcol[7]==spell.vals[3] )
					{
						cast = spellno;
						break;
					}
				}

				if (spell.pos==3 && attackmode & USE_POS3 )
				{
					if ( pixcol[8]==spell.vals[0] && pixcol[9]==spell.vals[1] && pixcol[10]==spell.vals[2] && pixcol[11]==spell.vals[3] )
					{
						cast = spellno;
						break;
					}
				}

				if (spell.pos==4 && attackmode & USE_POS4 )
				{
					if ( pixcol[12]==spell.vals[0] && pixcol[13]==spell.vals[1] && pixcol[14]==spell.vals[2] && pixcol[15]==spell.vals[3] )
					{
						cast = spellno;
						break;
					}
				}

				spellno++;
			}
			
			if (prio_order.Count>0)
			{
				int prio = (int)prio_order[0];
				//Log(String.Format("At {0} list is: ({1})",currenttime, lastPlayerStatus.value));
				for (int i=0; i<prio_order.Count && i<2; i++)
				{
					//Log(String.Format("{0}\t{1}\t{2}", i, spells[prio_order[i]].name, spells[prio_order[i]].value));
				}
				
				//Pick top one to cast!
				Log(String.Format("{0,8} {1,4}: {3}({4})", currenttime, lastPlayerStatus.value, prio, spells[prio].name,spells[prio].value));
				spellstatuses[prio].castedat= currenttime;
				spellstatuses[prio].availableat=(currenttime+spells[prio].casttime+spells[prio].cooldown);
				spellstatuses[prio].ready = false;
				lastPlayerStatus.value -= spells[prio].value;
				lastPlayerStatus.timestamp = currenttime;
				lastPlayerStatus.gcdoff_at = (spells[prio].globalcd+currenttime);
				lastPlayerStatus.castdone_at = (spells[prio].casttime+currenttime);
			}
			else
				Log(String.Format("{0,8} {1,4}: <n/a>", currenttime, lastPlayerStatus.value));
			*/
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		}
		
		void InitializePlayerStatus()
		{
			/*
			lastPlayerStatus.health = 320000;
			lastPlayerStatus.value 	= 120;
			lastPlayerStatus.tokens = 0;			
			lastPlayerStatus.gcdoff_at = 0;
			lastPlayerStatus.timestamp = 0;
			*/
		}
		
		void Label1Click(object sender, EventArgs e)
		{
				
		}
		void QuitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		void Button2Click(object sender, EventArgs e)
		{
			WindowHelper.ResetWoW();
		}
		void LoadToonToolStripMenuItemClick(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				Spells newtoon = new Spells();
				if ( newtoon.Load( openFileDialog1.FileName ) )
				{
					spells = newtoon;
				}
			}
		}
		
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
	
		}
	}
		
	public class Spell
	{
		public string key;
		public string name;
		public int    pos;
		public int[]  vals = new int[4];
		
		public void SpellLine(String line)			
		{
			line = line.Replace("\t",""); //remove tabs
			line = line.Replace(" ","");  //remove spaces
			List<string> tokens = new List<string>(line.Split(','));
			
			key = tokens[0];
			name = tokens[1];
			pos = System.Int32.Parse(tokens[2]);
			vals[0] = System.Int32.Parse(tokens[3]);
			vals[1] = System.Int32.Parse(tokens[4]);
			vals[2] = System.Int32.Parse(tokens[5]);
			vals[3] = System.Int32.Parse(tokens[6]);
		}
	}
	
	public class Spells
	{
		public string toon;
		public List<Spell> spells = new List<Spell>();
		
		public bool Load(string filename)
		{
			string[] lines = System.IO.File.ReadAllLines(filename);
			if ( ((lines[0].Equals("Rotator") && lines[1].Equals("v0.2") ) ) )
			{    
			    toon = lines[2];
			    for (int i=3; i< lines.Length;i++)
			    {
			    	if (lines[i].Count()>8)
			    	{
			    		Spell s = new Spell();
			    		s.SpellLine(lines[i]);
			    		spells.Add(s);
			    	}
			    }
			}
			return true;
		}
		
		public static int[,] spots = { { 828,840,683,695 }, { 894,906,683,695 } };
		
		public Spells()
		{
		}
		
	}

	public static class WindowHelper
	{
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;

		public static int win_pos_x = 190;
		public static int win_pos_y = 0;
		public static int win_size_x = 1726;
		public static int win_size_y = 1108;

		
		// ******************************************************************
	    [DllImport("user32.dll")]
	    static extern IntPtr GetDC(IntPtr hwnd);
	
	    [DllImport("user32.dll")]
	    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
	
	    [DllImport("gdi32.dll")]
	    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
	
		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

		// Delegate to filter which windows to include 
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


		// Activate an application window.
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		
		[DllImport("USER32.DLL")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
    		public int Left;        // x position of upper-left corner
    		public int Top;         // y position of upper-left corner
    		public int Right;       // x position of lower-right corner
    		public int Bottom;      // y position of lower-right corner
		}

		/// <summary> Get the text for the window pointed to by hWnd </summary>
		public static string GetWindowText(IntPtr hWnd)
		{
		    int size = GetWindowTextLength(hWnd);
		    if (size > 0)
		    {
		        var builder = new StringBuilder(size + 1);
		        GetWindowText(hWnd, builder, builder.Capacity);
		        return builder.ToString();
		    }
		
		    return String.Empty;
		}
		
		/// <summary> Find all windows that match the given filter </summary>
		/// <param name="filter"> A delegate that returns true for windows
		///    that should be returned and false for windows that should
		///    not be returned </param>
		public static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)
		{
		  IntPtr found = IntPtr.Zero;
		  List<IntPtr> windows = new List<IntPtr>();
		
		  EnumWindows(delegate(IntPtr wnd, IntPtr param)
		  {
		      if (filter(wnd, param))
		      {
		          // only add the windows that pass the filter
		          windows.Add(wnd);
		      }
		
		      // but return true here so that we iterate all windows
		      return true;
		  }, IntPtr.Zero);
		
		  return windows;
		}
		
		/// <summary> Find all windows that contain the given title text </summary>
		/// <param name="titleText"> The text that the window title must contain. </param>
		public static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
		{
		    return FindWindows(delegate(IntPtr wnd, IntPtr param)
		    {
		        return GetWindowText(wnd).Contains(titleText);
		    });
		} 

		// Dont use!
		static public uint GetPixelColor(int x, int y)
	    {
	        IntPtr hdc = GetDC(IntPtr.Zero);
	        uint pixel = GetPixel(hdc, x, y);
	        ReleaseDC(IntPtr.Zero, hdc);
	        return pixel;
	    }	
		
		// Dont use!
		static public List<uint> GetListPixelColors(List<int> xs, List<int> ys)
		{
	        IntPtr hdc = GetDC(IntPtr.Zero);
	        List<uint> pix = new List<uint>();
	        for (int i=0; i<xs.Count; i++)
	        	pix.Add(GetPixel(hdc, xs[i], ys[i]));
	        return pix;
		}
		
		// 
		static public Rectangle BoundingBox(IEnumerable<Point> points)
		{
    		var x_query = from Point p in points select p.X;
    		int xmin = x_query.Min();
    		int xmax = x_query.Max();

    		var y_query = from Point p in points select p.Y;
    		int ymin = y_query.Min();
    		int ymax = y_query.Max();

    		return new Rectangle(xmin, ymin, (xmax - xmin)+1, (ymax - ymin)+1);
		}
		
		static public List<uint> GetSCListPixelColors(List<Point> ps, Rectangle boundingbox)
		{
			ScreenCapture sc = new ScreenCapture();
			// capture the screen and stores it in a image
			Image img = sc.CaptureScreenRectangle(boundingbox);
			Bitmap bmp = new Bitmap(img);
	        List<uint> pix = new List<uint>();
	        for (int i=0; i<ps.Count; i++)
	        {
	        	System.Drawing.Color cl = bmp.GetPixel(ps[i].X-boundingbox.X,ps[i].Y-boundingbox.Y);
	        	pix.Add((uint)(cl.ToArgb()&0x00FFFFFF));
	        }
	        return pix;
		}
	        
		static public List<uint> GetSCListPixelColors(List<Point> ps, Point tl, Point br)
		{
			ScreenCapture sc = new ScreenCapture();
			// capture the screen and stores it in a image
			Image img = sc.CaptureScreen();
			Bitmap bmp = new Bitmap(img);
	        List<uint> pix = new List<uint>();
	        for (int i=0; i<ps.Count; i++)
	        {
	        	System.Drawing.Color cl = bmp.GetPixel(ps[i].X, ps[i].Y);
	        	pix.Add((uint)(cl.ToArgb()&0x00FFFFFF));
	        }
	        return pix;
		}

		static private IntPtr FindWoWWindow()
		{
			IntPtr wowwindow = IntPtr.Zero;
			var wowwindows = FindWindowsWithText("World of Warcraft");
			if (wowwindows.Count()>0)
				wowwindow = wowwindows.ElementAt(0);
			return wowwindow;
		}
		static public bool ResetWoW()
		{
			IntPtr wHandle = FindWoWWindow();
			if (wHandle == IntPtr.Zero) return false;

			SetWindowPos(wHandle, 0, win_pos_x, win_pos_y, win_size_x, win_size_y, SWP_NOZORDER | SWP_SHOWWINDOW);
			return true;
 		}

		static public Rectangle FindWoW()
		{
			IntPtr wHandle = FindWoWWindow();
			if (wHandle == IntPtr.Zero) return new Rectangle();
			
			RECT r = new RECT();
			if (GetWindowRect( wHandle, out r) ) return new Rectangle(r.Left,r.Top,r.Right-r.Left,r.Bottom-r.Top);
			return new Rectangle();
		}
		
		static public bool ActivateWoW()
		{
			IntPtr wHandle = FindWoWWindow();
			if (wHandle == IntPtr.Zero) return false;
			return SetForegroundWindow( wHandle );
		}

	}
    /// <summary>
    /// Provides functions to capture the entire screen, 
    /// or a particular window, and save it to a file.
    /// </summary>
    public class ScreenCapture
    {
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow( User32.GetDesktopWindow() );
        }
        
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. 
        /// (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle,ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,width,height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest,0,0,width,height,hdcSrc,0,0,GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest,hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle,hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreenPart(Point tl, Point br)
        {
            return CaptureWindow( User32.GetDesktopWindow(), tl, br );
        }
        
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. 
        /// (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle, Point tl, Point br)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,br.X-tl.X,br.Y-tl.Y);
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            GDI32.BitBlt(hdcDest,0,0,br.X-tl.X,br.Y-tl.Y,hdcSrc,tl.X,tl.Y,GDI32.SRCCOPY);
            GDI32.SelectObject(hdcDest,hOld);
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle,hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            GDI32.DeleteObject(hBitmap);
            return img;
        }



            
        /// <summary>
        /// Creates an Image object containing part of a screen shot of the entire desktop specified by a rectangle
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreenRectangle(Rectangle rectangle)
        {
            return CaptureWindowRectangle( User32.GetDesktopWindow(), rectangle );
        }
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific part (rectangle) of awindow 
        /// </summary>
        /// <param name="handle">The handle to the window. 
        /// (In windows forms, this is obtained by the Handle property)</param>
        /// <param name="rectangle">The rectangle specifying the part</param>
        /// <returns></returns>
        public Image CaptureWindowRectangle(IntPtr handle, Rectangle rectangle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            int width = rectangle.Width;
            int height = rectangle.Height;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,rectangle.Width,rectangle.Height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest,0,0,rectangle.Width,rectangle.Height,hdcSrc,rectangle.Left,rectangle.Top,GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest,hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle,hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        	
        
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename,format);
        }
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename,format);
        }
        /// <summary>
        /// Captures a part of a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="rectangle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, Rectangle rectangle, string filename, ImageFormat format)
        {
            Image img = CaptureWindowRectangle(handle,rectangle);
            img.Save(filename,format);
        }
        /// <summary>
        /// Captures a part of a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(Rectangle rectangle, string filename, ImageFormat format)
        {
            Image img = CaptureScreenRectangle(rectangle);
            img.Save(filename,format);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject,int nXDest,int nYDest,
                int nWidth,int nHeight,IntPtr hObjectSource,
                int nXSrc,int nYSrc,int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC,int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC,IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd,IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd,ref RECT rect);
        }
    }
}
