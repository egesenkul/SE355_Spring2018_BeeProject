﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a vertex class that has the vertex and the edges between other vertexes.

public class Vertex : MonoBehaviour {

	//Vertex will contain an edgelist and a method to make the connections.
	public List<Edge> edgeList = new List<Edge>();
	
	//Method to add the edges.
	public void AddEdge(int weight, Vertex vertex)
	{
		Edge tempEdge = new Edge(weight, vertex);
		edgeList.Add(tempEdge);
		Edge otherEdge = new Edge(weight, this);
		vertex.edgeList.Add(otherEdge);
	}

}

/*public class Edge
{
	private int weight = 0;
	private Vertex vertex;
	
	public Edge(int weight,Vertex vertex)
	{
		Debug.Log("Edge creation.");
		this.weight = weight;
		this.vertex = vertex;
	}
}*/