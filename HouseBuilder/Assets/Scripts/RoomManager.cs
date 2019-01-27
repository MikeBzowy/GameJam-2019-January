using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomManager : MonoBehaviour {
	public FurnitureSpawner fs;
	public PlayerScore ps;

	List<GameObject> walls;
	public GameObject ground;

	List<int[]> graph;
	List<int[]> cycles;

	public struct Room {
		public float minX;
		public float maxX;
		public float minY;
		public float maxY;
	}
	public List<Room> rooms;
	public List<List<GameObject>> roomContents;

	List<Vector3[]> wallBounds;
	List<List<Vector2>> polygons;
	// Use this for initialization
	void Start () {
		graph = new List<int[]> ();
		polygons = new List<List<Vector2>> ();
		rooms = new List<Room> ();
		roomContents = new List<List<GameObject>> ();
	}

	public void EndGame(){
		BuildGraph ();	
	}

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

		/*foreach (int[] cy in cycles)
		{
			string s = "" + walls[cy[0]].name;

			for (int i = 1; i < cy.Length; i++)
				s += "," + walls[cy[i]].name;

			Debug.Log(s);
		}*/

		//BuildWallBounds ();
		//BuildPolygons ();

		GenerateRoomBounds ();
		FillRooms ();
		for (int i = 0; i < roomContents.Count; i ++) {
			String s = "Room " + i + " : ";
			foreach (GameObject g in roomContents[i]) {
				s += g.transform.GetChild(0).name + ", ";
			}
			Debug.Log (s);
		}

		ps.ScoreGame ();
	}

	void FillRooms(){
		for (int i = 0; i < rooms.Count; i++) {
			roomContents.Add(getContents (rooms [i]));
		}
	}

	List<GameObject> getContents(Room r) {
		List<GameObject> myStuff = new List<GameObject>();
		foreach(GameObject f in fs.placedFurniture) {
			if (f.transform.position.x < r.maxX && f.transform.position.x > r.minX && f.transform.position.y < r.maxY && f.transform.position.y > r.minY) {
				myStuff.Add (f);
			}
		}
		return myStuff;
	}

	void GenerateRoomBounds() {
		for (int i = 0; i < cycles.Count; i++) {
			addRoomBounds (cycles[i]);
		}
	}

	void addRoomBounds (int[] cycle) {
		float minX = walls[cycle[0]].transform.position.x;

		float maxX = walls[cycle[0]].transform.position.x;

		float minY = walls[cycle[0]].transform.position.y;

		float maxY = walls[cycle[0]].transform.position.y;

		for (int i = 0; i < cycle.Length; i++) {
			if (cycle [i] < walls.Count) {
				if (walls [cycle [i]].transform.position.x < minX)
					minX = walls [cycle [i]].transform.position.x;

				if (walls [cycle [i]].transform.position.x > maxX)
					maxX = walls [cycle [i]].transform.position.x;

				if (walls [cycle [i]].transform.position.y < minY)
					minY = walls [cycle [i]].transform.position.y;

				if (walls [cycle [i]].transform.position.y > maxY)
					maxY = walls [cycle [i]].transform.position.y;
			}
		}

		Room r = new Room ();
		r.maxX = maxX;
		r.minX = minX;
		r.maxY = maxY;
		r.minY = minY;
		rooms.Add (r);
	}

	void BuildWallBounds() {
		wallBounds = new List<Vector3[]> ();
		for(int i = 0; i< walls.Count; i++) {
			if (walls [i].tag == "wall") {
				wallBounds.Add (GetColliderVertexPositions (walls [i]));
			}
		}
	}

	void BuildPolygons() {
		for (int j = 0; j < cycles.Count; j ++) {
			List<Vector2> vertices = new List<Vector2>();
			for (int i = 0; i < cycles [j].Length; i++) {
				int current = i;
				int next = i + 1;
				if (next >= cycles[j].Length)
					next = 0;
				int[] cy = cycles [j];
				if (walls [cy[current]].tag != "wall") {
					vertices.Add( getLowestPoint (wallBounds [cy[next]]));
				} else if (walls [cy[next]].tag != "wall") {
					//Debug.Log ("Myval: " + cy [next]+ " , length: " + wallBounds.Count + " Num walls: " + walls.Count);
					vertices.Add(getLowestPoint (wallBounds [cy[current]]));
				} else {
					Vector2[] points = getClosestPoints (wallBounds [cy[current]], wallBounds [cy[next]]);
					vertices.Add (points [0]);
					vertices.Add (points [1]);
				}
				

			}
			polygons.Add (vertices);
		}

		foreach (List<Vector2> p in polygons) {
			String s = "P-gon: ";
			for (int i = 0; i < p.Count; i++) {
				s += p[i] + ", ";
			}
			Debug.Log (s);
		}
	}

	Vector2 getLowestPoint(Vector3[] a) {
		int current = 0;
		float lowest = a [0].y;

		for (int i = 0; i < a.Length; i++) {
			if (a [i].y < lowest) {
				current = i;
				lowest = a [i].y;
			}
		}

		return a [current];
	}

	Vector2[] getClosestPoints(Vector3[] a, Vector3[] b) {
		int currentA = 0;
		int currentB = 0;
		float minDistance = Vector2.Distance (a [0], b [0]);

		for (int i = 0; i < a.Length; i++) {
			for (int j = 0; j < b.Length; j++) {
				float myDist = Vector2.Distance (a [i], b [j]);
				if (myDist < minDistance) {
					minDistance = myDist;
					currentA = i;
					currentB = j;
				}
			}
		}

		return new Vector2[2] {a[currentA], b[currentB]};
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
		if (arrays.Count == 0)
			return new T[0, 2];
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

	Vector3[] GetColliderVertexPositions (GameObject go) {
		var vertices = new Vector3[8];
		var thisMatrix = go.transform.localToWorldMatrix;
		var storedRotation = go.transform.rotation;
		go.transform.rotation = Quaternion.identity;

		var extents = go.GetComponent<BoxCollider>().bounds.extents;
		vertices [0] = thisMatrix.MultiplyPoint3x4 (extents);// + go.transform.position;
		vertices[1] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, extents.z));// + go.transform.position;
		vertices[2] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z));// + go.transform.position;
		vertices[3] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z));// + go.transform.position;
		vertices[4] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z));// + go.transform.position;
		vertices[5] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z));// + go.transform.position;
		vertices[6] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z));// + go.transform.position;
		vertices[7] = thisMatrix.MultiplyPoint3x4(-extents);// + go.transform.position;
		//vertices [8] = go.transform.position;

		go.transform.rotation = storedRotation;
		return vertices;
	}
}
