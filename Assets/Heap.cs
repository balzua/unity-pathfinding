using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    private int count;

    public Heap(int maxHeapSize) 
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        // To insert the item, place it at the end of the heap
        // Remember to update that item's heapIndex accordingly
        item.HeapIndex = count;
        items[count] = item;
        // However, the item doesn't necessarily belong at the end. We must check that its parent's value priority is less than its value.
        // This handled by a helper function BubbleUp() below
        BubbleUp(item);
        count++;
    }

    public T GetMinimum()
    {
        T minimum = items[0];
        count--;
        // Move the last item in the array to the 0th position, then sort down to move it into its proper place.
        items[0] = items[count];
        items[0].HeapIndex = 0;
        BubbleDown(items[0]);
        return minimum;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public int Count
    {
        get
        {
            return count;
        }
    }

    public void UpdateItem(T item)
    {
        // If an item priority has changed, trigger a resort. Note that in the pathfinding, items can only increase priority, meaning it should only move upwards
        BubbleUp(item);
    }

    private void BubbleDown(T item)
    {
        // We need to check if the item is greater than either child. Left child of item at index n has index 2n+1, right child has 2n+2.
        while (true)
        {
            int leftChildIndex = item.HeapIndex * 2 + 1;
            int rightChildIndex = item.HeapIndex * 2 + 2;

            // This will be set to the index of the item the parent needs to be swapped with.
            int swapIndex = 0;
            // To determine which child (if any) needs to be swapped with, we need to check if the item has a left / right child, and which one has higher priority
            if (leftChildIndex < count)
            {
                swapIndex = leftChildIndex;
                if (rightChildIndex < count)
                {
                    if (items[leftChildIndex].CompareTo(items[rightChildIndex]) < 0)
                    {
                        // The left child has higher priority than the right child, so the right child has priority.
                        swapIndex = rightChildIndex;
                    } 
                }
                // The swapIndex now contains the index of the child with higher priority; now check that this child has higher priority than its parent.
                if (items[item.HeapIndex].CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        
    }

    private void BubbleUp(T item)
    {
        // In the heap, the parent of a node is (index - 1)/2
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int tmp = itemB.HeapIndex;
        itemB.HeapIndex = itemA.HeapIndex;
        itemA.HeapIndex = tmp;
    }

}

// For the heap, we need to be able to compare items (items must implement IComparable), and they must also have a property heapIndex so that we can know where in the heap array the item is stored.
// Therefore, any type stored in the heap must implement IHeapItem
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
