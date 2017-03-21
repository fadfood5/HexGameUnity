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
	public int AIPlayer = 0;
    public int HumanPlayer;
/*	public List<Vertex> vertices;
	public List<Edge> allPossibleMoves;
	public List<Edge> allEdges = new List<Edge>();
    public List<Edge> HumanPlayer = new List<Edge>();
	public List<Edge> AIPlayerEdges = new List<Edge>();*/
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
//		Debug.Log ("Make Line");
		float x1 = this.transform.Find ("Points").FindChild (i.ToString ()).transform.position.x;
//		Debug.Log (x1);
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

        if (p.checkIfLoss())
        {
            Debug.Log("Game is over. Player " + currentPlayer + " lost.");
			debug("Game is over. Player " + currentPlayer + " lost.");
			this.transform.Find ("Buttons").gameObject.SetActive (false);
            //          while(true)
            //        {
            //            Debug.Log("End.");
            //      }
        }

        else
        {
            changeTurn();
        }
	}

	public void debug(string t){
		this.gameObject.transform.Find ("Debugger").GetComponentInChildren<Text> ().text = t;
	}

    
    //Game Start Point
	public void choosePlayer(int i){
		isPlayerOneAI = i;
		currentPlayer = 1;
		if (isPlayerOneAI == 0) {
			debug ("AI is player one");
            AIPlayer = 1;
            HumanPlayer = 2;
			AI ();
		} else {
			debug ("AI is player two. Your turn.");
            AIPlayer = 2;
            HumanPlayer = 1;
		}
		this.gameObject.transform.Find ("ChoosePlayer").gameObject.SetActive (false);

		//MAIN
		for (int j = 1; j < 7; j++) {
			p.addVertex (j);
		}
//		Edge ed = new Edge (0, 1, 1);
//		p.addEdge (ed, 1);
//		p.checkIfEdgeExists (ed);

	}

	public int AI(){
//        Debug.Log(currentPlayer);
          Debug.Log("AI Turn.");
		this.gameObject.transform.Find ("Buttons").gameObject.SetActive (false);
        if (currentPlayer == AIPlayer)
        {
            if (firstMoves == true)
            {
                System.Random rnd = new System.Random();
                int r1 = rnd.Next(1, 6);
                int r2 = rnd.Next(1, 6);

                // ensures the numbers are not the same (i.e. 2,2)
                while (r1 == r2)
                {
                    r2 = rnd.Next(1, 6);
                }

                //			Debug.Log (r1);
                //			Debug.Log (r2);
                Edge AIEdge = new Edge(r1, r2, AIPlayer);
                MakeLine(r1, r2);
                p.AIPlayerEdges.Add(AIEdge);
                p.allEdges.Add(AIEdge);
                Debug.Log("AI Edge: (" + r1 + ", " + r2 + ")");
                Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
                firstMoves = false;

            }
            else
            {
//                Debug.Log("First move already made.");
                int firstNode = 0;
                int secondNode = 0;


                //ensures that the AI is supposed to move now

                // checks every vertex possible
          //      Debug.Log("Vertices: " + p.vertices.Count);
                if (p.vertices.Count != 0)
                {
                    foreach (Vertex vertex in p.vertices)
                    {
                        int count = 0;
                        //checks all edges that have already been made
                    //    Debug.Log("Edge check: number is " + p.allEdges.Count);
                        if (p.allEdges.Count != 0)
                        {
                            foreach (Edge edgeItem in p.allEdges)
                            {
                                // checks if the current vertex has been used already
                       //         Debug.Log("Comparing vertex " + vertex.i + " to edge (" + edgeItem.x + ", " + edgeItem.y + ")");
                                if (vertex.i == edgeItem.x || vertex.i == edgeItem.y)
                                {

                                    count++;
                                }

                            }

                         //   Debug.Log("Possible move: (" + firstNode + ", " + secondNode + ")");
                            if (count == 0)
                            {
                                if (firstNode == 0)
                                {
                           //         Debug.Log("Add first vertex.");
                                    firstNode = vertex.i;
                                }
                                else if (firstNode != 0 && secondNode == 0)
                                {
                            //        Debug.Log("Add second vertex.");
                                    secondNode = vertex.i;
                                }

                            }

                            else
                            {

                                //make some sub-optimal move
                            }

                        } // end edge count check

                    } // end vertex check

                } // end vertices count check

                            // if both vertices have not been used already, it is an optimal move
                            if (firstNode != 0 && secondNode != 0)
                            {
                                //make move with firstNode and secondNode
                          //      Debug.Log("Two empty nodes found.");
                                Edge edgeMove = new Edge(firstNode, secondNode, AIPlayer);
                                p.allEdges.Add(edgeMove);
                                p.AIPlayerEdges.Add(edgeMove);
                                MakeLine(firstNode, secondNode);
                                Debug.Log("AI Edge: (" + firstNode + ", " + secondNode + ")");
                          //      Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
                            }

                            //if we find one vertex that has been used, we make do with the open vertex
                            else if (firstNode != 0 && secondNode == 0)
                            {
                    //choose a vertex we already picked, using lookahead function
                    Debug.Log("Calculating move with one possible node: " + firstNode);
                                CalculateMove(firstNode);
                            }

                            // if no vertex is free, make a move with what has already been made
                            else if (firstNode == 0 && secondNode == 0)
                            {
                    //use lookahead function, find optimal move with all possible moves to make
                    Debug.Log("Calculating move, considering all possibilites");
                                CalculateMove();
                            }
                }
             
        }
		Debug.Log("Your turn.");
		this.gameObject.transform.Find ("Buttons").gameObject.SetActive (true);
		return 0;
	}

	public void click1(Button temp){
		int num = Int32.Parse (temp.transform.Find ("Text").GetComponent<Text> ().text);
		if (newEdge.Count == 0) {
			newEdge.Add(num);
		} else {
			newEdge.Add(num);
//			Debug.Log ("(" + newEdge [0] + ", " + newEdge[1] + ")");
            Edge edgeCheck = new global::run.Edge(newEdge[0], newEdge[1], currentPlayer);
            bool comparison = p.checkIfEdgeExists(edgeCheck);
            if (comparison == true) // if edge exists, have human player restart turn
            {
                newEdge.Clear();
//				debug("Edge already exists. Try another move.");
            }

            else
            {
                p.HumanPlayer.Add(edgeCheck);
                p.allEdges.Add(edgeCheck); 
                MakeLine(newEdge[0], newEdge[1]);
                newEdge.Clear();
            }
		}
	}

	public void changeTurn(){
		if (currentPlayer == 1) {
			currentPlayer = 2;
			//Debug.Log ("Changed player to 2");
			//Debug.Log ("currentPlayer " + currentPlayer);
			//Debug.Log ("isPlayerOneAI " + isPlayerOneAI);
			if (isPlayerOneAI == 0)
				debug ("Your turn.");
			else {
				debug ("AI's turn.");
				AI ();
			}
		} else if(currentPlayer == 2){
			currentPlayer = 1;
			//Debug.Log ("Changed player to 1");
			//Debug.Log ("currentPlayer " + currentPlayer);
			//Debug.Log ("isPlayerOneAI " + isPlayerOneAI);
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
		public List<Vertex> vertices;
		public List<Edge> allEdges;
		public List<Edge> HumanPlayer;
		public List<Edge> AIPlayerEdges;

		public Hexagon(int v){
			//Create new lists to keep track of edges
			vertices = new List<Vertex>();
			allEdges = new List<Edge>();
			HumanPlayer = new List<Edge>();
			AIPlayerEdges = new List<Edge>();
		}

		public void addEdge(Edge i, int player){

			//Create temp Edge Object
			Edge newEdge = new Edge(i.x, i.y, player);

			//Add edge to corresponding list
			if (player == 1)
				HumanPlayer.Add (newEdge);
			else if (player == 2)
				AIPlayerEdges.Add (newEdge);

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
		public bool checkIfEdgeExists(Edge ed){
            Debug.Log(allEdges.Count);
            if(ed.x == ed.y)
            {
                Debug.Log("Cannot use same vertex to make an edge. Make another move.");
                return true;
            }
			foreach (Edge item in allEdges) {
				Debug.Log ("Edge: " + item.x + ", " + item.y);
				if ((item.x == ed.x && item.y == ed.y) || (item.x == ed.y && item.y == ed.x)) {
					Debug.Log ("Edge already exists! Make another move");
					return true;
				}
			}
			Debug.Log ("Edge does not exist.");
			return false;
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
				foreach (Edge item in AIPlayerEdges) {
					Console.Write ("(" + item.x + ", " + item.y + ")");
					count++;
				}
			}
		}

		// if the current list of edges means we lose, returns true
		public bool checkIfLoss(){
 //           List<Edge> checkEdges;

			// check each edge we have made
			foreach (Edge item in allEdges)
			{
				foreach (Edge item2 in allEdges)
				{
					if (item2 != item && (item.x == item2.x || item.y == item2.y || item.x == item2.y || item.y == item2.x) && item2.z == item.z)
					{
						foreach(Edge item3 in allEdges)
						{
							if(item3 != item2 && item3 != item && (item2.x == item3.x || item2.x == item3.y || item2.y == item3.x || item2.y == item3.y) && item3.z == item.z)
							{
								if (item3.x == item.x || item3.x == item.y || item3.y == item.x || item3.y == item.y)
								{
                                    //in order for there to be a triangle, there must only be three points
                                    // if there is a fourth point, there is no triangle
                                    if (((item3.x != item.x && item3.x != item.y) && (item3.x != item2.x && item3.x != item2.y)) ||
                                       (item3.y != item.x && item3.y != item.y && item3.y != item2.x && item3.y != item2.y))
                                    {
                                    //    Debug.Log("False positive.");
                                    }

                                    else
                                    {
                                        Debug.Log("Current player: ");
                                        Debug.Log("(" + item.x + ", " + item.y + ", " + item.z + ")");
                                        Debug.Log("(" + item2.x + ", " + item2.y + ", " + item.z + ")");
                                        Debug.Log("(" + item3.x + ", " + item3.y + ", " + item.z + ")");
                                        return true;
                                    }
								}
							}
						}
					}
				}

			}
//            Debug.Log("No loss. Continue.");
			return false;

		}
	}

	// AI makes a move, determining the best possible choice
	public bool legalMove(Edge check)
    {
     //   Debug.Log("Checking if (" + check.x + ", " + check.y + ") is a good move.");
        Edge check2 = new Edge(check.y, check.x, AIPlayer);
        foreach(Edge item in p.allEdges)
        {
            if ((check.x == item.x && check.y == item.y) || (check2.x == item.x && check2.y == item.y))
            {
     //           Debug.Log("(" + check.x + ", " + check.y + ") is not a good move." );
                return false;
            }
        }
   //     Debug.Log("(" + check.x + ", " + check.y + ") is a good move.");
        return true;
    }

    public bool PossibleLegalMove(List<Edge> domain, Edge check)
    {
        //   Debug.Log("Checking if (" + check.x + ", " + check.y + ") is a good move.");
        Edge check2 = new Edge(check.y, check.x, AIPlayer);
        foreach (Edge item in domain)
        {
            if ((check.x == item.x && check.y == item.y) || (check2.x == item.x && check2.y == item.y))
            {
                //           Debug.Log("(" + check.x + ", " + check.y + ") is not a good move." );
                return false;
            }
        }
        //     Debug.Log("(" + check.x + ", " + check.y + ") is a good move.");
        return true;
    }

    // predicts possible moves to make using a specific edge given by CalculateMove()
    public bool checkMoveLookahead(Edge possibleMove, int player){
        List<Edge> edgeChecker = new List<Edge>();
        if (player == AIPlayer)
            edgeChecker = p.AIPlayerEdges;
        else if (player == HumanPlayer)
            edgeChecker = p.HumanPlayer;


		// check each move we already made
		foreach(Edge item in edgeChecker)
		{

			foreach(Edge item2 in edgeChecker)
			{

				if(item2 != item && (item.x == item2.x || item.y == item2.y || item.x == item2.y || item.y == item2.x))
				{

                    // if these edges connect, we would lose
                    if (possibleMove != item2 && possibleMove != item && (item2.x == possibleMove.x || item2.x == possibleMove.y 
                        || item2.y == possibleMove.x || item2.y == possibleMove.y))
                    {

                        if (possibleMove.x == item.x || possibleMove.x == item.y || possibleMove.y == item.x || possibleMove.y == item.y)
                        {

                            if (((possibleMove.x != item.x && possibleMove.x != item.y) && (possibleMove.x != item2.x && possibleMove.x != item2.y)) ||
                                       (possibleMove.y != item.x && possibleMove.y != item.y && possibleMove.y != item2.x && possibleMove.y != item2.y))
                            { }

                            else
                            { return true; }
                        }
					}
				}
			}
		}

		return false;
	}

    public bool predictioncheckMoveLookahead(List<Edge> domain, Edge possibleMove, int player)
    {


        // check each move we already made
        foreach (Edge item in domain)
        {

            foreach (Edge item2 in domain)
            {

                if (item2 != item && (item.x == item2.x || item.y == item2.y || item.x == item2.y || item.y == item2.x))
                {

                    // if these edges connect, we would lose
                    if (possibleMove != item2 && possibleMove != item && (item2.x == possibleMove.x || item2.x == possibleMove.y
                        || item2.y == possibleMove.x || item2.y == possibleMove.y))
                    {

                        if (possibleMove.x == item.x || possibleMove.x == item.y || possibleMove.y == item.x || possibleMove.y == item.y)
                        {

                            if (((possibleMove.x != item.x && possibleMove.x != item.y) && (possibleMove.x != item2.x && possibleMove.x != item2.y)) ||
                                       (possibleMove.y != item.x && possibleMove.y != item.y && possibleMove.y != item2.x && possibleMove.y != item2.y))
                            { }

                            else
                            { return true; }
                        }
                    }
                }
            }
        }

        return false;
    }


    //uses lookahead function to determine if this is a good move to make
    public void CalculateMove(int nodeOne){
        //at end, will contain all legal moves
        Debug.Log("Initiate oneNode calculation");
        List<Edge> possibleMoves = new List<Edge>();
//		int count = 0; // determines if the edge has been made already

		//check all vertices in the game and makes predictive moves
		foreach (Vertex vertex in p.vertices)
		{
			//checks if move has been made already
			Edge possibleMove1 = new Edge(nodeOne, vertex.i, AIPlayer);
			Edge possibleMove2 = new Edge(vertex.i, nodeOne, AIPlayer);

			//if new move, check if it is legal
			if (legalMove(possibleMove1) || legalMove(possibleMove2))
			{
				//if possible Move makes us lose, ignore it
				if (!checkMoveLookahead(possibleMove1, AIPlayer))
				{
					possibleMoves.Add(possibleMove1);
				}

                else
                {
                    Debug.Log("(" + possibleMove1.x + ", " + possibleMove1.y + ") would make us lose. Ignore.");
                }
			}
		}

        Edge useMove = possibleMoves.First();
        p.AIPlayerEdges.Add(useMove);
        p.allEdges.Add(useMove);
        MakeLine(useMove.x, useMove.y);

        Debug.Log("AI Edge: (" + useMove.x + ", " + useMove.y + ")");
 //       Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
        possibleMoves.Clear();
    }
		
	public void CalculateMove(){
        //at end, will contain all legal moves
        Debug.Log("Initiate noNode calculation.");
        List<Edge> possibleMoves = new List<Edge>();
        List<Edge> failureMoves = new List<Edge>();

        //check all vertices in the game and makes predictive moves
        foreach (Vertex vertex1 in p.vertices)
        {
            foreach (Vertex vertex2 in p.vertices)
            {

                if (vertex1 != vertex2)
                {
                    //checks if move has been made already
                    Edge possibleMove1 = new Edge(vertex1.i, vertex2.i, AIPlayer);
                    Edge possibleMove2 = new Edge(vertex2.i, vertex1.i, AIPlayer);
                    foreach (Edge item in p.AIPlayerEdges)
                    {

                        if (legalMove(possibleMove1) || legalMove(possibleMove2))
                        {
                            //if possible Move makes us lose, ignore it
                            if (!checkMoveLookahead(possibleMove1, AIPlayer))
                            {
                                possibleMoves.Add(possibleMove1);
                            }

                            else
                            {
                                Debug.Log("(" + possibleMove1.x + ", " + possibleMove1.y + ") would make us lose.");
                                failureMoves.Add(possibleMove1);

                            }
                        }
                    }

                    
                }
            }

        }
        if (possibleMoves.Count == 0)
        {
            Debug.Log("No moves can be made without losing.");
            p.AIPlayerEdges.Add(failureMoves.Last());
            p.allEdges.Add(failureMoves.Last());
            MakeLine(failureMoves.Last().x, failureMoves.Last().y);
            Debug.Log("AI Edge: (" + failureMoves.Last().x + ", " + failureMoves.Last().y + ")");

        }
        else
        {
            Edge useMove = possibleMoves.First();
            useMove = AILookAhead(possibleMoves);
            p.AIPlayerEdges.Add(useMove);
            p.allEdges.Add(useMove);
            MakeLine(useMove.x, useMove.y);
            Debug.Log("AI Edge: (" + useMove.x + ", " + useMove.y + ")");
            Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
            possibleMoves.Clear();

        }
    }

    public Edge HumanCalculateMoves(List<Edge> moves, Edge AIMove)
    {
        //at end, will contain all legal moves
        Debug.Log("HumanCalculateMoves()");
        List<Edge> possibleMoves = new List<Edge>();
        Edge useMove = new Edge(0,0,0);
        //check all vertices in the game and makes predictive moves
        foreach (Vertex vertex1 in p.vertices)
        {
            foreach (Vertex vertex2 in p.vertices)
            {

                if (vertex1 != vertex2)
                {
                    
                    int playerTest = 0;
                    //checks if move has been made already
                    if (AIPlayer == 2)
                    {
                        Debug.Log("AI went second.");
                        playerTest = 1;
                    }
                    else if (AIPlayer == 1)
                    {
                        Debug.Log("AI went first.");

                        playerTest = 2;
                    }
                    if ((vertex1.i != AIMove.x || vertex2.i != AIMove.y))
                    {
                        Debug.Log("PlayerTest: " + playerTest);
                        Edge possibleMove1 = new Edge(vertex1.i, vertex2.i, playerTest);
                        Edge possibleMove2 = new Edge(vertex2.i, vertex1.i, playerTest);

                            if (PossibleLegalMove(moves, possibleMove1) || PossibleLegalMove(moves, possibleMove2))
                            {
                                //if possible Move makes us lose, ignore it
                                if (!predictioncheckMoveLookahead(p.HumanPlayer, possibleMove1, playerTest))
                                {
                                    possibleMoves.Add(possibleMove1);
                                }

                                else
                                {
                                    Debug.Log("(" + possibleMove1.x + ", " + possibleMove1.y + ") would make us lose. Ignore.");
                                }
                            }
                        }
                    }

                
            }

        }
        if (possibleMoves.Count == 0)
        {
            Debug.Log("No moves can be made, make random.");

        }
        else
        {
            Debug.Log(possibleMoves.First().x + "," + possibleMoves.First().y + "," + possibleMoves.First().z);
            useMove = possibleMoves.First();
//            AILookAhead(possibleMoves);
            /*            MakeLine(useMove.x, useMove.y);
                        p.AIPlayerEdges.Add(useMove);
                        p.allEdges.Add(useMove);
                        Debug.Log("AI Edge: (" + useMove.x + ", " + useMove.y + ")");
                        Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
                        possibleMoves.Clear();*/

        

        }

        return useMove;

    }

    public Edge predictionMove(List<Edge> domain, List<Edge> AICurrentMoves, Edge futureHumanMove)
    {
        Debug.Log("PredicitionMove()");
        List<Edge> possibleMoves = new List<Edge>();
        List<Edge> failureMove = new List<Edge>();
   //     Edge useMove = new Edge(0, 0, AIPlayer);
        foreach (Vertex vertex1 in p.vertices)
        {
            foreach (Vertex vertex2 in p.vertices)
            {

                if (vertex1 != vertex2)
                {
                 
                    if (vertex1.i != futureHumanMove.x || vertex2.i != futureHumanMove.y)
                    {
                        Edge possibleMove1 = new Edge(vertex1.i, vertex2.i, AIPlayer);
                        Edge possibleMove2 = new Edge(vertex2.i, vertex1.i, AIPlayer);

                            if (PossibleLegalMove(domain, possibleMove1) || PossibleLegalMove(domain, possibleMove2))
                            {
                                //if possible Move makes us lose, ignore it
                                if (!predictioncheckMoveLookahead(AICurrentMoves, possibleMove1, AIPlayer))
                                {
                                    possibleMoves.Add(possibleMove1);
                                }

                                else
                                {
                                    Debug.Log("(" + possibleMove1.x + ", " + possibleMove1.y + ") would make us lose in the future.");
                                    failureMove.Add(possibleMove1);

                                }
                            }
                        

                    }
                }
            }

        }
        if (possibleMoves.Count == 0)
        {
            Debug.Log("No moves can be made without losing.");
            return failureMove.Last();

        }
        else
        {
//            useMove = possibleMoves.First();
            //            AILookAhead(possibleMoves);
            /*            MakeLine(useMove.x, useMove.y);
                        p.AIPlayerEdges.Add(useMove);
                        p.allEdges.Add(useMove);
                        Debug.Log("AI Edge: (" + useMove.x + ", " + useMove.y + ")");
                        Debug.Log("AI Edges: " + p.AIPlayerEdges.Count);
                        possibleMoves.Clear();*/



        }

        return possibleMoves.First();

    }

    public Edge AILookAhead( List<Edge> possibleMoves)
    {
        Debug.Log("AILookAhead()");
        Edge idealMove = possibleMoves.First();
        List<Edge> humanEdges = p.HumanPlayer.ToList();
        List<Edge> allNewEdges = p.allEdges.ToList();
        allNewEdges.Add(idealMove);
        Debug.Log("Possible move is: (" + idealMove.x + ", " + idealMove.y + ")");
        Edge playerMove = HumanCalculateMoves(allNewEdges, idealMove);
        List<Edge> futureAIMoves = p.AIPlayerEdges.ToList();
        futureAIMoves.Add(idealMove);
        allNewEdges.Add(playerMove);

        idealMove = predictionMove(allNewEdges, futureAIMoves, playerMove);



        return idealMove;
    }
}
