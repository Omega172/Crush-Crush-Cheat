using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012B RID: 299
public static class Translations
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x00046424 File Offset: 0x00044624
	public static string GetTranslation(string id, string english)
	{
		id = id.ToLowerInvariant();
		if (id == "everything_else_28_7" && english.ToLower() == "ok")
		{
			id = "everything_else_28_6";
		}
		if (id == "achievements_5_0")
		{
			id = "achievements_389_0";
		}
		if (Translations.PreferredLanguage == Translations.Language.English || string.IsNullOrEmpty(id) || !Translations.translations.ContainsKey(id))
		{
			return english;
		}
		return Translations.translations[id];
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000464B0 File Offset: 0x000446B0
	public static string TryGetTranslation(string id)
	{
		string result = null;
		id = id.ToLowerInvariant();
		if (Translations.translations.TryGetValue(id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000464DC File Offset: 0x000446DC
	public static string GetDialogueTranslation(string id, string english)
	{
		return Translations.GetDialogueTranslation(id, english, null);
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000464E8 File Offset: 0x000446E8
	public static string GetDialogueTranslation(GirlModel.GirlText text, Balance.GirlName girlName, string type)
	{
		if (text == null)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, type + " for " + girlName.ToFriendlyString() + " was null.");
			text = new GirlModel.GirlText
			{
				ID = 0,
				AudioID = null,
				English = "Unknown text..."
			};
		}
		string audioID = text.AudioID;
		string text2 = girlName.ToLowerFriendlyString();
		text2 = text2 + "_" + text.ID.ToString();
		string english = text.English;
		if (text2 == "karma_0")
		{
			text2 = "sutra_4";
		}
		if (girlName == Balance.GirlName.Peanut || girlName == Balance.GirlName.Ruri || girlName == Balance.GirlName.Wendy || girlName == Balance.GirlName.Generica)
		{
			text2 = text2.Replace("_", "full_");
		}
		return Translations.GetTranslation(text2, english);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000465B4 File Offset: 0x000447B4
	public static string GetDialogueTranslation(string id, string english, string voiceoverId)
	{
		if (id == "karma_0")
		{
			id = "sutra_4";
		}
		return Translations.GetTranslation(id, english);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000465D4 File Offset: 0x000447D4
	public static string GetRandomTranslation(Translations.Dialogue[] dialogues)
	{
		int num = UnityEngine.Random.Range(0, dialogues.Length);
		Translations.Dialogue dialogue = dialogues[num];
		while (dialogue.Id == null || dialogue.English == null)
		{
			num = UnityEngine.Random.Range(0, dialogues.Length);
			dialogue = dialogues[num];
		}
		string id = dialogue.Id;
		string english = dialogue.English;
		return Translations.GetDialogueTranslation(id, english);
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00046644 File Offset: 0x00044844
	public static string GetTranslatedResult(string english)
	{
		if (Translations.PreferredLanguage == Translations.Language.English)
		{
			return english;
		}
		return "...";
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00046658 File Offset: 0x00044858
	private static string GetLanguageId(Translations.Language language)
	{
		switch (language)
		{
		case Translations.Language.English:
			return "en-US";
		case Translations.Language.Chinese:
			return "zh-CN";
		case Translations.Language.French:
			return "fr";
		case Translations.Language.German:
			return "de";
		case Translations.Language.Italian:
			return "it";
		case Translations.Language.Polish:
			return "pl";
		case Translations.Language.Portuguese:
			return "pt-PT";
		case Translations.Language.BrazilianPortuguese:
			return "pt-BR";
		case Translations.Language.Russian:
			return "ru";
		case Translations.Language.Spanish:
			return "es-ES";
		case Translations.Language.Japanese:
			return "ja";
		case Translations.Language.MexicanSpanish:
			return "es-MX";
		}
		return "Unknown";
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000466F4 File Offset: 0x000448F4
	public static void Init(Action onSuccess)
	{
		if (Translations.CurrentLanguage.Value != (int)Translations.PreferredLanguage || Translations.translations == null)
		{
			if (Translations.translations == null)
			{
				Translations.translations = new Dictionary<string, string>();
			}
			else
			{
				Translations.translations.Clear();
			}
			if (Translations.PreferredLanguage != Translations.Language.English)
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, string.Format("{0}.txt", Translations.GetLanguageId(Translations.PreferredLanguage)))))
					{
						while (!streamReader.EndOfStream)
						{
							string text = streamReader.ReadLine();
							for (int i = 0; i < text.Length; i++)
							{
								if (text[i] == ' ')
								{
									string key = text.Substring(0, i).ToLowerInvariant();
									string text2 = text.Substring(i + 1);
									text2 = text2.Replace("\\n", "\n");
									Translations.translations.Add(key, text2);
									break;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log(string.Format("Error while loading the language {0}: {1}", Translations.GetLanguageId(Translations.PreferredLanguage), ex.Message));
				}
			}
		}
		Translations.CurrentLanguage.Value = (int)Translations.PreferredLanguage;
		Translations.UpdateDates(GameState.CurrentState.gameObject);
		Translations.UpdateUI(GameState.CurrentState.gameObject);
		GameState.GetGirlScreen().SetGirl();
		Translations.UpdateGirlNames();
		if (onSuccess != null)
		{
			onSuccess();
		}
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000468A4 File Offset: 0x00044AA4
	public static string GetDisclaimer(Translations.Language language)
	{
		switch (language)
		{
		case Translations.Language.English:
			return "Disclaimer:  Translations of Crush Crush are provided by fan volunteers using Crowdin. Sad Panda Studios cannot guarantee the accuracy of translations, which may contain incorrect language or offensive material. Sad Panda Studios will not be held responsible for such inaccuracies, and player discretion is advised in using them.";
		case Translations.Language.Chinese:
			return "免责声明:  Crush Crush 的翻译由使用 Crowdin 的粉丝志愿者提供。 Sad Panda Studios 无法保证翻译的准确性，其中可能包含错误的文字或冒犯性词语。 Sad Panda Studios 对此类错误不承担任何责任，玩家自行决定是否使用这些翻译。";
		case Translations.Language.French:
			return "Clause de non-responsabilité:  Les traductions de Crush Crush sont fournies par des fans bénévoles à l'aide de Crowdin. Sad Panda Studios ne peut garantir l'exactitude des traductions, qui peuvent contenir un langage inapproprié ou du contenu offensant. Sad Panda Studios ne pourra être tenu responsable pour des telles inexactitudes, et leur utilisation est à la seule discrétion du joueur.";
		case Translations.Language.German:
			return "Haftungsausschluss:  Die Übersetzungen von Crush Crush werden von Fans über Crowdin auf freiwilliger Basis angefertigt. Sad Panda Studios übernimmt keine Gewähr für die Richtigkeit der Übersetzungen, die sprachlich falsch sein oder anstößiges Material enthalten könnten. Sad Panda Studios haftet für solche Ungenauigkeiten nicht, den Spielern wird empfohlen, vorsichtig mit den Texten umzugehen.";
		case Translations.Language.Italian:
			return "Dichiarazione di non responsabilità:  Le traduzioni di Crush Crush sono fornite volontariamente da appassionati utilizzando Crowdin. Sad Panda Studios non può garantire la correttezza delle traduzioni, che potrebbero contenere linguaggio inadeguato o materiale offensivo. Sad Panda Studios declina qualsivoglia responsabilità nei confronti di tali inesattezze, e il loro utilizzo è a discrezione esclusiva del giocatore.";
		case Translations.Language.Polish:
			return "Wyłączenie odpowiedzialności:  Tłumaczenie Crush Crush odbywa się przy udziale wolontariuszy na platformie CrowdIn. Sad Panda Studios nie jest w stanie zagwarantować poprawności tłumaczenia, które może zawierać błędy językowe i/lub wulgaryzmy. Sad Panda Studios nie ponosi odpowiedzialności za takie nieścisłości, a gracze są proszeni o tego uszanowanie.";
		case Translations.Language.Portuguese:
			return "Exoneração de responsabilidade:  As traduções de Crush Crush são fornecidas voluntariamente por fãs através do Crowdin. A Sad Panda Studios não pode garantir a precisão das traduções, que podem conter linguagem incorreta ou material ofensivo. A Sad Panda Studios não será responsável por tais imprecisões, ficando a sua utilização ao critério do jogador.";
		case Translations.Language.BrazilianPortuguese:
			return "Avisos Legais:  As traduções do Crush Crush são fornecidas por fãs voluntários utilizando o Crowdin. A Sad Panda Studios não pode garantir a precisão das traduções, que podem conter linguagem incorreta ou material ofensivo. A Sad Panda Studios não será considerada responsável por tais imprecisões, e o uso fica a critério do jogador.";
		case Translations.Language.Russian:
			return "Отказ от ответственности:  Перевод Crush Crush выполняют любители-добровольцы с использованием платформы Crowdin. Sad Panda Studios не может гарантировать точность перевода, и такой перевод может содержать ошибки и ненормативную лексику. Sad Panda Studios не несет ответственности за такие неточности, и игрокам рекомендуется действовать осмотрительно, полагаясь на текст с такими неточностями.";
		case Translations.Language.Spanish:
			return "Renuncia de responsabilidad:  Las traducciones de Crush Crush se han proporcionado por voluntarios a través de Crowdin. Sad Panda Studios no puede garantizar la precisión de las traducciones que podrían ser incorrectas o contener lenguaje ofensivo. Sad Panda Studios no se hace responsable de tales imprecisiones, y el uso del juego queda a discreción del jugador.";
		case Translations.Language.Japanese:
			return "免責事項:  Crush Crush の翻訳は、Crowdin を使用してファンがボランティアで提供しているものです。翻訳には不適切な表現や不快な内容が含まれる場合があり、Sad Panda Studios は翻訳の正確性を保証することはできません。 Sad Panda Studios はそのような誤りに対して責任を負わず、その利用にあたってはプレイヤーが判断するものとします。";
		case Translations.Language.Hindi:
			return "अस्वीकरण:  Crush Crush के अनुवाद को प्रशंसक वालंटियरों द्वारा Crowdin का उपयोग करते हुए प्रदान किया गया है। Sad Panda Studios अनुवाद की शुद्धता संबंधी गारंटी नहीं ले सकता, जिसमें संभव है कि ग़लत भाषा या आपत्तिजनक सामग्री शामिल हो। Sad Panda Studios ऐसी अशुद्धियों के लिए जिम्मेदार नहीं माना जाएगा, और ऐसे में सलाह दी जाती है कि खिलाड़ी अपने विवेक पर अनुवाद का उपयोग करें।";
		case Translations.Language.MexicanSpanish:
			return "Renuncia de responsabilidad:  Las traducciones de Crush Crush se han proporcionado por voluntarios a través de Crowdin. Sad Panda Studios no puede garantizar la precisión de las traducciones que podrían ser incorrectas o contener lenguaje ofensivo. Sad Panda Studios no se hace responsable de tales imprecisiones, y el uso del juego queda a discreción del jugador.";
		default:
			return "Disclaimer:  Translations of Crush Crush are provided by fan volunteers using Crowdin.  Sad Panda Studios cannot guarantee the accuracy of translations, which may contain incorrect language or offensive material.  Sad Panda Studios will not be held responsible for such inaccuracies, and player discretion is advised in using them.";
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00046948 File Offset: 0x00044B48
	public static string GetLTETaskId(int id)
	{
		switch (id)
		{
		case 1:
			return "limited_time_events_1_4";
		case 2:
			return "limited_time_events_1_3";
		case 3:
			return "limited_time_events_7_1";
		case 4:
			return "limited_time_events_1_1";
		case 5:
			return "limited_time_events_9_1";
		case 6:
			return "limited_time_events_1_2";
		case 7:
			return "limited_time_events_7_2";
		case 8:
			return "limited_time_events_8_3";
		case 9:
			return "limited_time_events_9_2";
		case 10:
			return "limited_time_events_10_1";
		case 11:
			return "limited_time_events_11_1";
		case 12:
			return "limited_time_events_1_5";
		case 13:
			return "limited_time_events_7_3";
		case 14:
			return "limited_time_events_18_3";
		case 15:
			return "limited_time_events_18_4";
		case 16:
			return "limited_time_events_18_5";
		case 17:
			return "limited_time_events_18_6";
		case 18:
			return "limited_time_events_18_7";
		default:
			return "limited_time_events_0_0";
		}
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00046A20 File Offset: 0x00044C20
	public static string GetGirlRequirement(Balance.GirlName girl)
	{
		string arg = string.Empty;
		switch (girl)
		{
		case Balance.GirlName.Mio:
			arg = "Level 1 " + Translations.TranslateSkill(Requirement.Skill.TechSavvy);
			break;
		case Balance.GirlName.Quill:
			arg = "Level 2 " + Translations.TranslateSkill(Requirement.Skill.Motivation);
			break;
		case Balance.GirlName.Elle:
			arg = "Level 2 " + Translations.TranslateSkill(Requirement.Skill.Wisdom);
			break;
		case Balance.GirlName.Nutaku:
			arg = "Level 6 " + Translations.TranslateSkill(Requirement.Skill.Buff);
			break;
		case Balance.GirlName.Iro:
			arg = "Level 6 " + Translations.TranslateSkill(Requirement.Skill.Badass);
			break;
		case Balance.GirlName.Bonnibel:
			arg = "Level 10 " + Translations.TranslateSkill(Requirement.Skill.Suave);
			break;
		case Balance.GirlName.Ayano:
			arg = "Level 12 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			break;
		case Balance.GirlName.Fumi:
			arg = "Level 14 " + Translations.TranslateSkill(Requirement.Skill.Smart);
			break;
		case Balance.GirlName.Bearverly:
			arg = "Level 17 " + Translations.TranslateSkill(Requirement.Skill.Funny);
			break;
		case Balance.GirlName.Nina:
			arg = "$100,000,000";
			break;
		case Balance.GirlName.Alpha:
			arg = "Level 25 " + Translations.TranslateSkill(Requirement.Skill.Angst);
			break;
		case Balance.GirlName.Pamu:
			arg = "Level 28 " + Translations.TranslateSkill(Requirement.Skill.Lucky);
			break;
		case Balance.GirlName.Luna:
			arg = Translations.TranslateJob(Requirement.JobType.Wizard, 0);
			break;
		case Balance.GirlName.Eva:
			arg = Translations.TranslateJob(Requirement.JobType.Wizard, 1);
			break;
		case Balance.GirlName.Karma:
		case Balance.GirlName.Sutra:
			arg = "Level 55 " + Translations.TranslateSkill(Requirement.Skill.Mysterious);
			break;
		case Balance.GirlName.DarkOne:
			arg = string.Format("{1} {0}\n{2} {0}", Translations.GetTranslation("everything_else_4_9", "Lover"), Translations.TranslateGirlName(Balance.GirlName.Karma), Translations.TranslateGirlName(Balance.GirlName.Sutra));
			break;
		case Balance.GirlName.QPiddy:
			arg = string.Format("{1} {0}", Translations.GetTranslation("everything_else_4_9", "Lover"), Translations.TranslateGirlName(Balance.GirlName.DarkOne));
			break;
		case Balance.GirlName.Darya:
			arg = "Darya Bundle";
			break;
		case Balance.GirlName.Jelle:
		case Balance.GirlName.Quillzone:
		case Balance.GirlName.Bonchovy:
		case Balance.GirlName.Spectrum:
			arg = "Monster Bundle";
			break;
		case Balance.GirlName.Charlotte:
			arg = "Charlotte Bundle";
			break;
		case Balance.GirlName.Odango:
			arg = "Odango LTE";
			break;
		case Balance.GirlName.Shibuki:
			arg = "Shibuki LTE";
			break;
		case Balance.GirlName.Sirina:
			arg = "Sirina LTE";
			break;
		case Balance.GirlName.Catara:
			arg = "Catara Bundle";
			break;
		case Balance.GirlName.Vellatrix:
			arg = "Vellatrix LTE";
			break;
		case Balance.GirlName.Peanut:
			arg = "Complete Peanut\nPhone Fling";
			break;
		case Balance.GirlName.Roxxy:
			arg = "Roxxy LTE";
			break;
		case Balance.GirlName.Tessa:
			arg = "Tessa LTE";
			break;
		case Balance.GirlName.Claudia:
			arg = "Claudia LTE";
			break;
		case Balance.GirlName.Rosa:
			arg = "Rosa LTE";
			break;
		case Balance.GirlName.Juliet:
			arg = "Juliet LTE";
			break;
		case Balance.GirlName.Wendy:
			arg = "Complete Wendy\nPhone Fling +\nWendy LTE";
			break;
		case Balance.GirlName.Ruri:
			arg = "Complete Ruri\nPhone Fling +\nRuri LTE";
			break;
		case Balance.GirlName.Generica:
			arg = "Complete Generica\nPhone Fling +\nGenerica LTE";
			break;
		case Balance.GirlName.Suzu:
			arg = "Suzu Bundle";
			break;
		case Balance.GirlName.Lustat:
			arg = "Lustat LTE";
			break;
		case Balance.GirlName.Sawyer:
			arg = "Complete Sawyer\nPhone Fling +\nSawyer LTE";
			break;
		case Balance.GirlName.Explora:
			arg = "Explora Bundle";
			break;
		case Balance.GirlName.Esper:
			arg = "Esper LTE";
			break;
		case Balance.GirlName.Renee:
			arg = "Complete Renée\nPhone Fling +\nRenée LTE";
			break;
		case Balance.GirlName.Mallory:
			arg = "Mallory Bundle\nor\nCabin Fever";
			break;
		case Balance.GirlName.Lake:
			arg = "Complete Lake\nPhone Fling +\nLake LTE";
			break;
		}
		return string.Format("<b>{0}</b>\n{1}", Translations.GetTranslation("everything_else_6_0", "Requires:"), arg);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00046DB4 File Offset: 0x00044FB4
	public static string GetCellphoneRequirement(short id)
	{
		string arg = string.Empty;
		switch (id)
		{
		case 1:
			arg = Translations.TranslateGirlName(Balance.GirlName.Elle) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			break;
		case 2:
			arg = Translations.TranslateGirlName(Balance.GirlName.Iro) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			break;
		case 3:
			arg = Translations.TranslateGirlName(Balance.GirlName.Ayano) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			break;
		case 4:
			arg = Translations.TranslateGirlName(Balance.GirlName.Mio) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 5:
			arg = Translations.TranslateGirlName(Balance.GirlName.Iro) + " " + Translations.GetTranslation("everything_else_4_6", "Crush");
			break;
		case 6:
			arg = "Lvl 25 " + Translations.TranslateSkill(Requirement.Skill.Buff);
			break;
		case 7:
			arg = Translations.TranslateJob(Requirement.JobType.Computers, 3) + " (" + Translations.TranslateJob(Requirement.JobType.Computers, 0) + ")";
			break;
		case 8:
			arg = "3 " + Translations.TranslateGirlName(Balance.GirlName.Peanut) + " Pics";
			break;
		case 9:
			arg = "25x Reset Boost";
			break;
		case 10:
			arg = Translations.TranslateGirlName(Balance.GirlName.Pamu) + " " + Translations.GetTranslation("everything_else_4_0", "Adversary");
			break;
		case 11:
			arg = "3 " + Translations.TranslateGirlName(Balance.GirlName.Sawyer) + " Pics";
			break;
		case 12:
			arg = Translations.TranslateGirlName(Balance.GirlName.Nina) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			break;
		case 13:
			arg = "Lvl 25 " + Translations.TranslateSkill(Requirement.Skill.Angst);
			break;
		case 14:
			arg = "Lvl 25 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			break;
		case 15:
			arg = Translations.TranslateGirlName(Balance.GirlName.Bonnibel) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 16:
			arg = Translations.TranslateGirlName(Balance.GirlName.Bearverly) + " " + Translations.GetTranslation("everything_else_4_6", "Crush");
			break;
		case 17:
			arg = "Lvl 25 " + Translations.TranslateSkill(Requirement.Skill.Suave);
			break;
		case 18:
			arg = "3 Lake Pics";
			break;
		case 19:
			arg = Translations.TranslateGirlName(Balance.GirlName.Bonnibel) + " " + Translations.GetTranslation("everything_else_4_5", "Awkward Besties");
			break;
		case 20:
			arg = Translations.TranslateGirlName(Balance.GirlName.Fumi) + " " + Translations.GetTranslation("everything_else_4_5", "Awkward Besties");
			break;
		case 21:
			arg = Translations.TranslateGirlName(Balance.GirlName.QPiddy) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			break;
		case 22:
			arg = "Sacrifice " + Translations.TranslateGirlName(Balance.GirlName.QPiddy);
			break;
		case 23:
			arg = Translations.TranslateGirlName(Balance.GirlName.Cassie) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 24:
			arg = Translations.TranslateGirlName(Balance.GirlName.Mio) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 25:
			arg = Translations.TranslateGirlName(Balance.GirlName.Quill) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 26:
			arg = Translations.TranslateGirlName(Balance.GirlName.Elle) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 27:
			arg = Translations.TranslateGirlName(Balance.GirlName.Iro) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 28:
			arg = Translations.TranslateGirlName(Balance.GirlName.Bonnibel) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 29:
			arg = Translations.TranslateGirlName(Balance.GirlName.Fumi) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 30:
			arg = Translations.TranslateGirlName(Balance.GirlName.Bearverly) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 31:
			arg = Translations.TranslateGirlName(Balance.GirlName.Nina) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		case 32:
			arg = Translations.TranslateGirlName(Balance.GirlName.Alpha) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			break;
		}
		return string.Format("<b>{0}</b>\n{1}", Translations.GetTranslation("everything_else_6_0", "Requires:"), arg);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00047288 File Offset: 0x00045488
	public static string GetCellphoneName(PhoneModel girl)
	{
		switch (girl.Id)
		{
		case 1:
			return Translations.GetTranslation("everything_else_2_33", "Peanut");
		case 2:
			return Translations.GetTranslation("everything_else_2_38", "Wendy");
		case 3:
			return Translations.GetTranslation("everything_else_2_10", "Generica");
		case 4:
			return "Lotus";
		case 5:
			return "Sofia";
		case 6:
			return "Caitlin";
		case 7:
			return Translations.GetTranslation("everything_else_2_39", "Ruri");
		case 8:
			return "Miss Desirée";
		case 9:
			return "Honey";
		case 10:
			return Translations.GetTranslation("everything_else_2_43", "Sawyer");
		case 11:
			return "Lake";
		case 12:
			return "Willow";
		case 13:
			return "Nova";
		case 14:
			return "Blanche";
		case 15:
			return "Babybelle";
		case 16:
			return "Renée";
		case 17:
			return "Francine";
		case 18:
			return "Mur";
		case 19:
			return "Amelia";
		case 20:
			return "Dr. " + Translations.GetTranslation("everything_else_2_8", "Fumi");
		case 21:
			return Translations.GetTranslation("everything_else_2_5", "Dark One");
		case 22:
			return Translations.GetTranslation("everything_else_2_20", "QPernikiss");
		case 23:
			return Translations.GetTranslation("everything_else_2_4", "Cassie");
		case 24:
			return Translations.GetTranslation("everything_else_2_13", "Mio");
		default:
			return girl.Name;
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00047410 File Offset: 0x00045610
	public static string GetCellphoneConvoRequirement(short id, short conversationId)
	{
		string text = string.Empty;
		switch (id)
		{
		case 1:
			if (conversationId == 1)
			{
				text = Translations.GetCellphoneName(Universe.CellphoneGirls[8]) + " Pic 1";
			}
			break;
		case 2:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Iro) + " " + Translations.GetTranslation("everything_else_4_7", "Sweetheart");
			}
			break;
		case 3:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Ayano) + " " + Translations.GetTranslation("everything_else_4_6", "Crush");
			}
			break;
		case 4:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Quill) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			}
			else if (conversationId == 2)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Elle) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			}
			else if (conversationId == 3)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Iro) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			}
			break;
		case 5:
			if (conversationId == 1)
			{
				text = Translations.TranslateJob(Requirement.JobType.Lifeguard, 6) + " (" + Translations.TranslateJob(Requirement.JobType.Lifeguard, 0) + ")";
			}
			else if (conversationId == 2)
			{
				text = "Lvl 35 " + Translations.TranslateSkill(Requirement.Skill.Motivation);
			}
			else if (conversationId == 3)
			{
				text = "Lvl 45 " + Translations.TranslateSkill(Requirement.Skill.Lucky);
			}
			break;
		case 6:
			if (conversationId == 1)
			{
				text = Translations.TranslateJob(Requirement.JobType.Movies, 2) + " (" + Translations.TranslateJob(Requirement.JobType.Movies, 0) + ")";
			}
			else if (conversationId == 2)
			{
				text = "Lvl 55 " + Translations.TranslateSkill(Requirement.Skill.Wisdom);
			}
			else if (conversationId == 3)
			{
				text = "Lvl 60 " + Translations.TranslateSkill(Requirement.Skill.Funny);
			}
			break;
		case 7:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Bearverly) + " " + Translations.GetTranslation("everything_else_4_2", "Frenemy");
			}
			else if (conversationId == 2)
			{
				text = "Lvl 43 " + Translations.TranslateSkill(Requirement.Skill.Smart);
			}
			break;
		case 8:
			if (conversationId == 1)
			{
				text = "Lvl 62 " + Translations.TranslateSkill(Requirement.Skill.Wisdom);
			}
			break;
		case 9:
			if (conversationId == 1)
			{
				text = "x1024 " + Translations.GetTranslation("everything_else_26_0", "Reset Boost");
			}
			break;
		case 10:
			if (conversationId == 1)
			{
				text = "6 Lake Pics";
			}
			else if (conversationId == 2)
			{
				text = Translations.TranslateJob(Requirement.JobType.Sports, 6) + " (" + Translations.TranslateJob(Requirement.JobType.Sports, 0) + ")";
			}
			break;
		case 11:
			if (conversationId == 1)
			{
				text = "Lvl 25 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			}
			else if (conversationId == 2)
			{
				text = "Lvl 30 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			}
			else if (conversationId == 3)
			{
				text = "Lvl 40 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			}
			else if (conversationId == 4)
			{
				text = "Lvl 45 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			}
			break;
		case 12:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Nina) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			}
			else if (conversationId == 2)
			{
				text = "Lvl 70 " + Translations.TranslateSkill(Requirement.Skill.Wisdom);
			}
			else if (conversationId == 3)
			{
				text = "Lvl 73 " + Translations.TranslateSkill(Requirement.Skill.Tenderness);
			}
			break;
		case 13:
			if (conversationId == 1)
			{
				text = "Lvl 40 " + Translations.TranslateSkill(Requirement.Skill.Angst);
			}
			else if (conversationId == 2)
			{
				text = Translations.TranslateJob(Requirement.JobType.Art, 9) + " (" + Translations.TranslateJob(Requirement.JobType.Art, 0) + ")";
			}
			else if (conversationId == 3)
			{
				text = "Lvl 65 " + Translations.TranslateSkill(Requirement.Skill.Angst);
			}
			else if (conversationId == 4)
			{
				text = "Lvl 70 " + Translations.TranslateSkill(Requirement.Skill.Funny);
			}
			break;
		case 14:
			if (conversationId == 1)
			{
				text = "Lvl 42 " + Translations.TranslateSkill(Requirement.Skill.Suave);
			}
			else if (conversationId == 2)
			{
				text = "Lvl 48 " + Translations.TranslateSkill(Requirement.Skill.Badass);
				text = text + " & Lvl 48 " + Translations.TranslateSkill(Requirement.Skill.Lucky);
			}
			else if (conversationId == 3)
			{
				text = "Lvl 55 " + Translations.TranslateSkill(Requirement.Skill.Funny);
			}
			break;
		case 15:
			if (conversationId == 1)
			{
				text = "Lvl 46 " + Translations.TranslateSkill(Requirement.Skill.Funny);
			}
			else if (conversationId == 2)
			{
				text = "5 Nova Pics";
			}
			else if (conversationId == 3)
			{
				text = "Lvl 64 " + Translations.TranslateSkill(Requirement.Skill.Lucky);
			}
			break;
		case 16:
			if (conversationId == 1)
			{
				text = Translations.TranslateGirlName(Balance.GirlName.Bearverly) + " " + Translations.GetTranslation("everything_else_4_9", "Lover");
			}
			else if (conversationId == 2)
			{
				text = "Lvl 43 " + Translations.TranslateSkill(Requirement.Skill.Smart);
			}
			break;
		case 19:
			if (conversationId == 1)
			{
				text = Translations.TranslateJob(Requirement.JobType.Zoo, 7) + " (" + Translations.TranslateJob(Requirement.JobType.Zoo, 0) + ")";
			}
			else if (conversationId == 2)
			{
				text = Translations.TranslateJob(Requirement.JobType.Zoo, 9) + " (" + Translations.TranslateJob(Requirement.JobType.Zoo, 0) + ")";
			}
			else if (conversationId == 3)
			{
				text = Translations.TranslateJob(Requirement.JobType.Legal, 9) + " (" + Translations.TranslateJob(Requirement.JobType.Legal, 0) + ")";
			}
			break;
		}
		return string.Format("<b>{0} {1}</b>\n{2}", Translations.GetCellphoneName(Universe.CellphoneGirls[id]), Translations.GetTranslation("everything_else_6_0", "Requires:"), text);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00047A3C File Offset: 0x00045C3C
	public static string GetHobbyTitle(short id)
	{
		if (id == 4)
		{
			return Translations.GetTranslation("requirements_16", Universe.Hobbies[id].Name);
		}
		return Translations.GetTranslation(string.Format("hobby_{0}_0", id.ToString()), Universe.Hobbies[id].Name);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00047A94 File Offset: 0x00045C94
	public static string TranslateSkill(Requirement.Skill skill)
	{
		return Translations.TranslateSkill((short)(skill + 1));
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00047AA0 File Offset: 0x00045CA0
	public static string TranslateSkill(short id)
	{
		return Translations.GetTranslation(string.Format("hobby_{0}_1", id.ToString()), Universe.Hobbies[id].Resource.Name);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00047AD8 File Offset: 0x00045CD8
	public static string TranslateGift(Requirement.GiftType gift, Balance.GirlName girl)
	{
		foreach (KeyValuePair<short, GiftModel> keyValuePair in Universe.Gifts)
		{
			if (keyValuePair.Value.Requirement == gift)
			{
				return Translations.TranslateGift(keyValuePair.Key, girl);
			}
		}
		return "Unknown Gift";
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00047B64 File Offset: 0x00045D64
	public static string TranslateGift(short id, Balance.GirlName girl)
	{
		string english = Universe.Gifts[id].Names.Singular.English;
		string id2 = string.Format("gift_{0}", id.ToString());
		if (girl == Balance.GirlName.Explora && Universe.Gifts[id].ExploraNames != null)
		{
			english = Universe.Gifts[id].ExploraNames.Singular.English;
			id2 = string.Format("gift_{0}", ((int)(id + 64)).ToString());
		}
		else if (girl == Balance.GirlName.Mallory && Universe.Gifts[id].MalloryNames != null)
		{
			english = Universe.Gifts[id].MalloryNames.Singular.English;
			id2 = string.Format("gift_{0}", ((int)(id + 128)).ToString());
		}
		return Translations.GetTranslation(id2, english);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00047C4C File Offset: 0x00045E4C
	public static string TranslateDate(Requirement.DateType date)
	{
		switch (date)
		{
		case Requirement.DateType.MoonlightStroll:
			return Translations.GetTranslation("requirements_10_0", "Moonlight Stroll");
		default:
			if (date == Requirement.DateType.MovieTheater)
			{
				return Translations.GetTranslation("requirements_10_1", "Movie Theater");
			}
			if (date != Requirement.DateType.Beach)
			{
				return "Unknown Date";
			}
			return Translations.GetTranslation("requirements_10_3", "Beach");
		case Requirement.DateType.Sightseeing:
			return Translations.GetTranslation("requirements_10_2", "Sightseeing");
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00047CCC File Offset: 0x00045ECC
	public static string TranslateLongDate(Requirement.DateType date)
	{
		switch (date)
		{
		case Requirement.DateType.MoonlightStroll:
			return Translations.GetTranslation("everything_else_92_0", "A romantic moonlight stroll");
		default:
			if (date == Requirement.DateType.MovieTheater)
			{
				return Translations.GetTranslation("everything_else_93_0", "A movie date");
			}
			if (date != Requirement.DateType.Beach)
			{
				return "Unknown Date";
			}
			return Translations.GetTranslation("everything_else_91_0", "A tropical getaway");
		case Requirement.DateType.Sightseeing:
			return Translations.GetTranslation("everything_else_94_0", "A sightseeing adventure");
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00047D4C File Offset: 0x00045F4C
	public static string TranslateJob(Requirement.JobType job, int level = 0)
	{
		string text = job.ToFriendlyString();
		if (level == 0)
		{
			if (text == "Burger")
			{
				text = "Fast Food";
			}
		}
		else
		{
			short key = 0;
			for (int i = 0; i < Universe.Jobs.Count; i++)
			{
				if (1 << i == (int)job)
				{
					key = (short)(i + 1);
				}
			}
			JobModel jobModel = Universe.Jobs[key];
			JobModel.JobState[] states = jobModel.States;
			int num = Math.Max(0, Math.Min(states.Length - 1, level - 1));
			text = states[num].Name;
		}
		string str = "requirements_";
		switch (job)
		{
		case Requirement.JobType.Burger:
			str += "11";
			break;
		case Requirement.JobType.Restaurant:
			str += "12";
			break;
		default:
			if (job != Requirement.JobType.Art)
			{
				if (job != Requirement.JobType.Computers)
				{
					if (job != Requirement.JobType.Zoo)
					{
						if (job != Requirement.JobType.Hunting)
						{
							if (job != Requirement.JobType.Casino)
							{
								if (job != Requirement.JobType.Sports)
								{
									if (job != Requirement.JobType.Legal)
									{
										if (job != Requirement.JobType.Movies)
										{
											if (job != Requirement.JobType.Space)
											{
												if (job != Requirement.JobType.Slaying)
												{
													if (job != Requirement.JobType.Love)
													{
														if (job != Requirement.JobType.Wizard)
														{
															if (job != Requirement.JobType.Digger)
															{
																if (job != Requirement.JobType.Planter)
																{
																	return "NONE";
																}
																str += "36";
															}
															else
															{
																str += "35";
															}
														}
														else
														{
															str += "26";
														}
													}
													else
													{
														str += "25";
													}
												}
												else
												{
													str += "24";
												}
											}
											else
											{
												str += "23";
											}
										}
										else
										{
											str += "22";
										}
									}
									else
									{
										str += "21";
									}
								}
								else
								{
									str += "20";
								}
							}
							else
							{
								str += "19";
							}
						}
						else
						{
							str += "18";
						}
					}
					else
					{
						str += "17";
					}
				}
				else
				{
					str += "16";
				}
			}
			else
			{
				str += "15";
			}
			break;
		case Requirement.JobType.Cleaning:
			str += "13";
			break;
		case Requirement.JobType.Lifeguard:
			str += "14";
			break;
		}
		if (level <= 0)
		{
			return Translations.GetTranslation(str + "_0", text);
		}
		if (text == "Mercenary")
		{
			return Translations.GetTranslation("stats_11_8", text);
		}
		if (text == "Wizard" && level == 1)
		{
			return Translations.GetTranslation("stats_18_5", "Wizard");
		}
		if (text == "Love Doctor")
		{
			return Translations.GetTranslation("stats_15_5", "Love Doctor");
		}
		if (text == "Love Fairy")
		{
			return Translations.GetTranslation("stats_18_8", "Love Fairy");
		}
		return Translations.GetTranslation(str + "_" + level.ToString(), text);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x000480D4 File Offset: 0x000462D4
	public static string TranslateOutfit(Requirement.OutfitType outfit)
	{
		switch (outfit)
		{
		case Requirement.OutfitType.Monster:
			return Translations.GetTranslation("requirements_31_0", "Monster Outfit");
		case Requirement.OutfitType.Animated:
			return Translations.GetTranslation("requirements_29_0", "Animated Pose");
		default:
			if (outfit == Requirement.OutfitType.Christmas)
			{
				return Translations.GetTranslation("requirements_9_27", "Holiday Outfit");
			}
			if (outfit == Requirement.OutfitType.SchoolUniform)
			{
				return Translations.GetTranslation("requirements_9_22", "School Uniform");
			}
			if (outfit == Requirement.OutfitType.BathingSuit)
			{
				return Translations.GetTranslation("stats_12_7", "Bathing Suit");
			}
			if (outfit == Requirement.OutfitType.Unique)
			{
				return Translations.GetTranslation("requirements_29_0", "Unique Outfit");
			}
			if (outfit == Requirement.OutfitType.DiamondRing)
			{
				return Translations.GetTranslation("requirements_9_24", "Diamond Ring");
			}
			if (outfit == Requirement.OutfitType.Lingerie)
			{
				return Translations.GetTranslation("stats_20_7", "Lingerie");
			}
			if (outfit != Requirement.OutfitType.Nude)
			{
				return "Unknown Gift";
			}
			return Translations.GetTranslation("requirements_9_26", "Birthday Suit");
		case Requirement.OutfitType.DeluxeWedding:
			return Translations.GetTranslation("requirements_29_0", "DX Wedding Dress");
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x000481F4 File Offset: 0x000463F4
	public static string TranslateGirlName(Balance.GirlName name)
	{
		switch (name)
		{
		case Balance.GirlName.Cassie:
			return Translations.GetTranslation("everything_else_2_4", "Cassie");
		case Balance.GirlName.Mio:
			return Translations.GetTranslation("everything_else_2_13", "Mio");
		case Balance.GirlName.Quill:
			return Translations.GetTranslation("everything_else_2_16", "Quill");
		case Balance.GirlName.Elle:
			return Translations.GetTranslation("everything_else_2_6", "Elle");
		case Balance.GirlName.Nutaku:
			return Translations.GetTranslation("everything_else_2_15", "Nutaku");
		case Balance.GirlName.Iro:
			return Translations.GetTranslation("everything_else_2_9", "Iro");
		case Balance.GirlName.Bonnibel:
			return Translations.GetTranslation("everything_else_2_3", "Bonnibel");
		case Balance.GirlName.Ayano:
			return Translations.GetTranslation("everything_else_2_1", "Ayano");
		case Balance.GirlName.Fumi:
			return Translations.GetTranslation("everything_else_2_8", "Fumi");
		case Balance.GirlName.Bearverly:
			return Translations.GetTranslation("everything_else_2_2", "Bearverly");
		case Balance.GirlName.Nina:
			return Translations.GetTranslation("everything_else_2_14", "Nina");
		case Balance.GirlName.Alpha:
			return Translations.GetTranslation("everything_else_2_0", "Alpha");
		case Balance.GirlName.Pamu:
			return Translations.GetTranslation("everything_else_2_18", "Pamu");
		case Balance.GirlName.Luna:
			return Translations.GetTranslation("everything_else_2_12", "Luna");
		case Balance.GirlName.Eva:
			return Translations.GetTranslation("everything_else_2_7", "Eva");
		case Balance.GirlName.Karma:
			return Translations.GetTranslation("everything_else_2_11", "Karma");
		case Balance.GirlName.Sutra:
			return Translations.GetTranslation("everything_else_2_17", "Sutra");
		case Balance.GirlName.DarkOne:
			return Translations.GetTranslation("everything_else_2_5", "Dark One");
		case Balance.GirlName.QPiddy:
			return Translations.GetTranslation("everything_else_2_20", "QPernikiss");
		case Balance.GirlName.Darya:
			return Translations.GetTranslation("everything_else_2_21", "Darya");
		case Balance.GirlName.Jelle:
			return Translations.GetTranslation("everything_else_2_22", "Jelle");
		case Balance.GirlName.Quillzone:
			return Translations.GetTranslation("everything_else_2_23", "Quillzone");
		case Balance.GirlName.Bonchovy:
			return Translations.GetTranslation("everything_else_2_24", "Bonchovy");
		case Balance.GirlName.Spectrum:
			return Translations.GetTranslation("everything_else_2_25", "Spectrum");
		case Balance.GirlName.Charlotte:
			return Translations.GetTranslation("everything_else_2_26", "Charlotte");
		case Balance.GirlName.Odango:
			return Translations.GetTranslation("everything_else_2_27", "Odango");
		case Balance.GirlName.Shibuki:
			return Translations.GetTranslation("everything_else_2_28", "Shibuki");
		case Balance.GirlName.Sirina:
			return Translations.GetTranslation("everything_else_2_29", "Sirina");
		case Balance.GirlName.Catara:
			return Translations.GetTranslation("everything_else_2_30", "Catara");
		case Balance.GirlName.Vellatrix:
			return Translations.GetTranslation("everything_else_2_31", "Vellatrix");
		case Balance.GirlName.Peanut:
			return Translations.GetTranslation("everything_else_2_33", "Peanut");
		case Balance.GirlName.Roxxy:
			return Translations.GetTranslation("everything_else_2_32", "Roxxy");
		case Balance.GirlName.Tessa:
			return Translations.GetTranslation("everything_else_2_34", "Tessa");
		case Balance.GirlName.Claudia:
			return Translations.GetTranslation("everything_else_2_35", "Claudia");
		case Balance.GirlName.Rosa:
			return Translations.GetTranslation("everything_else_2_36", "Rosa");
		case Balance.GirlName.Juliet:
			return Translations.GetTranslation("everything_else_2_37", "Juliet");
		case Balance.GirlName.Wendy:
			return Translations.GetTranslation("everything_else_2_38", "Wendy");
		case Balance.GirlName.Ruri:
			return Translations.GetTranslation("everything_else_2_39", "Ruri");
		case Balance.GirlName.Generica:
			return Translations.GetTranslation("everything_else_2_10", "Generica");
		case Balance.GirlName.Suzu:
			return Translations.GetTranslation("everything_else_2_41", "Suzu");
		case Balance.GirlName.Lustat:
			return Translations.GetTranslation("everything_else_2_42", "Lustat");
		case Balance.GirlName.Sawyer:
			return Translations.GetTranslation("everything_else_2_43", "Sawyer");
		case Balance.GirlName.Explora:
			return Translations.GetTranslation("everything_else_2_44", "Explora");
		case Balance.GirlName.Esper:
			return Translations.GetTranslation("everything_else_2_46", "Esper");
		case Balance.GirlName.Renee:
			return Translations.GetTranslation("everything_else_2_47", "Renée");
		case Balance.GirlName.Mallory:
			return Translations.GetTranslation("everything_else_2_48", "Mallory");
		case Balance.GirlName.Lake:
			return Translations.GetTranslation("everything_else_2_49", "Lake");
		default:
			return string.Empty;
		}
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x000485C0 File Offset: 0x000467C0
	public static string TranslateBlood(string blood)
	{
		switch (blood)
		{
		case "A":
			return Translations.GetTranslation("stats_9_4", blood);
		case "A+":
			return Translations.GetTranslation("stats_13_4", blood);
		case "A-":
			return Translations.GetTranslation("stats_14_4", blood);
		case "B":
			return Translations.GetTranslation("stats_11_4", blood);
		case "AB":
			return Translations.GetTranslation("stats_16_4", blood);
		case "AB+":
			return Translations.GetTranslation("stats_18_4", blood);
		case "AB-":
			return Translations.GetTranslation("stats_4_4", blood);
		case "O+":
			return Translations.GetTranslation("stats_1_4", blood);
		case "O-":
			return Translations.GetTranslation("stats_17_4", blood);
		case "F":
			return Translations.GetTranslation("stats_12_4", blood);
		case "Minerals":
			return Translations.GetTranslation("stats_19_4", blood);
		case "Pink":
			return blood;
		}
		return blood;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00048754 File Offset: 0x00046954
	public static string TranslateBust(string bust)
	{
		if (Translations.PreferredLanguage == Translations.Language.French)
		{
			if (bust == "DD")
			{
				return "E";
			}
			if (bust == "E")
			{
				return "F";
			}
		}
		switch (bust)
		{
		case "A":
			return Translations.GetTranslation("stats_13_10", bust);
		case "B":
			return Translations.GetTranslation("stats_16_10", bust);
		case "C":
			return Translations.GetTranslation("stats_17_10", bust);
		case "D":
			return Translations.GetTranslation("stats_18_10", bust);
		case "DD":
			return Translations.GetTranslation("stats_19_10", bust);
		case "E":
			return Translations.GetTranslation("stats_3_10", bust);
		case "XXL":
			return Translations.GetTranslation("stats_9_10", bust);
		case "Varies":
			return Translations.GetTranslation("stats_7_10", bust);
		}
		return bust;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x000488B8 File Offset: 0x00046AB8
	public static string TranslateTalkText(int loveLevel)
	{
		if (loveLevel < 3)
		{
			return Translations.GetTranslation("everything_else_8_0", "Sorry");
		}
		if (loveLevel < 5)
		{
			return Translations.GetTranslation("everything_else_8_1", "Talk");
		}
		if (loveLevel < 9)
		{
			return Translations.GetTranslation("everything_else_8_2", "Flirt");
		}
		return Translations.GetTranslation("everything_else_8_3", "Seduce");
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0004891C File Offset: 0x00046B1C
	public static void TranslateAndScale(Text element, string translationId, string english, int defaultSize = 0)
	{
		element.text = Translations.GetTranslation(translationId, english);
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0004892C File Offset: 0x00046B2C
	private static void UpdateDates(GameObject gameObject)
	{
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00048930 File Offset: 0x00046B30
	public static void UpdateGirlNames()
	{
		Transform transform = GameState.CurrentState.transform.Find("Girls/Girl List/Scroll View/Content Panel");
		Transform[] array = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			Girl component = transform.GetChild(i).GetComponent<Girl>();
			Text component2 = component.transform.Find("Name").GetComponent<Text>();
			string text = Translations.TranslateGirlName(component.GirlName).ToUpperInvariant();
			if (component.GirlName == Balance.GirlName.Ayano && component.Love == 0 && component.Hearts == 0L)
			{
				text = Translations.GetTranslation("everything_else_2_10", "Generica").ToUpperInvariant();
			}
			array[i] = component2.transform;
			bool flag = Translations.PreferredLanguage == Translations.Language.Japanese || Translations.PreferredLanguage == Translations.Language.Chinese;
			component2.rectTransform.sizeDelta = ((!flag) ? new Vector2(88f, 30f) : new Vector2(30f, 88f));
			component2.rectTransform.rotation = Quaternion.Euler(0f, 0f, (float)((!flag) ? -90 : 0));
			if (flag)
			{
				string text2 = text;
				text = text2[0].ToString();
				for (int j = 1; j < text2.Length; j++)
				{
					text = text + "\n" + text2[j];
				}
				switch (text2.Length)
				{
				case 1:
				case 2:
				case 3:
					component2.fontSize = 24;
					break;
				case 4:
					component2.fontSize = 20;
					break;
				case 5:
					component2.fontSize = 16;
					break;
				case 6:
					component2.fontSize = 14;
					break;
				case 7:
					component2.fontSize = 12;
					break;
				}
				text = text.Replace("ー", "|");
			}
			component2.text = text;
			Text component3 = component.transform.Find("Requirement").GetComponent<Text>();
			component3.text = Translations.GetGirlRequirement(component.GirlName);
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00048B7C File Offset: 0x00046D7C
	public static void UpdateUI(GameObject rootObject)
	{
		rootObject.transform.Find("Bottom UI/GirlsBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_0", "GIRLS");
		rootObject.transform.Find("Bottom UI/JobsBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_1", "JOBS");
		rootObject.transform.Find("Bottom UI/HobbyBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_2", "HOBBIES");
		rootObject.transform.Find("Bottom UI/SkillsBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_3", "STATS");
		rootObject.transform.Find("Bottom UI/StoreBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_5", "STORE");
		rootObject.transform.Find("Bottom UI/AchievementsBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_4", "ACHIEVEMENTS");
		rootObject.transform.Find("Bottom UI/MoreBTN/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_6", "MORE");
		rootObject.transform.Find("Top UI/Achievement/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_55_0", "More Time Unlocked!");
		rootObject.transform.Find("Popups/Task System/Rewards").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_12_0", "Rewards:");
		rootObject.transform.Find("Popups/Task System/Tasks/New Tasks").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_22_0", "New daily tasks in:");
		rootObject.transform.Find("Popups/Task System/Miss Reward/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_6_0", "You missed the event?");
		rootObject.transform.Find("Popups/Task System/Miss Reward").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_5_0", "Missed last event's reward?");
		rootObject.transform.Find("Popups/Task System/Rewards/Purchase Now/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_13_0", "Insta-Finish");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 1/Reward Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_12_0", "Reward");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 2/Reward Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_12_0", "Reward");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 3/Reward Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_12_0", "Reward");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 1/Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_15_0", "START");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 2/Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_15_0", "START");
		rootObject.transform.Find("Popups/Task System/Tasks/Task 3/Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_15_0", "START");
		string str = (Translations.PreferredLanguage != Translations.Language.Spanish && Translations.PreferredLanguage != Translations.Language.English) ? " - " : " ";
		rootObject.transform.Find("Popups/Task System/Rewards/Information Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_23_0", "Log in every day and run the Event Timer to collect rewards!\nCollect Event Tokens for Exclusive prizes!") + str + Translations.GetTranslation("limited_time_events_23_1", string.Empty);
		rootObject.transform.Find("Popups/Intro Tutorial/Speech Box/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_2_19", "Qpiddy");
		rootObject.transform.Find("Popups/Intro Tutorial/Speech Box/Next Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_9", "Next");
		rootObject.transform.Find("Popups/Girl Introduction/Accept Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_9", "Next");
		rootObject.transform.Find("Popups/Girl Introduction/Skip Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_10_1", "Skip");
		rootObject.transform.Find("Girls/Girl Information/Interaction Buttons/Interaction Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_9_0", "Stats");
		rootObject.transform.Find("Girls/Girl Information/Affection Tab/Affection Meter Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_3_0", "Affection Meter");
		rootObject.transform.Find("Girls/Girl Information/Background/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_6_0", "Requirements:");
		rootObject.transform.Find("Girls/Popups/Gifting/Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_9_1", "Gift");
		rootObject.transform.Find("Girls/Popups/Gifting/Dialog/Disclaimer").GetComponent<Text>().text = Translations.GetTranslation("everything_else_14_1", "Outfits wearable at Lover level");
		rootObject.transform.Find("Girls/Popups/Dating/Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_9_2", "Date");
		rootObject.transform.Find("Girls/Girl Information/Interaction Speech Box/Content/Accept Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_10_0", "Close");
		rootObject.transform.Find("Girls/Girl Information/Stat Bonus").GetComponent<Text>().text = Translations.GetTranslation("everything_else_5_0", "+ Stat Bonus");
		rootObject.transform.Find("Girls").GetComponent<Girls>().UpdateHeartText(true);
		rootObject.transform.Find("Girls/Popups/Gift Buy Popup/Dialog/General Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_3", "or");
		rootObject.transform.Find("Girls/Popups/Date Overlay/Dialog/Progress Container/Complete Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_18_2", "Click to Complete");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(0).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_2", "Age:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(1).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_3", "Birthday:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(2).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_4", "Hobby:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(3).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_5", "Blood Type:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(4).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_6", "Fav Job:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(5).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_7", "Fav Food:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(6).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_8", "Gift Preference:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(7).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_9", "Occupation:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(8).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_10", "Liked Traits:");
		rootObject.transform.Find("Girls/Popups/Statistics/Grid").GetChild(9).Find("Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_13_11", "Bust:");
		rootObject.transform.Find("Girls/Popups/Date Buy Popup/Dialog/General Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_3", "or");
		rootObject.transform.Find("Girls/Popups/Date Buy Popup/Dialog/Pay Money Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_2", "Pay");
		rootObject.transform.Find("Girls/Popups/Date Buy Popup/Dialog/Pay Diamonds Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_2", "Pay");
		rootObject.transform.Find("Girls/Popups/Date Overlay/Dialog/Progress Container/Again Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_18_0", "Go Again?");
		rootObject.transform.Find("Girls/Popups/Gift Buy Popup/Dialog/Pay Money Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_2", "Pay");
		rootObject.transform.Find("Girls/Popups/Gift Buy Popup/Dialog/Pay Diamonds Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_16_2", "Pay");
		rootObject.transform.Find("Jobs/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_1", "JOBS");
		rootObject.transform.Find("Jobs/Promotion Indicator/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_19_0", "Next Promotion:");
		rootObject.transform.Find("Jobs/Gilding FYI/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_20_0", "10= x5 output $");
		rootObject.transform.Find("Hobbies/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_2", "HOBBIES");
		rootObject.transform.Find("Hobbies/Hobby Cost Image/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_24_0", "Each hobby costs:");
		rootObject.transform.Find("Hobbies/Gilding FYI/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_24_1", "10 = x16 Hobby Speed");
		string text = string.Concat(new string[]
		{
			Translations.GetTranslation("everything_else_28_2", "Looking to up your game, playa? It's easy peasy, lemon squeezy! Just turn your achievements into expedience!"),
			"\n\n",
			Translations.GetTranslation("everything_else_28_3", "Reset your progress and add your built up achievement bonus to your Reset Boost. This will speed up all the progress bars in the game!"),
			"\n\n",
			Translations.GetTranslation("everything_else_28_4", "Reset with confidence - all of your stats, money and hearts will be toast. However your Achievements and Time Blocks will be untouched!"),
			"\n\n",
			Translations.GetTranslation("everything_else_28_5", "Get on it! Don't even trip!")
		});
		rootObject.transform.Find("Stats/Prestige Popup/Prestige Title").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_147", "Are you ready to Reset?");
		rootObject.transform.Find("Stats/Prestige Popup/Prestige Text").GetComponent<Text>().text = text;
		rootObject.transform.Find("Stats/Prestige Popup/OK Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_6", "OK");
		rootObject.transform.Find("Stats/Prestige Popup/Cancel Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_7", "CANCEL");
		rootObject.transform.Find("Stats/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_3", "STATS");
		rootObject.transform.Find("Stats/TITLE ITEMS").GetComponent<Text>().text = Translations.GetTranslation("everything_else_96_0", "Avatar Items");
		rootObject.transform.Find("Stats/Prestige Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_0", "RESET");
		rootObject.transform.Find("Stats/Reset/Reset Explanation 1").GetComponent<Text>().text = Translations.GetTranslation("everything_else_26_1", "The Reset Boost is a special bonus unlocked by 'soft' resetting your game. This means that you will start over from the beginning, but with all of your achievements, Time Blocks, diamonds and store purchases unaffected.");
		rootObject.transform.Find("Stats/Reset/Reset Explanation 2").GetComponent<Text>().text = Translations.GetTranslation("everything_else_26_2", "This bonus makes everything go faster! Jobs, hobbies and cool downs!");
		rootObject.transform.Find("Stats/Reset/Reset Now Text 1").GetComponent<Text>().text = Translations.GetTranslation("everything_else_27_1", "The Bonus Add is how much will be added to your Reset Boost if you reset right now. The bigger the better! Note that you get better bonuses for completing more challenging relationship levels.");
		rootObject.transform.Find("Stats/Reset/Reset Now Text 2").GetComponent<Text>().text = Translations.GetTranslation("everything_else_27_2", "It might feel a bit strange starting over from the beginning, but this bonus starts to add up quickly!");
		Translations.statsTransTime = Translations.GetTranslation("everything_else_25_0", "Time Blocks:");
		Translations.statsTransMoney = Translations.GetTranslation("everything_else_25_1", "Total Money Earned:");
		Translations.statsTransGirls = Translations.GetTranslation("everything_else_25_2", "Girls Met:");
		Translations.statsTransJobs = Translations.GetTranslation("everything_else_25_3", "Jobs Unlocked") + ": ";
		Translations.statsTransHobbies = Translations.GetTranslation("everything_else_25_4", "Hobbies Learned:");
		Translations.statsTransHearts = Translations.GetTranslation("everything_else_25_5", "Hearts Earned:");
		Translations.statsTransDates = Translations.GetTranslation("everything_else_25_6", "Dates Taken:");
		Translations.statsTransGifts = Translations.GetTranslation("everything_else_25_7", "Gifts Given:");
		Translations.statsTransResetBoost = Translations.GetTranslation("everything_else_26_0", "Reset Boost");
		Translations.statsTransBonusAdd = Translations.GetTranslation("everything_else_27_0", "Bonus Add");
		rootObject.transform.Find("Achievements/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_4", "ACHIEVEMENTS");
		rootObject.transform.Find("Store Revamp/Navigation/Diamonds Button/Text").GetComponent<Text>().text = Translations.GetTranslation("stats_19_6", "Diamonds");
		rootObject.transform.Find("Store Revamp/Navigation/Bundles Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_121_1", "Bundles");
		rootObject.transform.Find("Store Revamp/Navigation/Skip Reset Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_34_0", "Skip Reset");
		rootObject.transform.Find("Store Revamp/Navigation/Time Blocks Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_121_2", "Time Blocks");
		rootObject.transform.Find("Store Revamp/Navigation/Boosts Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_121_3", "Boosts");
		rootObject.transform.Find("Store Revamp/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_5", "STORE");
		rootObject.transform.Find("Store Revamp/Skip Reset Tab/Skip Reset/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_34_0", "Skip Reset");
		rootObject.transform.Find("Store Revamp/Skip Reset Tab/Skip Reset/Purchase/Unit").GetComponent<Text>().text = Translations.GetTranslation("everything_else_34_0", "Skip Reset");
		rootObject.transform.Find("Store Revamp/Boosts Tab/Time Skips/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_37_0", "Time Skips");
		rootObject.transform.Find("Store Revamp/Boosts Tab/Speed Boosts/Header").GetComponent<Text>().text = Translations.GetTranslation("everything_else_118_0", "Speeds up all progress bars!");
		rootObject.transform.Find("Store Revamp/Boosts Tab/Speed Boosts/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_36_0", "Speed Boosts");
		rootObject.transform.Find("Store Revamp/Boosts Tab/Time Skips/Header").GetComponent<Text>().text = Translations.GetTranslation("everything_else_119_0", "Jumps progress ahead!");
		rootObject.transform.Find("Store Revamp/Boosts Tab/Speed Boosts/Footer").GetComponent<Text>().text = Translations.GetTranslation("everything_else_116_0", "Max Boost of x8,192");
		rootObject.transform.Find("Store Revamp/Need Help/Help").GetComponent<Text>().text = Translations.GetTranslation("everything_else_121_4", "Need Help?");
		rootObject.transform.Find("Popups/Store Help/Dialog/Top Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_124", "Store Information");
		rootObject.transform.Find("Popups/Store Help/Dialog/Bottom Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_122", "Your store purchases are processed using a third party payment provider.  Sad Panda Studios does not have any control over the payment process, and in most cases cannot provide refunds.  However, if you do run into any issues during a purchase, please reach out to us at support@sadpandastudios.com and we will do our best to address your concerns.") + "\n\n" + Translations.GetTranslation("everything_else_123", "Purchases do persist across reset boosts, but most purchases do not persist across full resets.");
		rootObject.transform.Find("Store Revamp").GetComponent<Store2>().UpdateUI();
		rootObject.transform.Find("More/Background/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_1_6", "MORE");
		rootObject.transform.Find("More/Buttons/Memory Album Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_0", "Memory Album");
		rootObject.transform.Find("More/Buttons/Settings Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_1", "Settings");
		rootObject.transform.Find("More/Buttons/Login Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_109_0", "Login to Save");
		rootObject.transform.Find("More/Buttons/Save Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_3", "Save Game");
		rootObject.transform.Find("More/Buttons/Load Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_4", "Load Game");
		rootObject.transform.Find("More/Buttons/Redeem Coupon/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_5", "Enter Coupon");
		rootObject.transform.Find("More/Buttons/View Credits/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_6", "View Credits");
		rootObject.transform.Find("More/Buttons/Reset All/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_7", "Full Reset");
		rootObject.transform.Find("More/Policies/Privacy Policy/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_43_1", "Privacy Policy");
		rootObject.transform.Find("More/Policies/Terms of Use/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_43_3", "Terms of Use");
		rootObject.transform.Find("More/Twitter/Follow us").GetComponent<Text>().text = Translations.GetTranslation("everything_else_43_4", "Follow us:");
		rootObject.transform.Find("More/Version Info").GetComponent<Text>().text = Translations.GetTranslation("everything_else_44_0", "Thanks for playing our game!") + "  V.0." + GameState.CurrentVersion.ToString() + " Copyright 2022 Sad Panda Studios Ltd.";
		rootObject.transform.Find("More/Copyright Info").GetComponent<Text>().text = Translations.GetTranslation("everything_else_45_0", "Unauthorized use and/or duplication of this material without express and written permission from Sad Panda Studios Ltd. is strictly prohibited.");
		rootObject.transform.Find("More/Select Save Slot/Dialog/Select Slot").GetComponent<Text>().text = Translations.GetTranslation("everything_else_50_1", "Please select a slot:");
		rootObject.transform.Find("More/Select Save Slot/Dialog/Save Slot Autosave/Time Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_101_2", "WebGL (HTML5) Only");
		rootObject.transform.Find("More/Select Save Slot/Dialog/Save Slot Autosave/Slot Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_50_2", "Autosave");
		rootObject.transform.Find("More/Select Save Slot/Dialog/Save Slot 1/Slot Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_50_3", "Slot {0}"), "1");
		rootObject.transform.Find("More/Select Save Slot/Dialog/Save Slot 2/Slot Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_50_3", "Slot {0}"), "2");
		rootObject.transform.Find("More/Redeem Coupon Popup/Dialog/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_51_0", "Enter Your Coupon") + " (xxx-xxxx-xxx)";
		rootObject.transform.Find("More/Redeem Coupon Popup/Dialog/Accept Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_51_1", "Accept");
		string text2 = string.Concat(new string[]
		{
			Translations.GetTranslation("everything_else_48_0", "Do you really want to 'Hard Reset' your game and wipe all progress?"),
			"\n\n",
			Translations.GetTranslation("everything_else_48_1", "Warning: This is a full reset, which will wipe out your money, stats, achievements, time blocks, diamonds and all purchases."),
			"\n\n",
			Translations.GetTranslation("everything_else_48_2", "You will be starting the game anew! Are you sure?")
		});
		rootObject.transform.Find("More/Hard Reset Game Popup/Dialog Box/Text").GetComponent<Text>().text = text2;
		rootObject.transform.Find("More/Hard Reset Game Popup/Dialog Box/Yes Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_2", "Yes") + " - " + Translations.GetTranslation("everything_else_49_1", "Reset!");
		rootObject.transform.Find("More/Hard Reset Game Popup/Dialog Box/No Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_3", "No");
		rootObject.transform.Find("Popups/Credits/Content/Thanks Container/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_44_0", "Thanks for playing our game!");
		rootObject.transform.Find("Popups/Credits/Skip Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_10_1", "Skip");
		rootObject.transform.Find("Popups/Album Popup/Dialog/UI Container/Remember Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_111_0", "Reminisce");
		rootObject.transform.Find("Popups/Memory Album/Dialog/Title Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_0", "Memory Album");
		rootObject.transform.Find("Popups/Memory Album/Dialog/Next Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_9", "Next");
		rootObject.transform.Find("Popups/Memory Album/Dialog/Previous Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_8", "Previous");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Settings").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_1", "Settings").ToUpperInvariant();
		rootObject.transform.Find("Popups/Settings Popup/Dialog/SFX Slider/Label").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_0", "SFX Volume");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Music Slider/Label").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_1", "Music Volume");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Misc Background/Misc").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_3", "Miscellaneous");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Particle Checkbox/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_4", "Particles");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Intros Checkbox/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_5", "Skip Intros");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Popup Checkbox/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_6", "Skip Image Popups");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/NSFW Checkbox/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_7", "Uncensored Mode");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Cloud Checkbox/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_52_8", "Disable Backup to Cloud");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language").GetComponent<Text>().text = Translations.GetTranslation("everything_else_53_0", "Select a language:");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/English/Text").GetComponent<Text>().text = "English";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Chinese/Text").GetComponent<Text>().text = "中文";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/French/Text").GetComponent<Text>().text = "Français";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/German/Text").GetComponent<Text>().text = "Deutsche";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Italian/Text").GetComponent<Text>().text = "Italiano";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Japanese/Text").GetComponent<Text>().text = "日本語";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Polish/Text").GetComponent<Text>().text = "Polskie";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Portuguese/Text").GetComponent<Text>().text = "Português";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Portuguese (Brazil)/Text").GetComponent<Text>().text = "Português";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Russian/Text").GetComponent<Text>().text = "русский";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Spanish/Text").GetComponent<Text>().text = "Español";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Language Selection/Languages/Spanish (Mexico)/Text").GetComponent<Text>().text = "Español";
		rootObject.transform.Find("Popups/Settings Popup/Dialog/OK Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_6", "OK");
		rootObject.transform.Find("Popups/Settings Popup/Dialog/Cancel Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_7", "CANCEL");
		rootObject.transform.Find("Popups/Settings Popup/NSFW Information Dialog/Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_95_0", "What’s Uncensored Mode?");
		rootObject.transform.Find("Popups/Settings Popup/NSFW Information Dialog/Dialog/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_95_1", "Uncensored Mode is a DLC upgrade to Crush Crush that adds lots of 18+ hentai content to the game. Naturally, it’s for adults only!") + "\n\n" + Translations.GetTranslation("everything_else_95_2", "If you’re interested in purchasing this DLC, head to sadpandastudios.com for information. Proceeds from DLC sales go to making more DLC content!");
		rootObject.transform.Find("Popups/Settings Popup/NSFW Information Dialog/Dialog/OK Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_6", "OK");
		rootObject.transform.Find("Popups/Diamonds Popup/Dialog/Bottom Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_30_1", "Would you like to buy more?");
		rootObject.transform.Find("Popups/No Time Popup/Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_31_0", "You don't have any free time!");
		rootObject.transform.Find("Popups/No Time Popup/Dialog/Pay Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_31_2", "Go To Store");
		rootObject.transform.Find("Popups/Confirm Purchase/Dialog/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_40_0", "Confirm Diamond Purchase:");
		rootObject.transform.Find("Popups/View Saved Game/Dialog Box/Information Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_82_2", "Would you like to load your game from save file?") + "\n" + Translations.GetTranslation("everything_else_82_3", "Here is where you were at in your last save:");
		rootObject.transform.Find("Popups/View Saved Game/Dialog Box/Load Button/yes txt").GetComponent<Text>().text = Translations.GetTranslation("everything_else_82_1", "Load");
		rootObject.transform.Find("Popups/View Saved Game/Dialog Box/Close Button/no txt").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_3", "No");
		rootObject.transform.Find("Popups/Close Game Dialog/Dialog/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_0", "Are you sure that you would like to quit?") + "\n" + Translations.GetTranslation("everything_else_83_1", "Your game will be saved automatically");
		rootObject.transform.Find("Popups/Close Game Dialog/Dialog/Yes Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_2", "Yes");
		rootObject.transform.Find("Popups/Close Game Dialog/Dialog/No Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_3", "No");
		rootObject.transform.Find("Popups/Diamonds Popup/Dialog/Bottom Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_30_1", "Would you like to buy more?");
		rootObject.transform.Find("Popups/Diamonds Popup/Dialog/Accept Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_30_3", "Okay");
		rootObject.transform.Find("Popups/Diamonds Popup/Dialog/Deny Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_83_3", "No");
		rootObject.transform.Find("Popups/Affection Milestone/Accept Button/Accept Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_10_2", "Thanks");
		rootObject.transform.Find("Popups/Final Outro/Prestige Popup/Dialog/Prestige Title").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_147", "Are you ready to Reset?");
		rootObject.transform.Find("Popups/Final Outro/Prestige Popup/Dialog/Prestige Text").GetComponent<Text>().text = text;
		rootObject.transform.Find("Popups/Final Outro/Prestige Popup/Dialog/OK Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_6", "OK");
		rootObject.transform.Find("Popups/Final Outro/Prestige Popup/Dialog/Cancel Button/Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_28_7", "CANCEL");
		rootObject.transform.Find("Popups/Final Outro/Text").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_144_0", "What will you do?");
		rootObject.transform.Find("Popups/Final Outro/Left Button/Text").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_144_1", "Sacrifice Yourself");
		rootObject.transform.Find("Popups/Final Outro/Right Button/Text").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_144_2", "Wave Farewell to Q-Pernikiss");
		rootObject.transform.Find("Popups/Final Outro/Left Explanation").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_145", "Sacrifice yourself to save the world! This is the true ending of Crush Crush, and will soft-reset your game once the credits roll. Pick this path to unlock the final ending!");
		rootObject.transform.Find("Popups/Final Outro/Right Explanation").GetComponent<Text>().text = Translations.GetTranslation("qpiddy_146", "Let QPernikiss make the ultimate sacrifice! This lets you keep playing the game as normal - super handy if you're still working on your achievements!");
		rootObject.transform.Find("Top UI/Task Blocker/Task Button/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_25_0", "Event!");
	}

	// Token: 0x04000847 RID: 2119
	public static Voiceover voiceover;

	// Token: 0x04000848 RID: 2120
	private static Dictionary<string, string> translations;

	// Token: 0x04000849 RID: 2121
	public static Translations.Language PreferredLanguage = Translations.Language.English;

	// Token: 0x0400084A RID: 2122
	public static ReactiveProperty<int> CurrentLanguage = new ReactiveProperty<int>(0);

	// Token: 0x0400084B RID: 2123
	private static Translations.Language LoadingLanguage = Translations.Language.English;

	// Token: 0x0400084C RID: 2124
	public static string statsTransTime = "Time Blocks:";

	// Token: 0x0400084D RID: 2125
	public static string statsTransMoney = "Total Money Earned:";

	// Token: 0x0400084E RID: 2126
	public static string statsTransGirls = "Girls Met:";

	// Token: 0x0400084F RID: 2127
	public static string statsTransJobs = "Jobs Unlocked:";

	// Token: 0x04000850 RID: 2128
	public static string statsTransHobbies = "Hobbies Learned:";

	// Token: 0x04000851 RID: 2129
	public static string statsTransHearts = "Hearts Earned:";

	// Token: 0x04000852 RID: 2130
	public static string statsTransDates = "Dates Taken:";

	// Token: 0x04000853 RID: 2131
	public static string statsTransGifts = "Gifts Given:";

	// Token: 0x04000854 RID: 2132
	public static string statsTransResetBoost = "Reset Boost";

	// Token: 0x04000855 RID: 2133
	public static string statsTransBonusAdd = "Bonus Add:";

	// Token: 0x04000856 RID: 2134
	public static bool isHardReset = false;

	// Token: 0x0200012C RID: 300
	public enum Language
	{
		// Token: 0x0400085A RID: 2138
		English,
		// Token: 0x0400085B RID: 2139
		Chinese,
		// Token: 0x0400085C RID: 2140
		French,
		// Token: 0x0400085D RID: 2141
		German,
		// Token: 0x0400085E RID: 2142
		Italian,
		// Token: 0x0400085F RID: 2143
		Polish,
		// Token: 0x04000860 RID: 2144
		Portuguese,
		// Token: 0x04000861 RID: 2145
		BrazilianPortuguese,
		// Token: 0x04000862 RID: 2146
		Russian,
		// Token: 0x04000863 RID: 2147
		Spanish,
		// Token: 0x04000864 RID: 2148
		Japanese,
		// Token: 0x04000865 RID: 2149
		Hindi,
		// Token: 0x04000866 RID: 2150
		MexicanSpanish
	}

	// Token: 0x0200012D RID: 301
	public struct Dialogue
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x0004A8B0 File Offset: 0x00048AB0
		public Dialogue(string id, string english)
		{
			this.Id = id;
			this.English = english;
			this.VoiceId = this.Id;
			this.Text = string.Empty;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0004A8D8 File Offset: 0x00048AD8
		public Dialogue(string id, string english, string voiceid)
		{
			this.Id = id;
			this.English = english;
			this.VoiceId = voiceid;
			this.Text = string.Empty;
		}

		// Token: 0x04000867 RID: 2151
		public string Id;

		// Token: 0x04000868 RID: 2152
		public string English;

		// Token: 0x04000869 RID: 2153
		public string VoiceId;

		// Token: 0x0400086A RID: 2154
		public string Text;
	}
}
