using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200002D RID: 45
	public class Utility
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00008E08 File Offset: 0x00007008
		public static string GetPlatformName()
		{
			return Utility.GetPlatformForAssetBundles(Application.platform);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008E14 File Offset: 0x00007014
		public static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			default:
				if (platform == RuntimePlatform.OSXPlayer)
				{
					return "OSX";
				}
				if (platform == RuntimePlatform.WindowsPlayer)
				{
					return "Windows";
				}
				if (platform == RuntimePlatform.WebGLPlayer)
				{
					return "WebGL";
				}
				if (platform != RuntimePlatform.tvOS)
				{
					Debug.Log("Unknown BuildTarget: Using Default Enum Name: " + platform);
					return platform.ToString();
				}
				return "tvOS";
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.LinuxPlayer:
				return "Linux";
			}
		}

		// Token: 0x040000F8 RID: 248
		public const string AssetBundlesOutputPath = "AssetBundles";
	}
}
