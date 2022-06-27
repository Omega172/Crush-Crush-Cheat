using System;
using System.Collections;
using UnityEngine;

namespace Spine.Unity
{
	// Token: 0x02000233 RID: 563
	public class WaitForSpineTrackEntryEnd : IEnumerator
	{
		// Token: 0x060011CC RID: 4556 RVA: 0x0007DB84 File Offset: 0x0007BD84
		public WaitForSpineTrackEntryEnd(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0007DB94 File Offset: 0x0007BD94
		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x0007DBAC File Offset: 0x0007BDAC
		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x0007DBB8 File Offset: 0x0007BDB8
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0007DBBC File Offset: 0x0007BDBC
		private void HandleEnd(TrackEntry trackEntry)
		{
			this.m_WasFired = true;
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0007DBC8 File Offset: 0x0007BDC8
		private void SafeSubscribe(TrackEntry trackEntry)
		{
			if (trackEntry == null)
			{
				Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
			}
			else
			{
				trackEntry.End += this.HandleEnd;
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0007DC04 File Offset: 0x0007BE04
		public WaitForSpineTrackEntryEnd NowWaitFor(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
			return this;
		}

		// Token: 0x04000E4C RID: 3660
		private bool m_WasFired;
	}
}
