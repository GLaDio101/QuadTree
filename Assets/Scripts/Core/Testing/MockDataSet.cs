using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Testing
{
    [Serializable]
    public class MockDataSet : MonoBehaviour
    {
        public bool CustomKey;

        public string Key;

        public List<MockDataVo> DataList;

        public MockDataVo GetActiveDataVos()
        {
            return DataList.Find(vo => vo.IsActive);
        }
    }
}