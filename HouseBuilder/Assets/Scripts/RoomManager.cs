using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomManager : MonoBehaviour {
	public FurnitureSpawner fs;

	List<GameObject> walls;
	public GameObject ground;

	List<int[]> graph;
	List<int[]> cycles;
	// Use this for initialization
	void Start () {
		graph = new List<int[]> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			Debug.Log ("Pressed G");
			BuildGraph ();
		}
	}

	public bool IsPointInPolygon( Vector2 p, Vector2[] polygon )
	{
		float minX = polygon[ 0 ].x;
		float maxX = polygon[ 0 ].x;
		float minY = polygon[ 0 ].y;
		float maxY = polygon[ 0 ].y;
		for ( int i = 1 ; i < polygon.Length ; i++ )
		{
			Vector2 q = polygon[ i ];
			minX = Math.Min( q.x, minX );
			maxX = Math.Max( q.x, maxX );
			minY = Math.Min( q.y, minY );
			maxY = Math.Max( q.y, maxY );
		}

		if ( p.x < minX || p.x > maxX || p.y < minY || p.y > maxY )
		{
			return false;
		}

		// http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
		bool inside = false;
		for ( int i = 0, j = polygon.Length - 1 ; i < polygon.Length ; j = i++ )
		{
			if ( ( polygon[ i ].y > p.y ) != ( polygon[ j ].y > p.y ) &&
				p.x < ( polygon[ j ].x - polygon[ i ].x ) * ( p.y - polygon[ i ].y ) / ( polygon[ j ].y - polygon[ i ].y ) + polygon[ i ].x )
			{
				inside = !inside;
			}
		}

		return inside;
	}

	public float Area(List<Vector2> vertices)
	{
		vertices.Add(vertices[0]);
		return Math.Abs(vertices.Take(vertices.Count - 1).Select((p, i) => (p.x * vertices[i + 1].y) - (p.y * vertices[i + 1].x)).Sum() / 2);
	}

	public void BuildGraph() {
		walls = new List<GameObject> (fs.placedWalls);
		walls.Add (ground);
		Debug.Log ("Build Graph, walls.Count: " + walls.Count);
		foreach (GameObject wall in fs.placedWalls) {
			DetectConnections (wall);
		}
		int[,] formattedGraph = CreateRectangularArray<int> (graph);
		cycles = akCyclesInUndirectedGraphs.Main (formattedGraph);

		foreach (int[] cy in cycles)
		{
			string s = "" + walls[cy[0]].name;

			for (int i = 1; i < cy.Length; i++)
				s += "," + walls[cy[i]].name;

			Debug.Log(s);
		}
	}

	void DetectConnections(GameObject wall) {
		Collider[] colls = Physics.OverlapBox (wall.transform.position, wall.transform.localScale / 2 + new Vector3(0.25f,0.25f,0.25f), wall.transform.rotation);

		int myIndex = walls.IndexOf (wall);
		foreach (Collider c in colls) {
			if ((c.gameObject.tag == "wall" || c.gameObject.tag == "ground") && wall != c.gameObject) {
				graph.Add (new int[] { myIndex, walls.IndexOf (c.gameObject) });
			}
		}
	}
		
	static T[,] CreateRectangularArray<T>(IList<T[]> arrays)
	{
		// TODO: Validation and special-casing for arrays.Count == 0
		int minorLength = arrays[0].Length;
		T[,] ret = new T[arrays.Count, minorLength];
		for (int i = 0; i < arrays.Count; i++)
		{
			var array = arrays[i];
			if (array.Length != minorLength)
			{
				throw new ArgumentException
				("All arrays must be the same length");
			}
			for (int j = 0; j < minorLength; j++)
			{
				ret[i, j] = array[j];
			}
		}
		return ret;
	}
}
