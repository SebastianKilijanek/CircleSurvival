using System;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool
{
    private Dictionary<PoolTypeEnum, List<IPoolable>> AvailableObjects = new Dictionary<PoolTypeEnum, List<IPoolable>>();

    public void Add(PoolTypeEnum type, IPoolable poolabelObject)
    {
        if (!AvailableObjects.ContainsKey(type))
        {
            AvailableObjects.Add(type, new List<IPoolable>());
        }
        poolabelObject.Deactivate();
        AvailableObjects[type].Add(poolabelObject);
    }

    public IPoolable Fetch(PoolTypeEnum type, Func<PoolTypeEnum, IPoolable> instantiateCallback)
    {
        var firstAvailable = AvailableObjects[type].FirstOrDefault(po => po.IsAvailable());
        if (firstAvailable == null && instantiateCallback != null)
        {
            var instance = instantiateCallback(type);
            Add(type, instance);
            firstAvailable = instance;
        }
        return firstAvailable;
    }

    public List<IPoolable> GetAllActive(PoolTypeEnum type)
    {
        return AvailableObjects[type].Where(po => !po.IsAvailable()).ToList();
    }
}

public enum PoolTypeEnum
{
    ContinueButton,
    GameOverButton,
}
