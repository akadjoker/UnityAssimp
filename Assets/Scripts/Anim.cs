using System;
[System.Serializable]
public class Anim
{
	public string name;
	public int frameStart;
	public int frameEnd;
	public int fps;
	
	public Anim(string name,int start,int end,int fps ) 
	{
		this.name = name;
		this.frameStart = start;
		this.frameEnd = end;
		this.fps = fps;
		
		
	}
}
