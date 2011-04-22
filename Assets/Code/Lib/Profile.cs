using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Profile
{
	private class Section {
		public String name;
		public int count;
		public float sumTime;
		public float minTime;
		public float maxTime;
		
		public Section(String name){
			this.name = name;
			count = 0;
			sumTime = 0.0f;
			minTime = 0.0f;
			maxTime = 0.0f;
		}
		
		public void addDeltaTime(float deltaTime){
			if (count == 0){
				count = 1;
				
				sumTime = deltaTime;
				minTime = deltaTime;
				maxTime = deltaTime;
			} else {
				count += 1;
				sumTime += deltaTime;
				
				minTime = Mathf.Min(deltaTime, minTime);
				maxTime = Mathf.Max(deltaTime, maxTime);
			}
		}
				
		private String formatTime(float time){
			return "" + Math.Round((float)(time * 1000.0f), 3);	
		}
		
		public String toString(){
			return "[name=" + name + " count=" + count + " avg/min/max/sum=" + 
					formatTime(sumTime/(float)count) + "/" + 
					formatTime(minTime) + "/" + 
					formatTime(maxTime) + "/" + 
					formatTime(sumTime) + "]";
		}
	}
	
	private class SectionComparer : IComparer<Section>
	{
		public int Compare(Section a, Section b)
        {
            if (a.sumTime > b.sumTime)
            {
                return -1;
            }
            else if (a.sumTime < b.sumTime)
            {
                return 1;
            }
			else 
			{
				return 0;	
			}
        }
	}
	
	private class CurrentState {
		public Section section;
		public float startTime;
				
		public CurrentState(String name, float startTime){
			this.section = getSection(name);
			this.startTime = startTime;
		}
	}
	
	// --------------------------------------
	
	private static Stack currentStateStack = new Stack();
	private static Dictionary<String, Section> sections = new Dictionary<String, Section>();
	private static bool enabled = false;
	
	private static float getTime()
	{
		return Time.realtimeSinceStartup;
	}
	
	private static Section getSection(String name){
		if(!sections.ContainsKey(name)){
			sections.Add(name, new Section(name));	
		}
		
		return sections[name];
	}
	
	public static void start(String name)
	{
		if (enabled)
		{
			currentStateStack.Push(new CurrentState(name, getTime()));
		}
	}

	public static void end()
	{
		if (enabled)
		{
			CurrentState state = (CurrentState)currentStateStack.Pop();
			state.section.addDeltaTime(getTime() - state.startTime);
		}
	}
	
	public static void print()
	{
		if (enabled)
		{
			long memoryInKb = GC.GetTotalMemory(false) / 1024;
			String o = "Memory: " + memoryInKb + "k\n";
	
			List<Section> sorted = new List<Section>(sections.Values);
			sorted.Sort(new SectionComparer());
				
			foreach(Section s in sorted){
				o += s.toString() + "\n";
			}
		
			Debug.Log(o);
		}
	}
	
	public static void clear()
	{
		if (enabled)
		{
			sections.Clear();
			currentStateStack.Clear();
		}
	}
}

