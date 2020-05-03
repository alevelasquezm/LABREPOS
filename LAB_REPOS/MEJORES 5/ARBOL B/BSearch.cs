using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_REPOS.MEJORES_5.ARBOL_B
{
    public class BSearch
    {
        public Node node;
        public Soda Search(string name)
        {
            return SearchItem(name, node);
        }
        public Soda SearchItem(string name, Node node)
        {
            if (node.leftChild == null)
            {
                if (node.intermideateChild == null)
                {
                    if (node.rightChild == null)
                    {

                    }
                }
                if (node.rightVal == null)
                {
                    if (node.leftVal.Name == name)
                    {
                        return node.leftVal;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (node.rightVal.Name == name)
                    {
                        return node.rightVal;
                    }
                    else if (node.leftVal.Name == name)
                    {
                        return node.leftVal;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (node.rightVal == null)
            {
                if (node.leftVal.Name == name)
                {
                    return node.leftVal;
                }
                else
                {
                    if (name.CompareTo(node.leftVal.Name) == 1)
                    {
                        return SearchItem(name, node.intermideateChild);
                    }
                    else
                    {
                        return SearchItem(name, node.leftChild);
                    }
                }
            }
            else
            {
                if (name == node.leftVal.Name)
                {
                    return node.leftVal;
                }
                if (name == node.rightVal.Name)
                {
                    return node.rightVal;
                }
                if (name.CompareTo(node.leftVal.Name) == -1)
                {
                    return SearchItem(name, node.leftChild);
                }
                if (node.leftVal.Name.CompareTo(name) == -1 && name.CompareTo(node.rightVal.Name) == -1)
                {
                    return SearchItem(name, node.intermideateChild);
                }
                else
                {
                    return SearchItem(name, node.rightChild);
                }
            }
        }
    }
}
