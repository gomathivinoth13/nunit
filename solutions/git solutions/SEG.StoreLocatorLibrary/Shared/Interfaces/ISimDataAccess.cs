using System.Collections.Generic;
using SEG.StoreLocatorLibrary.Shared.Types;

namespace SEG.StoreLocatorLibrary.Shared.Interfaces
{
    public interface ISimDataAccess
    {
        Option<IList<SimStore>> SimStores();
    }
}
