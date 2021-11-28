using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AprendaUnity
{
    [CustomEditor(typeof(fov))]
    public class fovEditor : Editor
    {
        private void OnSceneGUI()
        {
            fov f = (fov)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(f.transform.position, Vector3.up, Vector3.forward, 360, f.viewRadius);

            Vector3 viewAngleA = f.DirForAnlge(-f.viewAngle / 2);
            Vector3 viewAngleB = f.DirForAnlge(f.viewAngle / 2);

            Handles.color = Color.blue;

            Handles.DrawLine(f.transform.position, f.transform.position + viewAngleA * f.viewRadius);
            Handles.DrawLine(f.transform.position, f.transform.position + viewAngleB * f.viewRadius);


            Vector3 viewAngleAb = f.DirForAnlge(-f.viewAngleB / 2);
            Vector3 viewAngleBb = f.DirForAnlge(f.viewAngleB / 2);

            Handles.color = Color.yellow;

            Handles.DrawLine(f.transform.position, f.transform.position + viewAngleAb * f.viewRadius);
            Handles.DrawLine(f.transform.position, f.transform.position + viewAngleBb * f.viewRadius);


        }
    }
}
