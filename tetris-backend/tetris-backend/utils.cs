using System;
using System.Collections.Generic;

namespace tetris_backend {

public struct Vec2i {
    public int x, y;
}

public class ReverseComparer<T> : IComparer<T> {
    public int Compare(T left, T right) {
        return Comparer<T>.Default.Compare(right, left);
    }
}
public static class Utils {
    public static IEnumerable<(int, int)> Range2D(int width, int height) {
        for (var x = 0; x < width; ++x) {
            for (var y = 0; y < height; ++y) {
                yield return (x, y);
            }
        }
    }
    public static IEnumerable<(int, int)> Range2D(int width) {
        return Range2D(width, width);
    }
}

public static class CollectionExtensions {
    public static IEnumerable<(int, int)> Indices<T>(this T[,] array) {
        return Utils.Range2D(array.GetLength(0), array.GetLength(1));
    }
}

} // namespace tetris_backend