using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ADR.Core;

public class AxieController : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }
    private void Rotation()
    {
        if (GameManager.Instance.State == GameManager.EntityState.Death) return;
        // Get the mouse position on the screen
        Vector3 mousePosition = Input.mousePosition;

        // Raycast to get the mouse position in the game world
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
