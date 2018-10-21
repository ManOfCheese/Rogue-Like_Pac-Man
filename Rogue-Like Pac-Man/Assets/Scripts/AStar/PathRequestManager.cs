using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();  //A queue holding all the request for pathfinding.
    PathRequest currentPathRequest;                                  //The current pathRequest we are handling.

    static PathRequestManager instance;                              //Singleton so that any unit can request a path.
    Pathfinding pathfinding;                                         //A reference to the pathfinding script.

    bool isProcessingPath;

    private void Awake() {
        instance = this;                            //Put this object in the Singleton.
        pathfinding = GetComponent<Pathfinding>();  //Get the pathfinindg component.
    }

    //Requests a path.
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);  //Make a new path request with the values that are passed in.
        instance.pathRequestQueue.Enqueue(newRequest);                           //Enqueue this new request.
        instance.TryProcessNext();                                               //Try to process it.
    }

    //Try to process the next path request.
    void TryProcessNext() {        
        if (!isProcessingPath && pathRequestQueue.Count > 0) {                                    //If there is not already a path being processed and the queue isn't empty.
            currentPathRequest = pathRequestQueue.Dequeue();                                      //Dequeue the path request.
            isProcessingPath = true;                                                              //We are now processing the path.
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);  //Execute StartFindPath in the pathfinder class and pass in the values we got.
        }
    }

    //When a path has been processed:
    public void FinishedProcessingPath(Vector3[] path, bool succes) {
        currentPathRequest.callback(path, succes);  //Gets the path and wether or not the pathfinding was a succes from the pathfinder script.
        isProcessingPath = false;                   //We are now no longer processing a path.
        TryProcessNext();                           //Try and process the next path.
    }

    //Structure for storing the data necessary for a path request.
    struct PathRequest {
        public Vector3 pathStart;                 //Start of the path.
        public Vector3 pathEnd;                   //End of the path.
        public Action<Vector3[], bool> callback;  //Action for when the path is returned.

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
