using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisBackend
{
	public struct Vec2i
	{
		public int X, Y;

		public bool GetIsWithin(Vec2i bounds) =>
			X >= 0 && X < bounds.X && Y >= 0 && Y < bounds.Y;

		public Vec2i(int x, int y) => (X, Y) = (x, y);
	}

	public class ReverseComparer<T> : IComparer<T>
	{
		public int Compare(T left, T right)
		{
			return Comparer<T>.Default.Compare(right, left);
		}
	}
	public static class Utils
	{
		public static IEnumerable<Vec2i> Range2D(Vec2i size)
		{
			for (var y = 0; y < size.Y; ++y)
			{
				for (var x = 0; x < size.X; ++x)
				{
					yield return new Vec2i(x, y);
				}
			}
		}
		public static IEnumerable<Vec2i> Range2D(int width) => Range2D(new Vec2i(width, width));

		public static T[][] CreateRectangularArray<T>(Vec2i size) =>
			Enumerable.Range(0, size.Y).Select(y => new T[size.X]).ToArray();
	}

	public static class CollectionExtensions
	{
		public static int GetWidth<T>(this T[][] array) =>
			array.Length != 0 ? array[0].Length : 0;

		public static Vec2i GetSize<T>(this T[][] array) =>
			new Vec2i(array.GetWidth(), array.Length);

		public static IEnumerable<Vec2i> Indices2D<T>(this T[][] array) =>
			Utils.Range2D(array.GetSize());

		public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> enumerable) =>
			enumerable.Select((value, index) => (index, value));
	}
}
