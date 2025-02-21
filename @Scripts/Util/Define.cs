public enum RoomType
{
	SunRoomA,
	LivingRoomA,
	LivingRoomB,
	Kitchen,
	KitchenRefrigerator,
	EntranceDoor,
	EntranceHallway,
	SunRoomB,
	SunRoomABookCase,
	SunRoomADrawer,
	SunRoomBCloset,
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
	
	//
	SunRoomB_Full,
	SunRoomA_Bookcase,
	SunRoomA_Drawer,
	SunRoomA_Desk,
	
	SunRoomABookcase_Full,
	SunRoomABookcase_Photo,
	
		
	SunRoomADrawer_Full,
	SunRoomADrawer_3,
	
	SunRoomBCloset_Full,
	SunRoomBCloset_Jacket,
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
	RemoveRoomObject,
	None,
}

public enum RoomObjectEventTriggerType
{
	Click,
	UseItem,
	Unlock
}