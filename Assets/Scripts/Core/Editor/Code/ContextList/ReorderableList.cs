using System;
using System.Collections.Generic;
using Core.Editor.Code.Wizards;

namespace Core.Editor.Code.ContextList
{
    public class SimpleReorderableList
    {
    }

    public class ReorderableList<T> : SimpleReorderableList
    {
        public List<T> List;
    }

    [Serializable]
    public class ReorderableContextList : ReorderableList<ContextVo>
    {
    }

 
}