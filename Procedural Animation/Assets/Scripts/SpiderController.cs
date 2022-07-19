using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float bodyOffset;

    private LegController[] legs;

    void Start()
    {
        legs = GetComponentsInChildren<LegController>();
    }

    public bool LegsGrounded()
    {
        LegController[] legs = GetComponentsInChildren<LegController>();

        for (int i = 0; i < legs.Length; i++){
            if (!legs[i].grounded){
                return false;
            }
        }

        return true;
    }

    private void Update() {
        Vector3 input = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
        if (input.x != 0)
        {
            transform.position += (-transform.right * speed * Time.deltaTime * input.x);
        }
        if (input.z != 0){
            transform.eulerAngles += new Vector3(0, input.z * rotationSpeed * Time.deltaTime, 0);
        }

        Transform body = GetComponentInChildren<SphereCollider>().transform;
        Vector3 averageTarget = Vector3.zero;
        for (int i = 0; i < legs.Length; i++)
        {
            averageTarget += legs[i].transform.position;
        }
        averageTarget /= legs.Length;
        body.position = averageTarget;
    }
}
