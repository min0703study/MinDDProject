using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;

namespace TableData
{
	public class Section
	{
		public int SectionId { get; set; }
		public string SectionType { get; set; }
		public string ChapterId { get; set;}
		
		public List<Dialogue> Dialogues { get; set; }
	}
	
	public class Dialogue
	{
		public int SectionId { get; set; }
		public string Type { get; set; }
		public string CharacterKey { get; set; }
		public string Text { get; set; }
		public string CharacterImageAsset { get; set; }
		public string RoomImageAsset { get; set; }
		public string SoundEffectAsset { get; set; }

	}
}

public class GameFlowTable: BaseTable<GameFlowTable>
{
	public List<TableData.Section> Sections { get; private set;} = new List<TableData.Section>();
	public Dictionary<int, TableData.Section> SectionDict { get; private set; } = new Dictionary<int, TableData.Section>();

	protected override void init()
	{
		base.init();
	}
	
	public void Load(string sectionAssetLabel, string dialogueAssetLabel)
	{
		TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>(sectionAssetLabel);
		Sections = JsonConvert.DeserializeObject<List<TableData.Section>>(textAsset.text);
		
		TextAsset dialogueAsset = ResourceManager.Instance.Load<TextAsset>(dialogueAssetLabel);
		var dialogues = JsonConvert.DeserializeObject<List<TableData.Dialogue>>(dialogueAsset.text);
		
		foreach (var section in Sections) 
		{
			section.Dialogues = new List<TableData.Dialogue>();
			foreach (var dialogue in dialogues) 
			{
				if(section.SectionId == dialogue.SectionId)
				{
					section.Dialogues.Add(dialogue);
				}
			}
			
			SectionDict.Add(section.SectionId, section);
		}
	}
	
	public TableData.Section GetSectionById(int sectionId) 
	{
		if(SectionDict.TryGetValue(sectionId, out var section))
		{
			return section;
		};	
		
		return null;
	}
}
