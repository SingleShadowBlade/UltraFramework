using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

public class Log
{
    public static Type type;
    public static object obj;
    public static bool enableDebug;

    public static void Info(params object[] objs)
    {
		if (type != null) {
			MethodInfo methodInfo = type.GetMethod ("Info", new Type[] { typeof (object[]) });
			if (methodInfo != null) {
				methodInfo.Invoke (obj, new object[] { objs });
				return;
			}
			methodInfo = type.GetMethod ("Info", new Type[] { typeof (object) });
			if (methodInfo != null) {
				methodInfo.Invoke (obj, new object[] { ToString (objs) });
				return;
			}
		}
    }

    public static void Warning(params object[] objs) {
		if (type != null) {
			MethodInfo methodInfo = type.GetMethod ("Warning", new Type[] { typeof (object[]) });
			if (methodInfo != null) {
				methodInfo.Invoke (obj, new object[] { objs });
				return;
			}
			methodInfo = type.GetMethod ("Warn", new Type[] { typeof (object) });
			if (methodInfo != null) {
				methodInfo.Invoke (obj, new object[] { ToString (objs) });
				return;
			}
		}
    }

    public static void Debug(params object[] objs)
    {
        if(enableDebug)
        {
			var file = File.AppendText ("log.txt");
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < objs.Length; i++) {
				stringBuilder.Append (objs[i].ToString ());
			}
			file.WriteLine (stringBuilder.ToString ());
			file.Flush ();
			file.Close ();
        }
        else
        {
			if (type != null) {
				MethodInfo methodInfo = type.GetMethod ("Warning", new Type[] { typeof (object[]) });
				if (methodInfo != null) {
					methodInfo.Invoke (obj, new object[] { objs });
					return;
				}
				methodInfo = type.GetMethod ("Warn", new Type[] { typeof (object) });
				if (methodInfo != null) {
					methodInfo.Invoke (obj, new object[] { ToString (objs) });
					return;
				}
			}
        }
    }

    public static string ToString(bool[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 2 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }
    
    public static string ToString(bool[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 1));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(bool[] array, string separator,
        StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append((array[i]) ? '1' : '0').Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(byte[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 5 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(byte[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 4));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(byte[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(short[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 6 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(short[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 5));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(short[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(char[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 2 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(char[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 1));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(char[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(int[] array)
    {
        if (array == null)
            return null;
        StringBuilder cb = new StringBuilder(array.Length * 9 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(int[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 8));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(int[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(long[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 16 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(long[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 15));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(long[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(float[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 10 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(float[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 9));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(float[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(double[] array)
    {
        StringBuilder cb = new StringBuilder(array.Length * 16 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(double[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 15));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(double[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }

    public static string ToString(object[] array)
    {
        if (array == null)
            return null;
        StringBuilder cb = new StringBuilder(array.Length * 25 + 2);
        cb.Append('{');
        ToString(array, ",", cb);
        cb.Append('}');
        return cb.ToString();
    }

    public static string ToString(object[] array, string separator)
    {
        StringBuilder cb = new StringBuilder(array.Length * (separator.Length + 24));
        ToString(array, separator, cb);
        return cb.ToString();
    }

    public static void ToString(object[] array, string separator, StringBuilder cb)
    {
        int n = array.Length - 1;
        for (int i = 0; i < n; i++)
            cb.Append(array[i]).Append(separator);
        if (n >= 0)
            cb.Append(array[n]);
    }
}
