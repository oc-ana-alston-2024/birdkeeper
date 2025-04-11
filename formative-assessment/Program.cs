// I came up with the name _after_ I created the solution, IT SHOULD HAVE BEEN BIRDKEEPER :sob:
namespace formative_assessment;

/// <summary>
/// Interface for any of the nodes in the tree   
/// </summary>
interface INode
{
    string Name { get; set; }
    TaxonomicGrouping? Parent { get; set; }

    // Dummy function to avoid errors as both classes will execute this differently
    public List<INode> Search(string query) { return new List<INode>(); }

    public string GetPath()
    {
        if (Parent != null)
        {
            return Parent.GetPath() + "/" + Name;
        }

        return Name;
    }
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
    public List<INode> Children { get; set; } = new();

    /// <summary>
    /// Makes the current grouping print its children out.
    /// </summary>
    public void GetChildren()
    {
        foreach (INode child in Children)
        {
            Console.WriteLine(child.Name);
        }
    }

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
    public List<INode> Search(string query)
    {
        List<INode> matches = new List<INode>();

        if (Name.ToLower() == query.ToLower().Trim())
        {
            matches.Add(this);
        }

        if (Children.Count > 0)
        {
            foreach (INode child in Children)
            {
                matches.AddRange(child.Search(query));
            }
        }

        return matches;
    }

    public override string ToString()
    {
        return Name;
    }
}

/// <summary>
/// Class for any kind of bird
/// </summary>
class Bird : INode
{
    public Bird(string name, string scientificName)
    {
        Name = name;
        ScientificName = scientificName;
    }
    
    public string Name { get; set; }
    public string ScientificName { get; set; }
    public TaxonomicGrouping? Parent { get; set; }

    /// <summary>
    /// Gets the placement of the node in its tree
    /// </summary>
    /// <returns>A path to this node</returns>
    public string GetPath()
    {
        if (Parent != null)
        {
            return Parent.GetPath() + "/" + ScientificName;
        }
        return ScientificName;
    }
    
    /// <summary>
    /// Searches for a node with the name given.
    /// </summary>
    /// <param name="query">Name to search for</param>
    /// <returns>A list of possible matches</returns>
    public List<INode> Search(string query)
    {
        List<INode> matches = new List<INode>();

        if (Name.ToLower() == query.ToLower().Trim() || ScientificName.ToLower() == query.ToLower().Trim())
        {
            matches.Add(this);
        }
        
        return matches;
    }

    public override string ToString()
    {
        return ScientificName + " - " + Name;
    }
}

class Program
{
    static void Main()
    {
        // Prompts the user until an answer is given, makes the code smaller.
        // Okay turns out this doesn't really reprompt the user at all, since an empty string is NOT null.
        // It's only looking for null to ensure that any user input is for SURE a string, because otherwise it is not robust.
        // And I like the behavior of the error handling in the main loop anyway, so I'm certainly not going to change it to catch empty strings.
        string Prompt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? response = Console.ReadLine();
                if (response != null) return response;
            }
        }

        // Just creates either a grouping or a bird, asking the user for which one before creating it. Means I have to copy code a lot less.
        void CreateNode(TaxonomicGrouping parent)
        {
            while (true)
            {
                string classification = Prompt("Bird or Grouping? ");
                if (classification.ToLower().Trim() == "bird")
                {
                    string name = Prompt("Enter name: ");
                    string scientificName = Prompt("Enter scientific name: ");
                    parent.AddChild(new Bird(name, scientificName));
                    break;
                }
                if (classification.ToLower().Trim() == "grouping")
                {
                    string name = Prompt("Enter name: ");
                    parent.AddChild(new TaxonomicGrouping(name));
                    break;
                }
                Console.WriteLine("Not a valid answer, try again!");
            }
        }
        
        // GIANT BRICK OF HARDCODING
        TaxonomicGrouping animalia = new TaxonomicGrouping("Animalia");
        TaxonomicGrouping chordata = new TaxonomicGrouping("Chordata");
        TaxonomicGrouping aves = new TaxonomicGrouping("Aves");
        animalia.AddChild(chordata);
        chordata.AddChild(aves);
        
        TaxonomicGrouping psittiaciformes = new TaxonomicGrouping("Psittitaciformes");
        TaxonomicGrouping apterygiformes = new TaxonomicGrouping("Apterygiformes");
        TaxonomicGrouping passeriformes = new TaxonomicGrouping("Passeriformes");
        aves.AddChild(psittiaciformes);
        aves.AddChild(apterygiformes);
        aves.AddChild(passeriformes);
        
        TaxonomicGrouping strigopidae = new TaxonomicGrouping("Strigopidae");
        TaxonomicGrouping apterygidae = new TaxonomicGrouping("Apterygidae");
        TaxonomicGrouping rhipidurae = new TaxonomicGrouping("Rhipidurae");
        TaxonomicGrouping meliphagidae = new TaxonomicGrouping("Meliphagidae");
        psittiaciformes.AddChild(strigopidae);
        apterygiformes.AddChild(apterygidae);
        passeriformes.AddChild(rhipidurae);
        passeriformes.AddChild(meliphagidae);
        
        TaxonomicGrouping nestor = new TaxonomicGrouping("Nestor");
        TaxonomicGrouping apteryx = new TaxonomicGrouping("Apteryx");
        TaxonomicGrouping rhipidura = new TaxonomicGrouping("Rhipidura");
        TaxonomicGrouping prosthemadera = new TaxonomicGrouping("Prosthemadera");
        strigopidae.AddChild(nestor);
        apterygidae.AddChild(apteryx);
        rhipidurae.AddChild(rhipidura);
        meliphagidae.AddChild(prosthemadera);

        Bird kaka = new Bird("Kaka", "Meridionalis");
        Bird kea = new Bird("Kea", "Notabilis");
        nestor.AddChild(kaka);
        nestor.AddChild(kea);

        Bird kiwi = new Bird("Little Spotted Kiwi", "Owenii");
        apteryx.AddChild(kiwi);

        Bird piwakawaka = new Bird("Piwakawaka", "Fuliginosa");
        rhipidura.AddChild(piwakawaka);

        Bird tui = new Bird("Tui", "Novaeseelandiae");
        prosthemadera.AddChild(tui);
        
        // Main loop
        while (true)
        {
            // Here's where the non-linearity begins so that ironically my code is more readable and so that my menus
            // don't end up 15 indents in, because honestly I like being able to use more than half of my IDE.
            // goto my beloved <3
            mainMenu:
            Console.WriteLine("1. Add new bird or grouping\n2. Search\n3. Exit");
            string choice = Prompt("Enter choice: ");
            if (choice == "1")
            {
                addNode:
                string rawGrouping = Prompt("Enter parent grouping: ");
                List<INode> results = animalia.Search(rawGrouping);
                // Heck yeah, I love that there's a RemoveAll function, I was looking through the methods for something like AddRange but for Remove
                // with the plan of using Linq Where() at first, but this is so much more simple, predicate<t> for the win :tada:
                results.RemoveAll(node => node.GetType() == typeof(Bird));
                if (!(results.Any()))
                {
                    Console.WriteLine("Invalid grouping, try again!");  
                    goto addNode;
                }
                
                if (results.Count() > 1)
                {
                    resultsClarification:
                    for (int i = 0; i < results.Count(); i++){
                        Console.WriteLine($"{i}. " + results[i].GetPath());
                    }

                    string index = Prompt("Please pick which grouping you want to add to. ");
                    if (int.TryParse(index, out int groupingIndex))
                    {
                        // Forgive me for my casting sins and assumptions
                        TaxonomicGrouping selectedGrouping = (TaxonomicGrouping) results[groupingIndex];
                        CreateNode(selectedGrouping);
                    }
                    else
                    {
                        Console.WriteLine("Invalid grouping, try again!");
                        goto resultsClarification;
                    }
                }

                else if (results.Count() == 1)
                {
                    TaxonomicGrouping selectedGrouping = (TaxonomicGrouping) results[0];
                    CreateNode(selectedGrouping);
                }

                else
                {
                    Console.WriteLine("Invalid grouping, try again!");
                    goto addNode;
                }
            }
            else if (choice == "2")
            {
                string search = Prompt("Enter search query: ");
                List<INode> results = animalia.Search(search);
                if (!(results.Any()))
                {
                    Console.WriteLine("No results found!");
                    goto mainMenu;
                }
                foreach (INode node in results)
                {
                    string nodeType = (node.GetType() == typeof(Bird)) ? "Bird" : "Taxonomic Grouping"; 
                    Console.WriteLine(node.GetPath() + " - " + nodeType);
                }
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice! Try again!");
                goto mainMenu;
            }
        }
    }
}