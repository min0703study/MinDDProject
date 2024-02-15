using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;

namespace TableData
{
	public class Section
	{
		public int SectionId { get; set; }
		public string SectionType { get; set; }
		public string ChapterId { get; set; }

		public List<Dialogue> Dialogues { get; set; }
		public MiniTrack MiniTrack { get; set; }

		public MainTrack MainTrack { get; set; }
	}

	public class Dialogue
	{
		public int SectionId { get; set; }
		public string Type { get; set; }
		public string CharacterKey { get; set; }
		public string Text { get; set; }
		public string CharacterImageAsset { get; set; }
		public string RoomImageAsset { get; set; }
		public string PopupImageAsset { get; set; }
		public string SoundEffectAsset { get; set; }

	}

	public class MiniTrack
	{
		public int SectionId { get; set; }
		public string MiniTrackAsset { get; set; }
	}

	public class MainTrack
	{
		public int SectionId { get; set; }
		public string MainTrackAsset { get; set; }
	}

	public class Item
	{
		public int ItemId { get; set; }
		public string ItemName { get; set; }

	}

	public class ObjectClickEvent
	{
		public string ObjectTextId { get; set; }
		public int SectionId { get; set; }
		public string Text { get; set; }
		public string ImageAsset { get; set; }
		public string EventType { get; set; }
		public string ItemTextId { get; set; }
	}
}

public class GameFlowTable : BaseTable<GameFlowTable>
{
	public List<TableData.Section> Sections { get; private set; } = new List<TableData.Section>();
	public Dictionary<int, TableData.Section> SectionDict { get; private set; } = new Dictionary<int, TableData.Section>();
	public Dictionary<string, TableData.ObjectClickEvent> ObjectClickEventDict { get; private set; } = new Dictionary<string, TableData.ObjectClickEvent>();

	protected override void init()
	{
		base.init();
	}

	public void Load(string sectionAssetLabel, string dialogueAssetLabel, string miniTrackAssetLabel, string mainTrackAssetLabel, string objectClickEventAssetLabel)
	{
		TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>(sectionAssetLabel);
		Sections = JsonConvert.DeserializeObject<List<TableData.Section>>(textAsset.text);

		TextAsset dialogueAsset = ResourceManager.Instance.Load<TextAsset>(dialogueAssetLabel);
		var dialogues = JsonConvert.DeserializeObject<List<TableData.Dialogue>>(dialogueAsset.text);

		TextAsset miniTrackAsset = ResourceManager.Instance.Load<TextAsset>(miniTrackAssetLabel);
		var miniTracks = JsonConvert.DeserializeObject<List<TableData.MiniTrack>>(miniTrackAsset.text);

		TextAsset mainTrackAsset = ResourceManager.Instance.Load<TextAsset>(mainTrackAssetLabel);
		var mainTracks = JsonConvert.DeserializeObject<List<TableData.MainTrack>>(mainTrackAsset.text);

		foreach (var section in Sections)
		{
			if (section.SectionType == "MiniTrack")
			{
				foreach (var miniTrack in miniTracks)
				{
					if (section.SectionId == miniTrack.SectionId)
					{
						section.MiniTrack = miniTrack;
					}
				}
			}
			else if (section.SectionType == "MainTrack")
			{
				foreach (var mainTrack in mainTracks)
				{
					if (section.SectionId == mainTrack.SectionId)
					{
						section.MainTrack = mainTrack;
					}
				}
			}
			else if (section.SectionType == "Dialogue")
			{
				section.Dialogues = new List<TableData.Dialogue>();
				foreach (var dialogue in dialogues)
				{
					if (section.SectionId == dialogue.SectionId)
					{
						section.Dialogues.Add(dialogue);
					}
				}
			}

			SectionDict.Add(section.SectionId, section);
		}


		TextAsset objectClickEventAsset = ResourceManager.Instance.Load<TextAsset>(objectClickEventAssetLabel);
		var objectClickEvents = JsonConvert.DeserializeObject<List<TableData.ObjectClickEvent>>(objectClickEventAsset.text);

		foreach (var objectClickEvent in objectClickEvents)
		{
			ObjectClickEventDict.Add(objectClickEvent.ObjectTextId, objectClickEvent);
		}

	}

	public TableData.Section GetSectionById(int sectionId)
	{
		if (SectionDict.TryGetValue(sectionId, out var section))
		{
			return section;
		};

		return null;
	}

	public TableData.ObjectClickEvent GetObjectClickEvent(string objectTextId)
	{
		if (ObjectClickEventDict.TryGetValue(objectTextId, out var objectClickEvent))
		{
			return objectClickEvent;
		};

		return null;
	}
}
