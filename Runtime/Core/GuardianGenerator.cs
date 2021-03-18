using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace oculog.Core
{
    public static class GuardianGenerator
    {
        public static bool GuardianAvailable { get; private set; }

        private const string ERROR_MSG = "No guardian found!";
        
        public static Mesh GenerateGuardian(int iterations, float guardianHeight)
        {
            var nodes = InitializeGuardianNodes();
            var result = new Mesh();

            //Iterate the nodes and create an upper and lower node set
            var lowerNodes = new List<Vector3>();
            var upperNodes = new List<Vector3>();

            var counter = 0;
            for (var i = 0; i < nodes.Count; i++)
            {
                if (counter == iterations)
                    counter = 0;

                if (counter == 0)
                {
                    lowerNodes.Add(nodes[i]);
                    upperNodes.Add(new Vector3(nodes[i].x, nodes[i].y + guardianHeight, nodes[i].z));
                }

                counter++;
            }
            
            //Generate the actual mesh
            var triangles = new List<int>();
            var joinedVertices = new List<Vector3>();

            joinedVertices.AddRange(lowerNodes);
            joinedVertices.AddRange(upperNodes);

            var nodeCount = lowerNodes.Count - 1;

            for (var i = 0; i < lowerNodes.Count; i++)
            {
                if(joinedVertices.Count - 1 < i + nodeCount) continue;
                
                //Lower Triangle
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + nodeCount);
                
                //Upper Triangle
                triangles.Add(i + 1);
                triangles.Add(i + nodeCount + 1);
                triangles.Add(i + nodeCount);
            }

            result.Clear();
            result.vertices = joinedVertices.ToArray();
            result.triangles = triangles.ToArray();
            result.RecalculateNormals();
            return result;
        }

        private static List<Vector3> InitializeGuardianNodes()
        {
            GuardianAvailable = true;
            
            var result = new List<Vector3>();
            var inputSubsystem = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances(inputSubsystem);
            
            if (inputSubsystem.Count <= 0)
            {
                InitializationFailed();
                return result;
            }

            if (!inputSubsystem[0].TryGetBoundaryPoints(result))
            {
                Debug.Log(result);
                InitializationFailed();
                return new List<Vector3>();
            }
            return result;
        }

        private static void InitializationFailed()
        {
            Debug.LogWarning(ERROR_MSG);
            GuardianAvailable = false;
        }
    }
}