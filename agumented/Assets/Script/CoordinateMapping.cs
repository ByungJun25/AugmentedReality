﻿using UnityEngine;
using System.Collections.Generic;
using Windows.Kinect;
using Kinect = Windows.Kinect;
using UnityEditor;

public class CoordinateMapping : MonoBehaviour
{
    private KinectSensor _sensor;
    private MultiSourceFrameReader _reader;
    private IList<Body> _bodies;
    private CameraMode _mode = CameraMode.Color;
    private List<GameObject> glist;
    public int count;
    public int MAXJOINT;
    public GameObject prefab;
    public bool offMeshRenderer = false;

    enum CameraMode
    {
        Color,
        Depth,
        Infrared
    }

    // Use this for initialization
    void Start()
    {
        count = 0;
        MAXJOINT = 25;
        glist = new List<GameObject>();
        for (count = 0; count < MAXJOINT; count++)
        {
            if(prefab == null)
            {
                Debug.Log("There isn't prefab.");
                break;
            }
            glist.Add((GameObject)Instantiate(prefab));
            //glist.Add(GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/pref/Cube.prefab", typeof(GameObject))) as GameObject);
        }
        OnJointMeshRenderer(offMeshRenderer, glist);
        _sensor = KinectSensor.GetDefault();
        if (_sensor != null)
        {
            _sensor.Open();

            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
    {
        var reference = e.FrameReference.AcquireFrame();

        // Body
        using (var frame = reference.BodyFrameReference.AcquireFrame())
        {
            if (frame != null)
            {
                if (count > MAXJOINT)
                {
                    count = 0;
                }
                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);
                foreach (var body in _bodies)
                {
                    if (body.IsTracked)
                    {
                        // COORDINATE MAPPING
                        foreach (Kinect.Joint joint in body.Joints.Values)
                        {
                            if (joint.TrackingState == TrackingState.Tracked)
                            {
                                // 3D space point
                                CameraSpacePoint jointPosition = joint.Position;

                                // 2D space point
                                Vector2 point = new Vector2();

                                if (_mode == CameraMode.Color)
                                {
                                    ColorSpacePoint colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                                    point.x = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                                    point.y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                                }
                                else if (_mode == CameraMode.Depth || _mode == CameraMode.Infrared) // Change the Image and Canvas dimensions to 512x424
                                {
                                    DepthSpacePoint depthPoint = _sensor.CoordinateMapper.MapCameraPointToDepthSpace(jointPosition);

                                    point.x = float.IsInfinity(depthPoint.X) ? 0 : depthPoint.X;
                                    point.y = float.IsInfinity(depthPoint.Y) ? 0 : depthPoint.Y;
                                }
                                glist[count++].transform.position = new Vector3(point.x, -point.y);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (_reader != null)
        {
            _reader.Dispose();
        }

        if (_sensor != null)
        {
            _sensor.Close();
        }
        _sensor = null;
    }

    void OnJointMeshRenderer(bool offMeshRenderer, List<GameObject> glist)
    {
        if(offMeshRenderer)
        {
            foreach(GameObject obj in glist)
            {
                obj.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}