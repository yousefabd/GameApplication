using System;
using UnityEditor;
using UnityEngine;

class CharacterOpponentAI : MonoBehaviour 
{
    public enum SOLDIERSTATE { ATTACKING, HEALING };
    public static Vector2 FIRST_CELL_POSITION = new Vector2 (-38.5f,40.5f);
    public static Vector2 LAST_CELL_POSITION = new Vector2(-40.5f, -38.5f);
    public bool almostDead = false;
    // Node 
    // Node 
    public class Node
    {
        public Entity entity;

        // Lower values indicate 
        // higher priority 
        public int priority;

        public Node next;
        public Node () { }
    }

    public static Node node = new Node();

    // Function to Create A New Node 
    public static Node newNode(Entity e, int p)
    {
        Node temp = new Node();
        temp.entity= e;
        temp.priority = p;
        temp.next = null;

        return temp;
    }

    // Return the value at head 
    public static Entity peek(Node head)
    {
        return (head).entity;
    }

    // Removes the element with the 
    // highest priority from the list 
    public static Node pop(Node head)
    {
        (head) = (head).next;
        return head;
    }

    // Function to push according to priority 
    public static Node push(Node head,Entity e, int p)
    {
        Node start = (head);

        // Create new Node 
        Node temp = newNode(e, p);

        // Special Case: The head of list 
        // has lesser priority than new node. 
        // So insert new node before head node 
        // and change head node. 
        if ((head).priority > p)
        {

            // Insert New Node before head 
            temp.next = head;
            (head) = temp;
        }
        else
        {

            // Traverse the list and find a 
            // position to insert new node 
            while (start.next != null &&
                start.next.priority <= p)
            {
                start = start.next;
            }

            // Either at the ends of the list 
            // or at required position 
            temp.next = start.next;
            start.next = temp;
        }
        return head;
    }

    // Function to check is list is empty 
    public static bool isEmpty(Node head)
    {
        return ((head) == null) ? true : false;
    }
   
        public void Start()
        {
        Collider2D[] colliders;
        Node PriorityQueue = new Node();
        }
    public void CreatePlan(Node pq) 
    {
        if (!isEmpty(pq)) 
        {
            pq =null;
            pq.next = null;
        }
        if (!almostDead)
        {
            Collider2D[] currentColliders = Physics2D.OverlapAreaAll(FIRST_CELL_POSITION, LAST_CELL_POSITION);
            foreach (Collider2D col in currentColliders) 
            {
                Entity currentEntity = col.GetComponent<Entity>();
                if (currentEntity.GetTeam()!=null)
                { }
            }
        }
        
    }


    
}



