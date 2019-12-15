using System;
using System.Collections.Generic;

namespace Core.Model.Vo
{
    [Serializable]
    public class UserVo
    {
        public string Id = String.Empty;

        public int LastSavedTime;

        public List<string> PurchasedProductIds = new List<string>();
    }
}