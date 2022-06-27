using System;

namespace Spine
{
	// Token: 0x020001B6 RID: 438
	public static class MathUtils
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x00060A18 File Offset: 0x0005EC18
		public static float Sin(float radians)
		{
			return (float)Math.Sin((double)radians);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00060A24 File Offset: 0x0005EC24
		public static float Cos(float radians)
		{
			return (float)Math.Cos((double)radians);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00060A30 File Offset: 0x0005EC30
		public static float SinDeg(float degrees)
		{
			return (float)Math.Sin((double)(degrees * 0.017453292f));
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00060A40 File Offset: 0x0005EC40
		public static float CosDeg(float degrees)
		{
			return (float)Math.Cos((double)(degrees * 0.017453292f));
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00060A50 File Offset: 0x0005EC50
		public static float Atan2(float y, float x)
		{
			return (float)Math.Atan2((double)y, (double)x);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00060A5C File Offset: 0x0005EC5C
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00060A74 File Offset: 0x0005EC74
		public static float RandomTriangle(float min, float max)
		{
			return MathUtils.RandomTriangle(min, max, (min + max) * 0.5f);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00060A88 File Offset: 0x0005EC88
		public static float RandomTriangle(float min, float max, float mode)
		{
			float num = (float)MathUtils.random.NextDouble();
			float num2 = max - min;
			if (num <= (mode - min) / num2)
			{
				return min + (float)Math.Sqrt((double)(num * num2 * (mode - min)));
			}
			return max - (float)Math.Sqrt((double)((1f - num) * num2 * (max - mode)));
		}

		// Token: 0x04000BCD RID: 3021
		public const float PI = 3.1415927f;

		// Token: 0x04000BCE RID: 3022
		public const float PI2 = 6.2831855f;

		// Token: 0x04000BCF RID: 3023
		public const float RadDeg = 57.295776f;

		// Token: 0x04000BD0 RID: 3024
		public const float DegRad = 0.017453292f;

		// Token: 0x04000BD1 RID: 3025
		private static Random random = new Random();
	}
}
