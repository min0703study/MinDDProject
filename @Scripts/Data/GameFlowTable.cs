using System.Collections.Generic;
using Newtonsoft.Json;

using UnityEngine;

namespace TableData
{
	public class Section
	{
		public int SectionIndex { get; set; }
		public string SectionType { get; set; }
		public string ChapterId { get; set; }
		public string SectionAsset { get; set; }

		public Dictionary<string, Dialogue> Dialogues { get; set; } = new Dictionary<string, Dialogue>();
		public MiniTrack MiniTrack { get; set; }

		public MainTrack MainTrack { get; set; }
	}

	public class Dialogue
	{
		public int SectionIndex { get; set; }
		public string StepId { get; set; }
		public string Type { get; set; }
		public string CharacterKey { get; set; }
		public string Text { get; set; }
		public string MissionText { get; set; }
		public string MissionStartRoom { get; set; }
		public string CharacterImageAsset { get; set; }
		public string RoomImageAsset { get; set; }
		public string PopupImageAsset { get; set; }
		public string SoundEffectAsset { get; set; }
		public string VisualSoundEffectAsset { get; set; }

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
		public string ItemTextId { get; set; }
		public string ItemName { get; set; }
		public string ItemImageAsset { get; set; }
		public string ItemDesc { get; set; }
	}

	public class ObjectEvent
	{
		public string TrackTextId { get; set; }
		public string ObjectTextId { get; set; }
		public string ActionType { get; set; }
		public string EventTriggerType { get; set; }
		public string Text { get; set; }
		public string ObjectImageAsset { get; set; }
	}
	
	public class RoomObject
	{
		public string ObjectTextId { get; set; }
		public string ObjectName { get; set; }
	}
}

public class GameFlowTable : BaseTable<GameFlowTable>
{
	public List<TableData.Section> Sections { get; private set; } = new List<TableData.Section>();
	public Dictionary<int, TableData.Section> SectionDict { get; private set; } = new Dictionary<int, TableData.Section>();
	public List<TableData.ObjectEvent> RoomObjectEvents { get; private set; } = new();
	public Dictionary<string, TableData.Item> ItemDict { get; private set; } = new Dictionary<string, TableData.Item>();
	public Dictionary<string, TableData.RoomObject> RoomObjectDict { get; private set; } = new();

	protected override void init()
	{
		base.init();
	}

	public void Load(string gameFlowAssetLabel, string gameFlowDetailLabel, string objectClickEventLabel, string itemAssetLabel, string roomObjectAssetLabel)
	{
		TextAsset textAsset = ResourceManager.Instance.Load<TextAsset>(gameFlowAssetLabel);
		Sections = JsonConvert.DeserializeObject<List<TableData.Section>>(textAsset.text);

		TextAsset gameflowDetailAsset = ResourceManager.Instance.Load<TextAsset>(gameFlowDetailLabel);
		var gameFlowDetails = JsonConvert.DeserializeObject<List<TableData.Dialogue>>(gameflowDetailAsset.text);

		foreach (var section in Sections)
		{
			foreach (var gameFlowDetail in gameFlowDetails)
			{
				if (section.SectionIndex == gameFlowDetail.SectionIndex)
				{
					section.Dialogues.Add(gameFlowDetail.StepId, gameFlowDetail);
				}
			}

			SectionDict.Add(section.SectionIndex, section);
		}

		TextAsset objectClickEventAsset = ResourceManager.Instance.Load<TextAsset>(objectClickEventLabel);
		var objectClickEvents = JsonConvert.DeserializeObject<List<TableData.ObjectEvent>>(objectClickEventAsset.text);

		foreach (var objectClickEvent in objectClickEvents)
		{
			RoomObjectEvents.Add(objectClickEvent);
		}

		TextAsset roomObjectAsset = ResourceManager.Instance.Load<TextAsset>(roomObjectAssetLabel);
		var roomObjects = JsonConvert.DeserializeObject<List<TableData.RoomObject>>(roomObjectAsset.text);

		foreach (var roomObject in roomObjects)
		{
			RoomObjectDict.Add(roomObject.ObjectTextId, roomObject);
		}

		TextAsset itemAsset = ResourceManager.Instance.Load<TextAsset>(itemAssetLabel);
		var items = JsonConvert.DeserializeObject<List<TableData.Item>>(itemAsset.text);

		foreach (var item in items)
		{
			ItemDict.Add(item.ItemTextId, item);
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

	public TableData.ObjectEvent GetRoomObjectEvent(string trackTextId, string objectTextId, RoomObjectEventTriggerType triggerType = RoomObjectEventTriggerType.Click)
	{
		foreach(var objectEvent in RoomObjectEvents) 
		{
			if(objectEvent.TrackTextId == trackTextId && objectEvent.ObjectTextId == objectTextId && objectEvent.EventTriggerType == triggerType.ToString()) 
			{
				return objectEvent;
			}
		}
		
		return null;
	}

	public TableData.Item GetItemById(string itemTextId)
	{
		if (ItemDict.TryGetValue(itemTextId, out var item))
		{
			return item;
		};

		return null;
	}
	
	public TableData.Item GetDialogue(string itemTextId)
	{
		if (ItemDict.TryGetValue(itemTextId, out var item))
		{
			return item;
		};

		return null;
	}
}
