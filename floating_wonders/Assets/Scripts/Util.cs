using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

/// <summary>
/// Utility class
/// I swear it's all my stuff
/// </summary>
public static class Util
{
    public static T Choose<T>(params T[] args)
    {
        if (args.Length > 0)
        {
            int choice = UnityEngine.Random.Range(0, args.Length);
            return args[choice];
        }
        else
        {
            return default(T);
        }
    }

    public static int Mean(params int[] numbers)
    {
        if (numbers == null)
        {
            return 0;
        }
        if (numbers.Length > 0)
        {
            int sum = 0;
            foreach (int i in numbers)
            {
                sum += i;
            }
            return sum / numbers.Length;
        }
        else
        {
            return 0;
        }
    }

    public static float Mean(params float[] numbers)
    {
        if (numbers == null)
        {
            return 0;
        }

        if (numbers.Length > 0)
        {
            float sum = 0;
            foreach (float i in numbers)
            {
                sum += i;
            }
            return sum / numbers.Length;
        }
        else
        {
            return 0;
        }
    }

    public static T Max<T>(T a, T b) where T : IComparable
    {
        if (a == null)
        {
            if (b == null)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        if (b == null)
        {
            return a;
        }
        return a.CompareTo(b) > 0 ? a : b;
    }

    public static T Min<T>(T a, T b) where T : IComparable
    {
        if (a == null)
        {
            if (b == null)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        if (b == null)
        {
            return a;
        }
        return a.CompareTo(b) < 0 ? a : b;
    }

    public static T Clamp<T>(T value, T x1, T x2) where T : IComparable
    {
        if (value == null)
        {
            return default(T);
        }
        return Max(Min(value, Max(x1, x2)), Min(x1, x2));
    }

    public static bool ValueInRange<T>(T value, T x1, T x2) where T : IComparable
    {
        if (value == null || x1 == null || x2 == null)
        {
            return false;
        }
        return (value.CompareTo(Min(x1, x2)) >= 0 && value.CompareTo(Max(x1, x2)) <= 0);
    }

    public static bool RangeOverlaps<T>(T x1, T x2, T y1, T y2) where T : IComparable
    {
        if (x1 == null || x2 == null || y1 == null || y2 == null)
        {
            return false;
        }

        // would have declared these at the initial point of the method, but pmd
        // complains
        T max1 = Max(x1, x2);
        T min1 = Min(x1, x2);
        T max2 = Max(y1, y2);
        T min2 = Min(y1, y2);

        return (max1.CompareTo(min2) > 0 && min1.CompareTo(max2) < 0);
    }

    public static float PointDistance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }

    public static float PointDistance(Vector2 a, Vector2 b)
    {
        return PointDistance(a.x, a.y, b.x, b.y);
    }

    public static float RadianToDegree(float angle)
    {
        return angle * (180.0f / (float)Math.PI);
    }

    public static float DegreeToRadian(float angle)
    {
        return (float)Math.PI * angle / 180.0f;
    }

    public static float PointDirection(float x1, float y1, float x2, float y2)
    {
        float angle = RadianToDegree(Mathf.Atan2(y2 - y1, x2 - x1));
        if (angle <= 0)
        {
            angle += 360;
        }
        return 360 - angle;
    }

    public static bool PointInCircle(float pointX, float pointY, float circleX, float circleY,
             float radius)
    {
        return (PointDistance(pointX, pointY, circleX, circleY) <= radius);
    }

    public static bool PointInRectangle(float pointX, float pointY, float x1, float y1, float x2,
             float y2)
    {
        return (ValueInRange(pointX, x1, x2) && ValueInRange(pointY, y1, y2));
    }

    public static bool RectangleOverlaps(float ax1, float ay1, float ax2, float ay2, float bx1,
             float by1, float bx2, float by2)
    {
        return (RangeOverlaps(ax1, ax2, bx1, bx2) && RangeOverlaps(ay1, ay2, by1, by2));
    }

    public static float LengthDirX(float len, float dir)
    {
        float ang = DegreeToRadian(-dir);
        return Mathf.Cos(ang) * len;
    }

    public static float LengthDirY(float len, float dir)
    {
        float ang = DegreeToRadian(-dir);
        return Mathf.Sin(ang) * len;
    }

    public static bool RectangleOverlapsCircle(float x1, float y1, float x2, float y2,
             float circleX, float circleY, float radius)
    {
        // calculate where the circle is in relation to the rectangle
        int pos = -1;
        bool colliding = false;

        // just to dont create an enum for this method only
        int topleft = 0;
        int topcent = 1;
        int topright = 2;
        int midleft = 3;
        int midcent = 4;
        int midright = 5;
        int botleft = 6;
        int botcent = 7;
        int botright = 8;

        if (circleX < x1)
        {
            if (circleY < y1)
            {
                pos = topleft;
            }
            else
            {
                if (circleY > y2)
                {
                    pos = botleft;
                }
                else
                {
                    pos = midleft;
                }
            }
        }
        else
        {
            if (circleX > x2)
            {
                if (circleY < y1)
                {
                    pos = topright;
                }
                else
                {
                    if (circleY > y2)
                    {
                        pos = botright;
                    }
                    else
                    {
                        pos = midright;
                    }
                }
            }
            else
            {
                if (circleY < y1)
                {
                    pos = topcent;
                }
                else
                {
                    if (circleY > y2)
                    {
                        pos = botcent;
                    }
                    else
                    {
                        pos = midcent;
                    }
                }
            }
        }

        // based on where it is, calculate if they intersect
        if (pos == topcent || pos == midleft || pos == midcent || pos == midright || pos == botcent)
        {
            if ((circleX > x1 - radius && circleX < x2 + radius) && (circleY > y1 - radius && circleY < y2 + radius))
            {
                colliding = true;
            }
        }
        else
        {
            if (pos == topleft && PointDistance(circleX, circleY, x1, y1) < radius)
            {
                colliding = true;
            }
            if (pos == topright && PointDistance(circleX, circleY, x2, y1) < radius)
            {
                colliding = true;
            }
            if (pos == botleft && PointDistance(circleX, circleY, x1, y2) < radius)
            {
                colliding = true;
            }
            if (pos == botright && PointDistance(circleX, circleY, x2, y2) < radius)
            {
                colliding = true;
            }
        }
        return colliding;
    }


    public static float Random(float num)
    {
        return UnityEngine.Random.Range(0, num);
    }

    public static float RandomRange(float num1, float num2)
    {
        float x1 = Min(num1, num2);
        float x2 = Max(num1, num2);
        float range = x2 - x1;
        return Random(range) + x1;
    }

    public static float WrapToModulus(float value, int modulus)
    {
        if (modulus == 0)
        {
            return 0;
        }
        else
        {
            float discard = value - (int)value;
            int n = modulus;
            int r = (int)value % n;
            if (r < 0)
            {
                r += n;
            }
            return r + discard;
        }
    }

    public static float AngleValue(float angle)
    {
        return WrapToModulus(angle, 360);
    }

    public static float AngleDifference(float angle1, float angle2)
    {
        float a1 = AngleValue(angle1);
        float a2 = AngleValue(angle2);
        float angle = (Mathf.Abs(a1 - a2)) % 360;

        if (angle > 180)
        {
            angle = 360 - angle;
        }
        return angle;
    }

    public static float AngleShortestRotationVerse(float angle1, float angle2)
    {
        float a1 = AngleValue(angle1);
        float a2 = AngleValue(angle2);
        if (a1 < a2)
        {
            if (Mathf.Abs(a1 - a2) < 180)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (Mathf.Abs(a1 - a2) < 180)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    public static float AngleRotate(float currentAngle, float destinationAngle, float rotSpeed)
    {
        float ca = AngleValue(currentAngle);
        float da = AngleValue(destinationAngle);
        float rs = Clamp(rotSpeed, 0f, 180f);
        float rotVerse = 0;
        float eps = 1e-5f;
        float minRs = 0.0001f;

        if (Mathf.Abs(ca - da) > eps)
        {
            rs = Clamp(rs, minRs, AngleDifference(ca, da));
            rotVerse = AngleShortestRotationVerse(ca, da);
            return AngleValue(ca + rs * rotVerse);
        }
        else
        {
            return ca;
        }
    }

    //ROTTO in un sistema di rotazione basato su frames al secondo non fissi.
    public static float AngleRotateHyperbolic(float currentAngle, float destinationAngle, float slownessFactor)
    {
        return AngleRotateHyperbolic(currentAngle, destinationAngle, slownessFactor, 0, 180);
    }

    public static float AngleRotateHyperbolic(float currentAngle, float destinationAngle, float slownessFactor,
             float minSpeed, float maxSpeed)
    {
        float ca = AngleValue(currentAngle);
        float da = AngleValue(destinationAngle);
        float sf = Max(slownessFactor, 1f);
        float minS = minSpeed;
        float maxS = maxSpeed;
        return AngleRotate(ca, da, Clamp(AngleDifference(ca, da) / sf, minS, maxS));
    }

    public static float Power(float baseNum, float exponent)
    {
        return Mathf.Pow(Mathf.Abs(baseNum), exponent);
    }

    /*public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }*/

    /// <summary>
    /// Returns the same color but with flat opaque alpha (1.0f)
    /// </summary>
    /// <param name="c">color to flatten</param>
    /// <returns>flattened color</returns>
    public static Color FlatColor(this Color c)
    {
        return new Color(c.r, c.g, c.b, 1f);
    }

    /// <summary>
    /// Creates a simple opaque gradient
    /// </summary>
    /// <param name="initialColor">left color</param>
    /// <param name="Color">right color</param>
    /// <returns>the generated gradient</returns>
    public static Gradient MakeSimpleGradient(Color initialColor, Color finalColor)
    {
        var g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(initialColor,0f),
                new GradientColorKey(finalColor,1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f,0f),
                new GradientAlphaKey(1f,1f)
            });
        return g;
    }
    /// <summary>
    /// Converts a latitude + longitude + radius spherical coordinate into a vec3 coordinate.
    /// </summary>
    /// <param name="lng">longitude (angular distance from Greenwitch, in the range 0 - 360)</param>
    /// <param name="lat">latitude (angular distance from the Equator, in the range -90 - 90)</param>
    /// <param name="radius">radius of the sphere</param>
    /// <returns>a Vector 3 describing the position of the latitude and longitude on the surface of the sphere</returns>
    public static Vector3 lonLatToVector3(float lng, float lat, float radius)
    {
        lat = Mathf.Deg2Rad * lat;
        lng = Mathf.Deg2Rad * lng;
        //flips the Y axis
        lat = Mathf.PI / 2 - lat;

        //distribute to sphere
        return new Vector3(
                    Mathf.Sin(lat) * Mathf.Sin(lng) * radius,
                    Mathf.Cos(lat) * radius,
                    Mathf.Sin(lat) * Mathf.Cos(lng) * radius
        );

    }

    /// <summary>
    /// Gets the specified component searching in all childs with a certain tag.
    /// </summary>
    /// <typeparam name="T">Type of the component to look for</typeparam>
    /// <param name="parent">parent GameObject</param>
    /// <param name="tag">tag to filter childern</param>
    /// <returns>the first matching component</returns>
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the child GameObject with the specified name
    /// </summary>
    /// <param name="parent">parent GameObject</param>
    /// <param name="childName">name of the child to look for</param>
    /// <returns>the first child found with the given name</returns>
    public static GameObject GetChild(this GameObject parent, string childName)
    {
        var t = parent.transform.GetComponentsInChildren<Transform>();
        foreach (Transform tr in t)
        {
            if (tr.gameObject.name == childName)
            {
                return tr.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the gray level of the given color, in the range 0.0f - 1.0f
    /// </summary>
    /// <param name="c">Color to check</param>
    /// <returns>level of gray in the range 0.0f - 1.0f</returns>
    public static float GetGray(this Color c)
    {
        return c.r * 0.2989f + c.g * 0.5870f + c.b * 0.1140f;
    }

    /// <summary>
    /// Blends two given color according to the rules of the normal blend mode, and returns the resulting color
    /// </summary>
    /// <param name="dest">color below</param>
    /// <param name="src">color above</param>
    /// <returns>the blended color</returns>
    public static Color NormalBlend(Color dest, Color src)
    {
        Color D;
        D = src * src.a + dest * dest.a * (1f - src.a);
        D.a = src.a + dest.a * (1 - src.a);
        return D;
    }

    /// <summary>
    /// Blends two given color according to the rules of the additive blend mode, and returns the resulting color
    /// </summary>
    /// <param name="dest">color below</param>
    /// <param name="src">color above</param>
    /// <returns>the blended color</returns>
    public static Color AdditiveBlend(Color dest, Color src)
    {
        Color res;
        res.r = dest.r + (src.r * src.a);
        res.g = dest.g + (src.g * src.a);
        res.b = dest.b + (src.b * src.a);
        res.a = dest.a + (src.a * src.a);
        return res;
    }

    /// <summary>
    /// Colorizes a pixel according to the rules of multiplicative blend
    /// </summary>
    /// <param name="pixel">original pixel</param>
    /// <param name="applyColor">target color</param>
    /// <returns>the colorized pixel</returns>
    public static Color ColorizePixel(Color pixel, Color applyColor)
    {
        //Color c = pixel * applyColor;
        //c.a *= applyColor.a;
        return pixel * applyColor;
    }

    public class ThreadData
    {
        public int start;
        public int end;
        public ThreadData(int s, int e)
        {
            start = s;
            end = e;
        }
    }

    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;

    public static Texture2D ResizeBilinear(this Texture2D tex, int newWidth, int newHeight, bool apply)
    {
        Texture2D t = new Texture2D(Mathf.Abs(newWidth), Mathf.Abs(newHeight));
        int w, h;
        w = t.width;
        h = t.height;
        Color[] newColors = new Color[w * h];

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                newColors[x + (y * w)] = tex.GetPixelBilinear((x + 0.0f) / newWidth, (y + 0.0f) / newHeight);
            }
        }
        t.SetPixels(newColors);
        if (apply)
        {
            t.Apply();
        }
        return t;
    }

    /// <summary>
    /// Scales the given texture to a new set of dimensions with the nearest point method
    /// </summary>
    /// <param name="tex">original Texture</param>
    /// <param name="newWidth">new width of the texture</param>
    /// <param name="newHeight">new height of the texture</param>
    public static void Point(Texture2D tex, int newWidth, int newHeight)
    {
        ThreadedScale(tex, newWidth, newHeight, false);
    }

    /// <summary>
    /// Scales the given texture to a new set of dimensions with the bilinear filtering method
    /// </summary>
    /// <param name="tex">original Texture</param>
    /// <param name="newWidth">new width of the texture</param>
    /// <param name="newHeight">new height of the texture</param>
    public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
    {
        ThreadedScale(tex, newWidth, newHeight, true);
    }

    /// <summary>
    /// Scales the given texture to a new set of dimensions
    /// </summary>
    /// <param name="tex">original Texture</param>
    /// <param name="newWidth">new width of the texture</param>
    /// <param name="newHeight">new height of the texture</param>
    /// <param name="useBilinear">wether to use bilinear filtering or just the nearest point sampling method</param>
    private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
    {
        texColors = tex.GetPixels();
        newColors = new Color[newWidth * newHeight];
        if (useBilinear)
        {
            ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
            ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
        }
        else
        {
            ratioX = ((float)tex.width) / newWidth;
            ratioY = ((float)tex.height) / newHeight;
        }
        w = tex.width;
        w2 = newWidth;
        var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
        var slice = newHeight / cores;

        finishCount = 0;
        if (mutex == null)
        {
            mutex = new Mutex(false);
        }
        if (cores > 1)
        {
            int i = 0;
            ThreadData threadData;
            for (i = 0; i < cores - 1; i++)
            {
                threadData = new ThreadData(slice * i, slice * (i + 1));
                ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                Thread thread = new Thread(ts);
                thread.Start(threadData);
            }
            threadData = new ThreadData(slice * i, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
            while (finishCount < cores)
            {
                Thread.Sleep(1);
            }
        }
        else
        {
            ThreadData threadData = new ThreadData(0, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
        }

        tex.Resize(newWidth, newHeight);
        tex.SetPixels(newColors);
        tex.Apply();

        texColors = null;
        newColors = null;
    }

    public static void BilinearScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor + 1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++)
            {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX - xFloor;
                newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    public static void PointScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            var thisY = (int)(ratioY * y) * w;
            var yw = y * w2;
            for (var x = 0; x < w2; x++)
            {
                newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    /// <summary>
    /// Interpolates two colors without limiting the color spectrum of the result
    /// </summary>
    /// <param name="c1">first color to interpolate</param>
    /// <param name="c2">second color to interpolate</param>
    /// <param name="value">interpolation value</param>
    /// <returns>the interpolated color</returns>
    public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }



    public static void AnchorToCorners(this RectTransform transform)
    {
        if (transform == null)
            throw new System.ArgumentNullException("transform");

        if (transform.parent == null)
            return;

        var parent = transform.parent.GetComponent<RectTransform>();

        Vector2 newAnchorsMin = new Vector2(transform.anchorMin.x + transform.offsetMin.x / parent.rect.width,
                          transform.anchorMin.y + transform.offsetMin.y / parent.rect.height);

        Vector2 newAnchorsMax = new Vector2(transform.anchorMax.x + transform.offsetMax.x / parent.rect.width,
                          transform.anchorMax.y + transform.offsetMax.y / parent.rect.height);

        transform.anchorMin = newAnchorsMin;
        transform.anchorMax = newAnchorsMax;
        transform.offsetMin = transform.offsetMax = new Vector2(0, 0);
    }

    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }

    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }

    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }

    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    public static void SetBottomLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetTopLeftPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetBottomRightPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 ActualSize(this RectTransform trans)
    {
        //var v = new Vector3[4];
        //trans.GetWorldCorners(v);
        //method one
        //return new Vector2(v[3].x - v[0].x, v[1].y - v[0].y);

        //method two
        return RectTransformUtility.PixelAdjustRect(trans, trans.GetComponentInParent<Canvas>()).size;
    }

    public static void RGBToHSV(Color color, out float h, out float s, out float v)
    {
        var cmin = Mathf.Min(color.r, color.g, color.b);
        var cmax = Mathf.Max(color.r, color.g, color.b);
        var d = cmax - cmin;
        if (d == 0)
        {
            h = 0;
        }
        else if (cmax == color.r)
        {
            h = Mathf.Repeat((color.g - color.b) / d, 6);
        }
        else if (cmax == color.g)
        {
            h = (color.b - color.r) / d + 2;
        }
        else
        {
            h = (color.r - color.g) / d + 4;
        }
        s = cmax == 0 ? 0 : d / cmax;
        v = cmax;
    }

    public static float GetAngle(this Vector2 vector)
    {
        return PointDirection(0, 0, vector.x, vector.y);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }

    public static RaycastHit2D SelfCast(this BoxCollider2D collider, LayerMask layerMask)
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, collider.gameObject.transform.rotation.z, Vector2.one, 0f, layerMask);
    }
}
