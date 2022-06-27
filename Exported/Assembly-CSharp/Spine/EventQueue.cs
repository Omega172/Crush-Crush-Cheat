using System;
using System.Collections.Generic;

namespace Spine
{
	// Token: 0x02000186 RID: 390
	internal class EventQueue
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x0005A384 File Offset: 0x00058584
		internal EventQueue(AnimationState state, Action HandleAnimationsChanged, Pool<TrackEntry> trackEntryPool)
		{
			this.state = state;
			this.AnimationsChanged = (Action)Delegate.Combine(this.AnimationsChanged, HandleAnimationsChanged);
			this.trackEntryPool = trackEntryPool;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000B93 RID: 2963 RVA: 0x0005A3C8 File Offset: 0x000585C8
		// (remove) Token: 0x06000B94 RID: 2964 RVA: 0x0005A3E4 File Offset: 0x000585E4
		internal event Action AnimationsChanged;

		// Token: 0x06000B95 RID: 2965 RVA: 0x0005A400 File Offset: 0x00058600
		internal void Start(TrackEntry entry)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.Start, entry, null));
			if (this.AnimationsChanged != null)
			{
				this.AnimationsChanged();
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0005A42C File Offset: 0x0005862C
		internal void Interrupt(TrackEntry entry)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.Interrupt, entry, null));
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0005A444 File Offset: 0x00058644
		internal void End(TrackEntry entry)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.End, entry, null));
			if (this.AnimationsChanged != null)
			{
				this.AnimationsChanged();
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0005A470 File Offset: 0x00058670
		internal void Dispose(TrackEntry entry)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.Dispose, entry, null));
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0005A488 File Offset: 0x00058688
		internal void Complete(TrackEntry entry)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.Complete, entry, null));
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0005A4A0 File Offset: 0x000586A0
		internal void Event(TrackEntry entry, Event e)
		{
			this.eventQueueEntries.Add(new EventQueue.EventQueueEntry(EventQueue.EventType.Event, entry, e));
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0005A4B8 File Offset: 0x000586B8
		internal void Drain()
		{
			if (this.drainDisabled)
			{
				return;
			}
			this.drainDisabled = true;
			List<EventQueue.EventQueueEntry> list = this.eventQueueEntries;
			AnimationState animationState = this.state;
			int i = 0;
			while (i < list.Count)
			{
				EventQueue.EventQueueEntry eventQueueEntry = list[i];
				TrackEntry entry = eventQueueEntry.entry;
				switch (eventQueueEntry.type)
				{
				case EventQueue.EventType.Start:
					entry.OnStart();
					animationState.OnStart(entry);
					break;
				case EventQueue.EventType.Interrupt:
					entry.OnInterrupt();
					animationState.OnInterrupt(entry);
					break;
				case EventQueue.EventType.End:
					entry.OnEnd();
					animationState.OnEnd(entry);
					goto IL_A2;
				case EventQueue.EventType.Dispose:
					goto IL_A2;
				case EventQueue.EventType.Complete:
					entry.OnComplete();
					animationState.OnComplete(entry);
					break;
				case EventQueue.EventType.Event:
					entry.OnEvent(eventQueueEntry.e);
					animationState.OnEvent(entry, eventQueueEntry.e);
					break;
				}
				IL_F9:
				i++;
				continue;
				IL_A2:
				entry.OnDispose();
				animationState.OnDispose(entry);
				this.trackEntryPool.Free(entry);
				goto IL_F9;
			}
			this.eventQueueEntries.Clear();
			this.drainDisabled = false;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0005A5E0 File Offset: 0x000587E0
		internal void Clear()
		{
			this.eventQueueEntries.Clear();
		}

		// Token: 0x04000AB4 RID: 2740
		private readonly List<EventQueue.EventQueueEntry> eventQueueEntries = new List<EventQueue.EventQueueEntry>();

		// Token: 0x04000AB5 RID: 2741
		internal bool drainDisabled;

		// Token: 0x04000AB6 RID: 2742
		private readonly AnimationState state;

		// Token: 0x04000AB7 RID: 2743
		private readonly Pool<TrackEntry> trackEntryPool;

		// Token: 0x02000187 RID: 391
		private struct EventQueueEntry
		{
			// Token: 0x06000B9D RID: 2973 RVA: 0x0005A5F0 File Offset: 0x000587F0
			public EventQueueEntry(EventQueue.EventType eventType, TrackEntry trackEntry, Event e = null)
			{
				this.type = eventType;
				this.entry = trackEntry;
				this.e = e;
			}

			// Token: 0x04000AB9 RID: 2745
			public EventQueue.EventType type;

			// Token: 0x04000ABA RID: 2746
			public TrackEntry entry;

			// Token: 0x04000ABB RID: 2747
			public Event e;
		}

		// Token: 0x02000188 RID: 392
		private enum EventType
		{
			// Token: 0x04000ABD RID: 2749
			Start,
			// Token: 0x04000ABE RID: 2750
			Interrupt,
			// Token: 0x04000ABF RID: 2751
			End,
			// Token: 0x04000AC0 RID: 2752
			Dispose,
			// Token: 0x04000AC1 RID: 2753
			Complete,
			// Token: 0x04000AC2 RID: 2754
			Event
		}
	}
}
