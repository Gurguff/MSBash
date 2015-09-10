/*
 * Created by SharpDevelop.
 * User: jonas
 * Date: 2015-08-26
 * Time: 13:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MSBash
{
	/// <summary>
	/// Description of NSpells.
	/// </summary>
	/*	
	public virtual class NSpell
	{
		//static part:
		public long id;
		public string name;
 		public long casttime;	//time from start to cast until actually happening - this is when it can be interrupted!
 		public long cooldown;	//time from cast until ready again
 		public long duration;  //time for the NSpell to be active
 		public long globalcd;  //time until a different action can be used
 		public long value;		//focus, rage...
 		public long tokens;	//the "other cost" (rogue points, palladin charges)
 		public long prios;
 		public long priom;
 		public bool  bigcd;
 		public bool  smallcd;
 		public bool	 defcd;
		 		
 		
 		public NSpell(long i, String n, long ct, long cd, long dr, long gcd, long cst, long sp, long mt )
 		{
 			id = i; name = n; casttime = ct; cooldown = cd;
 		    duration = dr; globalcd = gcd; value = cst; 
 		    prios = sp; priom = mt;
 		}
		
		
 		public int Trigger(NSpellStatus ss, PlayerStatus ps)
 		{
 			return -1;
 		}
	}
	
	public class ArcaneShot : NSpell
	{
		public ArcaneShot()
		{
			NSpell( 1, "Arcane Shot", 0,0,0,1500,40,10,10 );
		}
		public int Trigger(NSpellStatus ss, PlayerStatus ps)
		{
 			if (ps.value > 90)
 				return 1;

 			return -1;
		}
	}
	
	public class KillCommand : NSpell
	{
		public KillCommand()
		{
			NSpell( 2, "Kill Command", 0,6000,0,1500, 40, 5, 7 );
		}
		public int Trigger(NSpellStatus ss, PlayerStatus ps)
		{
			if (ps.value > value)
				return 1;
			return -1;
		}
	}
	*/
}
