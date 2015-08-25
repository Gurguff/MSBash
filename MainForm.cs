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
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace MSBash
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class Spell 
	{
		//static part:
		public Int64 id;
		public String name;
 		public Int64 casttime;	//time from start to cast until actually happening - this is when it can be interrupted!
 		public Int64 cooldown;	//time from cast until ready again
 		public Int64 duration;  //time for the spell to be active
 		public Int64 globalcd;  //time until a different action can be used
 		public Int64 value;		//focus, rage...
 		public Int64 tokens;	//the "other cost" (rogue points, palladin charges)
 		public Int64 prios;
 		public Int64 priom;
 		public bool  bigcd;
 		public bool  smallcd;
 		public bool	 defcd;
		 		
 		public Spell(Int64 i, String n, Int64 ct, Int64 cd, Int64 dr, Int64 gcd, Int64 cst, Int64 sp, Int64 mt )
 		{
 			id = i; name = n; casttime = ct; cooldown = cd;
 		    duration = dr; globalcd = gcd; value = cst; 
 		    prios = sp; priom = mt;
 		}
	}

	public class SpellStatus
	{
		public Int64 id;
		public Int64 castedat;
		public Int64 availableat;
		public bool	 ready;
		
		public SpellStatus(Int64 i, Int64 c, Int64 a, bool r)
		{
			id = i; castedat=c; availableat=a; ready=r;
		}
	}
	
	
	public class PlayerStatus
	{
		public Int64 value;
		public Int64 tokens;
		public Int32 health;
		public Boolean hasTarget;
		public Boolean inCombat;
		public Int64 gcdoff_at;
		public Int64 castdone_at;
		public Int64 timestamp;
		
	}

	public partial class MainForm : Form
	{
		
		Timer myTimer = new Timer();
		
		PlayerStatus lastPlayerStatus = new PlayerStatus();
		Spell[] spells = { 
			//			ID	Name			Cast	CD		Dura	GCD		Value	STP		MTP
			new Spell( 	1, 	"Kill Command", 0, 		6000, 	0, 		1000, 	40, 	1, 		5),
			new Spell( 	2, 	"Kill Shot", 	0, 		10000, 	0, 		0, 		0,	 	2, 		6),
			new Spell( 	3, 	"Barrage", 		0, 		3000, 	3000,	1000, 	60, 	3, 		2),
			new Spell( 	4, 	"Focus Shot",  	5000, 	1000, 	0, 		0, 		-50, 	4, 		4),
			new Spell( 	5, 	"Arcane Shot", 	0, 		3000, 	0, 		1000, 	30, 	5, 		3),
			new Spell( 	6, 	"Multi Shot", 	0, 		3000, 	0, 		1000, 	30, 	6, 		1),
		};
		
		SpellStatus[] spellstatuses = {
			new SpellStatus(1, -1, 0, false),
			new SpellStatus(2, -1, 0, false),
			new SpellStatus(3, -1, 0, false),
			new SpellStatus(4, -1, 0, false),
			new SpellStatus(5, -1, 0, false),
			new SpellStatus(6, -1, 0, false)
		};
		
		List<Int64> prio_order, pick_order;

		Int64 basetime = 0, currenttime = 0;
		
		
		List<String> logBuffer = new List<String>();
		
		public MainForm()
		{
			//myTimer.Tick += new EventHandler(myEvent);
    		//myTimer.Interval = 50;
    		//myTimer.Enabled = true;
    		//myTimer.Start();

			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		
			InitializePlayerStatus();
			
			TimeSpan ts = DateTime.Now.TimeOfDay;
			
			basetime = (Int64) ((3600000*ts.Hours)+(60000*ts.Minutes)+(1000*ts.Seconds)+ts.Milliseconds);
			
			
			Random rnd = new Random();
			
			List<int> xs = new List<int>();
			List<int> ys = new List<int>();
			
			Log( "Generating data" );
			
			for(int i = 0; i<500; i++)
			{
				xs.Add(rnd.Next(250,500));
				ys.Add(rnd.Next(250,500));       
			}
				
			Log( "Measuring..." );
			
			Stopwatch sw = new Stopwatch();						
			
			List<uint> pxs;
			sw.Start();
				pxs = WindowHelper.GetListPixelColors( xs, ys );
			sw.Stop();
			Log( String.Format("List Version: {0}", sw.Elapsed ));
			
			pxs = new List<uint>();
			
			sw.Reset();
			sw.Start();
			for (int i=0; i<500; i++)
				pxs.Add(WindowHelper.GetPixelColor( xs[i], ys[i] ));
			sw.Stop();
			Log( String.Format("Other Version: {0}", sw.Elapsed ));
			
			

			
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
			
			
			int visible = (lstLog.ClientSize.Height / lstLog.ItemHeight)-2;
			if (lstLog.Items.Count>visible) 
			{
				lstLog.Items.RemoveAt(0);
			}
			lstLog.Items.Add( msg );
			
			//loglbl.Text += String.Format("{0}\n",msg) ;
		}
		
		void myEvent(object source, EventArgs e)
		{
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
		
		}
			
		void UpdatePlayerStatus()
		{
			short v = (short) (((currenttime-lastPlayerStatus.timestamp)*2)/100);
			lastPlayerStatus.value += v;
			if (lastPlayerStatus.value>120) 
				lastPlayerStatus.value = 120;
			//Log("Adding {0} focus => {1}", v, lastPlayerStatus.value);
		}
		
		void Tick()
		{
			// Get statuses

			// Global Status
			// UpdateGlobalStatus() - here we just assume its done in the cast phase!

			// PlayerStatus
			UpdatePlayerStatus();
			
			// TODO: gcd check here !
			
			// SpellStatus
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
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		}
		
		void InitializePlayerStatus()
		{
			lastPlayerStatus.health = 320000;
			lastPlayerStatus.value 	= 120;
			lastPlayerStatus.tokens = 0;			
			lastPlayerStatus.gcdoff_at = 0;
			lastPlayerStatus.timestamp = 0;
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

		static public uint GetPixelColor(int x, int y)
	    {
	        IntPtr hdc = GetDC(IntPtr.Zero);
	        uint pixel = GetPixel(hdc, x, y);
	        ReleaseDC(IntPtr.Zero, hdc);
	        return pixel;
	    }	
		
		static public List<uint> GetListPixelColors(List<int> xs, List<int> ys)
		{
	        IntPtr hdc = GetDC(IntPtr.Zero);
	        List<uint> pix = new List<uint>();
	        for (int i=0; i<xs.Count; i++)
	        	pix.Add(GetPixel(hdc, xs[i], ys[i]));
	        return pix;
		}
	        
		static public bool ActivateWoW()
		{
			IntPtr wHandle = FindWindow(null,"World of Warcraft");
			if (wHandle == IntPtr.Zero) return false;
			return SetForegroundWindow( wHandle );
		}

	}
}
