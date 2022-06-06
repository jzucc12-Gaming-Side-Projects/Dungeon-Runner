using System.Collections.Generic;
using UnityEngine;

namespace JZ.POOL
{
    /// <summary>
    /// <para>Base object pool class</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class ObjectPool<T>
    {
        #region //Pool variables
        [SerializeField] protected T poolObject;
        [SerializeField] protected Transform poolContainer = null;
        [SerializeField] protected int poolSize = 1;
        protected List<T> pooledObjects = new List<T>();
        #endregion


        #region //Constructor
        public ObjectPool(int poolSize, T poolObject, Transform poolContainer)
        {
            this.poolSize = poolSize;
            this.poolObject = poolObject;
            this.poolContainer = poolContainer;
            CreatePool();
        }
        #endregion

        #region //Set Up
        public void CreatePool()
        {
            pooledObjects = new List<T>();

            foreach (Transform obj in poolContainer)
                GameObject.Destroy(obj.gameObject);
            poolContainer.DetachChildren();

            for (int ii = 0; ii < poolSize; ii++)
                AddPoolObject();
        }

        protected abstract T AddPoolObject();
        #endregion

        #region //Enabling objects
        public T GetObject()
        {
            //Check for available object in pool
            foreach(var po in pooledObjects)
            {
                if(IsActive(po)) continue;
                Activate(po);
                return po;
            }

            //Create a new object if none are available
            T newGO = AddPoolObject();
            Activate(newGO);
            return newGO;
        }

        public IEnumerable<T> GetObjects(int count)
        {
            for(int ii = 0; ii < count; ii++)
            {
                yield return GetObject();
            }
        }
        #endregion
    
        #region //Object activation
        public void DeactivateAll()
        {
            foreach(var obj in pooledObjects)
                Deactivate(obj);
        }

        //Called when an object is allowed back into the pool
        public abstract void Deactivate(T obj);

        //Called when an object is retrieved from the pool
        protected abstract void Activate(T obj);

        //Returns true if a pool object is available
        protected abstract bool IsActive(T obj);

        public int GetActiveCount()
        {
            int enabled = 0;
            foreach(T po in pooledObjects)
            {
                if(!IsActive(po)) continue;
                enabled++;
            }
            return enabled;
        }

        public int GetInactiveCount()
        {
            int enabled = GetActiveCount();
            return pooledObjects.Count - enabled;
        }
        #endregion
    }
}