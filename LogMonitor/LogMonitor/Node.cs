using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMonitor
{
    public class Node
    {
        public Node prev;
        public Node next;
        public string textToDispaly;

        public Node()
        {
            prev = null;
            next = null;
            textToDispaly = String.Empty;
        }

        public void setPrev(Node p)
        {
            prev = p;
        }

        public void setNext(Node n)
        {
            next = n;
        }

        public Node getPrev()
        {
            return prev;
        }

        public Node getNext()
        {
            return next;
        }

        public void setText(string t)
        {
            textToDispaly = t;
        }

        public string getText()
        {
            return textToDispaly;
        }

    }
}
