using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Infrastructure;

public abstract class TapButton : MonoBehaviour, IPoolable
{
    protected float LifeTime;
    protected float Timer;
    protected bool IsActive;

    private Button tapButton;

    protected RippleEffect ripplesExplosion;

    protected virtual void Awake()
    {
        tapButton = GetComponent<Button>();
        tapButton.onClick.RemoveAllListeners();
        tapButton.onClick.AddListener(OnButtonTap);

        ripplesExplosion = Camera.main.GetComponent<RippleEffect>();

        GameManager.GameEventBus.On<GameOverEvent>(OnGameOverEvent);
    }

    protected abstract IEnumerator LifeTimeCounter();

    protected abstract void OnButtonTap();

    protected void GameOver()
    {
        ripplesExplosion.Emit(new Vector2(transform.position.x / Screen.width, transform.position.y / Screen.height));
        GameManager.GameEventBus.Trigger<GameOverEvent>(new GameOverEvent());
        Deactivate();
    }

    public bool IsAvailable()
    {
        return !IsActive;
    }

    public virtual void Activate(Vector2 pos, float lifeTime)
    {
        IsActive = true;
        gameObject.SetActive(IsActive);
        transform.position = pos;
        LifeTime = lifeTime;
        Timer = 0;
        StartCoroutine(LifeTimeCounter());
    }

    public void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(IsActive);
        StopAllCoroutines();
    }

    public Vector2 GetPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    private void OnGameOverEvent(GameOverEvent arg)
    {
        tapButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        GameManager.GameEventBus.Off<GameOverEvent>(OnGameOverEvent);
    }
}
