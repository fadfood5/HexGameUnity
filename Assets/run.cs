using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class run : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public List<int> newEdge = new List<int> ();

	public Material material;
	//public LineRenderer l1;
	public void MakeLine(int i, int j){
		GameObject temp = new GameObject ();
		temp.AddComponent<LineRenderer>();
		LineRenderer l1 = temp.GetComponent<LineRenderer> ();
		Debug.Log ("Make Line");
		float x1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.x;
		Debug.Log (x1);
		float y1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.y;
		//float z1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.z;
		float x2 = this.transform.Find ("Points").FindChild (j.ToString ()).transform.position.x;
		float y2 = this.transform.Find ("Points").FindChild (j.ToString ()).transform.position.y;
		//float z2 = this.transform.Find ("Points").FindChild (j.ToString ()).transform.position.z;
		Vector3 point1 = new Vector3 (x1, y1, 1);
		Vector3 point2 = new Vector3 (x2, y2, 1);
		//LineRenderer l1 = gameObject.AddComponent<LineRenderer>();
		l1.startWidth = 1;
		l1.endWidth = 1;
		l1.numPositions = 2;
		l1.useWorldSpace = true;
		l1.material = material;
		Color bl = Color.black;
		l1.startColor = bl;
		l1.endColor = bl;
		l1.SetPosition (0, point1);
		l1.SetPosition (1, point2);
		temp.layer = 0;
	}

	public void click1(Button temp){
		int num = Int32.Parse (temp.transform.Find ("Text").GetComponent<Text> ().text);
		Debug.Log (num);
		if (newEdge.Count == 0) {
			newEdge.Add(num);
			Debug.Log ("(" + newEdge [0] + ", )");
		} else {
			newEdge.Add(num);
			Debug.Log ("(" + newEdge [0] + ", " + newEdge[1] + ")");
			MakeLine (newEdge [0], newEdge [1]);
			newEdge.Clear ();
		}
	}

	public class Globals{ 
		public int currentPlayer;
	}

	public class Edge{
		public int x;
		public int y;
		public int z;

		public Edge(int a, int b, int player){
			x = a;
			y = b;
			z = player;
		}
	}

	public class Vertex{
		public int i;

		public Vertex(int a){
			i = a;
		}
	}

	public class Hexagon{
		//Instantiate lists
		List<Vertex> vertices;
		List<Edge> allEdges;
		List<Edge> player1;
		List<Edge> player2;

		public Hexagon(int v){
			//Create new lists to keep track of edges
			vertices = new List<Vertex>();
			allEdges = new List<Edge>();
			player1 = new List<Edge>();
			player2 = new List<Edge>();
		}

		public void changePlayer(Globals global){
			if (global.currentPlayer == 1) {
				global.currentPlayer = 2;
				Debug.Log ("Player changed to 2");
			} else {
				global.currentPlayer = 1;
				Debug.Log ("Player changed to 1");
			}
		}

		public void addEdge(Edge i, int player){

			//Create temp Edge Object
			Edge newEdge = new Edge(i.x, i.y, player);

			//Add edge to corresponding list
			if (player == 1)
				player1.Add (newEdge);
			else if (player == 2)
				player2.Add (newEdge);

			//Add edge to list of all edges
			allEdges.Add(newEdge);

			Debug.Log("Added edge to vertex: (" + i.x + ", " + i.y + ")");
		}

		public void addVertex(int i){

			//Create temp Edge Object
			Vertex newVertex = new Vertex(i);

			//Add vertex to list of all vertices
			vertices.Add(newVertex);

			Debug.Log("Added vertex: " + i);
		}

		public void printEdges(){
			int count = 0;
			foreach(Edge item in allEdges){
				Debug.Log ("(" + item.x + ", " + item.y + ")");
				count++;
			}
		}
		public int checkIfEdgeExists(Edge ed){
			foreach (Edge item in allEdges) {
				Debug.Log ("Edge: " + item.x + ", " + item.y);
				if ((item.x == ed.x && item.y == ed.y) || (item.x == ed.y && item.y == ed.x)) {
					Debug.Log ("Edge already exists! Make another move");
					return 0;
				}
			}
			Debug.Log ("Edge does not exist.");
			return 1;
		}
		public void printEdges(int player){
			int count = 0;
			if (player == 1) {
				Debug.Log ("Player 1: ");
				foreach (Edge item in player1) {
					Debug.Log ("(" + item.x + ", " + item.y + ")");
					count++;
				}
			} else if (player == 2) {
				Debug.Log ("Player 2: ");
				foreach (Edge item in player2) {
					Debug.Log ("(" + item.x + ", " + item.y + ")");
					count++;
				}
			}
		}
	}
		
}
