using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    private Camera mainCamera;

    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    [SerializeField] private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }

        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target)) { return; }
        
        targets.Remove(target);
        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        
        if (targets.Count == 0) { return false; }

        Target closetTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPosition = mainCamera.WorldToViewportPoint(target.transform.position);
           
            Debug.Log("TARGET POS: " + mainCamera.WorldToViewportPoint(target.transform.position).ToString());
            
            if(!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 distanceToCenter = viewPosition - new Vector2(0.5f, 0.5f);
            
            if(distanceToCenter.sqrMagnitude < closestTargetDistance)
            {
                closetTarget= target;
                closestTargetDistance = distanceToCenter.sqrMagnitude;
            }
        }

        if (closetTarget == null) { return false; }

        CurrentTarget = closetTarget;
        targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if(CurrentTarget == null) { return; }
        targetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            targetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

}
