// I came up with the name _after_ I created the solution
namespace birdsorter;

/// <summary>
/// Interface for any of the items -or nodes- in the tree   
/// </summary>
interface INode
{
    string Name { get; set; }
    TaxonomicGrouping? Parent { get; set; }

    // Dummy function to avoid errors as both classes will execute this differently
    public INode? Search(string query) { return null; }
}

/// <summary>
/// Class for any of the groups which hold other groups or birds
/// </summary>
class TaxonomicGrouping: INode
{
    public TaxonomicGrouping(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; }
    public TaxonomicGrouping? Parent { get; set; }
    public List<INode> Children { get; set; } = new List<INode>();

    /// <summary>
    /// Adds a child to this node's tree under this node
    /// </summary>
    /// <param name="child">The child being added to the tree</param>
    public void AddChild(INode child)
    {
        if (child.Parent != null)
        {
            child.Parent.Children.Remove(child);
        }
        Children.Add(child);
        child.Parent = this;
    }

    /// <summary>
    /// Gets the placement of the node in its tree
    /// </summary>
    /// <returns>A path to this node</returns>
    public string GetPath()
    {
        if (Parent != null)
        {
            return Parent.GetPath() + "/" + Name;
        }
        return Name;
    }
    
    /// <summary>
    /// Searches for a node with the name given.
    /// </summary>
    /// <param name="query">Name to search for</param>
    /// <returns>A list of possible matches</returns>
    public List<INode>? Search(string query)
    {
        if (query == Name)
        {
            return new List<INode> { this };
        }

        if (Children.Count > 0)
        {
            List<INode> matches = new List<INode>();
            foreach (INode child in Children)
            {
                INode? result = child.Search(query);
                if (result != null)
                {
                    matches.Add(result);
                }
            }

            if (matches.Count > 0)
            {
                return matches;
            }
        }
        
        return null;
    }

    public override string ToString()
    {
        return Name;
    }
}

class Program
{
    static void Main(string[] args)
    {
        
    }
}