using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to leg; controls joints.
/// </summary>
public class LegController : MonoBehaviour
{
    private Transform[] bones = new Transform[2];
    private Transform foot;
    private Vector3 previousTarget;
    private Vector3 activeTarget;
    private SpiderController body;
    private Coroutine activeCoroutine;

    public bool grounded { get; set; }
    public Vector3 currentTarget { get; set; }

    public float range;
    public float offset;
    [Range(.01f, 10)]
    public float moveTime;

    void Start() {
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            bones[i] = colliders[i].transform;
        }

        foot = GetComponentInChildren<SphereCollider>().transform;
        body = transform.parent.GetComponent<SpiderController>();
        grounded = true;
        currentTarget = Vector3.zero;
    }

    void LateUpdate() 
    {
        UpdateLegs();
        transform.position = new Vector3(transform.position.x, activeTarget.y + GetComponentInParent<SpiderController>().bodyOffset, transform.position.z);
    }

    void UpdateLegs()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, (transform.forward - (transform.up/3)).normalized, (bones[0].lossyScale.z + bones[1].lossyScale.z), LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, (transform.forward - (transform.up/2)).normalized * (bones[0].lossyScale.z + bones[1].lossyScale.z), Color.red);

        if (grounded && activeCoroutine == null)
        {
            if (hits.Length > 0)
            {
                if (currentTarget == Vector3.zero)
                {
                    activeCoroutine = StartCoroutine(TransitionCoroutine(hits[0].point - (transform.parent.right / 10) * offset));
                }
                else if (Vector3.Distance(currentTarget, hits[0].point) > range)
                {
                    activeCoroutine = StartCoroutine(TransitionCoroutine(hits[0].point));
                }
            }
        }

        if (activeTarget != Vector3.zero){
            Vector3 elbowLocation = IKCalculator.GetElbowPoint(transform, bones[0], bones[1], activeTarget);
            
            SetBone(bones[0], transform.position, elbowLocation);
            SetBone(bones[1], elbowLocation, activeTarget);
        }
    }

    private IEnumerator TransitionCoroutine(Vector3 newTarget)
    {
        while(!body.LegsGrounded())
        {
            yield return null;
        }
        
        previousTarget = currentTarget;
        currentTarget = newTarget;
        grounded = false;

        float timeStarted = Time.time;

        float timePassed;
        do
        {
            timePassed = Time.time - timeStarted;
            float interRatio = timePassed / moveTime;
            activeTarget = Vector3.Lerp(previousTarget, currentTarget, interRatio);
            yield return null;
        } while (timePassed < moveTime);

        activeCoroutine = null;
        grounded = true;
    }

    void SetBone(Transform bone, Vector3 start, Vector3 end)
    {
        bone.position = start;
        bone.LookAt(end);

        float length = bone.lossyScale.z;

        bone.position += bone.forward * (length / 2);
    }

    private void OnDrawGizmos() {
        if (currentTarget != null)
        {
            Gizmos.DrawSphere(currentTarget, .1f);
        }
    }
}
