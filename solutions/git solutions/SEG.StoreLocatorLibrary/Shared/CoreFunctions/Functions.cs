using System.Collections.Generic;
using SEG.StoreLocatorLibrary.Shared;

namespace SEG.StoreLocatorLibrary.Shared.CoreFunctions
{
    public static class Functions
    {
        public static int GetChainIDFromAppCode(string appCode)
        {
            var code = appCode.Substring(0, 2);
            int.TryParse(code, out int i);
            return i + 1;
        }

    }
}
