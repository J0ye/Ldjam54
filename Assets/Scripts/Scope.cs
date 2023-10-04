using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Scope : MonoBehaviour
{
    public float lineLength = 0.1f;

    protected LineRenderer lr;
    protected RangeEquipment re;
    // Start is called before the first frame update
    void Start()
    {
        if(!transform.parent.gameObject.TryGetComponent<RangeEquipment>(out re))
        {
            Debug.LogWarning("Laser with out gun");
            Destroy(this);
        }
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(re.GetClosestEnemyInRange() != null)
        {
            SetLineToTarget(re.GetClosestEnemyInRange().transform.position);
        }
        else
        {
            ResetLine();
        }
    }

    protected void SetLineToTarget(Vector3 target)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target);
    }

    protected void ResetLine()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + (Vector3.forward*lineLength));

    }
}
