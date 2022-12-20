#pragma once

namespace LoveLevels
{
	enum LoveLevel
	{
		Adversary,
		Nuisance,
		Frenemy,
		Acquaintance,
		Friendzoned,
		Awkward_Besties,
		Crush,
		Sweetheart,
		Girlfriend,
		Lover
	};
}

namespace RequirementTypes
{
	enum RequirementType
	{
		Skill,
		Money,
		Job,
		Hobby,
		Time,
		Affection,
		Gift,
		Date,
		Heart,
		Achievement,
		Diamond,
		Prestige,
		PrestigeConsume,
		Album,
		TotalDates,
		TotalGifts,
		MoneyConsume,
		DiamondConsume,
		JobGild,
		HobbyGild,
		AllJobs,
		AllHobbies,
		GirlsAtLover,
		Unknown
	};
}

namespace Skills
{
	enum Skill
	{
		Suave,
		Funny,
		Buff,
		TechSavvy,
		Tenderness,
		Motivation,
		Wisdom,
		Badass,
		Smart,
		Angst,
		Mysterious,
		Lucky
	};
}

namespace Jobypes
{
	enum JobType
	{
		None = 0,
		Burger = 1,
		Restaurant = 2,
		Cleaning = 4,
		Lifeguard = 8,
		Art = 16,
		Computers = 32,
		Zoo = 64,
		Hunting = 128,
		Casino = 256,
		Sports = 512,
		Legal = 1024,
		Movies = 2048,
		Space = 4096,
		Slaying = 8192,
		Love = 16384,
		Wizard = 32768,
		Digger = 65536,
		Planter = 131072
	};
}

namespace OutfitTypes
{
	enum OutfitType
	{
		None = 0,
		Monster = 1,
		Animated = 2,
		DeluxeWedding = 4,
		Christmas = 262144,
		SchoolUniform = 524288,
		BathingSuit = 1048576,
		Unique = 2097152,
		DiamondRing = 4194304,
		Lingerie = 536870912,
		Nude = 1073741824,
		All = 1616642050
	};
}

namespace GiftTypes
{
	enum GiftType
	{
		None = 0,
		Shell = 1,
		Rose = 2,
		HandLotion = 4,
		Donut = 8,
		FruitBasket = 16,
		Chocolates = 32,
		Book = 64,
		Earrings = 128,
		Drink = 256,
		Flowers = 512,
		Cake = 1024,
		PlushyToy = 2048,
		TeaSet = 4096,
		Shoes = 8192,
		CutePuppy = 16384,
		Necklace = 32768,
		DesignerBag = 65536,
		NewCar = 131072,
		Christmas = 262144,
		SchoolUniform = 524288,
		BathingSuit = 1048576,
		Unique = 2097152,
		DiamondRing = 4194304,
		USB = 8388608,
		Potion = 16777216,
		MagicCandles = 33554432,
		EnchantedScarf = 67108864,
		BewitchedJam = 134217728,
		MysticSlippers = 268435456,
		Lingerie = 536870912,
		Nude = 1073741824,
		Apple = 16,
		CD = 2,
		Java = 8,
		Crypto = 64,
		Magnet = 128,
		Pizza = 256,
		RAM = 16384,
		Potato = 2,
		BabyChick = 16,
		Telescope = 256,
		HerbalTea = 4096,
		Soup = 16384,
		Lozenge = 131072,
		Medicine = 8388608
	};
}

namespace DateTypes
{
	enum DateType
	{
		MoonlightStroll = 1,
		CoffeeShop = 2,
		Sightseeing = 4,
		MovieTheater = 8,
		Beach = 16
	};
}
