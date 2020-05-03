using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B
{
    public class BTree
    {
        public List<Soda> soda = new List<Soda>();
        public Node node;
        public static int grade = 5;
        public Node root = null;
        public int number = 0;

        public void insert(Soda info)
        {
            if (node == null)
            {
                var newNode = new Node();
                newNode.rightVal = info;
                node = newNode;
            }
            else
            {
                var x = ins(info, node);
                if (x != null)
                {
                    node = x;
                }
            }
        }
        public Node ins(Soda data, Node node)
        {
            if (node.leftChild == null && node.intermideateChild == null && node.rightChild == null)
            {
                if (node.rightVal == null)
                {
                    if (node.leftVal.Name.CompareTo(data.Name) == 1)
                    {
                        node.rightVal = node.leftVal;
                        node.leftVal = data;
                    }
                    else
                    {
                        node.rightVal = data;
                    }
                    return null;
                }
                else
                {

                    if (node.rightVal.Name.CompareTo(data.Name) == -1)
                    {
                        var actualNode = new Node();
                        var Up = new Node();
                        Up.leftVal = node.rightVal;
                        actualNode.leftVal = data;
                        node.rightVal = null;
                        Up.leftChild = node;
                        Up.intermideateChild = actualNode;


                        return Up;
                    }

                    else if (node.leftVal.Name.CompareTo(data.Name) == 1)
                    {
                        var actualNode = new Node();
                        var Up = new Node();
                        actualNode.leftChild = node.rightChild;
                        node.rightVal = null;
                        Up.leftVal = node.leftVal;
                        node.leftVal = data;
                        Up.leftChild = node;
                        Up.intermideateChild = actualNode;


                        return Up;
                    }
                    else
                    {
                        var actualNode = new Node();
                        var Up = new Node();
                        Up.leftVal = data;
                        actualNode.leftVal = node.rightVal;
                        node.rightVal = null;
                        Up.leftChild = node;
                        Up.intermideateChild = actualNode;
                        return Up;
                    }
                }
            }
            else
            {
                if (node.rightVal == null)
                {
                    if (node.leftVal.Name.CompareTo(data.Name) == -1)
                    {
                        var inserTemp = ins(data, node.intermideateChild);
                        if (inserTemp != null)
                        {
                            node.rightVal = inserTemp.leftVal;
                            node.intermideateChild = inserTemp.leftChild;
                            node.rightChild = inserTemp.intermideateChild;
                        }
                        return null;
                    }
                    else
                    {
                        var inserTemp = ins(data, node.leftChild);
                        if (inserTemp != null)
                        {
                            node.rightVal = node.leftVal;
                            node.leftVal = inserTemp.leftVal;
                            node.leftChild = inserTemp.leftChild;
                            node.rightChild = node.intermideateChild;
                            node.intermideateChild = inserTemp.intermideateChild;
                        }
                        return null;
                    }
                }
                else
                {

                    if (data.Name.CompareTo(node.leftVal.Name) == -1)
                    {
                        var inserTemp = ins(data, node.leftChild);
                        if (inserTemp != null)
                        {
                            var Up = new Node();
                            Up.leftVal = node.leftVal;
                            Up.leftChild = inserTemp;
                            node.leftVal = node.rightVal;
                            node.rightVal = null;
                            node.leftChild = node.intermideateChild;
                            node.intermideateChild = node.rightChild;
                            node.rightChild = null;
                            Up.intermideateChild = node;
                            return Up;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else if ((data.Name.CompareTo(node.rightVal.Name) == -1))
                    {
                        var insertTemp = ins(data, node.intermideateChild);
                        if (insertTemp != null)
                        {
                            var Up = new Node();
                            var actualNode = new Node();
                            Up.leftVal = insertTemp.leftVal;
                            actualNode.leftVal = node.rightVal;
                            actualNode.intermideateChild = node.rightChild;
                            actualNode.leftChild = insertTemp.intermideateChild;
                            node.rightVal = null;
                            node.rightChild = null;
                            Up.leftChild = node;
                            node.intermideateChild = insertTemp.leftChild;
                            Up.intermideateChild = actualNode;
                            actualNode.leftChild = insertTemp.intermideateChild;
                            return Up;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        var inserTemp = ins(data, node.rightChild);
                        if (inserTemp != null)
                        {
                            var Up = new Node();
                            Up.leftChild = node.rightChild;
                            node.rightChild = null;
                            node.rightVal = null;
                            Up.leftChild = node;
                            Up.intermideateChild = inserTemp;
                            return Up;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
