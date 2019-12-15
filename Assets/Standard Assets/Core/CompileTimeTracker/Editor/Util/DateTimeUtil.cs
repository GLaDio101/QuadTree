using System;

namespace Standard_Assets.Core.CompileTimeTracker.Editor.Util {
	public static class DateTimeUtil {
    public static bool SameDay(DateTime a, DateTime b) {
      return a.Date == b.Date;
    }
  }
}
