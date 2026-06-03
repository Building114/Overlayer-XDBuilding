using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Overlayer.Unity;

public static class ImageLoaderCompat
{
    private static bool initialized;

    private static MethodInfo loadImageWithMarkNonReadable;
    private static MethodInfo loadImageSimple;
    private static MethodInfo encodeToPNG;

    public static bool LoadImage(Texture2D texture, byte[] bytes, bool markNonReadable = false)
    {
        if (texture == null || bytes == null || bytes.Length == 0)
        {
            return false;
        }

        EnsureInitialized();

        try
        {
            if (loadImageWithMarkNonReadable != null)
            {
                object result = loadImageWithMarkNonReadable.Invoke(null, new object[] {
                    texture,
                    bytes,
                    markNonReadable
                });

                return result is bool ok ? ok : true;
            }

            if (loadImageSimple != null)
            {
                object result = loadImageSimple.Invoke(null, new object[] {
                    texture,
                    bytes
                });

                return result is bool ok ? ok : true;
            }
        }
        catch
        {
            // 图片加载失败不能拖死整个模组。
        }

        return false;
    }

    public static byte[] EncodeToPNG(Texture2D texture)
    {
        if (texture == null)
        {
            return Array.Empty<byte>();
        }

        EnsureInitialized();

        try
        {
            if (encodeToPNG != null)
            {
                object result = encodeToPNG.Invoke(null, new object[] { texture });

                return result as byte[] ?? Array.Empty<byte>();
            }
        }
        catch
        {
            // 导出失败时返回空数组，避免编译后运行直接炸。
        }

        return Array.Empty<byte>();
    }

    private static void EnsureInitialized()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        Type imageConversionType = FindImageConversionType();

        if (imageConversionType == null)
        {
            return;
        }

        loadImageWithMarkNonReadable = imageConversionType.GetMethod(
            "LoadImage",
            BindingFlags.Public | BindingFlags.Static,
            null,
            new[] { typeof(Texture2D), typeof(byte[]), typeof(bool) },
            null
        );

        loadImageSimple = imageConversionType.GetMethod(
            "LoadImage",
            BindingFlags.Public | BindingFlags.Static,
            null,
            new[] { typeof(Texture2D), typeof(byte[]) },
            null
        );

        encodeToPNG = imageConversionType.GetMethod(
            "EncodeToPNG",
            BindingFlags.Public | BindingFlags.Static,
            null,
            new[] { typeof(Texture2D) },
            null
        );
    }

    private static Type FindImageConversionType()
    {
        Type type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(assembly => assembly.GetType("UnityEngine.ImageConversion", false))
            .FirstOrDefault(found => found != null);

        if (type != null)
        {
            return type;
        }

        try
        {
            Assembly assembly = Assembly.Load("UnityEngine.ImageConversionModule");
            return assembly.GetType("UnityEngine.ImageConversion", false);
        }
        catch
        {
            return null;
        }
    }
}
