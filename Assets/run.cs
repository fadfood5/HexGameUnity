using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class run : MonoBehaviour {
    public bool gameFinished = false;
	public int currentPlayer;
	public int AIPlayer;
	public List<Vertex> vertices;
	public List<Edge> allPossibleMoves;
	public List<Edge> allEdges;
	public List<Edge> HumanPlayer;
	public List<Edge> AIPlayerEdges;
	Hexagon p = new Hexagon (6);

	public Material material;

	public List<int> newEdge = new List<int> ();

	public int isPlayerOneAI;
	public bool firstMoves = true;

	void Start () {
	}

	public void MakeLine(int i, int j){
		GameObject temp = new GameObject ();
		temp.AddComponent<LineRenderer>();
		LineRenderer l1 = temp.GetComponent<LineRenderer> ();
		Debug.Log ("Make Line");
		float x1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.x;
		Debug.Log (x1);
		float y1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.y;
		float x2 = this.transform.Find ("Points").FindChild (j.ToString ()).transform.position.x;
		float y2 = this.transform.Find ("Points").FindChild (j.ToString ()).transform.position.y;
		Vector3 point1 = new Vector3 (x1, y1, 1);
		Vector3 point2 = new Vector3 (x2, y2, 1);
		l1.startWidth = 3;
		l1.endWidth = 3;
		l1.numPositions = 2;
		l1.useWorldSpace = true;
		l1.material = material;
		Color bl = Color.black;
		Color rd = Color.blue;
		if (currentPlayer == 1) {
			l1.startColor = bl;
			l1.endColor = bl;
		} else if(currentPlayer == 2){
			l1.startColor = rd;
			l1.endColor = rd;
		}
		l1.SetPosition (0, point1);
		l1.SetPosition (1, point2);
		temp.layer = 0;
		changeTurn ();
	}

	public void debug(string t){
		this.gameObject.transform.Find ("Debugger").GetComponentInChildren<Text> ().text = t;
	}

	public void choosePlayer(int i){
		isPlayerOneAI = i;
		currentPlayer = 1;
		if (isPlayerOneAI == 0) {
			debug ("AI is player one");
			AI ();
		} else {
			debug ("AI is player two. Your turn.");
		}
		this.gameObject.transform.Find ("ChoosePlayer").gameObject.SetActive (false);

		//MAIN
		for (int j = 1; j < 7; j++) {
			p.addVertex (j);
		}
		Edge ed = new Edge (0, 1, 1);
		p.addEdge (ed, 1);
		p.checkIfEdgeExists (ed);

	}

	public int AI(){
        if (currentPlayer == 1 || AIPlayer == 2)
        {
            if (firstMoves == true) {
			System.Random rnd = new System.Random ();
			int r1 = rnd.Next (1, 6);
			int r2 = rnd.Next (1, 6);
			Debug.Log (r1);
			Debug.Log (r2);
			MakeLine (r1, r2);

            }
		    else {
            int firstNode = 0;
            int secondNode = 0;
            int count = 0;

            //ensures that the AI is supposed to move now

                // checks every vertex possible
                foreach (Vertex vertex in vertices)
                {
                    //checks all edges that have already been made
                    foreach (Edge edgeItem in allEdges)
                    {
                        // checks if the current vertex has been used already
                        if (vertex.i == edgeItem.x || vertex.i == edgeItem.y)
                        {
                            count++;
                        }

                    }


                    if (count == 0)
                    {
                        if (firstNode == 0)
                        {
                            firstNode = vertex.i;
                        }
                        else
                            secondNode = vertex.i;

                    }

                    // if both vertices have not been used already, it is an optimal move
                    if (firstNode != 0 && secondNode != 0)
                    {
                        //make move with firstNode and secondNode
                        Edge edgeMove = new Edge(firstNode, secondNode, AIPlayer);
                        allEdges.Add(edgeMove);
                        AIPlayerEdges.Add(edgeMove);
                        MakeLine(firstNode, secondNode);
                    }

                    //if we find one vertex that has been used, we make do with the open vertex
                    else if (firstNode != 0 || secondNode == 0)
                    {
                        //choose a vertex we already picked, using lookahead function
                        CalculateMove(firstNode);
                    }

                    // if no vertex is free, make a move with what has already been made
                    else if (firstNode == 0 && secondNode == 0)
                    {
                        //use lookahead function, find optimal move with all possible moves to make
                        CalculateMove();
                    }
                }
            }
        }
		return 0;
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

	public void changeTurn(){
		if (currentPlayer == 1) {
			currentPlayer = 2;
			Debug.Log ("Changed player to 2");
			Debug.Log ("currentPlayer " + currentPlayer);
			Debug.Log ("isPlayerOneAI " + isPlayerOneAI);
			if (isPlayerOneAI == 0)
				debug ("Your turn.");
			else {
				debug ("AI's turn.");
				AI ();
			}
		} else if(currentPlayer == 2){
			currentPlayer = 1;
			Debug.Log ("Changed player to 1");
			Debug.Log ("currentPlayer " + currentPlayer);
			Debug.Log ("isPlayerOneAI " + isPlayerOneAI);
			if (isPlayerOneAI == 0) {
				debug ("AI's turn.");
				AI ();
			}else
				debug ("Your turn.");
		}
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
		List<Edge> HumanPlayer;
		List<Edge> AIPlayer;

		public Hexagon(int v){
			//Create new lists to keep track of edges
			vertices = new List<Vertex>();
			allEdges = new List<Edge>();
			HumanPlayer = new List<Edge>();
			AIPlayer = new List<Edge>();
		}

		public void addEdge(Edge i, int player){

			//Create temp Edge Object
			Edge newEdge = new Edge(i.x, i.y, player);

			//Add edge to corresponding list
			if (player == 1)
				HumanPlayer.Add (newEdge);
			else if (player == 2)
				AIPlayer.Add (newEdge);

			//Add edge to list of all edges
			allEdges.Add(newEdge);

			Console.WriteLine("Added edge to vertex: (" + i.x + ", " + i.y + ")");
		}

		public void addVertex(int i){

			//Create temp Edge Object
			Vertex newVertex = new Vertex(i);

			//Add vertex to list of all vertices
			vertices.Add(newVertex);

			Console.WriteLine("Added vertex: " + i);
		}

		public void printEdges(){
			int count = 0;
			foreach(Edge item in allEdges){
				Console.Write ("(" + item.x + ", " + item.y + ")");
				count++;
			}
		}
		public int checkIfEdgeExists(Edge ed){
			foreach (Edge item in allEdges) {
				Console.WriteLine ("Edge: " + item.x + ", " + item.y);
				if ((item.x == ed.x && item.y == ed.y) || (item.x == ed.y && item.y == ed.x)) {
					Console.WriteLine ("Edge already exists! Make another move");
					return 0;
				}
			}
			Console.WriteLine ("Edge does not exist.");
			return 1;
		}
		public void printEdges(int player){
			int count = 0;
			if (player == 1) {
				Console.Write ("Player 1: ");
				foreach (Edge item in HumanPlayer) {
					Console.Write ("(" + item.x + ", " + item.y + ")");
					count++;
				}
			} else if (player == 2) {
				Console.Write ("Player 2: ");
				foreach (Edge item in AIPlayer) {
					Console.Write ("(" + item.x + ", " + item.y + ")");
					count++;
				}
			}
		}

		// if the current list of edges means we lose, returns true
		public bool checkIfLoss(){

			// check each edge we have made
			foreach (Edge item in AIPlayer)
			{
				foreach (Edge item2 in AIPlayer)
				{
					if (item2 != item && item.y == item2.x)
					{
						foreach(Edge item3 in AIPlayer)
						{
							if(item3 != item2 && item3 != item && item2.y == item3.x)
							{
								if (item3.y == item.x)
								{
									return true;
								}
							}
						}
					}
				}

			}

			return false;

		}
	}

	// AI makes a move, determining the best possible choice
	public bool legalMove(Edge check)
    {
        Edge check2 = new Edge(check.y, check.x, AIPlayer);
        foreach(Edge item in allEdges)
        {
            if ((check.x == item.x && check.y == item.y) || (check2.x == item.x && check2.y == item.y))
            {
                return false;
            }
        }

        return true;
    }

	// predicts possible moves to make using a specific edge given by CalculateMove()
	public bool checkMoveLookahead(Edge possibleMove){   
		// check each move we already made
		foreach(Edge item in AIPlayerEdges)
		{
			foreach(Edge item2 in AIPlayerEdges)
			{
				if(item2 != item)
				{
					// if these edges connect, we would lose
					if(item.y == item2.x && item2.y == possibleMove.x && possibleMove.y == item.x)
					{
						return true;
					}
					// checks the opposite case
					else if (item.x == item2.y && item2.x == possibleMove.y && possibleMove.x == item.y)
					{
						return true;
					}
				}
			}
		}

		return false;
	}


	//uses lookahead function to determine if this is a good move to make
	public void CalculateMove(int nodeOne){
		//at end, will contain all legal moves
		List<Edge> possibleMoves = new List<Edge>();
//		int count = 0; // determines if the edge has been made already

		//check all vertices in the game and makes predictive moves
		foreach (Vertex vertex in vertices)
		{
			//checks if move has been made already
			Edge possibleMove1 = new Edge(nodeOne, vertex.i, AIPlayer);
			Edge possibleMove2 = new Edge(vertex.i, nodeOne, AIPlayer);
/*			foreach (Edge item in AIPlayerEdges)
			{
				if (possibleMove1 == item || possibleMove2 == item)
				{
					count++;
				}
			}
*/
			//if new move, check if it is legal
			if (legalMove(possibleMove1) || legalMove(possibleMove2))
			{
				//if possible Move makes us lose, ignore it
				if (!checkMoveLookahead(possibleMove1))
				{
					possibleMoves.Add(possibleMove1);
				}
			}
		}
	}
		
	public void CalculateMove(){
		//at end, will contain all legal moves
		List<Edge> possibleMoves = new List<Edge>();
		int count = 0; // determines if the edge has been made already

		//check all vertices in the game and makes predictive moves
		foreach (Vertex vertex1 in vertices)
		{
			foreach (Vertex vertex2 in vertices)
			{
				//checks if move has been made already
				Edge possibleMove1 = new Edge(vertex1.i, vertex2.i, AIPlayer);
				Edge possibleMove2 = new Edge(vertex2.i, vertex1.i, AIPlayer);
				foreach (Edge item in AIPlayerEdges)
				{
					if (possibleMove1 == item || possibleMove2 == item)
					{
						count++;
					}
				}

				//if new move, check if it is legal
				if (count == 0)
				{
					//if possible Move makes us lose, ignore it
					if (!checkMoveLookahead(possibleMove1))
					{
						possibleMoves.Add(possibleMove1);
					}
				}
			}
		}
	}
}
