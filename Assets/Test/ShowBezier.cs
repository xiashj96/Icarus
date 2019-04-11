using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBezier : MonoBehaviour
{
	public int lineSegments = 100;
	Vector3 P0, P1, P2, P3;

    Vector3 Bezier(float t)
    {
    	Vector3 P10 = (1 - t) * P0 + t * P1;
		Vector3 P11 = (1 - t) * P1 + t * P2;
		Vector3 P12 = (1 - t) * P2 + t * P3;

		Vector3 P20 = (1 - t) * P10 + t * P11;
		Vector3 P21 = (1 - t) * P11 + t * P12;

		return (1 - t) * P20 + t * P21;
    }

    void OnDrawGizmos()
    {
        int num = transform.childCount;
    	P0 = transform.position;
    	for(int i = 2; i < num; i += 3)
    	{
    		P1 = transform.GetChild(i - 1).position;
    		P2 = transform.GetChild(i - 0).position;
    		P3 = transform.GetChild(i - 2).position;

    		Gizmos.color = Color.grey;
    		Gizmos.DrawLine(P0, P1);
    		Gizmos.DrawLine(P2, P3);

    		Gizmos.color = Color.yellow;
    		Vector3 lastPoint = Bezier(0);
    		for(int j = 1; j <= lineSegments; j++)
			{
				Vector3 point = Bezier(1.0f * j / lineSegments);
				Gizmos.DrawLine(lastPoint, point);
				lastPoint = point;
			}

			P0 = P3;
    	}
        
    }
}
