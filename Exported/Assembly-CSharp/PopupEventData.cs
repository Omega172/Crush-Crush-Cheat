using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E1 RID: 225
[Serializable]
public class PopupEventData
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x00026AF0 File Offset: 0x00024CF0
	public void HandleDataArray()
	{
		if (this._completedLteID == -1 && this._itemID == -1L && this._maxAllowedVersion == -1 && this._minAllowedVersion == -1)
		{
			return;
		}
		this._requirementData.ids = new int[4];
		this._requirementData.ids[0] = this._minAllowedVersion;
		this._requirementData.ids[1] = this._completedLteID;
		this._requirementData.ids[2] = this._maxAllowedVersion;
		this._requirementData.ids[3] = this._maxAllowedVersion;
		this._requirementData.itemId = this._itemID;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00026BA0 File Offset: 0x00024DA0
	public bool Serialize(Transform rootTransform)
	{
		if (this.FindDuplicatePaths(rootTransform))
		{
			return false;
		}
		this.HandleDataArray();
		this.HandleSerializationFromDataComponents(rootTransform);
		this.HandleSerializationFromBuiltInTypes(rootTransform);
		return true;
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00026BD0 File Offset: 0x00024DD0
	private bool FindDuplicatePaths(Transform rootTransform)
	{
		List<string> list = new List<string>();
		Transform[] componentsInChildren = rootTransform.GetComponentsInChildren<Transform>();
		foreach (Transform targetTransform in componentsInChildren)
		{
			string pathToParent = EventDataContainerController.GetPathToParent(targetTransform, rootTransform);
			if (list.Contains(pathToParent))
			{
				Debug.LogError("Found duplicate: " + pathToParent);
				return true;
			}
			list.Add(pathToParent);
		}
		return false;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00026C3C File Offset: 0x00024E3C
	private void HandleSerializationFromDataComponents(Transform rootTransform)
	{
		EventDataContainerController[] componentsInChildren = rootTransform.GetComponentsInChildren<EventDataContainerController>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		this.dataContainers = new List<DataContainerTemplate>(componentsInChildren.Length);
		foreach (EventDataContainerController eventDataContainerController in componentsInChildren)
		{
			if (!eventDataContainerController.CanSerialize())
			{
				Debug.LogError("Skipping serialization on: " + eventDataContainerController.name);
				UnityEngine.Object.DestroyImmediate(eventDataContainerController);
			}
			else
			{
				DataContainerTemplate item = new DataContainerTemplate
				{
					PathFromParent = EventDataContainerController.GetPathToParent(eventDataContainerController.transform, rootTransform),
					ComponentType = eventDataContainerController.GetDataContainerType(),
					DataContainer = eventDataContainerController.GetDataContainer()
				};
				UnityEngine.Object.DestroyImmediate(eventDataContainerController);
				this.dataContainers.Add(item);
			}
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00026CFC File Offset: 0x00024EFC
	private void HandleSerializationFromBuiltInTypes(Transform rootTransform)
	{
		foreach (Type type in this._componentToDataList)
		{
			Component[] componentsInChildren = rootTransform.GetComponentsInChildren(type);
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.SerializeFromComponent(type, componentsInChildren[i], rootTransform);
				}
			}
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00026D88 File Offset: 0x00024F88
	private void SerializeFromComponent(Type type, Component component, Transform rootTransform)
	{
		EventDataContainer eventDataContainer = null;
		Type typeFromHandle = typeof(EventDataContainer);
		if (type == this._componentToDataList[0])
		{
			eventDataContainer = new TMProTextData();
			typeFromHandle = typeof(TMProTextData);
			((TMProTextData)eventDataContainer).Serialize(component);
		}
		if (eventDataContainer == null)
		{
			return;
		}
		this.AddToList(eventDataContainer, typeFromHandle, component.transform, rootTransform);
		UnityEngine.Object.DestroyImmediate(component);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00026DF0 File Offset: 0x00024FF0
	private void AddToList(EventDataContainer dataContainer, Type type, Transform targetTransform, Transform rootTransform)
	{
		DataContainerTemplate item = new DataContainerTemplate
		{
			PathFromParent = EventDataContainerController.GetPathToParent(targetTransform, rootTransform),
			ComponentType = type.ToString(),
			DataContainer = dataContainer
		};
		this.dataContainers.Add(item);
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00026E34 File Offset: 0x00025034
	public IEnumerator Deserialize(Transform rootTransform)
	{
		DataContainerTemplate dataList = null;
		Transform targetTransform = null;
		for (int i = 0; i < this.dataContainers.Count; i++)
		{
			dataList = this.dataContainers[i];
			targetTransform = Utilities.GetTransformFromPath(rootTransform, dataList.PathFromParent);
			if (targetTransform == null)
			{
				Debug.LogError("No transform found for: " + dataList.PathFromParent);
			}
			else
			{
				Type componentType;
				try
				{
					componentType = Type.GetType(dataList.ComponentType);
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.LogError("Getting type failed for: " + dataList.ComponentType + "/n" + ex.Message);
					goto IL_1EF;
				}
				if (componentType == null || !componentType.IsSubclassOf(typeof(EventDataContainer)))
				{
					Debug.LogError(componentType.ToString() + " null or not a subclass of EventDataContainer. Path: " + dataList.PathFromParent);
				}
				else
				{
					string componentJson = SimpleJson.SerializeObject(dataList.DataContainer, null);
					EventDataContainer dataContainer = (EventDataContainer)SimpleJson.DeserializeObject(componentJson, componentType, null);
					if (!(dataContainer is IDeserializeWithWait))
					{
						dataContainer.Deserialize(rootTransform, targetTransform, dataContainer);
					}
					else
					{
						IDeserializeWithWait deserializeWithWait = dataContainer as IDeserializeWithWait;
						yield return deserializeWithWait.Deserialize(rootTransform, targetTransform, dataContainer);
					}
				}
			}
			IL_1EF:;
		}
		yield break;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00026E60 File Offset: 0x00025060
	public HideEventRequirement GetHideEventRequirement()
	{
		return this._requirementData.hideEventRequirement;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00026E70 File Offset: 0x00025070
	public RequirementData GetRequirementData()
	{
		return this._requirementData;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00026E78 File Offset: 0x00025078
	public void SetDescription(string description)
	{
		this._description = description;
	}

	// Token: 0x040004FA RID: 1274
	[SerializeField]
	private string _description = "Description won't be serialized!";

	// Token: 0x040004FB RID: 1275
	public string BundleWithHash = string.Empty;

	// Token: 0x040004FC RID: 1276
	[HideInInspector]
	public List<DataContainerTemplate> dataContainers;

	// Token: 0x040004FD RID: 1277
	[SerializeField]
	private RequirementData _requirementData;

	// Token: 0x040004FE RID: 1278
	[SerializeField]
	[Header("Optional")]
	private int _completedLteID = -1;

	// Token: 0x040004FF RID: 1279
	[SerializeField]
	private long _itemID = -1L;

	// Token: 0x04000500 RID: 1280
	[SerializeField]
	private int _maxAllowedVersion = -1;

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private int _minAllowedVersion = -1;

	// Token: 0x04000502 RID: 1282
	private List<Type> _componentToDataList = new List<Type>
	{
		typeof(Text)
	};
}
