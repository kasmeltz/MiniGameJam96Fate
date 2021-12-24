namespace HairyNerdStudios.GameJams.MiniGameJam96.Unity.MathExtensions
{
    using System;

    public static class MathExtensions
    {
        public static bool CloseTo(this float value, float other, float equalsRange) 
        {
            var diff = Math.Abs(value - other);
            return diff <= equalsRange;
        }

        public static float Quantize(this float value, float quantizationFactor)
        {
            return (float)Math.Round(value / quantizationFactor) * quantizationFactor;
        }
    }
}
