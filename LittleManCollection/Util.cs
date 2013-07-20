using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
