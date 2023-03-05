#ifndef GRAPH_H
#define GRAPH_H

#include <list>
#include <queue>
#include <vector>
#include <functional>


template <class NodeType, class ArcType> class GraphArc;
template <class NodeType, class ArcType> class GraphNode;

template<class NodeType, class ArcType>
class NodeComparer
{
public:
    typedef GraphNode<NodeType, ArcType> Node;

    bool operator()(Node const* t_n1, Node const* t_n2) const
    {
        return t_n1->m_data.m_pathCost + t_n1->m_data.heuristic > t_n2->m_data.m_pathCost + t_n2->m_data.heuristic;
    }
};

// ---------------------------------------------------------------------
//  Name:           Graph
//  Description:    Manages the nodes and connections (arc) between them
// ---------------------------------------------------------------------
template<class NodeType, class ArcType>
class Graph
{
public:
    // typedef the classes to make our lives easier.
    typedef GraphArc<NodeType, ArcType> Arc;
    typedef GraphNode<NodeType, ArcType> Node;

    // Constructor and destructor functions
    Graph(int size);
    ~Graph();
    Graph(int size, std::vector<std::pair<Cell, int>>* impassCells);

    // Accessors
    Node* nodeIndex(int index) const
    {
        return m_nodes.at(index);
    }

    // Public member functions.
    bool addNode(NodeType data, int index);
    void removeNode(int index);
    void clearArcsAndNodes();
    bool addArc(int from, int to, ArcType weight);
    void removeArc(int from, int to);
    Arc* getArc(int from, int to);
    void clearMarks();
    void depthFirst(Node* node, std::function<void(Node*)> f_visit);

    void breadthFirst(Node* node, std::function<void(Node*)> f_visit);
    void adaptedBreadthFirst(Node* current, Node* goal, std::function<void(Node*)> f_visit);
    void UCS(Node* current, Node* goal, std::function<void(Node*)> f_visit, std::vector<Node*>& path);
    void aStar(Node* start, Node* dest, std::vector<Node*>& path);
private:


    // ----------------------------------------------------------------
    //  Description:    A container of all the nodes in the graph.
    // ----------------------------------------------------------------

    std::vector<Node*> m_nodes;
    std::vector<std::pair<Cell, int>>* m_impassCells;

};

// ----------------------------------------------------------------
//  Name:           Graph
//  Description:    Constructor, this constructs an empty graph
//  Arguments:      The maximum number of nodes.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
Graph<NodeType, ArcType>::Graph(int maxNodes) : m_nodes(maxNodes, nullptr)
{
}

// ----------------------------------------------------------------
//  Name:           ~Graph
//  Description:    destructor, This deletes every node
//  Arguments:      None.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
Graph<NodeType, ArcType>::~Graph() {
    for (int index = 0; index < m_nodes.size(); index++)
    {
        if (m_nodes[index] != nullptr)
        {
            delete m_nodes.at(index);
        }
    }
}

template<class NodeType, class ArcType>
inline Graph<NodeType, ArcType>::Graph(int maxNodes, std::vector<std::pair<Cell, int>>* impassCells) : m_nodes(maxNodes, nullptr), m_impassCells(impassCells)
{
}

// ----------------------------------------------------------------
//  Name:           addNode
//  Description:    This adds a node at a given index in the graph.
//  Arguments:      The first parameter is the data to store in the node.
//                  The second parameter is the index to store the node.
//  Return Value:   true if successful
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
bool Graph<NodeType, ArcType>::addNode(NodeType data, int index)
{
    bool nodeNotPresent = false;
    // find out if a node does not exist at that index.
    if (nullptr == m_nodes.at(index))
    {
        nodeNotPresent = true;
        // create a new node, put the data in it, and unmark it.
        m_nodes.at(index) = new Node;
        m_nodes.at(index)->m_data = data;
        m_nodes.at(index)->setMarked(false);
    }

    return nodeNotPresent;
}

// ----------------------------------------------------------------
//  Name:           removeNode
//  Description:    This removes a node from the graph
//  Arguments:      The index of the node to return.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::removeNode(int index)
{
    // Only proceed if node does exist.
    if (nullptr != m_nodes.at(index))
    {
        // now find every arc that points to the node that
        // is being removed and remove it.        
        Arc* arc = nullptr;

        // loop through every node
        for (int node = 0; node < m_nodes.size(); node++)
        {
            // if the node is valid...
            if (nullptr != m_nodes.at(node)) {
                // see if the node has an arc pointing to the current node.
                arc = m_nodes.at(node)->getArc(m_nodes.at(index));
            }
            // if it has an arc pointing to the current node, then
            // remove the arc.
            if (arc != 0) {
                removeArc(node, index);
            }
        }


    //    // now that every arc pointing to the current node has been removed,
    //    // the node can be deleted.
    //    delete m_nodes.at(index);
    //    m_nodes.at(index) = nullptr;
    }
}

template<class NodeType, class ArcType>
inline void Graph<NodeType, ArcType>::clearArcsAndNodes()
{
    int x = m_nodes.size();
    m_nodes.clear();
    m_nodes.resize(x);
}

// ----------------------------------------------------------------
//  Name:           addArd
//  Description:    Adds an arc from the first index to the 
//                  second index with the specified weight.
//  Arguments:      The first argument is the originating node index
//                  The second argument is the ending node index
//                  The third argument is the weight of the arc
//  Return Value:   true on success.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
bool Graph<NodeType, ArcType>::addArc(int from, int to, ArcType weight)
{
    bool proceed = true;
    // make sure both nodes exist.
    if (nullptr == m_nodes.at(from) || nullptr == m_nodes.at(to))
    {
        proceed = false;
    }

    // if an arc already exists we should not proceed
    if (m_nodes.at(from)->getArc(m_nodes.at(to)) != nullptr)
    {
        proceed = false;
    }

    if (proceed == true)
    {
        // add the arc to the "from" node.
        m_nodes.at(from)->addArc(m_nodes.at(to), weight);
    }

    return proceed;
}

// ----------------------------------------------------------------
//  Name:           removeArc
//  Description:    This removes the arc from the first index to the second index
//  Arguments:      The first parameter is the originating node index.
//                  The second parameter is the ending node index.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::removeArc(int from, int to)
{
    // Make sure that the node exists before trying to remove
    // an arc from it.
    bool nodeExists = true;
    if (nullptr == m_nodes.at(from) || nullptr == m_nodes.at(to))
    {
        nodeExists = false;
    }

    if (nodeExists == true)
    {
        //remove the arc.
        m_nodes.at(from)->removeArc(m_nodes.at(to));
    }
}


// ----------------------------------------------------------------
//  Name:           getArc
//  Description:    Gets a pointer to an arc from the first index
//                  to the second index.
//  Arguments:      The first parameter is the originating node index.
//                  The second parameter is the ending node index.
//  Return Value:   pointer to the arc, or 0 if it doesn't exist.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
GraphArc<NodeType, ArcType>* Graph<NodeType, ArcType>::getArc(int from, int to)
{
    Arc* arc = 0;
    // make sure the to and from nodes exist
    if (nullptr != m_nodes.at(from) && nullptr != m_nodes.at(to))
    {
        arc = m_nodes.at(from)->getArc(m_nodes.at(to));
    }

    return arc;
}


// ----------------------------------------------------------------
//  Name:           clearMarks
//  Description:    This clears every mark on every node.
//  Arguments:      None.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::clearMarks()
{
    for (int index = 0; index < m_nodes.size(); index++)
    {
        if (nullptr != m_nodes.at(index))
        {
            m_nodes.at(index)->setMarked(false);
            m_nodes.at(index)->setPrevious(nullptr);
        }
    }
}


// ----------------------------------------------------------------
//  Name:           depthFirst
//  Description:    Performs a depth-first traversal on the specified 
//                  node.
//  Arguments:      The first argument is the starting node
//                  The second argument is the processing function.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::depthFirst(Node* node, std::function<void(Node*)> f_visit)
{
    if (nullptr != node) {
        // process the current node and mark it
        f_visit(node);
        node->setMarked(true);

        // go through each connecting node
        auto iter = node->arcList().begin();
        auto endIter = node->arcList().end();

        for (; iter != endIter; ++iter)
        {
            // process the linked node if it isn't already marked.
            if ((*iter).node()->marked() == false)
            {
                depthFirst((*iter).node(), f_visit);
            }
        }
    }
}


// ----------------------------------------------------------------
//  Name:           breadthFirst
//  Description:    Performs a depth-first traversal the starting node
//                  specified as an input parameter.
//  Arguments:      The first parameter is the starting node
//                  The second parameter is the processing function.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::breadthFirst(Node* node, std::function<void(Node*)> f_visit)
{
    if (nullptr != node)
    {
        std::queue<Node*> nodeQueue;
        // place the first node on the queue, and mark it.
        nodeQueue.push(node);
        node->setMarked(true);

        // loop through the queue while there are nodes in it.
        while (nodeQueue.size() != 0)
        {
            // process the node at the front of the queue.
            f_visit(nodeQueue.front());

            // add all of the child nodes that have not been 
            // marked into the queue
            auto iter = nodeQueue.front()->arcList().begin();
            auto endIter = nodeQueue.front()->arcList().end();

            for (; iter != endIter; iter++)
            {
                if ((*iter).node()->marked() == false)
                {
                    // mark the node and add it to the queue.
                    (*iter).node()->setMarked(true);
                    nodeQueue.push((*iter).node());
                }
            }

            // dequeue the current node.
            nodeQueue.pop();
        }
    }
}


// ----------------------------------------------------------------
//  Name:           adaptedBreadthFirst
//  Description:    Performs a breadth-first traversal the starting node
//                  specified as an input parameter, terminating at the goal.
//  Arguments:      The first parameter is the starting node.
//                  The second parameter is the goal node.
//  Return Value:   None.
// ----------------------------------------------------------------
template<class NodeType, class ArcType>
void Graph<NodeType, ArcType>::adaptedBreadthFirst(Node* current, Node* goal, std::function<void(Node*)> f_visit)
{
    bool goalReached = false;

    if (nullptr != current)
    {
        std::queue<Node*> nodeQueue;
        // place the first node on the queue, and mark it.
        nodeQueue.push(current);
        current->setMarked(true);

        // loop through the queue while there are nodes in it.
        while (nodeQueue.size() != 0 && goalReached == false)
        {
            //process the node at the front of the queue.
            f_visit(nodeQueue.front());

            // add all of the child nodes that have not been 
            // marked into the queue
            auto iter = nodeQueue.front()->arcList().begin();
            auto goalIter = nodeQueue.front()->arcList().end();

            for (; iter != goalIter && goalReached == false; iter++)
            {
                Arc arc = (*iter);
                if (arc.node() == goal)
                {
                    goalReached = true;
                    goal->setPrevious(nodeQueue.front());
                }
                else if (arc.node()->marked() == false)
                {
                    arc.node()->setPrevious(nodeQueue.front());
                    // mark the node and add it to the queue.
                    arc.node()->setMarked(true);
                    nodeQueue.push(arc.node());
                }
            }
            // dequeue the current node.
            nodeQueue.pop();
        }
    }
    Node* temp = goal;

    for (; temp->previous() != nullptr;)
    {
        std::cout << temp->m_data.m_name << std::endl;
        temp = temp->previous();
    }
    std::cout << temp->m_data.m_name << std::endl;
}

template<class NodeType, class ArcType>
inline void Graph<NodeType, ArcType>::UCS(Node* current, Node* goal, std::function<void(Node*)> f_visit, std::vector<Node*>& path)
{
    std::priority_queue<Node*, std::vector<Node*>, NodeComparer<NodeType, ArcType> > pq;
    // Initialise all pathcosts to infinity
    for (Node* node : m_nodes)
    {
        node->m_data.m_pathCost = std::numeric_limits<int>::max();
    }
    // Init cost to start as zero
    current->m_data.m_pathCost = 0;

    // Add current to queue
    pq.push(current);
    current->setMarked(true);

    while (!pq.empty() && pq.top() != goal)
    {
        auto iter = pq.top()->arcList().begin();
        auto goalIter = pq.top()->arcList().end();
        // Process each child node
        // The iter is the arc  - each arc has an endpoint which is the child node
        for (; iter != goalIter; iter++)
        {
            int arcWeight = iter->weight();
            int distChild = arcWeight + pq.top()->m_data.m_pathCost;
            // Get the child node from the arc
            Node* child = iter->node();

            //std::cout << child->m_data.m_name << std::endl;
            if (distChild < child->m_data.m_pathCost)
            {
                child->m_data.m_pathCost = distChild;
                child->setPrevious(pq.top());
            }
            if (!child->marked())
            {
                pq.push(child);
                child->setMarked(true);
            }
            //child->setPrevious(pq.top());
        }
        pq.pop();
    }

    Node* temp = goal;

    for (; temp->previous() != nullptr;)
    {
        path.push_back(temp);
        //std::cout << temp->m_data.m_name << std::endl;
        temp = temp->previous();
    }
    path.push_back(temp);
    //std::cout << temp->m_data.m_name << std::endl;
}

template<class NodeType, class ArcType>
inline void Graph<NodeType, ArcType>::aStar(Node* start, Node* dest, std::vector<Node*>& path)
{
    std::vector<int> affectedCells;

    for (std::pair<Cell, int> i : *m_impassCells)
    {
        std::vector<int> neighbours = i.first.neighbours();

        affectedCells.insert(affectedCells.end(), neighbours.begin(), neighbours.end());
    }

    std::priority_queue<Node*, std::vector<Node*>, NodeComparer<NodeType, ArcType> > pq;
    // Initialise all pathcosts to infinity
    for (Node* node : m_nodes)
    {
        node->m_data.m_pathCost = std::numeric_limits<int>::max();
        double diffxy = abs((node->m_data.xPos - dest->m_data.xPos) * (node->m_data.xPos - dest->m_data.xPos) + (node->m_data.yPos - dest->m_data.yPos) * (node->m_data.yPos - dest->m_data.yPos));
        node->m_data.heuristic = sqrt(diffxy);
        //int x = 0;
        //node->m_data.heuristic = (std::sqrt((node->m_data.xPos - dest->m_data.xPos) * (node->m_data.xPos - dest->m_data.xPos) - (node->m_data.yPos - dest->m_data.yPos) * (node->m_data.yPos - dest->m_data.yPos)));     
    }
    // Init cost to start as zero
    start->m_data.m_pathCost = 0;

    // Add current to queue
    pq.push(start);
    start->setMarked(true);

    while (!pq.empty() && pq.top() != dest)
    {
        //if (pq.top()->m_data.m_name == "l")
        //{
        //    int x = 0;
        //}
        auto iter = pq.top()->arcList().begin();
        auto goalIter = pq.top()->arcList().end();

        // Process each child node
        // The iter is the arc  - each arc has an endpoint which is the child node
        for (; iter != goalIter; iter++)
        {
            bool validCell = true;

            iter->node();

            Node* child = iter->node();

            if (child->previous() != pq.top())
            {
                for (int smokeCell : affectedCells)
                {
                    if (std::stoi(child->m_data.m_name) == smokeCell)
                    {
                        validCell = false;
                        break;
                    }
                }

                if (validCell)
                {
                    int arcWeight = iter->weight();
                    int distChild = arcWeight + pq.top()->m_data.m_pathCost;
                    // Get the child node from the arc

                    if (distChild < child->m_data.m_pathCost)
                    {
                        child->m_data.m_pathCost = distChild;
                        child->setPrevious(pq.top());
                    }
                    if (!child->marked())
                    {
                        pq.push(child);
                        child->setMarked(true);
                    }
                }
            }
        }

        pq.pop();
    }

    Node* temp = dest;

    for (; temp->previous() != nullptr;)
    {
        path.push_back(temp);
        //std::cout << temp->m_data.m_name << " || " << temp->m_data.heuristic + temp->m_data.m_pathCost << std::endl;
        temp = temp->previous();
    }
    path.push_back(temp);
    //std::cout << temp->m_data.m_name << " || " << temp->m_data.heuristic << std::endl;
}



#include "GraphNode.h"
#include "GraphArc.h"


#endif

