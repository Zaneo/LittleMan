// Copyright 2009, 2010, 2011, 2012, 2013 Matvei Stefarov <me@matvei.org>
// Creative Commons BY-NC-SA
// http://creativecommons.org/licenses/by-nc-sa/3.0/

using System;

namespace LittleMan.Util {
    public static class EnumUtil {
        public static bool TryParse<TEnum>(string value, out TEnum output, bool ignoreCase) {
            if (value == null) throw new ArgumentNullException("value");
            try {
                output = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                return true;
            }
            catch (ArgumentException) {
                output = default(TEnum);
                return false;
            }
        }
    }
}
