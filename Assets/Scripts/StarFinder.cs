using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class StarFinder : MonoBehaviour
{
    private string search_star;
    private GameObject search_object;
    private Vector3 star_vect;
    private float star_vect_mag;
    private float star_angle;

    private Vector3 pointer_vect;
    private float pointer_angle;
    private Transform pointer_transform;
    
    private Vector3 camera_vect;
    private float difference_angle;
    private float difference_mag;
    
    private bool activated = false;

    [SerializeField] private Image pointer;

    public void activate(string star_id)
    {
        search_star = star_id;
        search_object = GameObject.Find(search_star);
        if (search_object != null)
        {
            star_vect = search_object.GetComponent<Transform>().position;
            activated = true;
        }
        else
        {
            Debug.Log("Star not found: " + search_star);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            // get position of the star relative to the cursor
            pointer_vect = Camera.main.WorldToViewportPoint(star_vect) - new Vector3(0.5f, 0.5f);
            star_angle = (Mathf.Rad2Deg * Mathf.Atan(pointer_vect.y / pointer_vect.x)) % 360f;

            // get current angle of the pointer
            pointer_transform = pointer.GetComponent<Transform>();
            pointer_angle = pointer_transform.eulerAngles.z;

            // find the difference between star and pointer angles, flip direction if necisary
            difference_angle = star_angle - pointer_angle;
            if (pointer_vect.x < 0f) { difference_angle += 180f; }
        
            // flip direction if camera is is pointed toward the opposite sky hemisphere
            camera_vect = Camera.main.transform.forward;
            difference_mag = (star_vect + camera_vect).magnitude;
            if (difference_mag < star_vect_mag) { difference_angle += 180f; }
        
            // rotate pointer
            pointer_transform.Rotate(new Vector3(0, 0, difference_angle));
        }
    }
}
