using UnityEngine;

public interface IPoolable
{
    bool IsAvailable();
    void Deactivate();
    void Activate(Vector2 pos, float lifeTime);
    Vector2 GetPosition();
}
