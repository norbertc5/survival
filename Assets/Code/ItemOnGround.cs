using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : MonoBehaviour
{
    public Item item;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float modelSize = 0.5f;
    [SerializeField] float modelTiltAngle = 45;
    [SerializeField] Material outline;
    bool isOutlined;
    List<MeshRenderer> children = new List<MeshRenderer>();

    void Start()
    {
        var model = Instantiate(item.model, Vector3.zero, Quaternion.identity, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = new Vector3(modelSize, modelSize, modelSize);
        model.transform.localEulerAngles = new Vector3(modelTiltAngle, 0, 0);

        // collect mesh renderers
        for (int i = 0; i < model.transform.GetChild(0).childCount; i++)
        {
            children.Add(model.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>());
        }

        // layer must be 0, default it's 'HandItem' and it make issues
        model.layer = 0;
        children.ForEach((MeshRenderer m) => { m.gameObject.layer = 0; });
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void EnableOutline()
    {
        if (isOutlined) return;
        isOutlined = true;

        // set material
        children.ForEach((MeshRenderer m) => {
            Material[] mats = m.materials;
            mats[1] = outline;
            m.GetComponent<MeshRenderer>().materials = mats;
        });
    }

    void DisableOutline()
    {
        if(!isOutlined) return;
        isOutlined = false;

        children.ForEach((MeshRenderer m) => {
            Material[] mats = m.materials;
            mats[1] = null;
            m.GetComponent<MeshRenderer>().materials = mats;
        });
    }
}
