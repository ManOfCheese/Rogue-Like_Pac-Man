using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {

    T[] items;            //Array of all the heap items.
    int currenItemCount;  //Current amount of items on the heap.

    //Constructor
    public Heap(int maxHeapSize) {  
        items = new T[maxHeapSize];  //Initializes the items array with a size of maxHeapSize as passed into the constructor.
    }

    //Adds and item to the heap.
    public void Add(T item) {               
        item.HeapIndex = currenItemCount;  //The heap index is set to be equal to the current item count because it is the newest item.
        items[currenItemCount] = item;     //The item is assigned to the array at the index of currentItemCount.
        SortUp(item);                      //Execute SortUp to get it into the correct position in the heap.
        currenItemCount++;                 //Add one to the currentItemCount.
    }

    //Removes the first item from the heap.
    public T RemoveFirst() {            
        T firstItem = items[0];             //Create a variable of type T which is a reference to the first item, therfore index 0.
        currenItemCount--;                  //Subtract one from the current item amount.
        items[0] = items[currenItemCount];  //The last item of the heap is moved to index 0 to replace the first item.
        items[0].HeapIndex = 0;             //It's index is set to 0.
        SortDown(items[0]);                 //Execute SortDown to get it into the correct position in the heap.
        return firstItem;                   //Return the new first item.
    }

    //Updates item by executing SortUp on it.
    public void UpdateItem(T item) {
        SortUp(item);  //Execute SortUp on the item to get it into the correct position in the heap.
    }

    //Returns current amount of items in the heap.
    public int Count {
        get {
            return currenItemCount;  //Returns currentItemCount
        }
    }

    //Checks wether the heap contains a certain item.
    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);  //Returns true if it contains the item, false if it does not.
    }

    //Sorts a value high in the heap downwards by looking at its children.
    void SortDown(T item) {
        while (true) {                                     //Enter a loop.
            int childIndexLeft = item.HeapIndex * 2 + 1;   //Calculate the index of the left child.
            int childIndexRight = item.HeapIndex * 2 + 2;  //Calculate the index of the right child.
            int swapIndex = 0;                             //Set the swapindex to 0, the swapIndex is for knowing if and with which item to swap.

            if (childIndexLeft < currenItemCount) {                                       //If the index of the left child is less than the current item count
                swapIndex = childIndexLeft;                                               //it needs to be swapped.

                if (childIndexRight < currenItemCount) {                                  //If however the index of the right child is also less we need to check which has priority.
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {    //If the fCost of child index right is lower than that of child index left 
                        swapIndex = childIndexRight;                                      //change swap index to childIndexRight.
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) {                               //If the item being sorted has a higher fCost than the item indicated by the swap index.
                    Swap(item, items[swapIndex]);                                         //Swap 'em.
                }
                else {       //If the item being sorted has a lower fCost
                    return;  //Don't swap and return.
                }
            }
            else         //If the index of the left child is not less than currentItemCount.
            {
                return;  //No swap needs to occur.
            }
        }
    }
	
    //Sorts items up by looking at their parent.
    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;  //Calculates the index of the parent.

        while (true) {                               //Enter a loop.
            T parentItem = items[parentIndex];       //parenItem is the item contained in the array at the index just calculated.
            if (item.CompareTo(parentItem) > 0) {    //Compare the item to it's parent, if it's fCost is lower.
                Swap(item, parentItem);              //Swap ém.
            }
            else {                                   //If not
                break;                               //don't
            }
            
            parentIndex = (item.HeapIndex - 1) / 2;  //Recalculate parentIndex after the swap.
        }
    }

    //Swaps two items in the heap.
    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;     //Puts itemB in the array at the index of itemA.
        items[itemB.HeapIndex] = itemA;     //Does the same for itemA.
        int itemAIndex = itemA.HeapIndex;   //Temporarily stores the heapIndex for itemA.
        itemA.HeapIndex = itemB.HeapIndex;  //Set the heapIndex of itemA equal to that of itemb.
        itemB.HeapIndex = itemAIndex;       //Set the heapIndex of itemB equal to the integer we made earlier because we changed itemA's heapIndex already.
    }
}

//Interface to force inheritors to implement the heapIndex integer.
public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}
