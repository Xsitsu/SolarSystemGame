using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    List<Warp> _warps = new List<Warp>();

    static private WarpManager _instance;
	static public WarpManager Instance { get { return _instance; } }
    void Awake()
	{
		_instance = this;
	}

    void OnDestroy()
    {
        _instance = null;
    }

    void Start()
    {
        
    }

    void Update()
    {
        List<Warp> toRemove = new List<Warp>();
        foreach (Warp warp in _warps)
        {
            warp.Update(Time.deltaTime, StarSystemDisplay.Instance.CalculateCurrentTime());
            if (warp.IsFinished())
            {
                toRemove.Add(warp);
            }
        }

        foreach (Warp warp in toRemove)
        {
            _warps.Remove(warp);
        }
    }

    public Warp GetWarp(Structure entity)
    {
        foreach (Warp warp in _warps)
        {
            if (warp.warpee == entity) return warp;
        }
        return null;
    }
    public void InitializeWarp(Structure entity, Entity targ)
    {
        if (GetWarp(entity) != null) return;

        Warp warp = new Warp()
        {
            warpee = entity,
            target = targ,
        };
        _warps.Add(warp);
    }

    public float AngleBetweenVectors_Deg(Vector3 a, Vector3 b)
    {
        float dot = Vector3.Dot(a, b);
        float ratio = dot / (a.magnitude * b.magnitude);
        return Mathf.Rad2Deg * Mathf.Acos(ratio);
    }
    bool IsAligned(Vector3 fromPosition, Vector3 toPosition, Vector3 lookVector)
    {
        float angle = AngleBetweenVectors_Deg((toPosition - fromPosition), lookVector);
        if (angle < 1)
        {
            return true;
        }
        return false;
    }


}
