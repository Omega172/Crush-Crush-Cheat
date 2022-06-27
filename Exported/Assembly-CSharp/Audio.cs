using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class Audio : MonoBehaviour
{
	// Token: 0x06000126 RID: 294 RVA: 0x00008F6C File Offset: 0x0000716C
	public void SetVolume()
	{
		foreach (AudioSource audioSource in this.Sources)
		{
			if (audioSource != null)
			{
				audioSource.volume = Settings.EffectsVolume;
			}
		}
		if (this.Music != null)
		{
			this.Music.volume = Settings.MusicVolume;
			try
			{
				if (Settings.MusicVolume == 0f && this.Music.isPlaying)
				{
					this.Music.Pause();
				}
				else if (Settings.MusicVolume != 0f && !this.Music.isPlaying)
				{
					this.Music.Play();
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00009050 File Offset: 0x00007250
	private AudioSource FindAvailableSource()
	{
		if (this.Sources == null)
		{
			return null;
		}
		for (int i = 0; i < this.Sources.Length; i++)
		{
			if (!(this.Sources[i] == null))
			{
				if (!this.Sources[i].isPlaying)
				{
					return this.Sources[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x000090B8 File Offset: 0x000072B8
	private void OnApplicationFocus(bool focusStatus)
	{
		this.focus = focusStatus;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000090C4 File Offset: 0x000072C4
	public void PlayOnce(AudioClip clip)
	{
		if (clip == null)
		{
			Debug.LogWarning("Tried to play a null clip.");
			return;
		}
		if (Settings.EffectsVolume == 0f)
		{
			return;
		}
		if (clip.name.Contains("hoverselect") && !this.focus)
		{
			return;
		}
		AudioSource audioSource = this.FindAvailableSource();
		if (audioSource == null)
		{
			return;
		}
		try
		{
			audioSource.Stop();
			audioSource.clip = clip;
			audioSource.Play();
			audioSource.loop = false;
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00009170 File Offset: 0x00007370
	public AudioClip GetAudioClip(Audio.AudioFile audioFile)
	{
		switch (audioFile)
		{
		case Audio.AudioFile.ClickButton:
			return this.ClickButton;
		case Audio.AudioFile.Click2:
			return this.Click2;
		case Audio.AudioFile.Coins:
			return this.Coins;
		case Audio.AudioFile.GirlUnlock:
			return this.GirlFullUnlock;
		case Audio.AudioFile.Hearts:
			return this.Hearts1;
		case Audio.AudioFile.HoverSelect:
			return this.HoverSelect;
		case Audio.AudioFile.LevelUp:
			return this.LevelUp;
		case Audio.AudioFile.NotEnoughTime:
			return this.NotEnoughTime;
		case Audio.AudioFile.Poke:
			return this.Poke;
		case Audio.AudioFile.QPArrives:
			return this.QPArrives;
		case Audio.AudioFile.QPLeaves:
			return this.QPLeaves;
		case Audio.AudioFile.Talk:
			return this.TalkButton;
		case Audio.AudioFile.Tickle:
			return this.Tickle;
		case Audio.AudioFile.TimeBlock:
			return this.TimeBlockAdded;
		case Audio.AudioFile.HoverSelect3A:
			return this.HoverSelect3A;
		case Audio.AudioFile.HoverSelect06:
			return this.HoverSelect06;
		case Audio.AudioFile.Clicky00:
			return this.Clicky00;
		case Audio.AudioFile.BadReaction:
			return this.BadReaction;
		case Audio.AudioFile.Unlock00:
			return this.Unlock00;
		case Audio.AudioFile.ClickDrip:
			return this.ClickDrip;
		case Audio.AudioFile.PhoneBubble:
			return this.PhoneBubble;
		case Audio.AudioFile.PhoneVibrate:
			return this.PhoneVibrate;
		case Audio.AudioFile.Select:
			return this.Select;
		case Audio.AudioFile.PhoneRing:
			return this.PhoneRing;
		case Audio.AudioFile.Purchase:
			return this.Purchase;
		case Audio.AudioFile.Clicky03A:
			return this.Clicky03A;
		case Audio.AudioFile.OKButton:
			return this.OKButton;
		default:
			return null;
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000092B8 File Offset: 0x000074B8
	public void PlayOnce(Audio.AudioFile file)
	{
		this.PlayOnce(this.GetAudioClip(file));
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000092C8 File Offset: 0x000074C8
	public void PlayWithVolume(float volume)
	{
		this.Effect.Stop();
		this.Effect.volume = volume;
		this.Effect.Play();
	}

	// Token: 0x040000FE RID: 254
	public AudioClip Alert;

	// Token: 0x040000FF RID: 255
	public AudioClip BadReaction;

	// Token: 0x04000100 RID: 256
	public AudioClip CantDo;

	// Token: 0x04000101 RID: 257
	public AudioClip Check;

	// Token: 0x04000102 RID: 258
	public AudioClip Checkmark;

	// Token: 0x04000103 RID: 259
	public AudioClip ClickButton;

	// Token: 0x04000104 RID: 260
	public AudioClip Click2;

	// Token: 0x04000105 RID: 261
	public AudioClip Cling;

	// Token: 0x04000106 RID: 262
	public AudioClip Coins;

	// Token: 0x04000107 RID: 263
	public AudioClip Complete1;

	// Token: 0x04000108 RID: 264
	public AudioClip Ding;

	// Token: 0x04000109 RID: 265
	public AudioClip Ding1;

	// Token: 0x0400010A RID: 266
	public AudioClip FAQ;

	// Token: 0x0400010B RID: 267
	public AudioClip GirlFullUnlock;

	// Token: 0x0400010C RID: 268
	public AudioClip GirlHoverSelect;

	// Token: 0x0400010D RID: 269
	public AudioClip Hearts1;

	// Token: 0x0400010E RID: 270
	public AudioClip HoverSelect;

	// Token: 0x0400010F RID: 271
	public AudioClip JobLevelUp;

	// Token: 0x04000110 RID: 272
	public AudioClip LevelUp;

	// Token: 0x04000111 RID: 273
	public AudioClip Magic1;

	// Token: 0x04000112 RID: 274
	public AudioClip NewGirlArrives1;

	// Token: 0x04000113 RID: 275
	public AudioClip NewGirlArrives2;

	// Token: 0x04000114 RID: 276
	public AudioClip NextGirlLevel;

	// Token: 0x04000115 RID: 277
	public AudioClip NotEnoughTime;

	// Token: 0x04000116 RID: 278
	public AudioClip OKButton;

	// Token: 0x04000117 RID: 279
	public AudioClip Poke;

	// Token: 0x04000118 RID: 280
	public AudioClip Purchase;

	// Token: 0x04000119 RID: 281
	public AudioClip QPArrives;

	// Token: 0x0400011A RID: 282
	public AudioClip QPLeaves;

	// Token: 0x0400011B RID: 283
	public AudioClip Reset;

	// Token: 0x0400011C RID: 284
	public AudioClip S30260;

	// Token: 0x0400011D RID: 285
	public AudioClip Select;

	// Token: 0x0400011E RID: 286
	public AudioClip TalkButton;

	// Token: 0x0400011F RID: 287
	public AudioClip Tickle;

	// Token: 0x04000120 RID: 288
	public AudioClip TimeBlockAdded;

	// Token: 0x04000121 RID: 289
	public AudioClip Music1;

	// Token: 0x04000122 RID: 290
	public AudioClip HowlingWind;

	// Token: 0x04000123 RID: 291
	public AudioClip HolidayMusic;

	// Token: 0x04000124 RID: 292
	public AudioClip Arrow00;

	// Token: 0x04000125 RID: 293
	public AudioClip Arrow01;

	// Token: 0x04000126 RID: 294
	public AudioClip Cancel00;

	// Token: 0x04000127 RID: 295
	public AudioClip Cancel01;

	// Token: 0x04000128 RID: 296
	public AudioClip Cancel02;

	// Token: 0x04000129 RID: 297
	public AudioClip Cancel03;

	// Token: 0x0400012A RID: 298
	public AudioClip Cancel04;

	// Token: 0x0400012B RID: 299
	public AudioClip Drip00;

	// Token: 0x0400012C RID: 300
	public AudioClip ClickOrSelect00;

	// Token: 0x0400012D RID: 301
	public AudioClip ClickOrSelect01;

	// Token: 0x0400012E RID: 302
	public AudioClip ClickOrSelect02;

	// Token: 0x0400012F RID: 303
	public AudioClip Clicky00;

	// Token: 0x04000130 RID: 304
	public AudioClip Clicky01;

	// Token: 0x04000131 RID: 305
	public AudioClip Clicky03A;

	// Token: 0x04000132 RID: 306
	public AudioClip Clicky3B;

	// Token: 0x04000133 RID: 307
	public AudioClip Clicky04;

	// Token: 0x04000134 RID: 308
	public AudioClip HoverSelect00;

	// Token: 0x04000135 RID: 309
	public AudioClip HoverSelect01;

	// Token: 0x04000136 RID: 310
	public AudioClip HoverSelect02;

	// Token: 0x04000137 RID: 311
	public AudioClip HoverSelect3A;

	// Token: 0x04000138 RID: 312
	public AudioClip HoverSelect3B;

	// Token: 0x04000139 RID: 313
	public AudioClip HoverSelect04;

	// Token: 0x0400013A RID: 314
	public AudioClip HoverSelect06;

	// Token: 0x0400013B RID: 315
	public AudioClip HoverSelectList00;

	// Token: 0x0400013C RID: 316
	public AudioClip Unknown00;

	// Token: 0x0400013D RID: 317
	public AudioClip Unlock00;

	// Token: 0x0400013E RID: 318
	public AudioClip ClickDrip;

	// Token: 0x0400013F RID: 319
	public AudioClip PhoneVibrate;

	// Token: 0x04000140 RID: 320
	public AudioClip PhoneBubble;

	// Token: 0x04000141 RID: 321
	public AudioClip PhoneRing;

	// Token: 0x04000142 RID: 322
	public AudioSource[] Sources;

	// Token: 0x04000143 RID: 323
	public AudioSource Music;

	// Token: 0x04000144 RID: 324
	public AudioSource Effect;

	// Token: 0x04000145 RID: 325
	private bool focus = true;

	// Token: 0x02000031 RID: 49
	public enum AudioFile
	{
		// Token: 0x04000147 RID: 327
		None,
		// Token: 0x04000148 RID: 328
		ClickButton,
		// Token: 0x04000149 RID: 329
		Click2,
		// Token: 0x0400014A RID: 330
		Coins,
		// Token: 0x0400014B RID: 331
		GirlUnlock,
		// Token: 0x0400014C RID: 332
		Hearts,
		// Token: 0x0400014D RID: 333
		HoverSelect,
		// Token: 0x0400014E RID: 334
		LevelUp,
		// Token: 0x0400014F RID: 335
		NotEnoughTime,
		// Token: 0x04000150 RID: 336
		Poke,
		// Token: 0x04000151 RID: 337
		QPArrives,
		// Token: 0x04000152 RID: 338
		QPLeaves,
		// Token: 0x04000153 RID: 339
		Talk,
		// Token: 0x04000154 RID: 340
		Tickle,
		// Token: 0x04000155 RID: 341
		TimeBlock,
		// Token: 0x04000156 RID: 342
		HoverSelect3A,
		// Token: 0x04000157 RID: 343
		HoverSelect06,
		// Token: 0x04000158 RID: 344
		Clicky00,
		// Token: 0x04000159 RID: 345
		BadReaction,
		// Token: 0x0400015A RID: 346
		Unlock00,
		// Token: 0x0400015B RID: 347
		ClickDrip,
		// Token: 0x0400015C RID: 348
		PhoneBubble,
		// Token: 0x0400015D RID: 349
		PhoneVibrate,
		// Token: 0x0400015E RID: 350
		Select,
		// Token: 0x0400015F RID: 351
		PhoneRing,
		// Token: 0x04000160 RID: 352
		Purchase,
		// Token: 0x04000161 RID: 353
		Clicky03A,
		// Token: 0x04000162 RID: 354
		OKButton
	}
}
