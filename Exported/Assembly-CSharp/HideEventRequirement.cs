using System;

// Token: 0x020000DF RID: 223
[Flags]
public enum HideEventRequirement
{
	// Token: 0x040004F1 RID: 1265
	None = 0,
	// Token: 0x040004F2 RID: 1266
	CompletedLTE = 1,
	// Token: 0x040004F3 RID: 1267
	HasItem = 2,
	// Token: 0x040004F4 RID: 1268
	MaxAllowedVersion = 4,
	// Token: 0x040004F5 RID: 1269
	MinAllowedVersion = 8,
	// Token: 0x040004F6 RID: 1270
	OfferwallAvailable = 16
}
