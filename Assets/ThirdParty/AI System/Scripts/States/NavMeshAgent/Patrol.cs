using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AISystem.States.NavMeshAgent{
	[CanCreate(true)]
	[System.Serializable]
	public class Patrol : Movement {
		[ArrayElement]
		public Vector3[] path;
		public float threshold=0.5f;
		public bool random;
		private List<Vector3> runtimePath;
		private int pathIndex;

		public override void OnUpdate ()
		{
			if (agent.remainingDistance < threshold) {
				if(random){
					pathIndex=Random.Range(0,path.Length-1);
					agent.SetDestination(path[pathIndex]);
				}else{
					pathIndex++;
					if (pathIndex > runtimePath.Count-1) {
						pathIndex = 0;
					}
					agent.SetDestination(runtimePath[pathIndex]);
				}

			}
		}
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			CatmullRom(new List<Vector3>(path),out runtimePath,10,true);
		}

		public static void InvertPath (ref  List<Vector3> path)
		{
			List<Vector3> pInverted = new List<Vector3> ();
			for (int i = path.Count - 1; i >= 0; i--) {
				pInverted.Add (path [i]);
			}
			path = pInverted;
		}
		
		public static bool CatmullRom(List<Vector3> inCoordinates, out List<Vector3> outCoordinates, int samples, bool includeEndPoints)
		{
			if ((!includeEndPoints && inCoordinates.Count < 4) || (includeEndPoints && inCoordinates.Count < 2))
			{
				outCoordinates = null;
				return false;
			}
			if (includeEndPoints && inCoordinates.Count >= 2)
			{
				inCoordinates.Insert(0, inCoordinates[0]);
				inCoordinates.Insert(inCoordinates.Count - 1, inCoordinates[inCoordinates.Count - 1]);
			}
			List<Vector3> results = new List<Vector3>();
			for (int n = 1; n < inCoordinates.Count - 2; n++)
				for (int i = 0; i < samples; i++)
					results.Add(PointOnCurve(inCoordinates[n - 1], inCoordinates[n], inCoordinates[n + 1], inCoordinates[n + 2], (1f / samples) * i ));
			results.Add(inCoordinates[inCoordinates.Count - 2]);
			outCoordinates = results;
			return true;
		}
		
		public static Vector3 PointOnCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			Vector3 result = new Vector3();
			float t0 = ((-t + 2f) * t - 1f) * t * 0.5f;
			float t1 = (((3f * t - 5f) * t) * t + 2f) * 0.5f;
			float t2 = ((-3f * t + 4f) * t + 1f) * t * 0.5f;
			float t3 = ((t - 1f) * t * t) * 0.5f;
			result.x = p0.x * t0 + p1.x * t1 + p2.x * t2 + p3.x * t3;
			result.y = p0.y * t0 + p1.y * t1 + p2.y * t2 + p3.y * t3;
			result.z = p0.z * t0 + p1.z * t1 + p2.z * t2 + p3.z * t3;
			return result;
		}
	}
}