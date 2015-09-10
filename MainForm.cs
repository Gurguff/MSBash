/*
 * Created by SharpDevelop.
 * User: jonas
 * Date: 2015-08-12
 * Time: 09:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace MSBash
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	/// 
	public class Spell
	{
		string key;
		string name;
		int    pos;
		int[]  vals = new int[4];
		
		public Spell(string line)			
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
		string toon;
		IList<Spell> spells;
		
		public bool Load(string filename)
		{
			string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");
			if ( !((lines[0].Equals("Rotator") && lines[1].Equals("v0.2") ) ) )
			{    
			    toon = lines[2];
			    for (int i=3; i< lines.Length;i++)
			    {
			    	spells.Add(new Spell(lines[i]));
			    }
			}
			return true;
		}
		
		public static int[,] spots = { { 828,840,683,695 }, { 894,906,683,695 } };
		
		
		public Spells()
		{
		}
		
	}

	public partial class MainForm : Form
	{
		
		Timer myTimer = new Timer();
		
		
		List<String> logBuffer = new List<String>();
		List<Point> pixpos = new List<Point>();

		int ax = 974; 		// left edge of the button
		int ay = 798; 		// top edge of the buttons
        int bd = 40;  		// distance between buttons
		int sd = 12; 		// distance between samples
		int os = 6; 		// offset from button corner for first sample point
 		
		public MainForm()
		{
			int i,j,ox=1586,oy=778;

			InitializeComponent();

	 		int x1 = ax+os;
	 		int x2 = x1+sd;
	 		int y1 = ay+os;
	 		int y2 = y1+sd;
	
	 		for(i=0; i<4; i++)
	 		{
	 			pixpos.Add( new Point( x1,y1 ));
	 			pixpos.Add( new Point( x1,y2 ));
	 			pixpos.Add( new Point( x2,y1 ));
	 			pixpos.Add( new Point( x2,y2 ));
	 			
				x1 += bd;
				x2 += bd;
	 		}

	 		Rectangle bb = WindowHelper.BoundingBox( pixpos );

	 		Stopwatch sw = Stopwatch.StartNew();
	 		
	 		List<uint> pxs = WindowHelper.GetSCListPixelColors( pixpos,bb );
			
	 		sw.Stop();

	 		Log( String.Format("Time Version 4: {0}", sw.Elapsed ));
	 		
	 		
			//TimeSpan ts = DateTime.Now.TimeOfDay;
			//basetime = (Int64) ((3600000*ts.Hours)+(60000*ts.Minutes)+(1000*ts.Seconds)+ts.Milliseconds);
		
			
			
			//Version 3 [500 ~0.05s 1000 ~0.05s 4000 ~0.05s] :)
			//sw.Reset();
			//sw.Start();
			//	pxs = WindowHelper.GetSCListPixelColors( xs, ys );
			//sw.Stop();
			//Log( String.Format("Time Version 3: {0}", sw.Elapsed ));

			
			

			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			/*
			for (int t=0; currenttime<20000; t++)
			{
				Tick();
				currenttime += 300;
				short nextat = 0;
				if (lastPlayerStatus.gcdoff_at > lastPlayerStatus.castdone_at )
				{
					nextat = lastPlayerStatus.gcdoff_at;
					//Log("gcd");
				}
				else
				{
					nextat = lastPlayerStatus.castdone_at;
					//Log("cast");
				}
				if (nextat>currenttime )
					currenttime = nextat;
				
			}
			*/			
		}
		
		void Log( String msg )
		{
			Debug.WriteLine(msg);
			
			/*
			int visible = (lstLog.ClientSize.Height / lstLog.ItemHeight)-2;
			if (lstLog.Items.Count>visible) 
			{
				lstLog.Items.RemoveAt(0);
			}
			lstLog.Items.Add( msg );
			*/
			//loglbl.Text += String.Format("{0}\n",msg) ;
		}
		
		void myEvent(object source, EventArgs e)
		{
			/*
			TimeSpan ts = DateTime.Now.TimeOfDay;

			Int64 spantime = (Int64) ((3600000*ts.Hours)+(60000*ts.Minutes)+(1000*ts.Seconds)+ts.Milliseconds);
			currenttime = spantime-basetime; 
			
			long nextat = 0;
			if (lastPlayerStatus.gcdoff_at > lastPlayerStatus.castdone_at )
			{
				nextat = lastPlayerStatus.gcdoff_at;
				//Log("gcd");
			}
			else
			{
				nextat = lastPlayerStatus.castdone_at;
				//Log("cast");
			}
			
			if (nextat<=currenttime )
			{
				Tick();
			}
			*/
		}
			
		void UpdatePlayerStatus()
		{
			/*
			int i;
			List<uint> pixcol = WindowHelper.GetSCListPixelColors(pixpos);
			
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
			for (i=100; i<108; i++)
			{
				spellstatuses[i-100].ready = (pixcol[i]==0x0000FF01);
			}
			
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
			// Get statuses
			
			UpdateStatus();
			
			// TODO: gcd check here !
			
			// SpellStatus
			/*
			for (int i=0; i<spells.Length; i++)
			{
				if (spellstatuses[i].availableat<=currenttime &&			//spell is available at this time
				    spells[i].value<lastPlayerStatus.value				//player have enough value
				)
				{
					spellstatuses[i].ready = true;						// => spell is ready

					// Special overwrite rules:
					if (spells[i].id==2)
					{
						//Kill Shot never ready yet as we dont know targets health yet
						spellstatuses[i].ready = false;						
					}
					if (spells[i].id==4)
					{
						// Focus Shot only if value < 50
						if (lastPlayerStatus.value > 30)
							spellstatuses[i].ready = false;						
					}
					if (spells[i].id==5)
					{
						// Arcane Shot only if value > 90
						if (lastPlayerStatus.value < 80)
							spellstatuses[i].ready = false;						
					}
				}
			}

			// Prioritize
			pick_order = new List<Int64>();
			for (int i=0; i<spells.Length; i++)			
			{
				if (spellstatuses[i].ready)
				{
					//Log(String.Format("Found that spell {0} is ready to cast",spells[i].name));
					pick_order.Add((Int64)i);
				}
			}
			
			prio_order = new List<Int64>();
			for (int i=0; i<pick_order.Count; i++)			
			{
				bool added = false;
				if (prio_order.Count>0)
				{
					for (int j=0; j<i; j++)
					{
						if (spells[prio_order[j]].prios>spells[pick_order[i]].prios)
						{
							prio_order.Insert(j, (Int64)pick_order[i]);
							added = true;
							break;
						}
					}
				}
				if (!added)
					prio_order.Add((Int64)pick_order[i]);
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
	}
		
	public static class WindowHelper
	{
	    // ******************************************************************
	    [DllImport("user32.dll")]
	    static extern IntPtr GetDC(IntPtr hwnd);
	
	    [DllImport("user32.dll")]
	    static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
	
	    [DllImport("gdi32.dll")]
	    static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
	
		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Activate an application window.
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

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
	        
		static public List<uint> GetSCListPixelColors(List<Point> ps)
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


		static public bool ActivateWoW()
		{
			IntPtr wHandle = FindWindow(null,"World of Warcraft");
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
