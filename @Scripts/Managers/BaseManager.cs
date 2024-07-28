using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
{
	public static T Instance { get; private set; }
	
	bool isInit = false;

	protected virtual void Awake()
	{	
		if (Instance == null)
		{
			Instance = this as T;
			initAwake();
		}
		else
		{
			//Destroy(this);
		}
	}

	protected virtual void initAwake()
	{
		if (isInit == true)
			return;
			
		isInit = true;
		
	}
}