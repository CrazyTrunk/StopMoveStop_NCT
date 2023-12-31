using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : Singleton<IndicatorManager>
{
    [SerializeField] private LeanGameObjectPool indicatorPool;
    [SerializeField] private RectTransform indicatorCanvas;
    private List<Indicator> activeIndicators = new List<Indicator>();
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform indicatorParent;

    public LeanGameObjectPool IndicatorPool { get => indicatorPool; set => indicatorPool = value; }
    public List<Indicator> ActiveIndicators { get => activeIndicators; set => activeIndicators = value; }

    public void CreateIndicator(Enemy target, int level)
    {
        Indicator indicator = IndicatorPool.Spawn(Vector3.zero, Quaternion.identity, indicatorParent).GetComponent<Indicator>();
        indicator.SetTarget(target, indicatorCanvas, level);
        ActiveIndicators.Add(indicator);
    }
    public void RemoveIndicator(Indicator indicator)
    {
        ActiveIndicators.Remove(indicator);
        IndicatorPool.Despawn(indicator.gameObject);
    }
    private void Update()
    {
        for ( int i =0; i < ActiveIndicators.Count; i++)
        {
            if (ActiveIndicators[i].TargetIsOffScreen(mainCamera))
            {
                ActiveIndicators[i].UpdateIndicatorPosition(mainCamera);
                ActiveIndicators[i].gameObject.SetActive(true);
            }
            else
            {
                ActiveIndicators[i].gameObject.SetActive(false);

            }
        }
    }

}
