using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace Internship_Template.Common
{
    public class Internship_Util
    {

            /// <summary>
            /// 型指定に応じて値を返却します.
            ///  ※指定型が存在しない場合、NotSupportExceptionをThrowします.
            /// </summary>
            /// <typeparam name="T">変換型指定</typeparam>
            /// <param name="key">キー値</param>
            /// <returns>返還後値</returns>
            public static T GetValue<T>(string key)
            {
                if (typeof(T) == typeof(bool))
                {
                    return (T)(object)bool.Parse(getAppSetting(key));
                }

                if (typeof(T) == typeof(int))
                {
                    return (T)(object)int.Parse(getAppSetting(key));
                }

                if (typeof(T) == typeof(double))
                {
                    return (T)(object)double.Parse(getAppSetting(key));
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)getAppSetting(key);
                }

                throw new NotSupportedException(typeof(T).Name);
            }

            /// <summary>
            /// appSettingsに存在するaddタグの中から引数に該当するKey値をもつ値を取得します.
            /// </summary>
            /// <param name="key">キー値</param>
            /// <returns>キーに紐づく値</returns>
            private static string getAppSetting(string key)
            {
                return ConfigurationManager.AppSettings[key];
            }
    }
}