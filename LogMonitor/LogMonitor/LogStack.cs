using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMonitor
{
    public class LogStack
    {
        public Node head;
        public int size;

        public LogStack()
        {
            head = null;
            size = 0;
        }

        public int GetCount()
        {
            return size;
        }

        public void Push(Node node)
        {
            if (size == 0)
            {
                head = node;
                size++;
            }
            else
            {
                node.setNext(head);
                head.setPrev(node);
                head = node;
                size++;
            }
        }

        public Node Pop()
        {
            if (size == 0)
            {
                return null;
            }
            else
            {
                if (size==0)
                {
                    Node n = head;
                    head.setNext(null);
                    size--;
                    return n;
                }
                else
                {
                    Node n = head;
                    head = head.getNext();

                    if (size > 1)
                    {
                        head.setPrev(null);
                    }
                    n.setNext(null);
                    size--;
                    return n;
                }
            }
        }

        public Node ViewHead()
        {
            return head;
        }
    }
}
