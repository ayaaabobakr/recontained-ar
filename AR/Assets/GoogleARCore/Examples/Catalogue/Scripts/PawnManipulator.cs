//-----------------------------------------------------------------------
// <copyright file="PawnManipulator.cs" company="Google">
//
// Copyright 2019 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controls the placement of objects via a tap gesture.
    /// </summary>
    public class PawnManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject PawnPrefab;

        /// <summary>
        /// Manipulator prefab to attach placed objects to.
        /// </summary>
        public GameObject ManipulatorPrefab;

        // public Button AddButton;

        public bool chosenPrefab = false;

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {

                return true;
            }

            return false;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (chosenPrefab && Frame.Raycast(gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hit test is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    var rot = hit.Pose.rotation;
                    rot = Quaternion.Euler(new Vector3(rot.x, 180, rot.z));
                    // Instantiate Andy model at the hit pose.
                    var andyObject = Instantiate(PawnPrefab, hit.Pose.position, rot);

                    // Instantiate manipulator.
                    var manipulator = Instantiate(ManipulatorPrefab, hit.Pose.position, rot);

                    // Make Andy model a child of the manipulator.
                    andyObject.transform.parent = manipulator.transform;

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make manipulator a child of the anchor.
                    manipulator.transform.parent = anchor.transform;

                    // Select the placed object.
                    manipulator.GetComponent<Manipulator>().Select();

                    chosenPrefab = false;
                }
            }


            // if (chosenPrefab && Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
            // {
            //     if ((hit.Trackable is DetectedPlane) &&
            //         Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
            //             hit.Pose.rotation * Vector3.up) < 0)
            //     {
            //         Debug.Log("Hit at back of the current DetectedPlane");
            //     }
            //     else
            //     {
            //         if (PawnPrefab != null)
            //         {

            //             DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
            //             Pose pos = detectedPlane.CenterPose;

            //             var gameObject = Instantiate(PawnPrefab, pos.position, pos.rotation);

            //             // Instantiate manipulator.
            //             var manipulator =
            //                 Instantiate(ManipulatorPrefab, pos.position, pos.rotation);

            //             // Make game object a child of the manipulator.
            //             gameObject.transform.parent = manipulator.transform;

            //             // Create an anchor to allow ARCore to track the hitpoint as understanding of
            //             // the physical world evolves.
            //             var anchor = hit.Trackable.CreateAnchor(pos);

            //             // Make manipulator a child of the anchor.
            //             manipulator.transform.parent = anchor.transform;

            //             // Select the placed object.
            //             manipulator.GetComponent<Manipulator>().Select();

            //             chosenPrefab = false;
            //         }


            //     }

        }


        // if ((hit.Trackable is DetectedPlane) &&
        //     Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
        //         hit.Pose.rotation * Vector3.up) < 0)
        // {
        //     Debug.Log("Hit at back of the current DetectedPlane");
        // }
        // else
        // {

        //     // Instantiate game object at the hit pose.
        //     var gameObject = Instantiate(PawnPrefab, hit.Pose.position, hit.Pose.rotation);

        //     // Instantiate manipulator.
        //     var manipulator =
        //         Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

        //     // Make game object a child of the manipulator.
        //     gameObject.transform.parent = manipulator.transform;

        //     // Create an anchor to allow ARCore to track the hitpoint as understanding of
        //     // the physical world evolves.
        //     var anchor = hit.Trackable.CreateAnchor(hit.Pose);

        //     // Make manipulator a child of the anchor.
        //     manipulator.transform.parent = anchor.transform;

        //     // Select the placed object.
        //     manipulator.GetComponent<Manipulator>().Select();

        //     chosenPrefab = false;
        // }
        //334}

    }

}

