public enum RoomType
{
	SunRoomA,
	LivingRoomA,
	LivingRoomB,
	Kitchen,
	KitchenRefrigerator,
	EntranceDoor,
	EntranceHallway,
	None,
}

public enum FocusZone
{
	SunRoomA_Full,
	//
	LivingRoomA_Full,
	LivingRoomA_Drawer,
	
	LivingRoomB_Full,
	//
	Kitchen_Full,
	Kitchen_Refrigerator_Full,
	
	EntranceHallway_Full,
	//
	EntranceDoor_Full,
	EntranceDoor_LockA,
	EntranceDoor_LockB,
	EntranceDoor_LockC,
	EntranceDoor_Photo,
	
	Kitchen_Refrigerator_Photo,
	LivingRoomA_CatBooth,
	Kitchen_UnderDrawer,
	None,
}

public enum ItemState
{
	LocatedInRoom,
	StoredInInventory,
	AlreadyUsed,
}

public enum RoomObjectState
{
	LocatedInRoom,
	UsedItem,
	RemoveRoomObject
}

public enum RoomObjectEventTriggerType
{
	Click,
	UseItem,
	Unlock
}