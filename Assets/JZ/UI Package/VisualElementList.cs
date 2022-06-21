using System.Collections.Generic;
using UnityEngine.UIElements;

namespace JZ.UI
{
    public abstract class VisualElementList<T> where T : VisualElement
    {
        protected VisualElement container { get; private set; }
        public List<T> entries { get; private set; }

        public VisualElementList(VisualElement container)
        {
            this.container = container;
            entries = new List<T>();
        }


        public void ChangeCount(int oldCount, int newCount)
        {
            if (newCount > oldCount)
            {
                for(int ii = 0; ii < newCount - oldCount; ii++)
                    AddEntries(GenerateEntry());
            }
            else if(newCount < oldCount)
            {
                for(int ii = newCount; ii < oldCount; ii++)
                    RemoveEntries(entries[ii]);
                
                entries.RemoveRange(newCount, oldCount - newCount);
                entries.Capacity = newCount;
            }
        }

        protected virtual void AddEntries(T entry)
        {
            entries.Add(entry);
            container.Add(entry);
        }

        protected virtual void RemoveEntries(T entry)
        {
            container.Remove(entry);
        }

        protected abstract T GenerateEntry();
    }
}
